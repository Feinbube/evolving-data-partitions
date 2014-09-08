using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataFieldLayoutSimulation
{
    public abstract class Mutator
    {
        public abstract void Mutate(Random random, int[] field, int w, int h, int mutations);

        protected static void swap(int[] field, int i, int j)
        {
            int temp = field[i];
            field[i] = field[j];
            field[j] = temp;
        }

        protected static void swapColumn(int[] field, int x1, int x2, int w, int h)
        {
            if (x1 == x2) return;
            for (int y = 0; y < h; y++)
                swap(field, coords(x1, y, w), coords(x2, y, w));
        }

        protected static void swapRow(int[] field, int y1, int y2, int w, int h)
        {
            if (y1 == y2) return;
            for (int x = 0; x < w; x++)
                swap(field, coords(x, y1, w), coords(x, y2, w));
        }

        protected static int coords(int x, int y, int w)
        {
            return x + y * w;
        }

        protected static int at(int[] field, int x, int y, int w, int h, int borderValue)
        {
            if (x < 0 || x > w - 1) return borderValue;
            if (y < 0 || y > h - 1) return borderValue;

            return field[coords(x, y, w)];
        }

        protected static int similarNeighbors(int[] field, int x, int y, int w, int h, bool borderAsMatch)
        {
            int value = field[coords(x, y, w)];
            int result = 0;
            int borderValue = borderAsMatch ? value : -1;
            for (int xDiff = -1; xDiff <= 1; xDiff++)
                for (int yDiff = -1; yDiff <= 1; yDiff++)
                    result += value == at(field, x + xDiff, y + yDiff, w, h, borderValue) ? 1 : 0;
            return result - 1; // ignore match in center
        }

        protected static int freePosition(Random random, int validPositionCount, int[] field)
        {
            // go to random position, ignoring cells that are already in use
            int steps = random.Next(0, validPositionCount);
            int position = -1;
            while (steps >= 0)
            {
                position++;
                if (field[position] == -1)
                    steps--;
            }
            return position;
        }

        protected static int nextPosition(Random random, int processor, int[] field, int w, int h, int pos)
        {
            int x = pos % w;
            int y = pos / w;
            int directions = 0;
            for (int xDiff = -1; xDiff <= 1; xDiff++)
                for (int yDiff = -1; yDiff <= 1; yDiff++)
                {
                    int value = at(field, x + xDiff, y + yDiff, w, y, -1);
                    if (value != -1 && value != processor)
                        directions++;
                }
            if (directions == 0)
                return -1;
            int angle = random.Next(0, directions);
            for (int xDiff = -1; xDiff <= 1; xDiff++)
                for (int yDiff = -1; yDiff <= 1; yDiff++)
                {
                    int value = at(field, x + xDiff, y + yDiff, w, h, -1);
                    if (value != -1 && value != processor)
                    {
                        directions--;
                        if (directions < 0)
                            return coords(x + xDiff, y + yDiff, w);
                    }
                }
            return -1;
        }
    }
}
