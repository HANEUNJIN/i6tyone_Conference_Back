using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace eGhis_WebService_Core.Infrastructure.Utils
{
    public static class EnumUtil
    {
        #region GENERAL STATIC METHOD AREA *************************************************
        public static string GetDisplayName(Enum value)
        {
            var fieldInfo = value.GetType().GetField(value.ToString());
            var attribute = fieldInfo?.GetCustomAttribute<DisplayAttribute>();
            return attribute?.Name ?? value.ToString();
        }

        public static string GetDescription(Enum value)
        {
            var fieldInfo = value.GetType().GetField(value.ToString());
            var attribute = fieldInfo?.GetCustomAttribute<DescriptionAttribute>();
            return attribute?.Description ?? value.ToString();
        }
        #endregion
    }
}
