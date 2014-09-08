using ExecutionEnvironment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace AlgorithmPlotter
{
    public class ReadWriteDensePlotter : ReadWritePlotter
    {
        protected override int getColumn(List<IArray> arrays, int arrayId, int pos)
        {
            int result = 2;
            for (int aId = 0; aId < arrays.Count; aId++)
            {
                if (arrays[arrayId].OnlyWritten && arrays[aId].OnlyRead || arrays[arrayId].OnlyRead && arrays[aId].OnlyWritten)
                    continue;

                if (aId < arrayId) result += arrays[aId].Size + 1;
                else return result + pos;
            }

            throw new Exception();
        }
    }
}
