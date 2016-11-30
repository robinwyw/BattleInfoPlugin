using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BattleInfoPlugin.Models;
using Livet;
using MetroTrilithon.Mvvm;

namespace BattleInfoPlugin.ViewModels
{
    public class BattleFleetViewModel : ViewModel
    {
        private readonly BattleFleet _fleet;

        private readonly string _defaultName;

        private readonly string _combinedName;

        public IEnumerable<FleetData> Fleets => this._fleet.Fleets;

        public string Name => this.IsCombined
            ? this._combinedName
            : (string.IsNullOrWhiteSpace(this._fleet.Name) ? this._defaultName : this._fleet.Name);

        public bool IsCombined => this._fleet.FleetCount > 1;

        public bool IsVisible => this._fleet.FleetCount > 0;

        public string FleetFormation => this._fleet.Formation != Formation.なし
            ? this._fleet.Formation.ToString()
            : "";

        public BattleFleetViewModel(BattleFleet fleets, string defaultName)
        {
            this._fleet = fleets;
            this._defaultName = defaultName;
            this._combinedName = fleets.Type == FleetType.Friend ? "連合艦隊" : "敵連合艦隊";
        }

        public void ObserveUpdate()
        {
            this._fleet.Subscribe(nameof(BattleFleet.Fleets), this.Update, false).AddTo(this);
            this._fleet.Subscribe(nameof(BattleFleet.Name), () => this.RaisePropertyChanged(nameof(this.Name)), false).AddTo(this);
            this._fleet.Subscribe(nameof(BattleFleet.Formation), () => this.RaisePropertyChanged(nameof(this.FleetFormation)), false).AddTo(this);
        }

        private void Update()
        {
            this.RaisePropertyChanged(nameof(this.Fleets));
            this.RaisePropertyChanged(nameof(this.IsCombined));
            this.RaisePropertyChanged(nameof(this.IsVisible));
        }
    }
}
