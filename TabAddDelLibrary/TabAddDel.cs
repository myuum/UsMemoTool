using System;
using System.Windows;
using System.Windows.Controls;

namespace TabAddDelLibrary
{
    /// <summary>
    /// Follow steps 1a or 1b and then 2 to use this custom control in a XAML file.
    ///
    /// Step 1a) Using this custom control in a XAML file that exists in the current project.
    /// Add this XmlNamespace attribute to the root element of the markup file where it is 
    /// to be used:
    ///
    ///     xmlns:MyNamespace="clr-namespace:TabAddDelLibrary"
    ///
    ///
    /// Step 1b) Using this custom control in a XAML file that exists in a different project.
    /// Add this XmlNamespace attribute to the root element of the markup file where it is 
    /// to be used:
    ///
    ///     xmlns:MyNamespace="clr-namespace:TabAddDelLibrary;assembly=TabAddDelLibrary"
    ///
    /// You will also need to add a project reference from the project where the XAML file lives
    /// to this project and Rebuild to avoid compilation errors:
    ///
    ///     Right click on the target project in the Solution Explorer and
    ///     "Add Reference"->"Projects"->[Select this project]
    ///
    ///
    /// Step 2)
    /// Go ahead and use your control in the XAML file.
    ///
    ///     <MyNamespace:CustomControl1/>
    ///
    /// </summary>
    public class TabAddDel : TabControl
    {
        static TabAddDel()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(TabAddDel), new FrameworkPropertyMetadata(typeof(TabAddDel)));
        }

        /// <summary>
        /// タブ追加したときのイベント
        /// </summary>
        public static readonly RoutedEvent AddTabSettingEvent =
            EventManager.RegisterRoutedEvent(
                "AddTabSetting",
                RoutingStrategy.Bubble,
                typeof(RoutedEventHandler),
                typeof(TabAddDel)
            );

        public event RoutedEventHandler AddTabSetting
        {
            add { AddHandler(AddTabSettingEvent, value); }
            remove { RemoveHandler(AddTabSettingEvent, value); }
        }

        /// <summary>
        /// タブ追加したときのイベントのパラメータ
        /// </summary>
        public class AddTabSettingEventArgs : RoutedEventArgs
        {
            public TabItem item { get; set; }

            public AddTabSettingEventArgs(RoutedEvent routedEvent) : base(routedEvent) { }
        }

        void RaiseAddTabSettingEvent(TabItem item)
        {
            AddTabSettingEventArgs newEventArgs = new AddTabSettingEventArgs(AddTabSettingEvent);
            newEventArgs.item = item;
            RaiseEvent(newEventArgs);
        }

        /// <summary>
        /// タブのタイトル
        /// </summary>
        public static readonly DependencyProperty TitleProperty =
            DependencyProperty.Register(
                "Title",
                typeof(String),
                typeof(TabAddDel),
                new PropertyMetadata("")
            );

        public static string GetTitle(DependencyObject obj)
        {
            return (string)obj.GetValue(TitleProperty);
        }

        public static void SetTitle(DependencyObject obj, string value)
        {
            obj.SetValue(TitleProperty, value);
        }

        /// <summary>
        /// コントロールの初期化後の処理
        /// </summary>
        public override void EndInit()
        {
            base.EndInit();

            // 追加ボタン
            TabItem itemAdd = new TabItem();
            itemAdd.Header = "＋";
            Items.Add(itemAdd);

            // 最初のタブを追加
            AddTabItemCustom(Visibility.Hidden);

            // イベントを登録
            SelectionChanged += SelectedAddDelTab;

        }

        /// <summary>
        /// タブの切り替え時
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SelectedAddDelTab(object sender, SelectionChangedEventArgs e)
        {
            // 追加ボタン(＋)を選択する
            if (SelectedIndex == Items.Count - 1)
            {
                // 最初のタブを追加
                AddTabItemCustom(Visibility.Visible);

                // 追加したタブを選択状態にする
                SelectedIndex = Items.Count - 2;

                // 追加したタブにフォーカスを設定する
                ((TabItem)(Items[SelectedIndex])).Focus();

                e.Handled = true;
            }
        }

        /// <summary>
        /// 削除ボタンをクリック
        /// </summary>
        /// <param name="item">削除ボタンクリックしたアイテム</param>
        public void ClickDelButton(TabItem item)
        {
            // インデックスを1つ戻す
            SelectedIndex--;

            // タブを削除する
            Items.Remove(item);

            // タブのタイトルを再設定する
            for (int i = 1; i < Items.Count - 1; i++)
            {
                ((HeaderUserControl)((TabItem)Items[i]).Header).label.Content = GetTitle(this) + (i + 1);
            }
        }

        /// <summary>
        /// タブの追加
        /// </summary>
        /// <param name="visibility">削除ボタンの表示状態</param>
        private void AddTabItemCustom(Visibility visibility)
        {
            // タブを作成し、追加ボタンの前に追加
            TabItem item = new TabItem();
            item.Header = new HeaderUserControl(GetTitle(this) + Items.Count, visibility);
            Items.Insert(Items.Count - 1, item);

            // イベントハンドラの呼び出し
            if (AddTabSettingEvent != null)
            {
                RaiseAddTabSettingEvent(item);
            }
        }
    }
}