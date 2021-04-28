using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UsMemoTool.Utility 
{
    [AttributeUsage(AttributeTargets.Enum | AttributeTargets.Field)]
    class AliasAttribute : Attribute
    {
        private string _text;                                        // バックストア
        public string Text => _text;                                 // 文字列取得用プロパティ
        public AliasAttribute(string text) { _text = text; }         // 文字列を１つとるコンストラクタ 
    }
}
