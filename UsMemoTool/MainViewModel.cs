using System.Collections.Generic;
using System.Windows.Ink;
using System.Windows.Media;
using UsMemoTool.Data.Map;
using UsMemoTool.Utility;

namespace UsMemoTool
{
    public class MainViewModel
    {
        public UsMap[] Maps { get; } = UsMap.AllMap;
        private UsMap map = UsMap.TheSkeld;
        public UsMap Map 
        { 
            get=>map; 
            set
			{
                map = value;
                if (currentMap == null) return;
                currentMap.Map = map;
			} 
        } 
        
        private MapControl currentMap;
        public MapControl CurrntMap
		{
            get => currentMap;
            set
			{
                currentMap = value;
                if (currentMap == null) return;
                currentMap.InkColor = InkColor;
                currentMap.Map = Map;
			}
		}
	    private Color inkColor;

		public Color InkColor
		{
			get => inkColor;
			set 
            { 
                inkColor = value;
                if (currentMap == null) return;
                currentMap.InkColor = inkColor;
            }
		}

		//! Ctrl+Zのコマンドオブジェクト
		public DelegateCommand UndoCommand { get; private set; } 
        public MainViewModel()
        {
            UndoCommand = new DelegateCommand(UndoBody);
            Map = Maps[0];
        }
        public void UndoBody(object sender)
        {
            if (CurrntMap == null) return;
            CurrntMap.Undo();
        }
        
    }
}
