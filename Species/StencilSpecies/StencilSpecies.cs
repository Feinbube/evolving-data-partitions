using EvolutionFramework;
using EvolutionWpfControls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace DataFieldLayoutSimulation
{
    public class StencilSpecies : IEvolvable, IPresentable
    {
        public Mutator[] Mutators = { new BorderSwapMutator() }; // new BorderMoveMutator(), new BorderSwapMutator(), new CellSwapMutator(), new ClusterSwapMutator(), new DiagonalNeighborSwapMutator(), new GhostCellMoveMutator(), new GhostCellSwapMutator(), new NeighborSwapMutator(), new RowAndColumnSwapMutator(), new RowSwapMutator() };

        public StencilSpeciesCreator Creator = null;
        public int[] Field = null;

        public StencilSpecies(StencilSpecies other) : this(other.Creator, other.Field) { }

        public StencilSpecies(StencilSpeciesCreator creator, int[] field)
        {
            this.Creator = creator;
            this.Field = (int[])field.Clone();
        }

        public StencilSpecies(StencilSpeciesCreator creator)
        {
            this.Creator = creator;
            this.Field = new int[creator.FieldW * creator.FieldH];

            int fieldIndex = 0;
            for (int i = 0; i < creator.CellsPerProcessor.Length; i++)
                for (int i2 = 0; i2 < creator.CellsPerProcessor[i]; i2++)
                    Field[fieldIndex++] = i;

            shuffle(creator.Random, Field);
        }

        public IEvolvable Clone()
        {
            return new StencilSpecies(this);
        }

        public void Mutate(Random random)
        {
            int choice = random.Next(0, Mutators.Length);
            int mutations = 1;// random.Next(1, (Creator.FieldW + Creator.FieldH) / 2);
            Mutators[choice].Mutate(random, this.Field, Creator.FieldW, Creator.FieldH, mutations);

            // ROTATE STUFF!!
        }

        #region Crossover

        public IEvolvable Crossover(Random random, IEvolvable other)
        {
            StencilSpecies mate = (StencilSpecies)other;
            if (this.Equals(mate))
            {
                var result = this.Clone();
                result.Mutate(random);
                return result;
            }

            int[] cellsPerProcessor = (int[])Creator.CellsPerProcessor.Clone();
            int freeCells = this.Field.Length;

            // initialize field with -1 or cells that are equal in both mates
            int[] field = new int[this.Field.Length];
            freeCells -= fillWithEqualGenesOrMinusOne(field, this.Field, mate.Field, cellsPerProcessor);

            while (freeCells > 0)
            {
                int position = freePosition(random, freeCells, field);
                int processor = suitableValue(random, freeCells, this.Field, mate.Field, position, cellsPerProcessor);

                field[position] = processor;
                cellsPerProcessor[processor]--;
                freeCells--;
            }

            return new StencilSpecies(Creator, field);
        }

        public static int fillWithEqualGenesOrMinusOne(int[] field, int[] parent1, int[] parent2, int[] cellsPerProcessor)
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

        public static int suitableValue(Random random, int freeCells, int[] parent1, int[] parent2, int position, int[] cellsPerProcessor)
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

        #endregion Crossover

        public double Fitness
        {
            get
            {
                return -Overhead(Field, Creator.FieldW, Creator.FieldH);
                //return -OverheadNew(Creator.CellsPerProcessor, Field, Creator.FieldW, Creator.FieldH);
            }
        }

        public static int Overhead(int[] field, int w, int h)
        {
            int result = 0;

            int pos = 0;
            for (int y = 0; y < h; y++)
                for (int x = 0; x < w; x++)
                {
                    result += UniqueNeighborCount(field, x, y, w, h);
                    pos++;
                }
            return result;
        }

        public static int OverheadNew(int[] cellsPerProcessor, int[] field, int w, int h)
        {
            int[] cellsToCopy = new int[cellsPerProcessor.Length];

            int pos = 0;
            for (int y = 0; y < h; y++)
                for (int x = 0; x < w; x++)
                {
                    cellsToCopy[field[pos]] += UniqueNeighborCount(field, x, y, w, h);
                    pos++;
                }
            return cellsToCopy.Sum() + 2 * cellsPerProcessor.Length * cellsToCopy.Max();
        }

        public static int UniqueNeighborCount(int[] field, int x, int y, int w, int h)
        {
            int result = 0;

            int pos = x + y * w;
            int c = field[pos];
            int n1 = x - 1 < 0 ? c : field[pos - 1];
            int n2 = y - 1 < 0 ? c : field[pos - w];
            int n3 = x + 1 >= w ? c : field[pos + 1];
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

        public void Feed(Random random, int resources) { }

        public override string ToString()
        {
            return "Speciman with Fitness of " + Fitness;
        }

        private void shuffle(Random random, int[] field)
        {
            for (int i = 0; i < field.Length; i++)
                swap(field, i, random.Next(i, field.Length));
        }

        private static void swap(int[] field, int i, int j)
        {
            int temp = field[i];
            field[i] = field[j];
            field[j] = temp;
        }

        public int At(int positionX, int positionY)
        {
            return Field[positionX + positionY * Creator.FieldW];
        }

        public bool IsValid
        {
            get
            {
                if (Field.Length != Creator.FieldW * Creator.FieldH)
                    return false;

                var cellsPerProcessor = (int[])Creator.CellsPerProcessor.Clone();
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

        public override bool Equals(object obj)
        {
            StencilSpecies other = (StencilSpecies)obj;
            if (this.Field.Length != other.Field.Length) return false;
            for (int i = 0; i < this.Field.Length; i++)
                if (this.Field[i] != other.Field[i])
                    return false;

            return true;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public Control AsControl()
        {
            return new StencilSpeciesControl() { Item = this };
        }
    }
}
