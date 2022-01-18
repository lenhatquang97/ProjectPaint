using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using Contract;
using MaterialDesignThemes.Wpf;

namespace Rectangle2D
{
    public class Rectangle2D: IShape
    {
        private Point2D _leftTop = new Point2D();
        private Point2D _rightBottom = new Point2D();

        public string Name => "Rectangle";

        public PackIconKind IconKind => PackIconKind.RectangleOutline;

        public UIElement Draw()
        {
            var x = Math.Min(_rightBottom.X, _leftTop.X);
            var y = Math.Min(_rightBottom.Y, _leftTop.Y);
            var width = Math.Max(_rightBottom.X, _leftTop.X) - x;
            var height = Math.Max(_rightBottom.Y, _leftTop.Y) - y;

            var rect = new Rectangle()
            {
                Width = width,
                Height = height,
                Stroke = new SolidColorBrush(Colors.Red),
                StrokeThickness = 1
            };

            Canvas.SetLeft(rect, x);
            Canvas.SetTop(rect, y);

            return rect;
        }

        public void HandleStart(double x, double y)
        {
            _leftTop = new Point2D() { X = x, Y = y };
        }

        public void HandleEnd(double x, double y)
        {
            _rightBottom = new Point2D() { X = x, Y = y };

        }

        public IShape Clone()
        {
            return new Rectangle2D();
        }
    }
}
