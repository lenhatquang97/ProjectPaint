using System;
using System.Windows;
using MaterialDesignThemes.Wpf;
namespace Contract
{
    public interface IShape
    {
        public Settings Settings { get; set; }
        public string Name { get; }
        public PackIconKind IconKind { get; }
        public void HandleStart(double x, double y);
        public void HandleEnd(double x, double y);

        public UIElement Draw();
        public IShape Clone();
        
    }
}
