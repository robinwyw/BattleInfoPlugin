using System;
using System.ComponentModel.Composition;
using BattleInfoPlugin.Models.Repositories;
using BattleInfoPlugin.ViewModels;
using BattleInfoPlugin.Views;
using BattleInfoPlugin.Models;
using Grabacr07.KanColleViewer.Composition;

namespace BattleInfoPlugin
{
    [Export(typeof(ILocalizable))]
    [Export(typeof(IPlugin))]
    [Export(typeof(ITool))]
    [Export(typeof(IRequestNotify))]
    [ExportMetadata("Guid", "55F1599E-5FAD-4696-A972-BF8C4B3C1B76")]
    [ExportMetadata("Title", "BattleInfo")]
    [ExportMetadata("Description", "戦闘情報を表示します。")]
    [ExportMetadata("Version", "2.5")]
    [ExportMetadata("Author", "@veigr, @Madmanmayson, @laserdark")]
    public class Plugin : IPlugin, ITool, IRequestNotify, ILocalizable
    {
        private readonly ToolViewModel vm;
        internal static KcsResourceWriter ResourceWriter { get; private set; }
        internal static EnemyDataUpdater Updater { get; private set; }

        public Plugin()
        {
            this.vm = new ToolViewModel(this);
        }

        public async void Initialize()
        {
            Master.Current.Init();
            ResourceWriter = ResourceWriter ?? new KcsResourceWriter();
            Updater = Updater ?? new EnemyDataUpdater(EnemyDataProvider.Current);
            await MapCellMapping.Current.UpdateData();
        }

        public string Name => "BattleInfo";

        // タブ表示するたびに new されてしまうが、今のところ new しないとマルチウィンドウで正常に表示されない
        public object View => new ToolView { DataContext = this.vm };

        public event EventHandler<NotifyEventArgs> NotifyRequested;

        public void InvokeNotifyRequested(NotifyEventArgs e) => this.NotifyRequested?.Invoke(this, e);

        public void ChangeCulture(string cultureName) => ResourceService.Current.ChangeCulture(cultureName);
    }
}
