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
    public class RowPerRoundPlotter : Plotter
    {
        protected int row = 0;

        public override void NewRound() { row++; }

        public override void Plot(List<IArray> arrays)
        {
            List<int> threadIds = getThreadIds(arrays);
            foreach (int threadId in threadIds)
            {
                List<int> readColumns = getReadColumns(threadId, arrays);
                List<int> writeColumns = getWriteColumns(threadId, arrays);
                foreach (int from in readColumns)
                    foreach (int to in writeColumns)
                        drawLine(from, row-1, to, row, threadIds.IndexOf(threadId), threadIds.Count);
            }

            for (int arrayId = 0; arrayId < arrays.Count; arrayId++)
            {
                drawText(getColumn(arrays, arrayId, 0) - 0.5, row, "[" + arrayId.ToString() +  "]:");

                for (int pos = 0; pos < arrays[arrayId].Size; pos++)
                {
                    drawDot(getColumn(arrays, arrayId, pos), row);
                    drawText(getColumn(arrays, arrayId, pos), row, arrays[arrayId].AsText(pos));
                }
            }
        }

        public override void Reset()
        {
            base.Reset();
            row = 0;
        }

        protected virtual List<int> getReadColumns(int threadId, List<IArray> arrays)
        {
            return arrays
                 .Select((a, i) => new { ArrayIndex = i, Dictionary = a.ReadsByThreadId })
                 .Where(item => item.Dictionary.ContainsKey(threadId))
                 .SelectMany(item => item.Dictionary[threadId].Select(arrayPos => this.getColumn(arrays, item.ArrayIndex, arrayPos)))
                 .ToList();
        }

        protected virtual List<int> getWriteColumns(int threadId, List<IArray> arrays)
        {
            return arrays
                 .Select((a, i) => new { ArrayIndex = i, Dictionary = a.WritesByThreadId })
                 .Where(item => item.Dictionary.ContainsKey(threadId))
                 .SelectMany(item => item.Dictionary[threadId].Select(arrayPos => this.getColumn(arrays, item.ArrayIndex, arrayPos)))
                 .ToList();
        }

        protected virtual int getColumn(List<IArray> arrays, int arrayId, int pos)
        {
            int result = 2;
            for (int aId = 0; aId < arrays.Count; aId++)
                if (aId < arrayId) result += arrays[aId].Size + 1;
                else return result + pos;

            throw new Exception();
        }
    }
}
