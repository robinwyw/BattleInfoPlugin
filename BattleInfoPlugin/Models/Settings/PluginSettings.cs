using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleInfoPlugin.Models.Settings
{
    public static class PluginSettings
    {
        public static BattleDataSettings BattleData { get; } = new BattleDataSettings();

        public static BattleWindowSettings BattleWindow { get; } = new BattleWindowSettings();

        public static NotifierSettings Notifier { get; } = new NotifierSettings();

        public static PathsSettings Paths { get; } = new PathsSettings();
    }
}
