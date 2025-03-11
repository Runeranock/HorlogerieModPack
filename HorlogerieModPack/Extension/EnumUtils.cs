using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HorlogerieModPack.Extension
{
    public static class EnumUtils
    {
        public static bool IsInEnum<T>(this string value) where T : Enum
        {
            return Enum.GetNames(typeof(T)).Any(name => name.Equals(value, StringComparison.OrdinalIgnoreCase));
        }
    }
}
