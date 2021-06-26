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
            catch(Exception e){
                Console.WriteLine(e);
			}
            
        }     

        public static readonly Dictionary<PlayerColor, Color> ColorMapping = new Dictionary<PlayerColor, Color>()
        {
            { PlayerColor.Red,  System.Windows.Media.Color.FromRgb(197, 17, 17) },
            { PlayerColor.Blue,  System.Windows.Media.Color.FromRgb(19, 46, 209) },
            { PlayerColor.Green,  System.Windows.Media.Color.FromRgb(17, 127, 17) },
            { PlayerColor.Pink,  System.Windows.Media.Color.FromRgb(237, 84, 186) },
            { PlayerColor.Orange,  System.Windows.Media.Color.FromRgb(239, 125, 13) },
            { PlayerColor.Yellow,  System.Windows.Media.Color.FromRgb(245, 245, 87) },
            { PlayerColor.Black,  System.Windows.Media.Color.FromRgb(20, 20, 20) },
            { PlayerColor.White,  System.Windows.Media.Color.FromRgb(214, 224, 240) },
            { PlayerColor.Purple,  System.Windows.Media.Color.FromRgb(107, 47, 187) },
            { PlayerColor.Brown,  System.Windows.Media.Color.FromRgb(113, 73, 30) },
            { PlayerColor.Cyan,  System.Windows.Media.Color.FromRgb(56, 254, 220) },
            { PlayerColor.Lime,     System.Windows.Media.Color.FromRgb(80, 239, 57) },
            { PlayerColor.Maroon,   System.Windows.Media.Color.FromRgb(95, 29, 46)},
            { PlayerColor.Rose,     System.Windows.Media.Color.FromRgb(236, 192, 211)},
            { PlayerColor.Banana,   System.Windows.Media.Color.FromRgb(240, 231, 168)},
            { PlayerColor.Gray,     System.Windows.Media.Color.FromRgb(117, 133, 147)},
            { PlayerColor.Tan,      System.Windows.Media.Color.FromRgb(145, 136, 119)},
            { PlayerColor.Coral,   System.Windows.Media.Color.FromRgb(215, 100, 100)}

        };
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
