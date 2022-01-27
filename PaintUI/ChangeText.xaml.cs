using Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
    /// Interaction logic for ChangeText.xaml
    /// </summary>
    public partial class ChangeText : Window
    {
        public delegate void ChangeSettings(Settings se);
        public event ChangeSettings Handler;
        public Color color;
        public ChangeText()
        {
            InitializeComponent();
            color = Colors.Black;
        }

        private void ButtonColor_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.ColorDialog colorDialog = new System.Windows.Forms.ColorDialog();
            if(colorDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                color = Color.FromArgb(colorDialog.Color.A, colorDialog.Color.R, colorDialog.Color.G, colorDialog.Color.B);
                ButtonColor.Background = new SolidColorBrush(color);
            }
        }

        private void ButtonOK_Click(object sender, RoutedEventArgs e)
        {
            if(Handler != null)
            {
                Settings se = new Settings();
                se.Text = TextDrawField.Text;
                double val = se.FontSize;
                double.TryParse(NumberField.Text, out val);
                se.FontSize = val;
                se.Foreground = new ARGB(color.A, color.R, color.G, color.B);
                Handler(se);
            }
            this.Close();
        }

        private void ButtonCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void NumberField_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }
    }
}
