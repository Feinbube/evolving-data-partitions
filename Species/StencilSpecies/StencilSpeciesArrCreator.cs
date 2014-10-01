using EvolutionFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataFieldLayoutSimulation
{
    public class StencilSpeciesArrCreator : ICreator
    {
        Random random;

        int w = 0;
        int h = 0;
        
        int[] cellsPerProcessor = null;

        public StencilSpeciesArrCreator(Random random, int fieldW, int fieldH, double[] processorRatios)
        {
            this.random = random;
            
            this.w = fieldW;
            this.h = fieldH;

            if (processorRatios.Sum() != 1.0)
                throw new Exception("Processor ratios have to sum up to 1.0!");

            int cells = fieldW * fieldH;
            this.cellsPerProcessor = new int[processorRatios.Length];
            for (int i = 1; i < processorRatios.Length; i++)
                this.cellsPerProcessor[i] = (int)Math.Round(processorRatios[i] * cells);
            this.cellsPerProcessor[0] = cells - this.cellsPerProcessor.Sum();
        }

        public IEvolvable Create()
        {
            return new StencilSpeciesArr(random, w, h, cellsPerProcessor);
        }
    }
}
