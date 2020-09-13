using System;
using System.Collections;
using System.Collections.Generic;

namespace Safe.Core.Domain
{
    [Serializable]
    public sealed class ListOfNonNullOrWhiteSpaceStrings : IList<string>
    {
        private readonly List<string> _items = new List<string>();

        public string this[int index] 
        { 
            get => _items[index];
            set
            {
                if (string.IsNullOrWhiteSpace(value)) throw new ArgumentNullException(nameof(value));

                _items[index] = value;

            }
        }

        public int Count => _items.Count;

        public bool IsReadOnly => false;

        public void Add(string item)
        {
            if (string.IsNullOrWhiteSpace(item)) throw new ArgumentNullException(nameof(item));

            _items.Add(item);
        }

        public void Clear() => _items.Clear();

        public bool Contains(string item) => _items.Contains(item);

        public void CopyTo(string[] array, int arrayIndex) => _items.CopyTo(array, arrayIndex);

        public IEnumerator<string> GetEnumerator() => _items.GetEnumerator();

        public int IndexOf(string item) => _items.IndexOf(item);

        public void Insert(int index, string item)
        {
            if (string.IsNullOrWhiteSpace(item)) throw new ArgumentNullException(nameof(item));

            _items.Insert(index, item);
        }

        public bool Remove(string item) => _items.Remove(item);

        public void RemoveAt(int index) => _items.RemoveAt(index);

        IEnumerator IEnumerable.GetEnumerator() => _items.GetEnumerator();
    }
}
