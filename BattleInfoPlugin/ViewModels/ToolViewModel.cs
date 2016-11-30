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
        private readonly BattleEndNotifier notifier;

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


        #region IsShowLandBaseAirStage

        public bool IsShowLandBaseAirStage
        {
            get { return this.Battle.IsShowLandBaseAirStage; }
            set
            {
                if (this.Battle.IsShowLandBaseAirStage != value)
                {
                    this.Battle.IsShowLandBaseAirStage = value;
                    this.RaisePropertyChanged();
                }
            }
        }

        #endregion


        #region IsNotifierEnabled変更通知プロパティ
        // ここ以外で変更しないのでModel変更通知は受け取らない雑対応
        public bool IsNotifierEnabled
        {
            get
            { return this.notifier.IsEnabled; }
            set
            {
                if (this.notifier.IsEnabled == value)
                    return;
                this.notifier.IsEnabled = value;
                this.RaisePropertyChanged();
            }
        }
        #endregion


        #region IsNotifyOnlyWhenInactive変更通知プロパティ
        // ここ以外で変更しないのでModel変更通知は受け取らない雑対応
        public bool IsNotifyOnlyWhenInactive
        {
            get
            { return this.notifier.IsNotifyOnlyWhenInactive; }
            set
            {
                if (this.notifier.IsNotifyOnlyWhenInactive == value)
                    return;
                this.notifier.IsNotifyOnlyWhenInactive = value;
                this.RaisePropertyChanged();
            }
        }
        #endregion


        public ToolViewModel(Plugin plugin)
        {
            this.notifier = new BattleEndNotifier(plugin);
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
                TransitionViewModel = new BattleWindowViewModel()
            };
            this.Messenger.RaiseAsync(message);
        }
    }
}
