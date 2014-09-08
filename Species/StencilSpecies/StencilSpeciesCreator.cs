using EvolutionFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataFieldLayoutSimulation
{
    public class StencilSpeciesCreator : ICreator
    {
        public Random Random;
        public int FieldW = 0;
        public int FieldH = 0;
        public int ProcessorCount { get { return CellsPerProcessor.Length; } }
        public int[] CellsPerProcessor = null;

        public StencilSpeciesCreator(Random random, int fieldW, int fieldH, double[] processorRatios)
        {
            this.FieldW = fieldW;
            this.FieldH = fieldH;

            this.Random = random;

            if (processorRatios.Sum() != 1.0)
                throw new Exception("Processor ratios have to sum up to 1.0!");

            int cells = fieldW * fieldH;
            this.CellsPerProcessor = new int[processorRatios.Length];
            for (int i = 1; i < processorRatios.Length; i++)
                this.CellsPerProcessor[i] = (int)Math.Round(processorRatios[i] * cells);
            this.CellsPerProcessor[0] = cells - this.CellsPerProcessor.Sum();
        }

        public IEvolvable Create()
        {
            return new StencilSpecies(this);
        }
    }
}
