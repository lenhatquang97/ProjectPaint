using MaterialDesignThemes.Wpf;
using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Contract
{
    [Serializable]
    public class Point2D : IShape
    {
        public double X { get; set; }
        public double Y { get; set; }
        public string Name => "Point";

        public PackIconKind IconKind => PackIconKind.StarFourPoints;

        public Settings Settings { get; set; }

        public void HandleStart(double x, double y)
        {
            X = x;
            Y = y;
        }
        public void HandleEnd(double x, double y)
        {
            X = x;
            Y = y;
        }
        public UIElement Draw()
        {
            if(Settings == null)
            {
                Settings = new Settings();
            }
            Line l = new Line()
            {
                X1 = X,
                Y1 = Y,
                X2 = X,
                Y2 = Y,
                Stroke = new SolidColorBrush(Color.FromArgb(Settings.Stroke.A, Settings.Stroke.R, Settings.Stroke.G, Settings.Stroke.B)),
                StrokeThickness = Settings.StrokeThickness,
                Fill = new SolidColorBrush(Color.FromArgb(Settings.Fill.A, Settings.Fill.R, Settings.Fill.G, Settings.Fill.B))
            };

            return l;
        }
        public IShape Clone()
        {
            return new Point2D();
        }


    }
}
