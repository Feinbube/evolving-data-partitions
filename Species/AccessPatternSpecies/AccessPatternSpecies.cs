//using EvolutionFramework;
//using EvolutionWpfControls;
//using ExecutionEnvironment;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using System.Windows.Controls;

//namespace Species
//{
//    public class AccessPatternSpecies : IEvolvable, IPresentable
//    {
//        public Random Random;

//        public int[] IndicesPerProcessor { get; set; }
//        public byte ProcessorCount { get { return (byte)IndicesPerProcessor.Length; } }
//        public IAlgorithm Algorithm { get; set; }
//        public Arr<byte> IndexRange { get; set; }
//        public string AlgorithmType { get { return Algorithm.GetType().Name; } }

//        public AccessPatternSpecies(Random random, IAlgorithm algorithm, params double[] indicesPerProcessor)
//        {
//            this.Random = random;
//            this.Algorithm = algorithm;
//            this.IndexRange = algorithm.EmptyIndexRange();

//            int unassignedIndexCount = IndexRange.Size;
//            this.IndicesPerProcessor = new int[indicesPerProcessor.Length];
//            for (int i = indicesPerProcessor.Length - 1; i < 1; i--)
//            {
//                this.IndicesPerProcessor[i] = (int)Math.rou(indicesPerProcessor[i] * IndexRange.Size);
//                unassignedIndexCount -= this.IndicesPerProcessor[i];
//            }
//        }

//        public AccessPatternSpecies(AccessPatternSpecies model)
//        {
//            this.Random = model.Random;
//            this.ProcessorCount = model.ProcessorCount;
//            this.Algorithm = model.Algorithm.Clone();
//            this.IndexRange = model.IndexRange.Clone();
//        }

//        public double Fitness
//        {
//            get
//            {
//                RunResult result = Algorithm.Run(IndexRange, ProcessorCount);
//                IsValid = result.IsValid;
//                return result.Statistics().TotalReads;
//            }
//        }

//        public void Mutate()
//        {
//            IndexRange.Swap(Random.Next(IndexRange.Size), Random.Next(IndexRange.Size));
//        }

//        public IEvolvable Crossover(IEvolvable other)
//        {
//            AccessPatternSpecies mate = (AccessPatternSpecies)other;

//            if (this.ProcessorCount != mate.ProcessorCount || this.AlgorithmType != mate.AlgorithmType)
//                throw new Exception("Incompatible Mate!");

//            if (this.Equals(mate))
//            {
//                var result = this.Clone();
//                result.Mutate();
//                return result;
//            }

//            return new AccessPatternSpecies(this.Random, this.ProcessorCount, this.Algorithm.Clone()) { IndexRange = crossover(IndexRange, mate.IndexRange) };
//        }

//        private Arr<byte> crossover(Arr<byte> indexRange, Arr<byte> mateIndexRange)
//        {
//            int[] cPerProcessor = (int[])cellsPerProcessor.Clone();
//            int freeCells = this.Field.Length;

//            // initialize field with -1 or cells that are equal in both mates
//            Arr<int> field = new Arr<int>(this.Field.SizeX, this.Field.SizeY);
//            freeCells -= fillWithEqualGenesOrMinusOne(field, this.Field, mate.Field, cPerProcessor);

//            while (freeCells > 0)
//            {
//                int position = field.FreePosition(random.Next(0, freeCells), -1);
//                int processor = suitableValue(random, freeCells, this.Field, mate.Field, position, cPerProcessor);

//                field[position] = processor;
//                cPerProcessor[processor]--;
//                freeCells--;
//            }
//        }

//        public static int fillWithEqualGenesOrMinusOne(Arr<int> field, Arr<int> parent1, Arr<int> parent2, int[] cellsPerProcessor)
//        {
//            int result = 0;
//            for (int i = 0; i < field.Length; i++)
//            {
//                int processor = parent1[i];
//                if (processor == parent2[i])
//                {
//                    field[i] = processor;
//                    cellsPerProcessor[processor]--;
//                    result++;
//                }
//                else
//                    field[i] = -1;
//            }

//            return result;
//        }

//        public static int freePosition(Random random, int validPositionCount, int[] field)
//        {
//            // go to random position, ignoring cells that are already in use
//            int steps = random.Next(0, validPositionCount);
//            int position = -1;
//            while (steps >= 0)
//            {
//                position++;
//                if (field[position] == -1)
//                    steps--;
//            }
//            return position;
//        }

//        public static int suitableValue(Random random, int freeCells, Arr<int> parent1, Arr<int> parent2, int position, int[] cellsPerProcessor)
//        {
//            // randomly choose a value from one of the mates, if still available                               
//            int gene = random.Next(0, 2);
//            int processor = gene == 0 ? parent1[position] : parent2[position];

//            // if its not available choose the other one
//            if (cellsPerProcessor[processor] <= 0) processor = gene != 0 ? parent1[position] : parent2[position];

//            // if that is not available as well use one of the remaining processors
//            if (cellsPerProcessor[processor] <= 0)
//            {
//                int stepsToProcessor = random.Next(0, freeCells);
//                for (processor = 0; processor < cellsPerProcessor.Length; processor++)
//                {
//                    stepsToProcessor -= cellsPerProcessor[processor];
//                    if (stepsToProcessor < 0)
//                        break;
//                }
//            }

//            return processor;
//        }

//        public IEvolvable Clone()
//        {
//            return new AccessPatternSpecies(this);
//        }

//        public double DifferenceTo(IEvolvable other)
//        {
//            AccessPatternSpecies mate = (AccessPatternSpecies)other;
//            if (this.ProcessorCount != mate.ProcessorCount || this.AlgorithmType != mate.AlgorithmType)
//                return double.MaxValue;
//            return IndexRange.Differences(mate.IndexRange);
//        }

//        public bool IsValid { get; private set; }

//        public override string ToString()
//        {
//            return "Fitness: " + Fitness + "\r\n" + IndexRange.ToString();
//        }

//        public string PresentableTitle
//        {
//            get { return AlgorithmType; }
//        }

//        public System.Windows.UIElement PresentableControl
//        {
//            get
//            {
//                var stackPanel = new StackPanel();
//                stackPanel.Children.Add(new Label() { Content = this.Fitness.ToString("F") });
//                stackPanel.Children.Add(new ByteArray2DControl() { Array = this.IndexRange });
//                return stackPanel;
//            }
//        }

//        public override bool Equals(object obj)
//        {
//            return IndexRange.Equals((obj as AccessPatternSpecies).IndexRange)
//                && ProcessorCount.Equals((obj as AccessPatternSpecies).ProcessorCount)
//                && AlgorithmType.Equals((obj as AccessPatternSpecies).AlgorithmType);
//        }

//        public override int GetHashCode()
//        {
//            return IndexRange.GetHashCode() + ProcessorCount + AlgorithmType.GetHashCode();
//        }

//        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
//    }
//}
