using Microsoft.Win32;
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
using System.Windows.Shapes;

namespace InkSample
{
    /// <summary>
    /// MainWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class MainWindow : Window
    {
        private ViewModel viewModel;
        private System.Windows.Point iniP;
        public MainWindow()
        {
            InitializeComponent();

            DrawingAttributes drawingAttributes = new DrawingAttributes
            {
                Color = Colors.Red,
                Width = 2,
                Height = 2,
                StylusTip = StylusTip.Rectangle,
                //FitToCurve = true,
                IsHighlighter = false,
                IgnorePressure = true,

            };
            inkCanvasMeasure.DefaultDrawingAttributes = drawingAttributes;

            viewModel = new ViewModel
            {
                MeaInfo = "テスト·····",
                InkStrokes = new StrokeCollection(),
            };

            DataContext = viewModel;
        }

        private void OpenFile_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openDialog = new OpenFileDialog
            {
                Filter = "Image Files (*.jpg)|*.jpg|Image Files (*.png)|*.png|Image Files (*.bmp)|*.bmp",
                Title = "Open Image File"
            };
            if (openDialog.ShowDialog() == true)
            {
                BitmapImage image = new BitmapImage();
                image.BeginInit();
                image.UriSource = new Uri(openDialog.FileName, UriKind.RelativeOrAbsolute);
                image.EndInit();
                imgMeasure.Source = image;
            }
        }

        private void DrawSquare_Click(object sender, RoutedEventArgs e)
        {
            if (btnSquare.IsChecked == true)
            {
                btnEllipse.IsChecked = false;
            }
        }

        private void DrawEllipse_Click(object sender, RoutedEventArgs e)
        {
            if (btnEllipse.IsChecked == true)
            {
                btnSquare.IsChecked = false;
            }
        }

        private List<System.Windows.Point> GenerateEclipseGeometry(System.Windows.Point st, System.Windows.Point ed)
        {
            double a = 0.5 * (ed.X - st.X);
            double b = 0.5 * (ed.Y - st.Y);
            List<System.Windows.Point> pointList = new List<System.Windows.Point>();
            for (double r = 0; r <= 2 * Math.PI; r = r + 0.01)
            {
                pointList.Add(new System.Windows.Point(0.5 * (st.X + ed.X) + a * Math.Cos(r), 0.5 * (st.Y + ed.Y) + b * Math.Sin(r)));
            }
            return pointList;
        }
        private void InkCanvasMeasure_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                iniP = e.GetPosition(inkCanvasMeasure);
            }
        }

        private void InkCanvasMeasure_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                // Draw square
                if (btnSquare.IsChecked == true)
                {
                    System.Windows.Point endP = e.GetPosition(inkCanvasMeasure);
                    List<System.Windows.Point> pointList = new List<System.Windows.Point>
                    {
                        new System.Windows.Point(iniP.X, iniP.Y),
                        new System.Windows.Point(iniP.X, endP.Y),
                        new System.Windows.Point(endP.X, endP.Y),
                        new System.Windows.Point(endP.X, iniP.Y),
                        new System.Windows.Point(iniP.X, iniP.Y),
                    };
                    StylusPointCollection point = new StylusPointCollection(pointList);
                    Stroke stroke = new Stroke(point)
                    {
                        DrawingAttributes = inkCanvasMeasure.DefaultDrawingAttributes.Clone()
                    };
                    viewModel.InkStrokes.Clear();
                    viewModel.InkStrokes.Add(stroke);
                }
                // Draw Eclipse
                else if (btnEllipse.IsChecked == true)
                {
                    System.Windows.Point endP = e.GetPosition(inkCanvasMeasure);
                    List<System.Windows.Point> pointList = GenerateEclipseGeometry(iniP, endP);
                    StylusPointCollection point = new StylusPointCollection(pointList);
                    Stroke stroke = new Stroke(point)
                    {
                        DrawingAttributes = inkCanvasMeasure.DefaultDrawingAttributes.Clone()
                    };
                    viewModel.InkStrokes.Clear();
                    viewModel.InkStrokes.Add(stroke);
                }
            }
        }
    }
}