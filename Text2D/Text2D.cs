using Contract;
using MaterialDesignThemes.Wpf;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Text2D
{
    [Serializable]
    public class Text2D: IShape
    {
        private Point2D _leftTop = new Point2D();
        private Point2D _rightBottom = new Point2D();

        public string Name => "Text";

        public PackIconKind IconKind => PackIconKind.FormatText;

        public Settings Settings { get; set; }


        public UIElement Draw()
        {
            if (Settings == null)
            {
                Settings = new Settings();
            }
            var x = Math.Min(_rightBottom.X, _leftTop.X);
            var y = Math.Min(_rightBottom.Y, _leftTop.Y);

            var res = new TextBlock()
            {
                Text = Settings.Text,
                Foreground = new SolidColorBrush(Color.FromArgb(Settings.Foreground.A, Settings.Foreground.R, Settings.Foreground.G, Settings.Foreground.B)),
                FontSize = Settings.FontSize,
                RenderTransform = new RotateTransform(Settings.Angle)
            };

            Canvas.SetLeft(res, x);
            Canvas.SetTop(res, y);

            return res;
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
            return new Text2D();
        }

    }
}
