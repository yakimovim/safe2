using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace Safe.Core.Domain
{
    /// <summary>
    /// Represents container of information about one item.
    /// </summary>
    [Serializable]
    public sealed class Item
    {
        /// <summary>
        /// Title of the item.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Description of the item.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// List of tags for this item.
        /// </summary>
        public IList<string> Tags { get; } = new ListOfNonNullOrWhiteSpaceStrings();

        /// <summary>
        /// List of fields of this item.
        /// </summary>
        public IList<Field> Fields { get; } = new ListOfNonNullItems<Field>();

        /// <summary>
        /// Checks if the item contains given <paramref name="text"/>.
        /// </summary>
        /// <param name="text">Text to search in the item.</param>
        public bool Contains(string text)
        {
            if (string.IsNullOrWhiteSpace(text)) return false;

            var loweredText = text.ToLower(CultureInfo.CurrentCulture);

            if (Title?.ToLower(CultureInfo.CurrentCulture).Contains(loweredText)
                == true) return true;

            if (Description?.ToLower(CultureInfo.CurrentCulture).Contains(loweredText)
                == true) return true;

            if (Tags.Any(t => t?.ToLower(CultureInfo.CurrentCulture).Contains(loweredText) == true)) return true;

            if (Fields.Any(f => f.Contains(loweredText))) return true;

            return false;
        }
    }
}
