using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace UsMemoTool.Data.Map
{
    public class UsMap
    {
        readonly static public UsMap MiraHq = new UsMap("MIRA HQ");
        readonly static public UsMap Polus = new UsMap("POLUS");
        readonly static public UsMap TheSkeld = new UsMap("THE SKELD");
        readonly static public UsMap AirShip = new UsMap("AIR SHIP");
        readonly static public UsMap[] AllMap =
        {
            TheSkeld,
            MiraHq,
            Polus,
            AirShip
        };
        public string Name { get; private set; }
        public ImageSource Image { get; private set; }
        

        UsMap(string name)
        {
            Name = name;
            try
            {
                Image = new BitmapImage(new Uri("pack://application:,,,/Resource/Image/Map/" + Name + ".png"));
            }
            catch(Exception e)
            {
                throw e;
            }
            
        }
        public override string ToString()
        {
            return Name;
        }
    }
}
