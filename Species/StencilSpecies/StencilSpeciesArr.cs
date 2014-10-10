using EvolutionFramework;
using EvolutionWpfControls;
using ExecutionEnvironment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace DataFieldLayoutSimulation
{
    public class StencilSpeciesArr : IEvolvable, IPresentable
    {
        Random random;
        Arr<int> Field;
        int[] cellsPerProcessor;

        public Mutator[] Mutators;

        public StencilSpeciesArr(StencilSpeciesArr other)
            : this(other.random, other.Field, other.cellsPerProcessor, other.Mutators) { }

        public StencilSpeciesArr(StencilSpeciesArr other, Arr<int> field)
            : this(other.random, field, other.cellsPerProcessor, other.Mutators) { }

        public StencilSpeciesArr(Random random, int w, int h, int[] cellsPerProcessor)
            : this(random, w, h, cellsPerProcessor, new Mutator[] { new JumpToSameColorX2Mutator() }) { }

        // new JumpToSameColorX2Mutator(), new JumpToSameColorMutator(), new BorderMoveMutator(), new BorderSwapMutator(), new CellSwapMutator(), new ClusterSwapMutator(), new DiagonalNeighborSwapMutator(), new GhostCellMoveMutator(), new GhostCellSwapMutator(), new NeighborSwapMutator(), new RowAndColumnSwapMutator(), new RowSwapMutator() };

        public StencilSpeciesArr(Random random, int w, int h, int[] cellsPerProcessor, Mutator[] mutators)
            : this(random, initializeField(random, cellsPerProcessor, w, h), cellsPerProcessor, mutators) {}

        private static Arr<int> initializeField(Random random, int[] cellsPerProcessor, int w, int h)
        {
            Arr<int> result = new Arr<int>(w,h);
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

        public double Fitness
        {
            get { return -Overhead(false); }
        }

        public int Overhead(bool considerDistribution)
        {
            int[] cellsToCopy = new int[cellsPerProcessor.Length];

            int pos = 0;
            for (int y = 0; y < Field.H; y++)
                for (int x = 0; x < Field.W; x++)
                {
                    cellsToCopy[Field[pos]] += UniqueNeighborCount(Field, x, y);
                    pos++;
                }
            return considerDistribution ? cellsToCopy.Sum() + 2 * cellsPerProcessor.Length * cellsToCopy.Max() : cellsToCopy.Sum();
        }

        public static int UniqueNeighborCount(Arr<int> field, int x, int y)
        {
            int result = 0;

            int pos = x + y * field.W;
            int c = field[pos];

            int n1 = x - 1 < 0 ? c : field[pos - 1];
            int n2 = y - 1 < 0 ? c : field[pos - field.W];
            int n3 = x + 1 >= field.W ? c : field[pos + 1];
            int n4 = y + 1 >= field.H ? c : field[pos + field.W];

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

            // ROTATE STUFF!!
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

            return new StencilSpeciesArr(this, field);
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

        public Control PresentableControl
        {
            get { return new IntArray2DControl() { Array = this.Field }; }
        }
    }
}
