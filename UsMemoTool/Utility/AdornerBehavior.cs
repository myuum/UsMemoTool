using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System;

namespace UsMemoTool.Utility
{
    public class AdornerBehavior
    {
        /// <summary>
        /// IsEnabled 添付プロパティの定義
        /// </summary>
        public static readonly DependencyProperty IsEnabledProperty = DependencyProperty.RegisterAttached("IsEnabled", typeof(bool), typeof(AdornerBehavior), new PropertyMetadata(false, OnIsEnabledPropertyChanged));

        /// <summary>
        /// IsEnabled 添付プロパティを取得します。
        /// </summary>
        /// <param name="target">対象とする DependencyObject を指定します。</param>
        /// <returns>取得した値を返します。</returns>
        public static bool GetIsEnabled(DependencyObject target)
        {
            return (bool)target.GetValue(IsEnabledProperty);
        }

        /// <summary>
        /// IsEnabled 添付プロパティを設定します。
        /// </summary>
        /// <param name="target">対象とする DependencyObject を指定します。</param>
        /// <param name="value">設定する値を指定します。</param>
        public static void SetIsEnabled(DependencyObject target, bool value)
        {
            target.SetValue(IsEnabledProperty, value);
        }

        /// <summary>
        /// IsEnabled 添付プロパティ変更イベントハンドラ
        /// </summary>
        /// <param name="sender">イベント発行元</param>
        /// <param name="e">イベント引数</param>
        private static void OnIsEnabledPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var element = sender as UIElement;
            if (element == null)
                return;
            var isEnabled = GetIsEnabled(element);
            if (isEnabled)
            {
                element.PreviewMouseLeftButtonDown += element_PreviewMouseLeftButtonDown;
                element.PreviewMouseMove += element_PreviewMouseMove;
                element.PreviewMouseLeftButtonUp += element_PreviewMouseLeftButtonUp;
                element.GiveFeedback += element_GiveFeedback;
            }
            else
            {
            }
        }

        /// <summary>
        /// 装飾用コントロール
        /// </summary>
        private static GhostAdorner _adorner;

        /// <summary>
        /// PreviewMouseLeftButtonDown イベントハンドラ
        /// </summary>
        /// <param name="sender">イベント発行元</param>
        /// <param name="e">イベント引数</param>
        static void element_PreviewMouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            var originalElement = e.OriginalSource as FrameworkElement;
            var parent = originalElement != null ? FindAncestor<Panel>(originalElement) : null;
            var adornedElement = sender as FrameworkElement;
            if ((parent == null) || (adornedElement == null))
                return;

            var pt = e.GetPosition(adornedElement);
            var offset = new Point(-pt.X, -pt.Y);
            _adorner = new GhostAdorner(parent, adornedElement, pt, offset);

            adornedElement.CaptureMouse();
        }
        

        /// <summary>
        /// PreviewMouseLeftButtonUp イベントハンドラ
        /// </summary>
        /// <param name="sender">イベント発行元</param>
        /// <param name="e">イベント引数</param>
        static void element_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (_adorner != null)
            {
                _adorner.AdornedElement.ReleaseMouseCapture();
                _adorner.Detach();
                _adorner = null;
            }
        }

        /// <summary>
        /// PreviewMouseMove イベントハンドラ
        /// </summary>
        /// <param name="sender">イベント発行元</param>
        /// <param name="e">イベント引数</param>
        static void element_PreviewMouseMove(object sender, MouseEventArgs e)
        {
            if (_adorner != null)
            {
                if (_adorner.AdornedElement.IsMouseCaptured && (e.LeftButton == MouseButtonState.Pressed))
                {
                    var pt = e.GetPosition(_adorner.AdornedElement);
                    _adorner.CurrentPoint = pt;
                }
            }
        }
        static void element_GiveFeedback(object sender, GiveFeedbackEventArgs e)
        {
            Console.WriteLine("----GiveFeedback----");
            Console.WriteLine("Sender: " + sender.ToString());
        }

        /// <summary>
        /// 指定された型の親要素を探します。
        /// </summary>
        /// <typeparam name="T">親要素の型を指定します。</typeparam>
        /// <param name="element">探索を開始する要素を指定します。</param>
        /// <returns>親要素を返します。</returns>
        private static T FindAncestor<T>(FrameworkElement element)
            where T : FrameworkElement
        {
            do
            {
                element = VisualTreeHelper.GetParent(element) as FrameworkElement;
                if (element is T)
                    return element as T;
            } while (element != null);
            return null;
        }
    }
}
