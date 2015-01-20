using ExecutionEnvironment;
using Species;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SingleIndividualMutator
{
    public class IterativeSingleIndividualMutator
    {
        int targetW = 64;
        int targetH = 64;

        public StencilSpeciesArr Individual { get; set; }

        public IterativeSingleIndividualMutator(int w, int h, int processorCount)
        {
            this.targetW = w;
            this.targetH = h;

            int startW = Math.Min(w, processorCount);
            int startH = Math.Min(h, processorCount);

            Individual = (StencilSpeciesArr)new StencilSpeciesArrCreator(new Random(), startW, startH, processorCount).Create();
        }

        public void Progress()
        {
            if (Individual.NextStep(true))
                return;

            for (int i = 0; i < 3; i++)
                if (!Individual.NextStep(false))
                    break;

            if (Individual.NextStepX(true))
                return;

            for (int i = 0; i < 3; i++)
                if (!Individual.NextStepX(false))
                    break;

            Individual.Leap();
        }
    }
}
