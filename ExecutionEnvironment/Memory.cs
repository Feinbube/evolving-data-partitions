using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExecutionEnvironment
{
    public class Memory
    {
        public static Memory Instance = new Memory();
        private Memory() { }

        public IMemoryPlotter Plotter { get; set; }

        List<IArray> arrays = new List<IArray>();

        public void MemoryFence()
        {
            lock (this)
            {
                if (Plotter != null)
                {
                    Plotter.NewRound();
                    Plotter.Plot(arrays);
                }
                foreach (IArray array in arrays)
                    array.NewRound();
            }
        }

        internal void Add(IArray array)
        {
            arrays.Add(array);
        }

        public void Clear()
        {
            arrays.Clear();
        }
    }
}
