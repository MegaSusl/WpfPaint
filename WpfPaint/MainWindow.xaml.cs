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
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using System.Drawing;
using Color = System.Windows.Media.Color;
using Pen = System.Drawing.Pen;
using Point = System.Drawing.Point;
using Microsoft.Win32;
using OpenFileDialog = Microsoft.Win32.OpenFileDialog;
using SaveFileDialog = Microsoft.Win32.SaveFileDialog;

namespace WpfPaint
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : System.Windows.Window
    {
        System.Windows.Forms.PictureBox pic;
        //System.Windows.Forms.ToolTip f_tp;
        //System.Windows.Controls.ToolTip w_tp;


        Bitmap bm;
        Graphics g;
        bool paint = false;
        System.Drawing.Point px, py;
        Pen p = new Pen(System.Drawing.Color.Black, 1);
        Pen erase = new Pen(System.Drawing.Color.White, 10);
        int index;
        int x, y, sX, sY, cX, cY;
        System.Drawing.Color new_color;

        public MainWindow()
        {
            InitializeComponent();
            SetSIze();
            pic = new PictureBox();
            pictureBoxHost.Child = pic;
            bm = new Bitmap(1600, 800);
            g = Graphics.FromImage(bm);
            g.Clear(System.Drawing.Color.White);
            pic.Image = bm;
            new_color = ColorTranslator.FromHtml(colorPick.SelectedColor.ToString());
        }

        private void SetSIze()
        {
            p.StartCap = System.Drawing.Drawing2D.LineCap.Round;
            p.EndCap = System.Drawing.Drawing2D.LineCap.Round;
            erase.StartCap = System.Drawing.Drawing2D.LineCap.Round;
            erase.EndCap = System.Drawing.Drawing2D.LineCap.Round;
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
            System.Windows.Application.Current.Shutdown();
        }
        #endregion
        #region oldShit
        private void PencilBtn_Click(object sender, RoutedEventArgs e)
        {
            index = 1;
            Sbox.Text = "Mode: Brush";
            //Canvas.EditingMode = InkCanvasEditingMode.Ink;
            //Canvas.DefaultDrawingAttributes = Pen;
            //Canvas.DefaultDrawingAttributes.Color = (Color)colorPick.SelectedColor;
        }

        private void EraserBtn_Click(object sender, RoutedEventArgs e)
        {
            index = 2;
            Sbox.Text = "Mode: Eraser";
            //Canvas.EditingMode = InkCanvasEditingMode.EraseByPoint;
            //Canvas.DefaultDrawingAttributes = Eraser1;
        }
        //private void EraserBtn2_Click(object sender, RoutedEventArgs e)
        //{
        //    Canvas.EditingMode = InkCanvasEditingMode.EraseByStroke;
        //    Canvas.DefaultDrawingAttributes = Eraser1;
        //}

        private void Canvas_ActiveEditingModeChanged(object sender, RoutedEventArgs e)
        {
            //Sbox.Text = "Mode: " + Canvas.EditingMode.ToString();
            //if (Canvas.EditingMode == InkCanvasEditingMode.EraseByPoint)
            //{
            //    SizeSilder.IsEnabled = false;
            //}
            //else SizeSilder.IsEnabled = true;
        }

        private void Slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            //System.Threading.Thread.Sleep(1000);
            if (IsLoaded)
            {
                p.Width = (float)((Slider)sender).Value;
                erase.Width= (float)((Slider)sender).Value;
                //Canvas.DefaultDrawingAttributes.Width = ((Slider)sender).Value;
                //Canvas.DefaultDrawingAttributes.Height = ((Slider)sender).Value;
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
            //Color? color = colorPick.SelectedColor;
            string color = colorPick.SelectedColor.ToString();
            
            var Color = ColorTranslator.FromHtml(color);
            if (IsLoaded && color != null)
            {

                new_color = Color;
                //pic_color.BackColor = Color;
                p.Color = Color;

                StatusColor.Text = $"Color: {color}";
            }

        }

        

        private void MenuItemNew_Click(object sender, RoutedEventArgs e)
        {
            g.Clear(System.Drawing.Color.White);
            pic.Image = bm;
            index = 0;
            Sbox.Text = "Mode: None";
        }

        private void RectBtn_Click(object sender, RoutedEventArgs e)
        {
            index = 4;
            Sbox.Text = "Mode: Rectangle";
        }
        private void EllipseBtn_Click(object sender, RoutedEventArgs e)
        {
            index = 3;
            Sbox.Text = "Mode: Ellipse";
        }

        private void LineBtn_Click(object sender, RoutedEventArgs e)
        {
            index = 5;
            Sbox.Text = "Mode: Line";
        }

        private void pic_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            Point start = py;
            if (paint)
            {
                if (index == 3)
                {
                    g.DrawEllipse(p, cX, cY, sX, sY);
                }

                if (index == 4)
                {
                    g.DrawRectangle(p, Math.Min(start.X, x), Math.Min(start.Y, y), Math.Abs(start.X - x), Math.Abs(start.Y - y));
                }

                if (index == 5)
                {
                    g.DrawLine(p, cX, cY, x, y);
                }
            }
        }
        
        #endregion
        private void Fill_Click(object sender, RoutedEventArgs e)
        {
            index = 7;
            Sbox.Text = "Mode: Fill";
        }

        private void pictureBoxHost_Loaded(object sender, RoutedEventArgs e)
        {

            pic.Paint += pic_Paint;
            pic.MouseClick += pic_MouseClick;
            pic.MouseDown += pic_MouseDown;
            pic.MouseUp += pic_MouseUp;
            pic.MouseMove += pic_MouseMove;
            
        }
        void picturebox_Paint(object sender, System.Windows.Forms.PaintEventArgs e)
        {
            
            Bitmap bmp = new Bitmap(200, 100);
            var ulpoint = new System.Drawing.Point(0, 0);


            g = Graphics.FromImage(bmp);
            g.Clear(System.Drawing.Color.Black);
            pic.Image = bmp;

        }
        private void pic_MouseMove(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if (paint)
            {
                if (index == 1)
                {
                    px = e.Location;
                    g.DrawLine(p, px, py);
                    py = px;
                }
                if (index == 2)
                {
                    px = e.Location;
                    g.DrawLine(erase, px, py);
                    py = px;
                }
            }
            pic.Refresh();

            x = e.X;
            y = e.Y;
            sX = e.X - cX;
            sY = e.Y - cY;
        }

        private void MenuItemSave_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog save = new SaveFileDialog();
            save.Filter = "Image Files(*.jpg; *.jpeg; *.gif; *.bmp)|*.jpg; *.jpeg; *.gif; *.bmp";
            if (save.ShowDialog() == true)
            {
                if (pic.Image != null)
                {
                    pic.Image.Save(save.FileName);
                }
            }
        }

        private void MenuItemOpen_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Image Files(*.jpg; *.jpeg; *.gif; *.bmp)|*.jpg; *.jpeg; *.gif; *.bmp";
            if (ofd.ShowDialog() == true)
            {
                if (pic.Image != null)
                {
                    bm = new Bitmap(ofd.FileName);
                    g = Graphics.FromImage(bm);
                    pic.Image = bm;
                }
            }
        }

        private void Info_Click(object sender, RoutedEventArgs e)
        {
            InfoWindow info = new InfoWindow();
            info.Show();
        }

        private void pic_MouseUp(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            paint = false;

            sX = x - cX;
            sY = y - cY;
            Point start = py;
            if (index == 3)
            {
                g.DrawEllipse(p, cX, cY, sX, sY);
            }

            if (index == 4)
            {
                //g.DrawRectangle(p, cX, cY, sX, sY);
                g.DrawRectangle(p, Math.Min(start.X, e.X), Math.Min(start.Y, e.Y), Math.Abs(start.X - e.X), Math.Abs(start.Y - e.Y));
            }

            if (index == 5)
            {
                g.DrawLine(p, cX, cY, x, y);
            }
        }
        
        

        static Point set_point(PictureBox pb, Point pt)
        {
            float pX = 1f * pb.Width / pb.Width;
            float pY = 1f * pb.Height / pb.Height;
            return new Point((int)(pt.X * pX), (int)(pt.Y * pY));
        }
        private void pic_MouseClick(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if (index == 7)
            {
                
                System.Drawing.Point point = set_point(pic, e.Location);
                Fill(bm, point.X, point.Y, new_color);
            }
        }
        public void Fill(Bitmap bm, int x, int y, System.Drawing.Color new_clr)
        {
            System.Drawing.Color old_color = bm.GetPixel(x, y);
            Stack<System.Drawing.Point> pixel = new Stack<Point>();
            pixel.Push(new Point(x, y));
            bm.SetPixel(x, y, new_clr);
            if (old_color == new_clr) return;

            while (pixel.Count > 0)
            {
                Point pt = (Point)pixel.Pop();
                if (pt.X > 0 && pt.Y > 0 && pt.X < bm.Width - 1 && pt.Y < bm.Height - 1)
                {
                    validate(bm, pixel, pt.X - 1, pt.Y, old_color, new_clr);
                    validate(bm, pixel, pt.X, pt.Y - 1, old_color, new_clr);
                    validate(bm, pixel, pt.X + 1, pt.Y, old_color, new_clr);
                    validate(bm, pixel, pt.X, pt.Y + 1, old_color, new_clr);
                }
            }
        }
        private void validate(Bitmap bm, Stack<Point> sp, int x, int y, System.Drawing.Color old_color, System.Drawing.Color new_color)
        {
            System.Drawing.Color cx = bm.GetPixel(x, y);
            if (cx == old_color)
            {
                sp.Push(new Point(x, y));
                bm.SetPixel(x, y, new_color);
            }
        }
        private void pic_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            paint = true;
            py = e.Location;

            cX = e.X;
            cY = e.Y;
        }
    }
}
