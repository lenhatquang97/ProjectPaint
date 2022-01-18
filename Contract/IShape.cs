using System;
using System.Windows;
using MaterialDesignThemes.Wpf;
namespace Contract
{
    public interface IShape
    {
        string Name { get; }
        PackIconKind IconKind { get; }
        void HandleStart(double x, double y);
        void HandleEnd(double x, double y);

        UIElement Draw();
        IShape Clone();
        
    }
}
