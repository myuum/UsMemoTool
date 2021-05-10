using System.Collections.Generic;
using System.Windows.Ink;
using UsMemoTool.Data.Map;
using UsMemoTool.Utility;

namespace UsMemoTool
{
    public class MainViewModel
    {
        public UsMap[] Maps { get; } = UsMap.AllMap;
        public UsMap Map { get; set; }
        //! Ctrl+Zのコマンドオブジェクト
        public DelegateCommand UndoCommand { get; private set; } 
        public MainViewModel()
        {
            UndoCommand = new DelegateCommand(UndoBody);
            Map = Maps[0];
        }
        public void UndoBody(object sender)
        {
            if (InkStrokes.Count == 0) return;
            InkStrokes.RemoveAt(InkStrokes.Count-1);
        }
        public StrokeCollection InkStrokes { get; set; } = new StrokeCollection();
        
    }
}
