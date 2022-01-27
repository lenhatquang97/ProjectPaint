using Contract;
using MaterialDesignThemes.Wpf;
using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Line2D
{
    [Serializable]
    public class Line2D: IShape
    {
        private Point2D _start = new Point2D();
        private Point2D _end = new Point2D();

        public string Name => "Line";

        public PackIconKind IconKind => PackIconKind.ChartLineVariant;

        public Settings Settings { get; set; }

        public void HandleStart(double x, double y)
        {
            _start = new Point2D() { X = x, Y = y };
        }

        public void HandleEnd(double x, double y)
        {
            _end = new Point2D() { X = x, Y = y };
        }

        public UIElement Draw()
        {
            if (Settings == null)
            {
                Settings = new Settings();
            }
            Line l = new Line()
            {
                X1 = _start.X,
                Y1 = _start.Y,
                X2 = _end.X,
                Y2 = _end.Y,
                Stroke = new SolidColorBrush(Color.FromArgb(Settings.Stroke.A, Settings.Stroke.R, Settings.Stroke.G, Settings.Stroke.B)),
                StrokeThickness = Settings.StrokeThickness,
                Fill = new SolidColorBrush(Color.FromArgb(Settings.Fill.A, Settings.Fill.R, Settings.Fill.G, Settings.Fill.B))
            };
            return l;
        }

        public IShape Clone()
        {
            return new Line2D();
        }

    }
}
