using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleInfoPlugin.Models
{
    public class ShipDamage
    {
        /// <summary>
        /// x = 0 : unknown source;
        /// x &gt; 0 : source is friend;
        /// x &lt; 0 : source is enemy
        /// </summary>
        public int Source { get; private set; }

        /// <summary>
        /// x = 0 : unknown target;
        /// x &gt; 0 : target is friend;
        /// x &lt; 0 : target is enemy
        /// </summary>
        public int Target { get; private set; }

        public int Damage { get; private set; }

        public ShipDamage()
            : this(0, 0, 0)
        {
        }

        public ShipDamage(int source, int target, int damage)
        {
            this.Source = source;
            this.Target = target;
            this.Damage = damage;
        }
    }
}
