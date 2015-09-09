using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using MetroTrilithon.Serialization;

namespace BattleInfoPlugin.Models.Settings
{
    public static class BattleWindowSettings
    {
        public static SerializableProperty<bool> TopMost
            = new SerializableProperty<bool>(GetKey(), SettingsProvider.Local, false) { AutoSave = true };

        private static string GetKey([CallerMemberName] string caller = "")
            => nameof(BattleWindowSettings) + "." + caller;
    }
}
