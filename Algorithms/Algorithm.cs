using ExecutionEnvironment;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Algorithms
{
    public abstract class Algorithm
    {
        public Random Random = new Random();

        protected int sizeX = 0;
        protected int sizeY = 0;
        protected int sizeZ = 0;

        protected abstract void setup();
        protected virtual void printInput() { }
        protected abstract void algorithm();
        protected virtual void printResult() { }
        protected abstract void cleanup();
        protected abstract bool isValid();

        public RunResult Run(double sizeX, double sizeY, double sizeZ, bool print, int rounds, int warmupRounds)
        {
            Console.Write("Running " + this.GetType().Name);

            this.sizeX = sizeX > int.MaxValue ? int.MaxValue : (int)sizeX;
            this.sizeY = sizeY > int.MaxValue ? int.MaxValue : (int)sizeY;
            this.sizeZ = sizeZ > int.MaxValue ? int.MaxValue : (int)sizeZ;

            setup();
            Memory.Instance.MemoryFence();

            Console.Write("(" + this.sizeX + " / " + this.sizeY + " / " + this.sizeZ + ")...");

            if (print)
            {
                Console.WriteLine();
                Console.WriteLine("Input:");
                printInput();
            }

            for (int warmupRound = 0; warmupRound < warmupRounds; warmupRound++)
                run();

            System.Diagnostics.Stopwatch watch = new System.Diagnostics.Stopwatch();
            watch.Start();
            for (int round = 0; round < rounds; round++)
                run();
            watch.Stop();

            for (int warmupRound = 0; warmupRound < warmupRounds; warmupRound++)
                run();

            if (print)
            {
                Console.WriteLine();
                Console.WriteLine("Result:");
                printResult();
            }

            bool valid = checkResult(print);

            cleanup();

            Console.WriteLine("Done in " + watch.Elapsed.TotalSeconds + "s. " + (valid ? "SUCCESS" : "<!!! FAILED !!!>"));

            return new RunResult() { Name = this.GetType().Name, SizeX = this.sizeX, SizeY = this.sizeY, SizeZ = this.sizeZ, ElapsedTotalSeconds = watch.Elapsed.TotalSeconds, Valid = valid };
        }

        private void run()
        {
            algorithm();
            Memory.Instance.MemoryFence();
        }

        protected bool checkResult(bool throwOnError)
        {
            if (!isValid())
            {
                if (throwOnError)
                {
                    throw new Exception("Calculated Result is not valid.");  // this line gets annoying after some time...
                }
                return false;
            }

            return true;
        }

        #region Convenient Functions

        protected string doubleToString(double value)
        {
            return String.Format("{0:0.00}", value).Substring(0, 4);
        }

        protected void swap(ref Arr<byte> a, ref Arr<byte> b)
        {
            Arr<byte> tmp = a;
            a = b;
            b = tmp;
        }

        protected void printField<T>(Arr<T> field, int sizeX)
        {
            for (int i = 0; i < Math.Min(sizeX, 80); i++)
                Console.Write(typeToString(field[i]) + " ");

            if (sizeX > 80)
                Console.Write("...");

            Console.WriteLine();
        }

        protected void printField<T>(Arr<T> field, int sizeX, int sizeY)
        {
            for (int i = -1; i <= sizeX; i++)
            {
                for (int j = -1; j <= sizeY; j++)
                {
                    if (j == -1 || j == sizeY || i == -1 || i == sizeX)
                        Console.Write("**** ");
                    else
                        Console.Write(typeToString(field[i, j]) + " ");
                }
                Console.WriteLine();
            }
        }

        private string typeToString(object value)
        {
            return value is double || value is float ? String.Format("{0:0.00}", value).Substring(0, 4) : value.ToString();
        }

        protected void printField(Arr<byte> fields, int sizeX, int sizeY, Action<int, int> printAction)
        {
            for (int y = 0; y < sizeY; y++)
            {
                for (int x = 0; x < sizeX; x++)
                    printAction.Invoke(x, y);
                Console.WriteLine();
            }
        }

        protected void paintField(Arr<float> bitmap)
        {
            for (int x = 0; x < sizeX; x++)
                for (int y = 0; y < sizeY; y++)
                {
                    paintValue(bitmap[x, y], 0.0f, 32.0f, ' ');
                    paintValue(bitmap[x, y], 32.0f, 64.0f, '.');
                    paintValue(bitmap[x, y], 64.0f, 96.0f, '°');
                    paintValue(bitmap[x, y], 96.0f, 128.0f, ':');
                    paintValue(bitmap[x, y], 128.0f, 160.0f, '+');
                    paintValue(bitmap[x, y], 160.0f, 192.0f, '*');
                    paintValue(bitmap[x, y], 192.0f, 224.0f, '#');
                    paintValue(bitmap[x, y], 224.0f, 256.0f, '8');
                }
        }

        protected void paintValue(double value, double fromInclusive, double toExclusive, char output)
        {
            if (value >= fromInclusive && value < toExclusive)
                Console.Write(output);
        }

        #endregion Convenient Functions

        public override bool Equals(object obj)
        {
            return this.GetType() == obj.GetType();
        }

        public override int GetHashCode()
        {
            return this.GetType().GetHashCode();
        }

        public override string ToString()
        {
            return this.GetType().ToString();
        }
    }
}
