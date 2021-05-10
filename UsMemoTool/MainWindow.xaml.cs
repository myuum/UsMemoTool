using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;
using UsMemoTool.Data.Crew;
using System.Windows.Controls;
using System;
using UsMemoTool.Utility;

namespace UsMemoTool
{
    /// <summary>
    /// MainWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class MainWindow : Window
    {
        MainViewModel viewModel = new MainViewModel();
        ImageBrush imageBrush = new ImageBrush();
        FrameworkElement DragUI = null;

        private static readonly Dictionary<PlayerColor, Color> ColorMapping = new Dictionary<PlayerColor, Color>()
        {
            { PlayerColor.Red, Color.FromRgb(197, 17, 17) },
            { PlayerColor.Blue, Color.FromRgb(19, 46, 209) },
            { PlayerColor.Green, Color.FromRgb(17, 127, 17) },
            { PlayerColor.Pink, Color.FromRgb(237, 84, 186) },
            { PlayerColor.Orange, Color.FromRgb(239, 125, 13) },
            { PlayerColor.Yellow, Color.FromRgb(245, 245, 87) },
            { PlayerColor.Black, Color.FromRgb(0, 0, 0) },
            { PlayerColor.White, Color.FromRgb(214, 224, 240) },
            { PlayerColor.Purple, Color.FromRgb(107, 47, 187) },
            { PlayerColor.Brown, Color.FromRgb(113, 73, 30) },
            { PlayerColor.Cyan, Color.FromRgb(56, 254, 220) },
            { PlayerColor.Lime, Color.FromRgb(80, 239, 57) }

        };
        public MainWindow()
        {
            InitializeComponent();
            DataContext = viewModel;
            MapBox.SelectionChanged += (sender, e) =>
            {
                SetCanvasImage();
            };
            foreach (var child in LogicalTreeHelper.GetChildren(grid))
            {
                Console.WriteLine(child.ToString());
                if (child is CrewControl)
                {
                    ((CrewControl)child).IconClick += (sender, e) =>
                    {
                        var crew = sender as Crew;
                        MainCanvas.DefaultDrawingAttributes.Color = CrewToColor(crew);
                    };
                }
            }
            
        }

		public void SetCanvasImage()
        {
            if (viewModel == null) return;
            imageBrush.ImageSource = viewModel.Map.Image;
            this.MainCanvas.Background = imageBrush;
            MainCanvas.StrokeCollected += (object sender, InkCanvasStrokeCollectedEventArgs e) =>
            {
                var stroke = e.Stroke;
                var rect = new Rect(0, 0, MainCanvas.ActualWidth, MainCanvas.ActualHeight);
                var result = stroke.GetClipResult(rect);
                viewModel.InkStrokes.Remove(stroke);
                viewModel.InkStrokes.Add(result);
            };
            
            MainCanvas.DragOver += (object sender, DragEventArgs e) =>
            {
                var element = sender as FrameworkElement;
                if (element == null) return;
                var mousePoint = e.GetPosition(canvas);
                if (DragUI == null)
				{
                    if (e.Data.GetData("System.String") is string url)
					{
                        if (!url.StartsWith("pack://application:,,,/Resource/")) return;
                        DragUI = new Image()
                        {
                            Source = new System.Windows.Media.Imaging.BitmapImage(new Uri(url)),
                            Width = 20,
                            Height = 20,
                            HorizontalAlignment = HorizontalAlignment.Left,
                            VerticalAlignment = VerticalAlignment.Top,
                            IsManipulationEnabled = false
                        };
                        DragUI.MouseLeftButtonDown += DragUI_MouseLeftButtonDown;
                        DragUI.MouseMove += DragUI_MouseMove;
                        DragUI.MouseLeftButtonUp += DragUI_MouseLeftButtonUp;
                        DragUI.MouseRightButtonUp += DragUI_MouseRightButtonUp;
                        canvas.Children.Add(DragUI);
                        SetMousePoint(DragUI, e.GetPosition(canvas));
                    }   
                }
                else
				{
                    SetMousePoint(DragUI, e.GetPosition(canvas));
                    Console.WriteLine("DragOver");
                }
                
            };
            canvas.DragLeave += (object sender, DragEventArgs e) =>
            {
                //canvas.Children.Remove(DragUI);
                DragUI = null;
                Console.WriteLine("DragLeave:" +e.Source.ToString());
            };
            canvas.Drop += (object sender, DragEventArgs e) =>
            {
                Console.WriteLine("Drop");
                var index = canvas.Children.IndexOf(DragUI);
                if(canvas.Children[index] is Image image)
				{
                    image.IsManipulationEnabled = true;
                }
                DragUI = null;
            };
        }

		private void DragUI_MouseRightButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
		{
            Console.WriteLine("DragUI_MouseRightButtonUp");
            if (sender is Image uIElement)
            {
                canvas.Children.Remove(uIElement);
            }
		}

		private void DragUI_MouseLeftButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
		{
            Console.WriteLine("DragUI_MouseRightButtonUp");
            DragUI = null;
        }

        private void DragUI_MouseMove(object sender, System.Windows.Input.MouseEventArgs e)
        {
            SetMousePoint(DragUI, e.GetPosition(canvas));
        }

		private void DragUI_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
		{
            Console.WriteLine("DragUI_MouseLeftButtonDown");
            if (sender is Image uIElement)
            {
                DragUI = uIElement;
            }
        }

		private Color CrewToColor(Crew crew) => ColorMapping[crew.Color];
        private void SetMousePoint(FrameworkElement ui,Point mousePoint)
		{
            if (ui == null) return;
            var pos = mousePoint - new Point(ui.ActualWidth * 0.5 , ui.ActualHeight * 0.5);
            Console.WriteLine("Postion: "+pos.ToString());
            Canvas.SetLeft(ui, pos.X); 
            Canvas.SetTop(ui, pos.Y);

        }
        bool isClear = false;
        private void Button_Click(object sender, RoutedEventArgs e)
		{
            if(sender is Button btn)
			{
                isClear = !isClear;
                MainCanvas.EditingMode = isClear ? InkCanvasEditingMode.EraseByStroke : InkCanvasEditingMode.Ink;
                btn.Content = isClear ? "消去モード" : "描画モード";
            }
        }
		private void AllClear_Click(object sender, RoutedEventArgs e)
		{
            viewModel.InkStrokes.Clear();
            canvas.Children.Clear();
		}
	}
}
