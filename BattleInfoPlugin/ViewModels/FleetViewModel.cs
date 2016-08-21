using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BattleInfoPlugin.Models;
using Livet;

namespace BattleInfoPlugin.ViewModels
{
    public class FleetViewModel : ViewModel
    {
        public string Name => this.IsCombined ? "連合艦隊" : Fleets.FirstOrDefault()?.Name ?? "？？？";

        public bool IsCombined => this.Fleets.Length > 1;

        public bool IsVisible => this._Fleets.FirstOrDefault()?.IsVisible ?? false;

        public string FleetFormation => this.Fleets.FirstOrDefault()?.FleetFormation;

        private SingleFleetViewModel[] _Fleets = new SingleFleetViewModel[0];

        public SingleFleetViewModel[] Fleets
        {
            get { return this._Fleets; }
            set
            {
                if (this._Fleets != value)
                {
                    this._Fleets = value;
                    this.RaisePropertyChanged();
                    this.RaisePropertyChanged(nameof(this.Name));
                    this.RaisePropertyChanged(nameof(this.IsCombined));
                    this.RaisePropertyChanged(nameof(this.IsVisible));
                    this.RaisePropertyChanged(nameof(this.FleetFormation));
                }
            }
        }
    }
}
