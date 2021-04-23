using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BattleInfoPlugin.Models.Raw;
using Grabacr07.KanColleWrapper;
using Grabacr07.KanColleWrapper.Models;

namespace BattleInfoPlugin.Models
{
    public class LandBaseAirCombatResult
    {
        public string Name { get; }
        public Squadron[] Squadrons { get; }
        public AirCombatResult Stage1 { get; }
        public AirCombatResult Stage2 { get; }

        public LandBaseAirCombatResult(string name, AirCombatResult stage1, AirCombatResult stage2, Api_Squadron_Plane[] squadrons)
        {
            this.Name = name;
            this.Stage1 = stage1;
            this.Stage2 = stage2;
            this.Squadrons = squadrons.Select(s => new Squadron(s)).ToArray();
        }
    }

    public class Squadron
    {
        public SlotItemInfo Plane { get; }
        public int Count { get; }
        public int Max { get; }

        public Squadron(Api_Squadron_Plane squadron)
        {
            this.Plane = KanColleClient.Current.Master.SlotItems[squadron.api_mst_id];
            this.Count = squadron.api_count;
            this.Max = this.Plane == null ? 0 :
                            this.Plane.Type == SlotItemType.大型陸上機 ? 9 :
                                this.Plane.Type == SlotItemType.水上偵察機 ||
                                this.Plane.Type == SlotItemType.艦上偵察機 || this.Plane.Type == SlotItemType.艦上偵察機_II ||
                                this.Plane.Type == SlotItemType.大型飛行艇 ||
                                this.Plane.Type == SlotItemType.陸上偵察機 ? 4 : 18;
        }
    }
}
