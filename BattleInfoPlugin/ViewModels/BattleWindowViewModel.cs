using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BattleInfoPlugin.Models.Settings;
using Livet;

namespace BattleInfoPlugin.ViewModels
{
    public class BattleWindowViewModel : ViewModel
    {
        public string Title => "戦闘情報";


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
    }
}
