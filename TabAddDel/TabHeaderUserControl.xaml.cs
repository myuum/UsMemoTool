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

namespace TabAddDel
{
	/// <summary>
	/// TabHeaderUserControl.xaml の相互作用ロジック
	/// </summary>
	public partial class TabHeaderUserControl : UserControl
	{
		public TabHeaderUserControl()
		{
			InitializeComponent();
		}
		public TabHeaderUserControl(string labelName, Visibility visibility = Visibility.Visible)
		{
			InitializeComponent();
			label.Text = labelName;
			delBtn.Visibility = visibility;
			delBtn.Click += DelBtn_Click;
		}

		private void DelBtn_Click(object sender, RoutedEventArgs e)
		{
			TabItem item = (TabItem)this.Parent;
			TabControl tabControl = (TabControl)item.Parent;
			// 1つ前のタブを選択する
			tabControl.SelectedIndex--;
			// タブを削除する
			tabControl.Items.Remove(item);
		}
	}
}
