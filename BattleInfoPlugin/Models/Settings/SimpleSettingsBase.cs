using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace BattleInfoPlugin.Models.Settings
{
    public abstract class SimpleSettingsBase<T>
        where T : SimpleSettingsBase<T>
    {
        private static readonly string BaseKey = typeof(T).Name;

        protected static string GetKey([CallerMemberName] string propertyName = null)
        {
            return $"{BaseKey}.{propertyName}";
        }
    }
}
