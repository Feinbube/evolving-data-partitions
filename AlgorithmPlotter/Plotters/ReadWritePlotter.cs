using DrawingSupport;
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
    public class ReadWritePlotter : RowPerRoundPlotter
    {
        public override void Plot(List<IArray> arrays)
        {
            drawLine(0, row, 100, row, Draw.BlackBrush);

            List<int> threadIds = getThreadIds(arrays);
            foreach (int threadId in threadIds)
            {
                List<int> readColumns = getReadColumns(threadId, arrays);
                List<int> writeColumns = getWriteColumns(threadId, arrays);
                foreach (int from in readColumns)
                    foreach (int to in writeColumns)
                        drawLine(from, row + 0.10, to, row + 0.85, threadIds.IndexOf(threadId), threadIds.Count);
            }

            for (int arrayId = 0; arrayId < arrays.Count; arrayId++)
            {
                if (arrays[arrayId].OnlyWritten || arrays[arrayId].ReadAndWritten) drawAtWrites(arrays, arrayId, row + 0.85, threadIds);
                if (arrays[arrayId].OnlyRead || arrays[arrayId].ReadAndWritten) drawAtReads(arrays, arrayId, row + 0.10, threadIds);
                if (arrays[arrayId].NotUsed) drawAt(arrays, arrayId, row + 0.5, Draw.WhiteBrush);
            }
        }

        protected void drawAt(List<IArray> arrays, int arrayId, double y, SolidColorBrush brush)
        {
            drawText(getColumn(arrays, arrayId, 0) - 0.5 - dotRadius / zoomX - fontSize / zoomX / 2.0, y - dotRadius / zoomY - fontSize / zoomY / 2.0, "[" + arrayId.ToString() + "]");

            for (int pos = 0; pos < arrays[arrayId].Size; pos++)
            {
                drawDot(getColumn(arrays, arrayId, pos), y, brush);
                drawText(getColumn(arrays, arrayId, pos) + dotRadius / zoomX, y - fontSize / zoomY, arrays[arrayId].AsText(pos));
            }
        }

        protected void drawAtReads(List<IArray> arrays, int arrayId, double y, List<int> threadIds)
        {
            drawText(getColumn(arrays, arrayId, 0) - 0.5 - dotRadius / zoomX - fontSize / zoomX / 2.0, y - dotRadius / zoomY - fontSize / zoomY / 2.0, "[" + arrayId.ToString() + "]");

            for (int pos = 0; pos < arrays[arrayId].Size; pos++)
            {
                drawDot(getColumn(arrays, arrayId, pos), y, getBrushReads(arrays[arrayId], pos, threadIds));
                drawText(getColumn(arrays, arrayId, pos) + dotRadius / zoomX, y - fontSize / zoomY, arrays[arrayId].AsText(pos));
            }
        }

        protected void drawAtWrites(List<IArray> arrays, int arrayId, double y, List<int> threadIds)
        {
            drawText(getColumn(arrays, arrayId, 0) - 0.5 - dotRadius / zoomX - fontSize / zoomX / 2.0, y - dotRadius / zoomY - fontSize / zoomY / 2.0, "[" + arrayId.ToString() + "]");

            for (int pos = 0; pos < arrays[arrayId].Size; pos++)
            {
                drawDot(getColumn(arrays, arrayId, pos), y, getBrushWrites(arrays[arrayId], pos, threadIds));
                drawText(getColumn(arrays, arrayId, pos) + dotRadius / zoomX, y - fontSize / zoomY, arrays[arrayId].AsText(pos));
            }
        }

        protected SolidColorBrush getBrushReads(IArray array, int pos, List<int> threadIds)
        {
            List<int> reads = array.ReadsAtPosition(pos);
            return reads.Count == 0 ? Draw.WhiteBrush : reads.Count > 1 ? Draw.BlackBrush : brushFromThreadNumber(threadIds.IndexOf(reads.First()), threadIds.Count);
        }

        protected SolidColorBrush getBrushWrites(IArray array, int pos, List<int> threadIds)
        {
            List<int> writes = array.WritesAtPosition(pos);
            return writes.Count == 0 ? Draw.WhiteBrush : writes.Count > 1 ? Draw.BlackBrush : brushFromThreadNumber(threadIds.IndexOf(writes.First()), threadIds.Count);
        }
    }
}
