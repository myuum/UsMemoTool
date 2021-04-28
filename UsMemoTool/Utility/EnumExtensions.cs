using System;
using System.Reflection;   // これも必要
namespace UsMemoTool.Utility
{
    public static class EnumExtensions
    {
        // 列挙値に付与されている別名の取得
        public static string ToAliasString(this Enum target)
        {
            var attribute = target.GetType().GetMember(target.ToString())[0].GetCustomAttribute(typeof(AliasAttribute));

            return attribute == null ? null : ((AliasAttribute)attribute).Text;
        }
        // 列挙体に付与されている別名の取得
        public static string ToAliasEnumString(this Enum target)
        {
            Attribute attribute = target.GetType().GetCustomAttribute(typeof(AliasAttribute));

            return attribute == null ? null : ((AliasAttribute)attribute).Text;
        }
    }
}
