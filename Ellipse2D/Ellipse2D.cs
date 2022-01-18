using System;
using Contract;
using System.Windows;
using System.Windows.Shapes;
using System.Windows.Media;
using System.Windows.Controls;
using MaterialDesignThemes.Wpf;

namespace Ellipse2D
{
    public class Ellipse2D: IShape
    {
        private Point2D _leftTop = new Point2D();
        private Point2D _rightBottom = new Point2D();

        public string Name => "Ellipse";

        public PackIconKind IconKind => PackIconKind.Ellipse;

        public UIElement Draw()
        {
            var x = Math.Min(_rightBottom.X, _leftTop.X);
            var y = Math.Min(_rightBottom.Y, _leftTop.Y);
            var width = Math.Max(_rightBottom.X, _leftTop.X) - x;
            var height = Math.Max(_rightBottom.Y, _leftTop.Y) - y;
            var ellipse = new Ellipse()
            {
                Width = width,
                Height = height,
                Stroke = new SolidColorBrush(Colors.Red),
                StrokeThickness = 1
            };
            Canvas.SetLeft(ellipse, x);
            Canvas.SetTop(ellipse, y);

            return ellipse;
        }

        public void HandleStart(double x, double y)
        {
            _leftTop.X = x;
            _leftTop.Y = y;
        }

        public void HandleEnd(double x, double y)
        {
            _rightBottom.X = x;
            _rightBottom.Y = y;
        }

        public IShape Clone()
        {
            return new Ellipse2D();
        }
    }
}
