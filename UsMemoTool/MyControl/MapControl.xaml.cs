using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using UsMemoTool.Data.Map;
using UsMemoTool.MyControl;

namespace UsMemoTool
{
	/// <summary>
	/// MapControl.xaml の相互作用ロジック
	/// </summary>
	public partial class MapControl : UserControl
	{
        public Color InkColor
		{
            get => viewModel.InkColor;
            set => viewModel.InkColor = value;
		}
        public static readonly DependencyProperty InkColorProperty =
        DependencyProperty.Register(
        "InkColor",
        typeof(Color),
        typeof(MapControl));
        public UsMap Map
		{
            get => viewModel.Map;
            set
            {
                viewModel.Map = value;
                imageBrush.ImageSource = viewModel.Map.Image;
            }
        }
        public static readonly DependencyProperty MapProperty =
        DependencyProperty.Register(
        "Map",
        typeof(UsMap),
        typeof(MapControl));
        MapViewModel viewModel = new MapViewModel();
        ImageBrush imageBrush = new ImageBrush();
        Image DragUI = null;
        public MapControl()
		{
			init(UsMap.TheSkeld);
		}
		public MapControl(UsMap map)
		{
			init(map);
			
		}
		public void init(UsMap map)
		{
			InitializeComponent();
			DataContext = viewModel;
            viewModel.Map = map;
            SetCanvasImage();

        }
        public void Undo() => viewModel.UndoBody();
		bool isErasure = false;
		private void Button_Click(object sender, RoutedEventArgs e)
		{
			if (sender is Button btn)
			{
				isErasure = !isErasure;
				MainCanvas.EditingMode = isErasure ? InkCanvasEditingMode.EraseByStroke : InkCanvasEditingMode.Ink;
				btn.Content = isErasure ? "消去モード" : "描画モード";
			}
		}
		private void AllClear_Click(object sender, RoutedEventArgs e)
		{
			viewModel.InkStrokes.Clear();
			canvas.Children.Clear();
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
            inkGird.PreviewMouseLeftButtonUp += (sender, e) =>
            {
                if (DragUI == null) return;
                DragUI.IsHitTestVisible = true;
                DragUI = null;
            };
            MainCanvas.DragOver += (object sender, DragEventArgs e) =>
            {
                var element = sender as FrameworkElement;
                if (element == null) return;
                var mousePoint = e.GetPosition(element);
                if (DragUI == null)
                {
                    if (e.Data.GetData("System.String") is string url)
                    {
                        if (!url.StartsWith("pack://application:,,,/Resource/")) return;
                        DragUI = new Image()
                        {
                            Source = new BitmapImage(new Uri(url)),
                            Width = 20,
                            Height = 20,
                            HorizontalAlignment = HorizontalAlignment.Left,
                            VerticalAlignment = VerticalAlignment.Top,
                            IsHitTestVisible = true,
                            AllowDrop = true
                        };
                        
                        DragUI.MouseLeftButtonDown += DragUI_MouseLeftButtonDown;
                        DragUI.MouseRightButtonUp += DragUI_MouseRightButtonUp;
                        canvas.Children.Add(DragUI);
                    }
                }
                else
                {
                    Console.WriteLine();
                    Console.WriteLine("DragOver: "+mousePoint.ToString());
                    SetMousePoint(DragUI, mousePoint);
                }
            };
            /*
            canvas.DragLeave += (object sender, DragEventArgs e) =>
            {
                canvas.Children.Remove(DragUI);
                DragUI = null;
                
            };
            */
            canvas.PreviewDrop += (object sender, DragEventArgs e) =>
            {
                Console.WriteLine("Drop");
                DragUI = null;
            };

        }
        private void SetMousePoint(FrameworkElement ui, Point mousePoint)
        {
            if (ui == null) return;
            var pos = mousePoint - new Point(ui.ActualWidth * 0.5, ui.ActualHeight * 0.5);
            Console.WriteLine("Postion: " + mousePoint.ToString());
            Canvas.SetLeft(ui, pos.X);
            Canvas.SetTop(ui, pos.Y);

        }
        private void DragUI_MouseRightButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            Console.WriteLine("DragUI_MouseRightButtonUp");
            if (sender is Image uIElement)
            {
                canvas.Children.Remove(uIElement);
            }
        }
        private void DragUI_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            Console.WriteLine("DragUI_MouseLeftButtonDown");
            if (sender is Image uIElement)
            {
                DragUI = uIElement;
                DragDrop.DoDragDrop(DragUI,
                    uIElement.Source.ToString(),
                    DragDropEffects.Copy);
            }
        }
    }
}
