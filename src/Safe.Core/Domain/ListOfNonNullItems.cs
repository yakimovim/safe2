using System;
using System.Collections.ObjectModel;

namespace Safe.Core.Domain
{
    [Serializable]
    public sealed class ListOfNonNullItems<T> : Collection<T>
        where T : class
    {
        protected override void SetItem(int index, T item)
        {
            if (item == null) throw new ArgumentNullException(nameof(item));

            base.SetItem(index, item);
        }

        protected override void InsertItem(int index, T item)
        {
            if (item == null) throw new ArgumentNullException(nameof(item));

            base.InsertItem(index, item);
        }
    }
}
