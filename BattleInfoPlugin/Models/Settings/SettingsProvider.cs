using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MetroTrilithon.Serialization;

namespace BattleInfoPlugin.Models.Settings
{
    public static class SettingsProvider
    {
        private static string FilePath { get; } = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
            "grabacr.net",
            "KanColleViewer",
            "BattleInfoPlugin.xaml");

        public static ISerializationProvider Local { get; } = new FileSettingsProvider(FilePath);
    }
}
