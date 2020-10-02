using System;
using System.Collections.ObjectModel;

namespace Safe.Core.Domain
{
    [Serializable]
    public sealed class ListOfNonNullOrWhiteSpaceStrings : Collection<string>
    {
        protected override void SetItem(int index, string item)
        {
            if (string.IsNullOrWhiteSpace(item)) throw new ArgumentNullException(nameof(item));

            base.SetItem(index, item);
        }

        protected override void InsertItem(int index, string item)
        {
            if (string.IsNullOrWhiteSpace(item)) throw new ArgumentNullException(nameof(item));

            base.InsertItem(index, item);
        }
    }
}
