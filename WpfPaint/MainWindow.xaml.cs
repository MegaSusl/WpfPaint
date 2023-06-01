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
using Xceed.Wpf.Toolkit;

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
            Width = 2,
        };
        private readonly DrawingAttributes Eraser1 = new DrawingAttributes()
        {
            Height = 50,
            Width = 50
        };
        #region smth
        private void RowDefinition_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed) { this.DragMove(); }
        }

        private void Canvas_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {

        }

        private void minimizeBtn_Click(object sender, RoutedEventArgs e)
        {
            WindowState = System.Windows.WindowState.Minimized;
        }

        private void closeBtn_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
        #endregion
        private void PencilBtn_Click(object sender, RoutedEventArgs e)
        {
            Canvas.EditingMode = InkCanvasEditingMode.Ink;
            Canvas.DefaultDrawingAttributes = Pen;
            Canvas.DefaultDrawingAttributes.Color = (Color)colorPick.SelectedColor;
        }

        private void EraserBtn_Click(object sender, RoutedEventArgs e)
        {
            Canvas.EditingMode = InkCanvasEditingMode.EraseByPoint;
            Canvas.DefaultDrawingAttributes = Eraser1;
        }
        private void EraserBtn2_Click(object sender, RoutedEventArgs e)
        {
            Canvas.EditingMode = InkCanvasEditingMode.EraseByStroke;
            Canvas.DefaultDrawingAttributes = Eraser1;
        }

        private void Canvas_ActiveEditingModeChanged(object sender, RoutedEventArgs e)
        {
            Sbox.Text = "Mode: " + Canvas.EditingMode.ToString();
            if (Canvas.EditingMode == InkCanvasEditingMode.EraseByPoint)
            {
                SizeSilder.IsEnabled = false;
            }
            else SizeSilder.IsEnabled = true;
        }

        private void Slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            //System.Threading.Thread.Sleep(1000);
            if (IsLoaded)
            {
                Canvas.DefaultDrawingAttributes.Width = ((Slider)sender).Value;
                Canvas.DefaultDrawingAttributes.Height = ((Slider)sender).Value;
            }   
            
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Canvas.DefaultDrawingAttributes.Color = Colors.Black;
            Canvas.DefaultDrawingAttributes.Width = 3;
            Canvas.DefaultDrawingAttributes.Height = 3;
           

        }

        private void ColorPicker_SelectedColorChanged(object sender, RoutedPropertyChangedEventArgs<Color?> e)
        {
            Color? color= colorPick.SelectedColor;
            if (IsLoaded && color != null) 
            {
                Canvas.DefaultDrawingAttributes.Color = (Color)color;
            }
            
        }

        private void MenuItemNew_Click(object sender, RoutedEventArgs e)
        {
            Canvas.Strokes.Clear();
        }

        private void RectBtn_Click(object sender, RoutedEventArgs e)
        {
            Canvas.EditingMode = InkCanvasEditingMode.None;
            RectDraw();
        }
        private void RectDraw()
        {
            //if (MouseButtonState)
            //{

            //}
            Ellipse e1 = new Ellipse();
            e1.Width = 10;
            e1.Height = 10;
            var brush = new SolidColorBrush();
            brush.Color = Color.FromArgb(100, 0, 0, 0);
            e1.Stroke = brush;
            e1.StrokeThickness = 4;
            Canvas.Children.Add(e1);
        }

        private void Fill_Click(object sender, RoutedEventArgs e)
        {
            if (Fill.IsChecked == true)
            {
                Point GetMousePos() => myWindow.PointToScreen(Mouse.GetPosition(myWindow));
                Point why = GetMousePos();
                FloodFill(why, Colors.Black);
            }
            
        }

        private void FloodFill(Point startPoint, Color fillColor)
        {
            // Get the color of the starting point
            Color startColor = GetColorAtPoint(startPoint);

            // Create a queue for the points to be filled
            Queue<Point> fillQueue = new Queue<Point>();
            fillQueue.Enqueue(startPoint);

            while (fillQueue.Count > 0)
            {
                // Dequeue the next point and set its color to the fill color
                Point currentPoint = fillQueue.Dequeue();
                SetColorAtPoint(currentPoint, fillColor);

                // Check the neighboring points
                Point[] neighbors = new Point[]
                {
            new Point(currentPoint.X + 1, currentPoint.Y),
            new Point(currentPoint.X - 1, currentPoint.Y),
            new Point(currentPoint.X, currentPoint.Y + 1),
            new Point(currentPoint.X, currentPoint.Y - 1)
                };

                foreach (Point neighbor in neighbors)
                {
                    // Check if the neighbor is within the bounds of the ink canvas
                    if (IsPointWithinBounds(neighbor))
                    {
                        // Check if the neighbor has the same color as the starting point
                        Color neighborColor = GetColorAtPoint(neighbor);
                        if (neighborColor == startColor)
                        {
                            fillQueue.Enqueue(neighbor);
                        }
                    }
                }
            }
        }

        private Color GetColorAtPoint(Point point)
        {
            Point inkCanvasPoint = Canvas.PointFromScreen(point);
            StylusPointCollection stylusPoints = new StylusPointCollection(new Point[] { inkCanvasPoint });

            //Stroke hitStroke = Canvas.Strokes.HitTest(stylusPoints);

            //return hitStroke?.DrawingAttributes.Color ?? Colors.Transparent;

            return (Color)colorPick.SelectedColor;
        }

        private void SetColorAtPoint(Point point, Color color)
        {
            Point inkCanvasPoint = Canvas.PointFromScreen(point);
            StylusPointCollection stylusPoints = new StylusPointCollection(new Point[] { inkCanvasPoint });
            IEnumerable<Point> points = stylusPoints.Select(sp => new Point(sp.X, sp.Y));
            //Stroke hitStroke = Canvas.Strokes.HitTest(points, 1);

            //if (hitStroke != null)
            //{
            //    hitStroke.DrawingAttributes.Color = color;
            //}
        }

        private bool IsPointWithinBounds(Point point)
        {
            return point.X >= 0 && point.Y >= 0 && point.X < Canvas.ActualWidth && point.Y < Canvas.ActualHeight;
        }
    }
}
