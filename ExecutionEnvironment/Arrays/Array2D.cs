using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExecutionEnvironment
{
    public class Array2D<T> : Array1D<T>
    {
        public Array2D(int sizeX) : this(sizeX, 1, 1) { }

        public Array2D(int sizeX, int sizeY) : this(sizeX, sizeY, 1) { }

        public Array2D(int sizeX, int sizeY, int sizeZ) : base(sizeX, sizeY, sizeZ) { this.W = sizeX; this.Width = sizeX; this.H = sizeY; this.Height = sizeY; }

        public readonly int W;
        public readonly int Width;

        public readonly int H;
        public readonly int Height;

        public T At(int x, int y) { return this[x, y]; }

        public T At(int x, int y, T borderValue) { return OutOfBounds(x, y) ? borderValue : At(x, y); }

        public bool OutOfBounds(int x, int y) { return x < 0 || x > this.W - 1 || y < 0 || y > this.H - 1; }

        public int Coords(int x, int y) { return x + y * W; }

        public void SwapColumn(int x1, int x2)
        {
            if (x1 == x2) return;
            for (int y = 0; y < this.H; y++)
                Swap(Coords(x1, y), Coords(x2, y));
        }

        public void SwapRow(int y1, int y2)
        {
            if (y1 == y2) return;
            for (int x = 0; x < this.W; x++)
                Swap(Coords(x, y1), Coords(x, y2));
        }

        public void SwapCluster(int x1, int y1, int w, int h, int x2, int y2)
        {
            for (int x = 0; x < w; x++)
                for (int y = 0; y < h; y++)
                    Swap(Coords(x + x1, y + y1), Coords(x + x2, y + y2));
        }

        public int SimilarNeighbors(int x, int y, bool borderAsMatch) { return SimilarNeighbors(x, y, 1, borderAsMatch); }

        public int SimilarNeighbors(int x, int y, int rings, bool borderAsMatch) { return NeighborsWithValue(At(x, y), x, y, rings, borderAsMatch); }

        public int NeighborsWithValue(T value, int x, int y, bool borderAsMatch) { return NeighborsWithValue(value, x, y, 1, borderAsMatch); }

        public int NeighborsWithValue(T value, int x, int y, int rings, bool borderAsMatch)
        {
            int result = 0;
            for (int yDiff = -rings; yDiff <= rings; yDiff++)
                for (int xDiff = -rings; xDiff <= rings; xDiff++)
                    if ((borderAsMatch && OutOfBounds(x + xDiff, y + yDiff)) || value.Equals(At(x + xDiff, y + yDiff)))
                        result++;
            return result - 1; // ignore match in center
        }

        public int NeighborsWithValueHV(T value, int x, int y, int rings, bool borderAsMatch)
        {
            int result = 0;
            for (int i = 1; i <= rings; i++)
            {
                if ((borderAsMatch && OutOfBounds(x - i, y)) || (!OutOfBounds(x - i, y) && value.Equals(At(x - i, y))))
                    result++;
                if ((borderAsMatch && OutOfBounds(x + i, y)) || (!OutOfBounds(x + i, y) && value.Equals(At(x + i, y))))
                    result++;
                if ((borderAsMatch && OutOfBounds(x, y - i)) || (!OutOfBounds(x, y - i) && value.Equals(At(x, y - i))))
                    result++;
                if ((borderAsMatch && OutOfBounds(x, y + i)) || (!OutOfBounds(x, y + i) && value.Equals(At(x, y + i))))
                    result++;
            }
            return result;
        }

        public int NextPosition(Random random, T value, T freeValue, int x, int y)
        {
            int directions = 0;
            for (int xDiff = -1; xDiff <= 1; xDiff++)
                for (int yDiff = -1; yDiff <= 1; yDiff++)
                {
                    T v = At(x + xDiff, y + yDiff, freeValue);
                    if (!v.Equals(freeValue) && !v.Equals(value))
                        directions++;
                }

            if (directions == 0)
                return -1;

            int angle = random.Next(0, directions);
            for (int xDiff = -1; xDiff <= 1; xDiff++)
                for (int yDiff = -1; yDiff <= 1; yDiff++)
                {
                    T v = At(x + xDiff, y + yDiff, freeValue);
                    if (!v.Equals(freeValue) && !v.Equals(value))
                    {
                        directions--;
                        if (directions < 0)
                            return Coords(x + xDiff, y + yDiff);
                    }
                }

            return -1;
        }

        public override string ToString()
        {
            string result = "";
            int pos = 0;
            for (int y = 0; y < H; y++)
            {
                for (int x = 0; x < W; x++)
                    result += this[pos++].ToString() + " ";
                result += "\r\n";
            }
            return result;
        }
    }
}
