using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleInfoPlugin.Models
{
    public class FleetDamagesCombined
    {
        public IEnumerable<KeyValuePair<int, FleetDamages>> Damages { get; set; }
    }
}
