using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BattleInfoPlugin.Models.Notifiers;
using BattleInfoPlugin.Models.Settings;
using Livet;

namespace BattleInfoPlugin.ViewModels
{
    public class BattleWindowViewModel : ViewModel
    {
        public bool TopMost
        {
            get { return PluginSettings.BattleWindow.TopMost; }
            set
            {
                if (PluginSettings.BattleWindow.TopMost != value)
                {
                    PluginSettings.BattleWindow.TopMost.Value = value;
                    this.RaisePropertyChanged();
                }
            }
        }

        public BattleViewModel Battle { get; } = BattleViewModel.Current;

        public BattleEndNotifier Notifier { get; set; }
    }
}
