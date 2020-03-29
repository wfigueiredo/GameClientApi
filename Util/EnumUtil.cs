using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;

namespace GameProducer.Util
{
    public static class EnumUtil
    {
        public static string GetDisplayName(this Enum enumType)
        {
            return enumType.GetType()
                        .GetMember(enumType.ToString())
                        .First()
                        .GetCustomAttribute<DisplayAttribute>()
                        .Name;
        }
    }
}
