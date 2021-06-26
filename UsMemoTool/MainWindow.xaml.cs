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
        public MainWindow()
        {
            InitializeComponent();
            DataContext = viewModel;
            viewModel.InkColor = viewModel.InkColor = Crew.ColorMapping[PlayerColor.Black];
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

		private Color CrewToColor(Crew crew) => Crew.ColorMapping[crew.Color];

		private void Hyperlink_RequestNavigate(object sender, System.Windows.Navigation.RequestNavigateEventArgs e)
		{
            System.Diagnostics.Process.Start(e.Uri.AbsoluteUri);
        }
	}
}
