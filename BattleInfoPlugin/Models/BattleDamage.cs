using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleInfoPlugin.Models
{
    public class BattleDamage
    {
        public IDictionary<int, IDictionary<int, List<FleetDamages>>> Damages;
    }
}
