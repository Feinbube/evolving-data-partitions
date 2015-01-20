using EvolutionFramework;
using EvolutionWpfControls;
using ExecutionEnvironment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Species
{
    public class StencilSpeciesArr : IEvolvable, IPresentable
    {
        public bool Optimization = false;

        Random random;
        public Arr<int> Field;
        public int[] cellsPerProcessor;

        public Mutator[] Mutators;

        public StencilSpeciesArr(StencilSpeciesArr other)
            : this(other.random, other.Field, other.cellsPerProcessor, other.Mutators) { }

        public StencilSpeciesArr(StencilSpeciesArr other, Arr<int> field)
            : this(other.random, field, other.cellsPerProcessor, other.Mutators) { }

        public StencilSpeciesArr(Random random, int w, int h, int[] cellsPerProcessor)
            : this(random, w, h, cellsPerProcessor, new Mutator[] { new CellSwapMutator()/*new JumpToSameColorX2Mutator()*/ }) { }

        // new JumpToSameColorX2Mutator(), new JumpToSameColorMutator(), new BorderMoveMutator(), new BorderSwapMutator(), new CellSwapMutator(), new ClusterSwapMutator(), new DiagonalNeighborSwapMutator(), new GhostCellMoveMutator(), new GhostCellSwapMutator(), new NeighborSwapMutator(), new RowAndColumnSwapMutator(), new RowSwapMutator() };

        public StencilSpeciesArr(Random random, int w, int h, int[] cellsPerProcessor, Mutator[] mutators)
            : this(random, initializeField(random, cellsPerProcessor, w, h), cellsPerProcessor, mutators) { }

        private static Arr<int> initializeField(Random random, int[] cellsPerProcessor, int w, int h)
        {
            Arr<int> result = new Arr<int>(w, h);
            int fieldIndex = 0;
            for (int i = 0; i < cellsPerProcessor.Length; i++)
                for (int i2 = 0; i2 < cellsPerProcessor[i]; i2++)
                    result[fieldIndex++] = i;

            result.Shuffle(random);
            return result;
        }

        public StencilSpeciesArr(Random random, Arr<int> field, int[] cellsPerProcessor, Mutator[] mutators)
        {
            this.random = random;
            this.cellsPerProcessor = (int[])cellsPerProcessor.Clone();
            this.Field = field.Clone();
            this.Mutators = (Mutator[])mutators.Clone();
        }

        public void Optimize(int times)
        {
            if (!Optimization)
                return;

            for (int i = 0; i < times; i++)
                NextStep(true);
        }

        public double Fitness
        {
            get { return -Overhead(Field); }
        }

        public int Overhead(bool considerDistribution)
        {
            int[] cellsToCopy = new int[cellsPerProcessor.Length];

            int pos = 0;
            for (int y = 0; y < Field.H; y++)
                for (int x = 0; x < Field.W; x++)
                {
                    cellsToCopy[Field[pos]] += UniqueNeighborCount(Field, x, y, pos);
                    pos++;
                }
            return considerDistribution ? cellsToCopy.Sum() + 2 * cellsPerProcessor.Length * cellsToCopy.Max() : cellsToCopy.Sum();
        }

        public int OverheadSerial(Arr<int> field)
        {
            int[] cellsToCopy = new int[cellsPerProcessor.Length];

            int pos = 0;
            for (int y = 0; y < field.H; y++)
                for (int x = 0; x < field.W; x++)
                {
                    cellsToCopy[field[pos]] += UniqueNeighborCount(field, x, y, pos);
                    pos++;
                }
            return cellsToCopy.Sum();
        }

        private int uniqueNeighborCount(Arr<int> field, int y)
        {
            int result = 0;
            int pos = y * field.W;
            for (int x = 0; x < field.W; x++)
            {
                result += UniqueNeighborCount(field, x, y, pos);
                pos++;
            }
            return result;
        }

        public int Overhead(Arr<int> field)
        {
            return ParallelEnumerable.Range(0, field.H).Select(y => uniqueNeighborCount(field, y)).Sum();
        }

        public static int UniqueNeighborCount(Arr<int> field, int x, int y, int pos)
        {
            int result = 0;
            int w = field.W;
            int h = field.H;

            int c = field[pos];

            int n1 = x - 1 < 0 ? c : field[pos - 1];
            int n3 = x + 1 >= w ? c : field[pos + 1];
            int n2 = y - 1 < 0 ? c : field[pos - w];
            int n4 = y + 1 >= h ? c : field[pos + w];

            if (n1 != c)
            {
                if (n2 == n1) n2 = c;
                if (n3 == n1) n3 = c;
                if (n4 == n1) n4 = c;
                result++;
            }

            if (n2 != c)
            {
                if (n3 == n2) n3 = c;
                if (n4 == n2) n4 = c;
                result++;
            }

            if (n3 != c)
            {
                if (n4 == n3) n4 = c;
                result++;
            }

            if (n4 != c)
                result++;

            return result;
        }

        public void Mutate()
        {
            int choice = random.Next(0, Mutators.Length);
            int mutations = 1; // random.Next(1, 7);
            Mutators[choice].Mutate(random, this.Field, mutations);

            this.Optimize(1);
        }

        public IEvolvable Crossover(IEvolvable other)
        {
            StencilSpeciesArr mate = (StencilSpeciesArr)other;
            if (this.Equals(mate))
            {
                var result = this.Clone();
                result.Mutate();
                return result;
            }
            else
            {

                int[] cPerProcessor = (int[])cellsPerProcessor.Clone();
                int freeCells = this.Field.Length;

                // initialize field with -1 or cells that are equal in both mates
                Arr<int> field = new Arr<int>(this.Field.SizeX, this.Field.SizeY);
                freeCells -= fillWithEqualGenesOrMinusOne(field, this.Field, mate.Field, cPerProcessor);

                while (freeCells > 0)
                {
                    int position = field.FreePosition(random.Next(0, freeCells), -1);
                    int processor = suitableValue(random, freeCells, this.Field, mate.Field, position, cPerProcessor);

                    field[position] = processor;
                    cPerProcessor[processor]--;
                    freeCells--;
                }

                StencilSpeciesArr result = new StencilSpeciesArr(this, field);
                result.Optimize(3);
                return result;
            }
        }

        public static int fillWithEqualGenesOrMinusOne(Arr<int> field, Arr<int> parent1, Arr<int> parent2, int[] cellsPerProcessor)
        {
            int result = 0;
            for (int i = 0; i < field.Length; i++)
            {
                int processor = parent1[i];
                if (processor == parent2[i])
                {
                    field[i] = processor;
                    cellsPerProcessor[processor]--;
                    result++;
                }
                else
                    field[i] = -1;
            }

            return result;
        }

        public static int freePosition(Random random, int validPositionCount, int[] field)
        {
            // go to random position, ignoring cells that are already in use
            int steps = random.Next(0, validPositionCount);
            int position = -1;
            while (steps >= 0)
            {
                position++;
                if (field[position] == -1)
                    steps--;
            }
            return position;
        }

        public static int suitableValue(Random random, int freeCells, Arr<int> parent1, Arr<int> parent2, int position, int[] cellsPerProcessor)
        {
            // randomly choose a value from one of the mates, if still available                               
            int gene = random.Next(0, 2);
            int processor = gene == 0 ? parent1[position] : parent2[position];

            // if its not available choose the other one
            if (cellsPerProcessor[processor] <= 0) processor = gene != 0 ? parent1[position] : parent2[position];

            // if that is not available as well use one of the remaining processors
            if (cellsPerProcessor[processor] <= 0)
            {
                int stepsToProcessor = random.Next(0, freeCells);
                for (processor = 0; processor < cellsPerProcessor.Length; processor++)
                {
                    stepsToProcessor -= cellsPerProcessor[processor];
                    if (stepsToProcessor < 0)
                        break;
                }
            }

            return processor;
        }

        public IEvolvable Clone()
        {
            return new StencilSpeciesArr(this);
        }

        public double DifferenceTo(IEvolvable other)
        {
            return Field.Differences((other as StencilSpeciesArr).Field);
        }

        public bool IsValid
        {
            get
            {
                int[] cellsPerProcessor = (int[])this.cellsPerProcessor.Clone();
                if (cellsPerProcessor.Sum() != Field.Length)
                    return false;

                for (int i = 0; i < Field.Length; i++)
                    if (Field[i] == -1)
                        return false;
                    else
                    {
                        cellsPerProcessor[Field[i]]--;
                        if (cellsPerProcessor[Field[i]] < 0)
                            return false;
                    }

                if (cellsPerProcessor.Sum() != 0.0)
                    return false;

                return true;
            }
        }

        public override string ToString()
        {
            return "Fitness: " + Fitness + "\r\n" + Field.ToString();
        }

        public string PresentableTitle
        {
            get { return "Stencil"; }
        }

        public UIElement PresentableControl
        {
            get
            {
                var stackPanel = new StackPanel();
                stackPanel.Children.Add(new Label() { Content = this.Fitness.ToString("F") });
                stackPanel.Children.Add(new IntArray2DControl() { Array = this.Field });
                return stackPanel;
            }
        }

        public override bool Equals(object obj)
        {
            return Field.Equals((obj as StencilSpeciesArr).Field);
        }

        public override int GetHashCode()
        {
            return Field.GetHashCode();
        }

        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

        public bool NextStep(bool progress)
        {
            Arr<int> field = Field.Clone();
            int overhead = Overhead(Field);

            // remove invalid cells (cells with only neighbors of same color)
            Arr<int> f = field.Clone();
            int validCount = 0;
            for (int y = 0; y < f.H; y++)
                for (int x = 0; x < f.W; x++)
                    if (field.NeighborsWithValueHV(field[x, y], x, y, 1, true) == 4) f[x, y] = -1;
                    else validCount++;

            while (validCount > 0)
            {
                // choose start pos and color
                int startPos = f.ValidPosition(random.Next(0, validCount), -1);
                f[startPos] = -1;
                validCount--;
                int color = field[startPos];

                // remove all cells that have no neighbor with matching color and cells with same color
                Arr<int> f2 = f.Clone();
                int validCount2 = validCount;
                for (int y = 0; y < f.H; y++)
                    for (int x = 0; x < f.W; x++)
                        if (f[x, y] != -1 && (f[x, y] == color || f.NeighborsWithValueHV(color, x, y, 1, false) == 0))
                        {
                            f2[x, y] = -1;
                            validCount2--;
                        }

                while (validCount2 > 0)
                {
                    // select target at random
                    int targetPos = f2.ValidPosition(random.Next(0, validCount2), -1);
                    f2[targetPos] = -1;
                    validCount2--;

                    // swap
                    field.Swap(startPos, targetPos);

                    if ((progress && Overhead(field) < overhead) || (!progress && Overhead(field) <= overhead))
                    {
                        this.Field = field;
                        return true;
                    }
                    else
                        field.Swap(startPos, targetPos);
                }
            }

            if (!this.IsValid)
                throw new Exception("INVALID");

            return false;
        }

        public bool NextStepX(bool progress)
        {
            Arr<int> field = Field.Clone();
            int overhead = Overhead(Field);

            Arr<int> f = field.Clone();
            int validCount = field.Length;

            while (validCount > 0)
            {
                // choose start pos and color
                int startPos = f.ValidPosition(random.Next(0, validCount), -1);
                f[startPos] = -1;
                validCount--;
                int color = field[startPos];

                Arr<int> f2 = f.Clone();
                int validCount2 = validCount;

                while (validCount2 > 0)
                {
                    // select target at random
                    int targetPos = f2.ValidPosition(random.Next(0, validCount2), -1);
                    f2[targetPos] = -1;
                    validCount2--;

                    // swap
                    field.Swap(startPos, targetPos);

                    if ((progress && Overhead(field) < overhead) || (!progress && Overhead(field) <= overhead))
                    {
                        this.Field = field;
                        return true;
                    }
                    else
                        field.Swap(startPos, targetPos);
                }
            }

            if (!this.IsValid)
                throw new Exception("INVALID");

            return false;
        }

        public void Leap()
        {
            Arr<int> largerArray = new Arr<int>(Field.W * 2, Field.H * 2);

            for (int y = 0; y < Field.H; y++)
                for (int x = 0; x < Field.W; x++)
                {
                    int value = Field[x, y];
                    largerArray[x * 2, y * 2] = value;
                    largerArray[x * 2 + 1, y * 2] = value;
                    largerArray[x * 2, y * 2 + 1] = value;
                    largerArray[x * 2 + 1, y * 2 + 1] = value;
                }

            for (int i = 0; i < cellsPerProcessor.Length; i++)
                cellsPerProcessor[i] *= 4;

            Field = largerArray;
        }
    }
}
