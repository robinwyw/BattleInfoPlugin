using System;
using System.Reactive.Linq;
using System.Windows;
using BattleInfoPlugin.Models.Settings;
using Grabacr07.KanColleViewer.Composition;
using Grabacr07.KanColleWrapper;
using Livet;

namespace BattleInfoPlugin.Models.Notifiers
{
    public class BattleEndNotifier : NotificationObject
    {
        private static readonly BrowserImageMonitor monitor = new BrowserImageMonitor();

        private readonly Plugin plugin;

        #region IsEnabled変更通知プロパティ

        public bool IsEnabled
        {
            get { return NotifierSettings.IsEnabled; }
            set
            {
                if (NotifierSettings.IsEnabled == value)
                    return;
                NotifierSettings.IsEnabled.Value = value;
                this.RaisePropertyChanged();
            }
        }

        #endregion

        #region IsNotifyOnlyWhenInactive変更通知プロパティ

        public bool IsNotifyOnlyWhenInactive
        {
            get { return NotifierSettings.IsEnabledOnlyWhenInactive; }
            set
            {
                if (NotifierSettings.IsEnabledOnlyWhenInactive == value)
                    return;
                NotifierSettings.IsEnabledOnlyWhenInactive.Value = value;
                this.RaisePropertyChanged();
            }
        }

        #endregion

        public BattleEndNotifier(Plugin plugin)
        {
            this.plugin = plugin;

            var proxy = KanColleClient.Current.Proxy;

            proxy.api_req_combined_battle_battleresult
                .Subscribe(_ => this.NotifyEndOfBattle());

            proxy.ApiSessionSource.Where(x => x.Request.PathAndQuery == "/kcsapi/api_req_practice/battle_result")
                .Subscribe(_ => this.NotifyEndOfBattle());

            proxy.api_req_sortie_battleresult
                .Subscribe(_ => this.NotifyEndOfBattle());

            monitor.ConfirmPursuit += () => this.Notify(NotificationType.ConfirmPursuit, "追撃確認", "夜戦を行うかどうか選択してください。");
        }

        private void NotifyEndOfBattle()
        {
            this.Notify(NotificationType.BattleEnd, "戦闘終了", "戦闘が終了しました。");
        }

        private void Notify(string type, string title, string message)
        {
            var isActive = DispatcherHelper.UIDispatcher.Invoke(() => Application.Current.MainWindow.IsActive);
            if (this.IsEnabled && (!isActive || !this.IsNotifyOnlyWhenInactive))
                this.plugin.InvokeNotifyRequested(new NotifyEventArgs(type, title, message)
                {
                    Activated = () =>
                    {
                        DispatcherHelper.UIDispatcher.Invoke(() =>
                        {
                            var window = Application.Current.MainWindow;
                            if (window.WindowState == WindowState.Minimized)
                                window.WindowState = WindowState.Normal;
                            window.Activate();
                        });
                    },
                });
        }
    }
}
