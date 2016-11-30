using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleInfoPlugin
{
    public class ItemsCollection<T> : IReadOnlyList<T>
        where T : class
    {
        private readonly T[] _items;

        public ItemsCollection(IEnumerable<T> items)
        {
            this._items = items.ToArray();
        }

        public T this[int index]
        {
            get
            {
                if (index < this.Count && index >= 0)
                {
                    return this._items[index];
                }
                return null;
            }
        }

        public int Count => this._items.Length;

        public IEnumerator<T> GetEnumerator()
        {
            return ((IEnumerable<T>)this._items).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }
}
