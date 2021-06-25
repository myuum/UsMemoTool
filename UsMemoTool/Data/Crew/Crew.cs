using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using UsMemoTool.Utility;

namespace UsMemoTool.Data.Crew
{
    public class Crew
    {
        public PlayerColor Color { get; private set; }
        public ImageSource AriveImage { get; private set; }
        public ImageSource DiedImage { get; private set; }
        
        public Crew(PlayerColor color)
        {
            Color = color;
            var name = Color.ToString().ToLower();
            try
            {
                AriveImage = new BitmapImage(new Uri("pack://application:,,,/Resource/Image/Crew/au" + name + ".png"));
                DiedImage = new BitmapImage(new Uri("pack://application:,,,/Resource/Image/Crew/au" + name + "dead.png"));
            }
            catch { }
            
        }
    }
    [Alias("クルーカラー")]
    public enum PlayerColor
    {
        [Alias("赤")] Red,
        [Alias("青")] Blue,
        [Alias("緑")] Green,
        [Alias("ピンク")] Pink,
        [Alias("オレンジ")] Orange,
        [Alias("黄色")] Yellow,
        [Alias("黒")] Black,
        [Alias("白")] White,
        [Alias("紫")] Purple,
        [Alias("茶色")] Brown,
        [Alias("水色")] Cyan,
        [Alias("黄緑")] Lime,
        [Alias("小豆色")] Maroon,
        [Alias("ローズ")] Rose,
        [Alias("バナナ")] Banana,
        [Alias("灰色")] Gray,
        [Alias("タン")] Tan,
        [Alias("コーラル")] Coral
    }


}
