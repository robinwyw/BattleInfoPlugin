using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleInfoPlugin.Models
{
    public class Attack
    {
        /// <summary>
        /// x = 0 : unknown source;
        /// x &gt; 0 : source is friend;
        /// x &lt; 0 : source is enemy
        /// </summary>
        public int Source { get; private set; }

        public IEnumerable<Damage> Damages { get; private set; }

        public int TotalDamage => this.Damages.Sum(x => x.Value);

        public bool isSupport { get; private set; }

        public Attack(int source, int target, int damage, bool isCritical, bool friendSupport = false)
            : this(source, new[] { new Damage(target, damage, isCritical) }, friendSupport)
        {
        }

        public Attack(int source, IEnumerable<Damage> damages, bool friendSupport = false)
        {
            this.Source = source;
            this.Damages = damages.ToArray();
            this.isSupport = friendSupport;
        }

        public static readonly Attack Empty = new Attack(0, Enumerable.Empty<Damage>());
    }

    public class Damage
    {
        /// <summary>
        /// x = 0 : unknown target;
        /// x &gt; 0 : target is friend;
        /// x &lt; 0 : target is enemy
        /// </summary>
        public int Target { get; private set; }

        public int Value { get; private set; }

        public bool IsCritical { get; private set; }

        public Damage(int target, int value, bool isCritical)
        {
            this.Target = target;
            this.Value = value;
            this.IsCritical = isCritical;
        }
    }
}
