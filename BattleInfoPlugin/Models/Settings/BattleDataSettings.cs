using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using MetroTrilithon.Serialization;

namespace BattleInfoPlugin.Models.Settings
{
    public static class BattleDataSettings
    {
        public static SerializableProperty<bool> IsShowLandBaseAirStage { get; }
            = new SerializableProperty<bool>(GetKey(), SettingsProvider.Local, false) { AutoSave = true };

        public static string GetKey([CallerMemberName] string caller = "")
            => nameof(BattleDataSettings) + "." + caller;
    }
}
