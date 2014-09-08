using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace DrawingSupport
{
    public class Draw
    {
        public static SolidColorBrush WhiteBrush = (SolidColorBrush)new SolidColorBrush(Colors.White).GetAsFrozen();
        public static SolidColorBrush BlackBrush = (SolidColorBrush)new SolidColorBrush(Colors.Black).GetAsFrozen();
        public static SolidColorBrush RedBrush = (SolidColorBrush)new SolidColorBrush(Colors.Red).GetAsFrozen();
        public static SolidColorBrush BlueBrush = (SolidColorBrush)new SolidColorBrush(Colors.Blue).GetAsFrozen();

        public static void DrawDot(Panel surface, double x, double y) { DrawDot(surface, x, y, 0.5, BlackBrush); }

        public static void DrawDot(Panel surface, double x, double y, double r, SolidColorBrush brush) { DrawDot(surface, x, y, r, brush, BlackBrush); }

        public static void DrawDot(Panel surface, double x, double y, double r, SolidColorBrush fillBrush, SolidColorBrush borderBrush)
        {
            surface.Dispatcher.Invoke((Action)(() =>
            {
                surface.Children.Add(new Ellipse() { Height = 2 * r, Width = 2 * r, Margin = new Thickness(x - r, y - r, 0, 0), StrokeThickness = 1, Stroke = borderBrush, HorizontalAlignment = System.Windows.HorizontalAlignment.Left, VerticalAlignment = System.Windows.VerticalAlignment.Top, Fill = fillBrush });
            }));
        }

        public static void DrawRect(Panel surface, double x, double y, double w, double h) { DrawRect(surface, x, y, 1, 1, BlackBrush); }

        public static void DrawRect(Panel surface, double x, double y, double w, double h, SolidColorBrush brush) { DrawRect(surface, x, y, w, h, brush, BlackBrush); }

        public static void DrawRect(Panel surface, double x, double y, double w, double h, SolidColorBrush fillBrush, SolidColorBrush borderBrush)
        {
            surface.Dispatcher.Invoke((Action)(() =>
            {
                surface.Children.Add(new Rectangle() { Height = h, Width = w, Margin = new Thickness(x - w / 2.0, y - h / 2.0, 0, 0), StrokeThickness = 1, Stroke = borderBrush, HorizontalAlignment = System.Windows.HorizontalAlignment.Left, VerticalAlignment = System.Windows.VerticalAlignment.Top, Fill = fillBrush });
            }));
        }

        public static void DrawLine(Panel surface, double fromX, double fromY, double toX, double toY) { DrawLine(surface, fromX, fromY, toX, toY, BlackBrush, 1); }

        public static void DrawLine(Panel surface, double fromX, double fromY, double toX, double toY, SolidColorBrush brush) { DrawLine(surface, fromX, fromY, toX, toY, brush, 1); }

        public static void DrawLine(Panel surface, double fromX, double fromY, double toX, double toY, SolidColorBrush brush, double boldness)
        {
            surface.Dispatcher.Invoke((Action)(() =>
            {
                surface.Children.Add(new Line() { X1 = fromX, Y1 = fromY, X2 = toX, Y2 = toY, StrokeThickness = boldness, Stroke = brush });
            }));
        }

        public static void DrawText(Panel surface, double x, double y, string text, double fontSize = 10)
        {
            surface.Dispatcher.Invoke((Action)(() =>
            {
                surface.Children.Insert(0, new Label() { FontSize = fontSize, Content = text, Margin = new Thickness(x, y, 0, 0), Foreground=BlackBrush });
            }));
        }

        public static SolidColorBrush GenerateBrush(int index, int count, float saturation = 0.8f, float brightness = 0.8f)
        {
            return ColorHelper.GenerateBrush(index, count, saturation, brightness);
        }

        public static void Clear(Panel surface)
        {
            surface.Children.Clear();
        }
    }
}
