using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;
using UsMemoTool.Data.Crew;
using System.Windows.Controls;
using System;
using UsMemoTool.Utility;
using static TabAddDel.AddDelTabCustomControl;
using UsMemoTool.Data.Map;

namespace UsMemoTool
{
    /// <summary>
    /// MainWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class MainWindow : Window
    {
        MainViewModel viewModel = new MainViewModel();

        private static readonly Dictionary<PlayerColor, Color> ColorMapping = new Dictionary<PlayerColor, Color>()
        {
            { PlayerColor.Red, Color.FromRgb(197, 17, 17) },
            { PlayerColor.Blue, Color.FromRgb(19, 46, 209) },
            { PlayerColor.Green, Color.FromRgb(17, 127, 17) },
            { PlayerColor.Pink, Color.FromRgb(237, 84, 186) },
            { PlayerColor.Orange, Color.FromRgb(239, 125, 13) },
            { PlayerColor.Yellow, Color.FromRgb(245, 245, 87) },
            { PlayerColor.Black, Color.FromRgb(20, 20, 20) },
            { PlayerColor.White, Color.FromRgb(214, 224, 240) },
            { PlayerColor.Purple, Color.FromRgb(107, 47, 187) },
            { PlayerColor.Brown, Color.FromRgb(113, 73, 30) },
            { PlayerColor.Cyan, Color.FromRgb(56, 254, 220) },
            { PlayerColor.Lime,    Color.FromRgb(80, 239, 57) },
            { PlayerColor.Maroon,  Color.FromRgb(95, 29, 46)},
            { PlayerColor.Rose,    Color.FromRgb(236, 192, 211)},
            { PlayerColor.Banana,  Color.FromRgb(240, 231, 168)},
            { PlayerColor.Gray,    Color.FromRgb(117, 133, 147)},
            { PlayerColor.Tan,     Color.FromRgb(145, 136, 119)},
            { PlayerColor.Coral,  Color.FromRgb(215, 100, 100)}

        };
        public MainWindow()
        {
            InitializeComponent();
            DataContext = viewModel;
            viewModel.InkColor = viewModel.InkColor = ColorMapping[PlayerColor.Black];
            MapBox.SelectionChanged += (sender, e) =>
            {
                viewModel.Map = MapBox.SelectedItem as UsMap;
            };
            foreach (var child in LogicalTreeHelper.GetChildren(grid))
            {
                Console.WriteLine(child.ToString());
                if (child is CrewControl)
                {
                    ((CrewControl)child).IconButtonDown += (sender, e) =>
                    {
                        var crew = sender as Crew;
                        viewModel.CurrntMap.InkColor = CrewToColor(crew);
                        Console.WriteLine("IconButtonDown");
                    };
                    ((CrewControl)child).IconButtonUp += (sender, e) =>
                    {
                        var crew = sender as Crew;
                        Console.WriteLine("IconButtonUp");
                    };
                }
            }
            Tab.SelectionChanged += Tab_SelectionChanged;
        }

		private void Tab_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
            viewModel.CurrntMap = Tab.SelectedContent as MapControl; 
		}
		private void Tab_AddTabSetting(object sender, RoutedEventArgs e)
		{
            var map = new MapControl(viewModel.Map);
            var item = (e as TabEventArgs).item;
            item.Content = map;
        }

		private Color CrewToColor(Crew crew) => ColorMapping[crew.Color];

		private void Hyperlink_RequestNavigate(object sender, System.Windows.Navigation.RequestNavigateEventArgs e)
		{
            System.Diagnostics.Process.Start(e.Uri.AbsoluteUri);
        }
	}
}
