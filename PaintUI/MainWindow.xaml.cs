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
        IShape _preview;
        string _selectedShapeName = "";
        Dictionary<string, IShape> _prototypes = new Dictionary<string, IShape>();
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

            // Ve lai Xoa toan bo
            canvas.Children.Clear();

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
                _preview = _prototypes[_selectedShapeName];
            }
            
        }

        private void ButtonSave_Click(object sender, RoutedEventArgs e)
        {

            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Title = "Save a binary file";
            saveFileDialog.Filter = "Binary File | *.dat";
            saveFileDialog.ShowDialog();
            if(saveFileDialog.FileName != "")
            {
                String fileName = saveFileDialog.FileName;
                FileStream fs = new FileStream(fileName, FileMode.Create);
                BinaryFormatter bf = new BinaryFormatter();
            }


        }
    }
}
