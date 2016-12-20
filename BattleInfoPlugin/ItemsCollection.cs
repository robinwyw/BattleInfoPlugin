using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleInfoPlugin
{
    public class ItemsCollection<T> : IReadOnlyList<T>
    {
        private readonly T[] _items;

        public ItemsCollection(IEnumerable<T> items, int offset = 1)
        {
            this._items = items.ToArray();
            this.StartIndex = offset;
        }

        public T this[int index]
        {
            get
            {
                index -= this.StartIndex;
                if (index < this.Count && index >= 0)
                {
                    return this._items[index];
                }
                return default(T);
            }

            set
            {
                index -= this.StartIndex;
                this._items[index] = value;
            }
        }

        public int StartIndex { get; }

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
