using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Shapes;
using System.Windows.Media;
using System.Runtime.Serialization;

namespace Contract
{
    [Serializable]
    public class Settings
    {
        //Other objects
        public ARGB Fill;
        public double StrokeThickness;
        public ARGB Stroke;
        //Text setup
        public String Text;
        public ARGB Foreground;
        public double FontSize;
        //Image
        public String ImagePath;
        //Transform
        public double Angle;
        public Settings()
        {
            Fill = new ARGB(Colors.Transparent.A, Colors.Transparent.R, Colors.Transparent.G, Colors.Transparent.B);
            StrokeThickness = 1;
            Stroke = new ARGB(Colors.Black.A, Colors.Black.R, Colors.Black.G, Colors.Black.B);
            Text = "Hehe";
            Foreground = new ARGB(Colors.Black.A, Colors.Black.R, Colors.Black.G, Colors.Black.B);
            FontSize = 20;
            ImagePath = "";
            Angle = 0;
        }
        public Settings(Settings se)
        {
            Fill = se.Fill;
            StrokeThickness = se.StrokeThickness;
            Stroke = se.Stroke;
            Text = se.Text;
            Foreground = se.Foreground;
            FontSize = se.FontSize;
            ImagePath = se.ImagePath;
            Angle = se.Angle;
        }
    }
}
