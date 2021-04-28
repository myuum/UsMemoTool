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

namespace TabAddDelCustomControl
{
    /// <summary>
    /// このカスタム コントロールを XAML ファイルで使用するには、手順 1a または 1b の後、手順 2 に従います。
    ///
    /// 手順 1a) 現在のプロジェクトに存在する XAML ファイルでこのカスタム コントロールを使用する場合
    /// この XmlNamespace 属性を使用場所であるマークアップ ファイルのルート要素に
    /// 追加します:
    ///
    ///     xmlns:MyNamespace="clr-namespace:TabAddDelCustomControl"
    ///
    ///
    /// 手順 1b) 異なるプロジェクトに存在する XAML ファイルでこのカスタム コントロールを使用する場合
    /// この XmlNamespace 属性を使用場所であるマークアップ ファイルのルート要素に
    /// 追加します:
    ///
    ///     xmlns:MyNamespace="clr-namespace:TabAddDelCustomControl;assembly=TabAddDelCustomControl"
    ///
    /// また、XAML ファイルのあるプロジェクトからこのプロジェクトへのプロジェクト参照を追加し、
    /// リビルドして、コンパイル エラーを防ぐ必要があります:
    ///
    ///     ソリューション エクスプローラーで対象のプロジェクトを右クリックし、
    ///     [参照の追加] の [プロジェクト] を選択してから、このプロジェクトを選択します。
    ///
    ///
    /// 手順 2)
    /// コントロールを XAML ファイルで使用します。
    ///
    ///     <MyNamespace:CustomControl1/>
    ///
    /// </summary>
    public class TabAddDelCustomControl : TabControl
    {
        static TabAddDelCustomControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(TabAddDelCustomControl), new FrameworkPropertyMetadata(typeof(TabAddDelCustomControl)));
        }
        /// <summary>
        /// タブ追加したときのイベント
        /// </summary>
        public static readonly RoutedEvent AddTabSettingEvent =
            EventManager.RegisterRoutedEvent(
                "AddTabSetting",
                RoutingStrategy.Bubble,
                typeof(RoutedEventHandler),
                typeof(TabAddDelCustomControl)
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
                typeof(TabAddDelCustomControl),
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