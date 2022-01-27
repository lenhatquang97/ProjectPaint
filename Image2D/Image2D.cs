using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Contract;
using MaterialDesignThemes.Wpf;

namespace Image2D
{
    [Serializable]
    public class Image2D : IShape
    {
        private Point2D _leftTop = new Point2D();
        private Point2D _rightBottom = new Point2D();

        public Settings Settings { get; set; }

        public string Name => "Image";

        public PackIconKind IconKind => PackIconKind.Image;

        public IShape Clone()
        {
            return new Image2D();
        }

        public UIElement Draw()
        {
            if (Settings == null)
            {
                Settings = new Settings();
            }
            var x = Math.Min(_rightBottom.X, _leftTop.X);
            var y = Math.Min(_rightBottom.Y, _leftTop.Y);
            var width = Math.Max(_rightBottom.X, _leftTop.X) - x;
            var height = Math.Max(_rightBottom.Y, _leftTop.Y) - y;
            Image img = new Image()
            {
                Height = height,
                Width = width,
                RenderTransform = new RotateTransform(Settings.Angle)
            };
            BitmapImage bi = new BitmapImage();
            bi.BeginInit();
            bi.UriSource = new Uri(Settings.ImagePath);
            Debug.WriteLine(Settings.ImagePath);
            bi.EndInit();
            img.Source = bi;
            Canvas.SetLeft(img, x);
            Canvas.SetTop(img, y);
            return img;
        }

        public void HandleStart(double x, double y)
        {
            _leftTop = new Point2D() { X = x, Y = y };
        }

        public void HandleEnd(double x, double y)
        {
            _rightBottom = new Point2D() { X = x, Y = y };

        }
    }
}
