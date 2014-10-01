using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExecutionEnvironment
{
    public class Array3D<T> : Array2D<T>
    {
        public Array3D(int sizeX) : this(sizeX, 1, 1) { }

        public Array3D(int sizeX, int sizeY) : this(sizeX, sizeY, 1) { }

        public Array3D(int sizeX, int sizeY, int sizeZ) : base(sizeX, sizeY, sizeZ) { }

        public T At(int x, int y, int z) { return this[x, y, z]; }
    }
}
