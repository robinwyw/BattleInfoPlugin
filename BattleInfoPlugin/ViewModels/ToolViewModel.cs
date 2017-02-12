using System;
using System.Linq;
using BattleInfoPlugin.Models;
using BattleInfoPlugin.Models.Notifiers;
using Livet;
using Livet.EventListeners;
using Livet.Messaging;

namespace BattleInfoPlugin.ViewModels
{
    public class ToolViewModel : ViewModel
    {
        public BattleEndNotifier Notifier { get; }

        #region Battle

        private BattleViewModel _Battle;

        public BattleViewModel Battle
        {
            get { return this._Battle; }
            set
            {
                if (this._Battle != value)
                {
                    this._Battle = value;
                    this.RaisePropertyChanged();
                }
            }
        }

        #endregion

        public ToolViewModel(Plugin plugin)
        {
            this.Notifier = new BattleEndNotifier(plugin);
            this.Battle = BattleViewModel.Current;
        }

        public void OpenEnemyWindow()
        {
            var message = new TransitionMessage("Show/EnemyWindow")
            {
                TransitionViewModel = new EnemyWindowViewModel()
            };
            this.Messenger.RaiseAsync(message);
        }

        public void OpenBattleWindow()
        {
            var message = new TransitionMessage("Show/BattleWindow")
            {
                TransitionViewModel = new BattleWindowViewModel { Notifier = this.Notifier }
            };
            this.Messenger.RaiseAsync(message);
        }
    }
}
