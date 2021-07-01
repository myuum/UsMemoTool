using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;

namespace TabAddDel
{
    public class AddDelTabCustomControl : TabControl, ISupportInitialize
    {
        static AddDelTabCustomControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(AddDelTabCustomControl), new FrameworkPropertyMetadata(typeof(AddDelTabCustomControl)));
        }

        /// <summary>
        /// タブ追加したときのイベント
        /// </summary>
        public static readonly RoutedEvent AddTabSettingEvent =
            EventManager.RegisterRoutedEvent(
                "AddTabSetting",
                RoutingStrategy.Bubble,
                typeof(RoutedEventHandler),
                typeof(AddDelTabCustomControl)
            );

        public event RoutedEventHandler AddTabSetting
        {
            add { AddHandler(AddTabSettingEvent, value); }
            remove { RemoveHandler(AddTabSettingEvent, value); }
        }
        /// <summary>
        /// タブ削除したときのイベント
        /// </summary>
        public static readonly RoutedEvent DelTabSettingEvent =
            EventManager.RegisterRoutedEvent(
                "DelTabSetting",
                RoutingStrategy.Bubble,
                typeof(RoutedEventHandler),
                typeof(AddDelTabCustomControl)
            );
        public event RoutedEventHandler DelTabSetting
        {
            add { AddHandler(AddTabSettingEvent, value); }
            remove { RemoveHandler(AddTabSettingEvent, value); }
        }

        /// <summary>
        /// タブ追加したときのイベントのパラメータ
        /// </summary>
        public class TabEventArgs : RoutedEventArgs
        {
            public TabItem item { get; set; }
            public TabEventArgs(RoutedEvent routedEvent) : base(routedEvent) { }
        }

        void RaiseAddTabSettingEvent(TabItem item)
        {
            TabEventArgs newEventArgs = new TabEventArgs(AddTabSettingEvent);
            newEventArgs.item = item;
            RaiseEvent(newEventArgs);
        }
        void RaiseDelTabSettingEvent(TabItem item)
        {
            TabEventArgs newEventArgs = new TabEventArgs(DelTabSettingEvent);
            newEventArgs.item = item;
            RaiseEvent(newEventArgs);
        }
        /// <summary>
        /// タブのタイトル
        /// </summary>
        public static readonly DependencyProperty TitleProperty =
            DependencyProperty.Register(
                "Title",
                typeof(string),
                typeof(AddDelTabCustomControl),
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
            AddTabItemCustom(Visibility.Collapsed);

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
                AddTabItemCustom(Visibility.Visible);

                // 追加したタブを選択状態にする
                SelectedIndex = Items.Count - 2;

                // 追加したタブにフォーカスを設定する
                ((TabItem)(Items[SelectedIndex])).Focus();

                e.Handled = true;
            }
        }
        public void TabAllClear()
		{
            var delItems = new List<TabItem>();
            foreach(var obj in Items)
			{
                if(obj is TabItem item)
                if(item.Header is TabHeaderUserControl header)
				{
                    if (header.delBtn.Visibility != Visibility.Visible) continue;
                        delItems.Add(item);
                }
			}
            SelectedIndex = 0;
            foreach (var item in delItems)
                Items.Remove(item);
        }

        /// <summary>
        /// 削除ボタンをクリック
        /// </summary>
        /// <param name="item">削除ボタンクリックしたアイテム</param>
        public void ClickDelButton(TabItem item)
        {
            // インデックスを1つ戻す
            SelectedIndex--;
            // イベントハンドラの呼び出し
            if (DelTabSettingEvent != null)
            {
                RaiseDelTabSettingEvent(item);
            }
            // タブを削除する
            Items.Remove(item);
        }

        /// <summary>
        /// タブの追加
        /// </summary>

        /// <param name="visibility">削除ボタンの表示状態</param>
        private void AddTabItemCustom(Visibility visibility)
        {
            // タブを作成し、追加ボタンの前に追加
            TabItem item = new TabItem();
            item.Header = new TabHeaderUserControl(GetTitle(this) + Items.Count, visibility);
            Items.Insert(Items.Count - 1, item);
            // イベントハンドラの呼び出し
            if (AddTabSettingEvent != null)
            {
                RaiseAddTabSettingEvent(item);
            }
        }

    }
}
