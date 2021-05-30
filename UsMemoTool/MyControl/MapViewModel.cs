using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Ink;
using System.Windows.Media;
using UsMemoTool.Data.Map;
using UsMemoTool.Utility;

namespace UsMemoTool.MyControl
{
	public class MapViewModel
	{
        public UsMap Map { get; set; }
        public MapViewModel()
        {

        }
        public void UndoBody()
        {
            if (InkStrokes.Count == 0) return;
            InkStrokes.RemoveAt(InkStrokes.Count - 1);
        }
        public StrokeCollection InkStrokes { get; set; } = new StrokeCollection();
        public DrawingAttributes DrawAttributes { get; set; } = new DrawingAttributes()
        {
            FitToCurve = false,
            StylusTip = StylusTip.Ellipse,
        };
        public Color InkColor
		{
			get => DrawAttributes.Color;
            set => DrawAttributes.Color =value;
		}
	}
}
