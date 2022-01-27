using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace PaintUI
{
    /// <summary>
    /// Interaction logic for ChangePenWidthUI.xaml
    /// </summary>
    public partial class ChangePenWidthUI : Window
    {
        public delegate void ChangePenWidth(double val);
        public event ChangePenWidth Handler;
        public ChangePenWidthUI()
        {
            InitializeComponent();

        }
        public ChangePenWidthUI(double val)
        {
            InitializeComponent();
            PenWidthSlider.Value = val;
        }

        private void ButtonOK_Click(object sender, RoutedEventArgs e)
        {
            if (Handler != null)
            {
                double value = PenWidthSlider.Value;
                Handler(value);
                this.Close();
                return;
            }
            this.Close();
        }

        private void ButtonCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
            
        }
    }
}
