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
        public Stack<Stroke> UndoStrokes = new Stack<Stroke>();
        //! Ctrl+Zのコマンドオブジェクト
        public DelegateCommand UndoCommand { get; private set; } 
        public Stroke InkStroke
        {
            get => UndoStrokes.Peek();
            set => UndoStrokes.Push(value);
		}
        public MainViewModel()
        {
            UndoCommand = new DelegateCommand(UndoBody);
            Map = Maps[0];
        }
        public void UndoBody(object sender)
        {
            if (UndoStrokes.Count <= 0) return;
            var stroke = UndoStrokes.Pop();
            InkStrokes.Remove(stroke);
        }
        public StrokeCollection InkStrokes { get; set; } = new StrokeCollection();
    }
}
