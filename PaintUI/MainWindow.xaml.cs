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
using System.Windows.Navigation;
using System.Windows.Shapes;
using Fluent;
using Contract;
using System.IO;
using System.Reflection;
using MaterialDesignThemes.Wpf;
using Microsoft.Win32;
using System.Runtime.Serialization.Formatters.Binary;
using System.Diagnostics;

namespace PaintUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : RibbonWindow
    {
        public MainWindow(){
            InitializeComponent();
        }
        bool _isDrawing = false;
        List<IShape> _shapes = new List<IShape>();
        Settings se;
        IShape _preview;
        string _selectedShapeName = "";
        Dictionary<string, IShape> _prototypes = new Dictionary<string, IShape>();
        private ScaleTransform st = new ScaleTransform();
        private void canvas_MouseDown(object sender, MouseButtonEventArgs e)
        {
            _isDrawing = true;
            Point pos = e.GetPosition(canvas);
            _preview.HandleStart(pos.X, pos.Y);
        }

        private void canvas_MouseUp(object sender, MouseButtonEventArgs e)
        {
            _isDrawing = false;

            // Thêm đối tượng cuối cùng vào mảng quản lí
            Point pos = e.GetPosition(canvas);
            _preview.HandleEnd(pos.X, pos.Y);
            _shapes.Add(_preview);

            // Sinh ra đối tượng mẫu kế
            _preview = _prototypes[_selectedShapeName].Clone();
            _preview.Settings = new Settings(se);

            // Ve lai Xoa toan bo
            canvas.Children.Clear();
            Debug.WriteLine(_shapes.Count);

            // Ve lai tat ca cac hinh
            foreach (var shape in _shapes)
            {
                var element = shape.Draw();
                canvas.Children.Add(element);
            }
        }

        private void canvas_MouseMove(object sender, MouseEventArgs e)
        {
            if (_isDrawing)
            {
                Point pos = e.GetPosition(canvas);
                _preview.HandleEnd(pos.X, pos.Y);

                // Xoá hết các hình vẽ cũ
                canvas.Children.Clear();

                // Vẽ lại các hình trước đó
                foreach (var shape in _shapes)
                {
                    UIElement element = shape.Draw();
                    canvas.Children.Add(element);
                }
                // Vẽ hình preview đè lên
                canvas.Children.Add(_preview.Draw());
                Title = $"{pos.X} {pos.Y}";
            }
        }
        private void RibbonWindow_Loaded(object sender, RoutedEventArgs e)
        {
            se = new Settings();
            var exeFolder = AppDomain.CurrentDomain.BaseDirectory;
            var dlls = new DirectoryInfo(exeFolder).GetFiles("*.dll");

            foreach (var dll in dlls)
            {
                var assembly = Assembly.LoadFrom(dll.FullName);
                var types = assembly.GetTypes();

                foreach (var type in types)
                {
                    if (type.IsClass)
                    {
                        if (typeof(IShape).IsAssignableFrom(type))
                        {
                            var shape = Activator.CreateInstance(type) as IShape;
                            _prototypes.Add(shape.Name, shape);
                        }
                    }
                }
            }

            foreach (var item in _prototypes)
            {
                var shape = item.Value as IShape;
                if(shape.Name == "Point"){
                    continue;
                }
                var icon = new PackIcon()
                {
                    Kind = shape.IconKind,
                    Width = 30,
                    Height = 30
                };
                var button = new Fluent.Button()
                {
                    Name = "Button" + shape.Name,
                    Header = shape.Name,
                    Foreground = new SolidColorBrush(Colors.Indigo),
                    LargeIcon = icon,
                    Margin = new Thickness(10,10,0,0)
                };
                button.Click += prototypeButton_Click;
                GrObjGroupBox.Items.Add(button);
            }


            _selectedShapeName = _prototypes.First().Value.Name;
            _preview = _prototypes[_selectedShapeName].Clone();
        }
        private void prototypeButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender != null)
            {
                _selectedShapeName = (sender as Fluent.Button).Header as String;
                if(_selectedShapeName == "Text")
                {
                    ChangeText changeText = new ChangeText();
                    changeText.Handler += ChangeTextSettings;
                    changeText.Show();
                }
                else if(_selectedShapeName == "Image")
                {
                    OpenFileDialog openFileDialog = new OpenFileDialog();
                    openFileDialog.Filter = "Image files (*.png;*.jpeg)|*.png;*.jpeg|All files (*.*)|*.*";
                    if (openFileDialog.ShowDialog() == true)
                    {
                        if(openFileDialog.FileName.Length != 0){
                            se.ImagePath = System.IO.Path.GetFullPath(openFileDialog.FileName);
                        }
                    }
                }
                _preview = _prototypes[_selectedShapeName].Clone();
                _preview.Settings = new Settings(se);
            }
            
        }
        private void ChangeTextSettings(Settings sec)
        {
            se.Text = sec.Text;
            se.FontSize = sec.FontSize;
            se.Foreground = sec.Foreground;
            _preview.Settings = new Settings(se);
        }
        private void ButtonSave_Click(object sender, RoutedEventArgs e)
        {

            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Title = "Save a binary file";
            saveFileDialog.Filter = "Binary File |*.dat|Png Image |*.png";
            saveFileDialog.ShowDialog();
            if(saveFileDialog.FileName != "")
            {
                String fileName = saveFileDialog.FileName;
                FileStream fs = new FileStream(fileName, FileMode.Create);
                if (fileName.Contains(".dat"))
                {
                    BinaryFormatter bf = new BinaryFormatter();
                    bf.Serialize(fs, _shapes);
                }
                else if (fileName.Contains(".png"))
                {
                    RenderTargetBitmap bmp = new RenderTargetBitmap((int)canvas.ActualWidth, (int)canvas.ActualHeight, 100.0, 100.0, PixelFormats.Default);
                    bmp.Render(canvas);
                    PngBitmapEncoder encoder = new PngBitmapEncoder();
                    encoder.Frames.Add(BitmapFrame.Create(bmp));
                    encoder.Save(fs);
                }
                fs.Close();
            }


        }

        private void ButtonImport_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Title = "Open Dat File";
            openFileDialog.Filter = "Binary File |*.dat|Png Image |*.png";
            openFileDialog.ShowDialog();
            if(openFileDialog.FileName != "")
            {
                String fileName = openFileDialog.FileName;
                FileStream fs = new FileStream(fileName, FileMode.Open);
                if (fileName.Contains(".dat"))
                {
                    BinaryFormatter bf = new BinaryFormatter();

                    List<IShape> ls = (List<IShape>)bf.Deserialize(fs);
                    if (ls == null)
                    {
                        fs.Close();
                        return;
                    }
                    _shapes.Clear();
                    canvas.Children.Clear();
                    foreach (var shape in ls)
                    {
                        _shapes.Add(shape);
                        var element = shape.Draw();
                        canvas.Children.Add(element);
                    }
                }
                else if (fileName.Contains(".png"))
                {
                    _shapes.Clear();
                    canvas.Children.Clear();
                    MemoryStream memoStream = new MemoryStream();
                    fs.CopyTo(memoStream);
                    BitmapImage bmi = new BitmapImage();
                    bmi.BeginInit();
                    bmi.StreamSource = memoStream;
                    bmi.EndInit();
                    ImageBrush brush = new ImageBrush(bmi);
                    canvas.Background = brush;

                }
                fs.Close();
            }
        }

        private void ButtonFillColor_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.ColorDialog colorDialog = new System.Windows.Forms.ColorDialog();
            if(colorDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                se.Fill = new ARGB(colorDialog.Color.A,colorDialog.Color.R,colorDialog.Color.G, colorDialog.Color.B);
                _preview.Settings = new Settings(se);
            }

        }

        private void ButtonPenWidth_Click(object sender, RoutedEventArgs e)
        {
            ChangePenWidthUI openPenWidthDialog = new ChangePenWidthUI(se.StrokeThickness);
            openPenWidthDialog.Handler += ChangePenWidth;
            openPenWidthDialog.Show();            

        }
        private void ChangePenWidth(double val)
        {
            se.StrokeThickness = val;
            _preview.Settings = new Settings(se);
        }

        private void ButtonStrokeColor_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.ColorDialog colorDialog = new System.Windows.Forms.ColorDialog();
            if (colorDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                se.Stroke = new ARGB(colorDialog.Color.A, colorDialog.Color.R, colorDialog.Color.G, colorDialog.Color.B);
                _preview.Settings = new Settings(se);
            }
        }
        
        private void canvas_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            canvas.RenderTransform = st;
            if (e.Delta > 0){
                st.ScaleX *= 1.1;
                st.ScaleY *= 1.1;
                
            }
            else
            {
                st.ScaleX /= 1.1;
                st.ScaleY /= 1.1;
            }
        }

        private void ButtonRotatePlus_Click(object sender, RoutedEventArgs e)
        {
            se.Angle = se.Angle >= 90 ? 90 : se.Angle + 15 ;
            Debug.WriteLine(se.Angle);
            _preview.Settings = new Settings(se);
        }

        private void ButtonRotateMinus_Click(object sender, RoutedEventArgs e)
        {
            se.Angle = se.Angle <= 0 ? 0 : se.Angle - 15;
            Debug.WriteLine(se.Angle);
            _preview.Settings = new Settings(se);
        }

        private void ButtonNew_Click(object sender, RoutedEventArgs e)
        {
            canvas.Children.Clear();
            _shapes.Clear();
            se = new Settings();
            st = new ScaleTransform();
            _isDrawing = false;
            _selectedShapeName = _prototypes.First().Value.Name;
            _preview = _prototypes[_selectedShapeName].Clone();
        }

        private void ButtonPrint_Click(object sender, RoutedEventArgs e)
        {
            PrintDialog printDialog = new PrintDialog();
            if(printDialog.ShowDialog() == true)
            {
                Size size = new Size(printDialog.PrintableAreaWidth, printDialog.PrintableAreaHeight);
                canvas.Measure(size);
                canvas.Arrange(new Rect(5,5,size.Width,size.Height));
                if(printDialog.ShowDialog() == true)
                {
                    printDialog.PrintVisual(canvas, "Print your image");
                }
            }
        }
    }
}
