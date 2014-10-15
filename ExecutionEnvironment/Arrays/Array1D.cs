using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExecutionEnvironment
{
    public class Array1D<T> : Array<T>
    {
        public Array1D(int sizeX) : this(sizeX, 1, 1) { }

        public Array1D(int sizeX, int sizeY) : this(sizeX, sizeY, 1) { }

        public Array1D(int sizeX, int sizeY, int sizeZ) : base(sizeX, sizeY, sizeZ) { }

        public void Swap(int i, int j)
        {
            T temp = this[i];
            this[i] = this[j];
            this[j] = temp;
        }

        public T At(int x) { return this[x]; }

        public void Shuffle(Random random)
        {
            for (int i = 0; i < this.Length; i++)
                Swap(i, random.Next(i, this.Length));
        }

        public int FreePosition(int steps, T freeValue)
        {
            // go to random position, ignoring cells that are already in use
            int position = -1;
            while (steps >= 0)
            {
                position++;
                if (this[position].Equals(freeValue))
                    steps--;
            }
            return position;
        }

        public int ValidPosition(int steps, T invalidValue)
        {
            // go to random position, ignoring cells that are already in use
            int position = -1;
            while (steps >= 0)
            {
                position++;
                if (!this[position].Equals(invalidValue))
                    steps--;
            }
            return position;
        }

        public double Differences(Array<int> other)
        {
            int result = 0;
            for (int pos = 0; pos < Size; pos++)
                if (!this[pos].Equals(other[pos]))
                    result++;

            return result;
        }

        public IEnumerable<T> Take(int count) { lock (memory) { return memory.Take(count); } }

        public T Max { get { lock (memory) { return memory.Max(); } } }

        public T Min { get { lock (memory) { return memory.Min(); } } }

        public double Sum { get { return memory.Sum(a => (double)Convert.ChangeType(a, typeof(double))); } }
    }
}
