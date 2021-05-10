using System.Windows;
using System.Windows.Documents;
using System.Windows.Media;

namespace UsMemoTool.Utility
{
   

    /// <summary>
    /// ゴーストを表示する装飾用コントロールを表します。
    /// </summary>
    internal class GhostAdorner : Adorner
    {
        /// <summary>
        /// 新しいインスタンスを生成します。
        /// </summary>
        /// <param name="visual">装飾する要素を指定します。</param>
        /// <param name="adornedElement">装飾に用いる要素を指定します。</param>
        /// <param name="point">装飾を表示する位置を、装飾する要素に対する相対位置として指定します。</param>
        /// <param name="offset">装飾を表示する位置に対するオフセットを指定します。</param>
        public GhostAdorner(Visual visual, UIElement adornedElement, Point point, Point offset)
            : base(adornedElement)
        {
            this._layer = AdornerLayer.GetAdornerLayer(visual);
            this.CurrentPoint = point;
            this.Offset = offset;

            Attach();
        }
        public GhostAdorner(Visual visual, UIElement adornedElement, Point point)
            : base(adornedElement)
        {
            this._layer = AdornerLayer.GetAdornerLayer(visual);
            this.CurrentPoint = point;
            this.Offset = new Point(-point.X, -point.Y);

            Attach();
        }

        #region 依存関係プロパティ

        /// <summary>
        /// CurrentPoint 依存関係プロパティの定義
        /// </summary>
        public static readonly DependencyProperty CurrentPointProperty = DependencyProperty.Register("CurrentPoint", typeof(Point), typeof(GhostAdorner), new FrameworkPropertyMetadata(default(Point), FrameworkPropertyMetadataOptions.AffectsRender));

        /// <summary>
        /// ゴーストの表示位置を取得または設定します。
        /// </summary>
        public Point CurrentPoint
        {
            get { return (Point)GetValue(CurrentPointProperty); }
            set { SetValue(CurrentPointProperty, value); }
        }

        /// <summary>
        /// Offset 依存関係プロパティの定義
        /// </summary>
        public static readonly DependencyProperty OffsetProperty = DependencyProperty.Register("Offset", typeof(Point), typeof(GhostAdorner), new FrameworkPropertyMetadata(default(Point), FrameworkPropertyMetadataOptions.AffectsRender));

        /// <summary>
        /// ゴーストの表示位置のオフセットを取得または設定します。
        /// </summary>
        public Point Offset
        {
            get { return (Point)GetValue(OffsetProperty); }
            set { SetValue(OffsetProperty, value); }
        }

        #endregion 依存関係プロパティ

        #region 公開メソッド

        /// <summary>
        /// アタッチします。
        /// </summary>
        public void Attach()
        {
            if (this._layer != null)
            {
                if (!this._isAttached)
                {
                    this._layer.Add(this);
                    this._isAttached = true;
                }
            }
        }

        /// <summary>
        /// デタッチします。
        /// </summary>
        public void Detach()
        {
            if (this._layer != null)
            {
                if (this._isAttached)
                {
                    this._layer.Remove(this);
                    this._isAttached = false;
                }
            }
        }

        #endregion 公開メソッド

        #region 描画オーバーライド

        /// <summary>
        /// 描画処理のオーバーライド
        /// </summary>
        /// <param name="drawingContext">描画先のコンテキストを指定します。</param>
        protected override void OnRender(DrawingContext drawingContext)
        {
            var pt = new Point(this.CurrentPoint.X + this.Offset.X, this.CurrentPoint.Y + this.Offset.Y);
            var rect = new Rect(pt, this.AdornedElement.RenderSize);
            var brush = new VisualBrush(this.AdornedElement);
            brush.Opacity = 0.3;

            drawingContext.DrawRectangle(brush, null, rect);
        }

        #endregion 描画オーバーライド

        #region private フィールド

        /// <summary>
        /// 装飾層
        /// </summary>
        private AdornerLayer _layer;

        /// <summary>
        /// アタッチされているかどうか
        /// </summary>
        private bool _isAttached;

        #endregion private フィールド
    }
}