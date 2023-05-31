using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WpfPaint
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private readonly DrawingAttributes Pen = new DrawingAttributes()
        {
            Color = Colors.Red,
            Height = 2,
            Width = 2
        };
        private readonly DrawingAttributes Eraser1 = new DrawingAttributes()
        {
            Height = 50,
            Width = 50
        };
        private void RowDefinition_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed) { this.DragMove(); }
        }

        private void Canvas_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {

        }

        private void minimizeBtn_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void closeBtn_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void PencilBtn_Click(object sender, RoutedEventArgs e)
        {
            Canvas.EditingMode = InkCanvasEditingMode.Ink;
            Canvas.DefaultDrawingAttributes = Pen;
        }

        private void EraserBtn_Click(object sender, RoutedEventArgs e)
        {
            Canvas.EditingMode = InkCanvasEditingMode.EraseByPoint;
            Canvas.DefaultDrawingAttributes = Eraser1;
        }
    }
}
