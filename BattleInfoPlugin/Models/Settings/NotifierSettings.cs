using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using MetroTrilithon.Serialization;

namespace BattleInfoPlugin.Models.Settings
{
    public sealed class NotifierSettings : SimpleSettingsBase<NotifierSettings>
    {
        public SerializableProperty<bool> IsEnabled { get; }
            = SettingsHelper.GenerateProperty(GetKey(), true);

        public SerializableProperty<bool> IsEnabledOnlyWhenInactive { get; }
            = SettingsHelper.GenerateProperty(GetKey(), true);
    }
}
