using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExecutionEnvironment
{
    public class Arr<T> : Array3D<T>
    {
        public Arr(int sizeX) : this(sizeX, 1, 1) { }

        public Arr(int sizeX, int sizeY) : this(sizeX, sizeY, 1) { }

        public Arr(int sizeX, int sizeY, int sizeZ) : base(sizeX, sizeY, sizeZ) { }

        public Arr(T[] values) : this(values.Length) { this.Write(values); }

        public Arr(T[] values, int w, int h) : this(w, h) { this.Write(values); }

        public Arr<T> Clone()
        {
            Arr<T> clone = new Arr<T>(this.SizeX, this.SizeY, this.SizeZ);
            clone.memory = (T[])this.memory.Clone();
            clone.written = (bool[])this.written.Clone();
            return clone;
        }        
    }
}
