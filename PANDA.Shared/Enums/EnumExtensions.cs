using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace PANDA.Shared.Enums;

public static class EnumExtensions
{
    public static string ToDisplayString(this Enum value)
    {
        var member = value.GetType().GetMember(value.ToString());
        var displayAttribute = member[0]
            .GetCustomAttribute<DisplayAttribute>();

        return displayAttribute?.Name ?? value.ToString();
    }
}
