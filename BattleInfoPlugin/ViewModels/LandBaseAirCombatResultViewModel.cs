using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BattleInfoPlugin.Models;

namespace BattleInfoPlugin.ViewModels
{
    public class LandBaseAirCombatResultViewModel : IAirCombatResultViewModel
    {
        public string Name { get; }
        public Squadron[] Squadrons { get; }
        public bool IsHappen { get; }
        public AirCombatResultViewModel Stage1 { get; }
        public AirCombatResultViewModel Stage2 { get; }
        public int Count { get; }
        public int LostCount { get; }
        public int RemainingCount { get; }

        public LandBaseAirCombatResultViewModel(LandBaseAirCombatResult result, FleetType type)
        {
            this.Name = result.Name;
            this.Stage1 = new AirCombatResultViewModel(result.Stage1, type);
            this.Stage2 = new AirCombatResultViewModel(result.Stage2, type);
            this.Count = this.Stage1.Count;
            this.LostCount = this.Stage1.LostCount + this.Stage2.LostCount;
            this.RemainingCount = this.Count - this.LostCount;

            this.IsHappen = this.Count > 0;

            this.Squadrons = (type == FleetType.First) ? result.Squadrons : new Squadron[0];
        }
    }
}
