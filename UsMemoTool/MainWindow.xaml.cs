using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;
using UsMemoTool.Data.Crew;
using System.Windows.Controls;
using System;

namespace UsMemoTool
{
    /// <summary>
    /// MainWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class MainWindow : Window
    {
        MainViewModel viewModel = new MainViewModel();
        ImageBrush imageBrush = new ImageBrush();
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
            SetCanvasImage();
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
            MainCanvas.StrokeCollected += (object sender, InkCanvasStrokeCollectedEventArgs e) =>
            {
                viewModel.InkStroke = e.Stroke;
            };
        }

		public void SetCanvasImage()
        {
            if (viewModel == null) return;
            imageBrush.ImageSource = viewModel.Map.Image;
            this.MainCanvas.Background = imageBrush;
        }
        private Color CrewToColor(Crew crew) => ColorMapping[crew.Color];

		private void Button_Click(object sender, RoutedEventArgs e)
		{
            Console.WriteLine(MainCanvas.Strokes);
        }
	}
}
