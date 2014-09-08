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
    public abstract class Plotter : IMemoryPlotter
    {
        public Panel Surface { get; set; }

        public abstract void NewRound();

        public abstract void Plot(List<IArray> arrays);

        public virtual void Reset() { Surface.Children.Clear(); }

        protected List<int> getThreadIds(List<IArray> arrays)
        {
            return arrays.SelectMany(a => a.ReadsByThreadId.Keys.Concat(a.WritesByThreadId.Keys)).Distinct().ToList();
        }

        #region Drawing

        protected double zoomX = 30;
        protected double zoomY = 60;
        protected double dotRadius = 3;
        protected double lineWidth = 1;
        protected double fontSize = 7;

        protected void drawDot(double x, double y) { drawDot(x, y, Draw.BlackBrush); }

        protected void drawDot(double x, double y, SolidColorBrush brush)
        {
            Draw.DrawDot(Surface, zoomX * x, zoomY * y, dotRadius, brush);
        }

        protected void drawLine(double fromX, double fromY, double toX, double toY, int threadNumber, int threadCount)
        {
            drawLine(fromX, fromY, toX, toY, brushFromThreadNumber(threadNumber, threadCount));
        }

        protected void drawLine(double fromX, double fromY, double toX, double toY, SolidColorBrush brush)
        {
            Draw.DrawLine(Surface, zoomX * fromX, zoomY * fromY, zoomX * toX, zoomY * toY, brush, lineWidth);
        }

        protected void drawText(double x, double y, string text)
        {
            Draw.DrawText(Surface, zoomX * x - dotRadius, zoomY * y - dotRadius, text, fontSize);
        }

        protected Thickness GetMargin(double x, double y)
        {
            return new Thickness(zoomX * x - dotRadius, zoomY * y - dotRadius, 0, 0);
        }

        protected SolidColorBrush brushFromThreadNumber(int threadNumber, int threadCount)
        {
            return Draw.GenerateBrush(threadNumber, threadCount, 1, 1);
        }

        #endregion Drawing
    }
}
