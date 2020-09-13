using System;
using System.Collections;
using System.Collections.Generic;

namespace Safe.Core.Domain
{
    [Serializable]
    public sealed class ListOfNonNullItems<T> : IList<T>
        where T : class
    {
        private readonly List<T> _items = new List<T>();

        public T this[int index] 
        { 
            get => _items[index];
            set
            {
                if (value == null) throw new ArgumentNullException(nameof(value));

                _items[index] = value;

            }
        }

        public int Count => _items.Count;

        public bool IsReadOnly => false;

        public void Add(T item)
        {
            if (item == null) throw new ArgumentNullException(nameof(item));

            _items.Add(item);
        }

        public void Clear() => _items.Clear();

        public bool Contains(T item) => _items.Contains(item);

        public void CopyTo(T[] array, int arrayIndex) => _items.CopyTo(array, arrayIndex);

        public IEnumerator<T> GetEnumerator() => _items.GetEnumerator();

        public int IndexOf(T item) => _items.IndexOf(item);

        public void Insert(int index, T item)
        {
            if (item == null) throw new ArgumentNullException(nameof(item));

            _items.Insert(index, item);
        }

        public bool Remove(T item) => _items.Remove(item);

        public void RemoveAt(int index) => _items.RemoveAt(index);

        IEnumerator IEnumerable.GetEnumerator() => _items.GetEnumerator();
    }
}
