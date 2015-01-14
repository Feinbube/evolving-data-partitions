using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExecutionEnvironment;

namespace Species
{
    public class Stencil : Algorithm<float>
    {
        int W = 10;
        int H = 10;

        NotifyingArr<float> inputArray = null;
        NotifyingArr<float> outputArray = null;

        public Stencil() { }
        public Stencil(Stencil model)
        {
            this.W = model.W;
            this.H = model.H;
            this.inputArray = model.inputArray.Clone();
            this.outputArray = model.outputArray.Clone();
        }

        public override Arr<byte> EmptyIndexRange()
        {
            return new Arr<byte>(W, H, 0);
        }

        public override List<NotifyingArr<float>> Arrays()
        {
            inputArray = new NotifyingArr<float>(W, H, 0);
            for (int i = 0; i < inputArray.Size; i++)
                inputArray[i] = i;

            outputArray = new NotifyingArr<float>(W, H, 0);

            return new List<NotifyingArr<float>>() { inputArray, outputArray };
        }

        protected override void apply(int indexX, int indexY, int indexZ)
        {
            outputArray[outputArray.Coords(indexX, indexY)] = inputArray.At(indexX, indexY, 0)
            + inputArray.At(indexX - 1, indexY, 0) + inputArray.At(indexX + 1, indexY, 0)
            + inputArray.At(indexX, indexY - 1, 0) + inputArray.At(indexX, indexY + 1, 0);
        }

        public override IAlgorithm Clone()
        {
            return new Stencil(this);
        }
    }
}
