using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using MetroTrilithon.Serialization;

namespace BattleInfoPlugin.Models.Settings
{
    public sealed class BattleDataSettings : SimpleSettingsBase<BattleDataSettings>
    {
        public SerializableProperty<bool> IsShowLandBaseAirStage { get; }
            = SettingsHelper.GenerateProperty(GetKey(), false);
    }
}
