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
    public class TwoDPlotter : Plotter
    {
        public override void NewRound()
        {
            zoomX = 8;
            zoomY = 8;
            dotRadius = 2;
            Surface.Children.Clear();
        }

        public override void Plot(List<IArray> arrays)
        {
            for (int x = 0; x < 100; x++)
                for (int y = 0; y < 100; y++)
                    drawDot(x, y);

            for (int x = 0; x < 100; x++)
                for (int y = 0; y < 100; y++)
                    drawLine(x, y, y, x, x%10, 10);
        }
    }
}
