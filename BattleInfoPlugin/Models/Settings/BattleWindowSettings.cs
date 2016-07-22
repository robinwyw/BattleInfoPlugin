using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using MetroTrilithon.Serialization;

namespace BattleInfoPlugin.Models.Settings
{
    public sealed class BattleWindowSettings : SimpleSettingsBase<BattleWindowSettings>
    {
        public SerializableProperty<bool> TopMost
            = SettingsHelper.GenerateProperty(GetKey(), false);
    }
}
