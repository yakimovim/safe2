using System;
using System.Globalization;

namespace Safe.Core.Domain
{
    /// <summary>
    /// Types of fields.
    /// </summary>
    public enum FieldTypes
    {
        /// <summary>
        /// Single line text field.
        /// </summary>
        SingleLineText,
        /// <summary>
        /// Multi line text field.
        /// </summary>
        MultiLineText,
        /// <summary>
        /// Password field.
        /// </summary>
        Password
    }

    /// <summary>
    /// Represents single field of an item.
    /// </summary>
    [Serializable]
    public abstract class Field
    {
        /// <summary>
        /// Label of the field.
        /// </summary>
        public string Label { get; set; }

        /// <summary>
        /// Type of the field.
        /// </summary>
        public abstract FieldTypes Type { get; }

        /// <summary>
        /// Checks if the field contains given <paramref name="text"/>.
        /// </summary>
        /// <param name="text">Text to search in the field.</param>
        public abstract bool Contains(string text);
    }

    /// <summary>
    /// Represents field with a single line of text.
    /// </summary>
    [Serializable]
    public sealed class SingleLineTextField : Field
    {
        /// <inheritdoc />
        public override FieldTypes Type => FieldTypes.SingleLineText;

        /// <inheritdoc />
        public override bool Contains(string text)
        {
            if (string.IsNullOrWhiteSpace(text)) return false;

            return Text?.ToLower(CultureInfo.CurrentCulture)
                .Contains(text.ToLower(CultureInfo.CurrentCulture))
                == true;
        }

        /// <summary>
        /// Text content of the field.
        /// </summary>
        public string Text { get; set; }
    }

    /// <summary>
    /// Represents field with multiple lines of text.
    /// </summary>
    [Serializable]
    public sealed class MultiLineTextField : Field
    {
        /// <inheritdoc />
        public override FieldTypes Type => FieldTypes.MultiLineText;

        /// <inheritdoc />
        public override bool Contains(string text)
        {
            if (string.IsNullOrWhiteSpace(text)) return false;

            return Text?.ToLower(CultureInfo.CurrentCulture).Contains(text.ToLower(CultureInfo.CurrentCulture)) == true;
        }

        /// <summary>
        /// Text content of the field.
        /// </summary>
        public string Text { get; set; }
    }

    /// <summary>
    /// Represents field with a password.
    /// </summary>
    [Serializable]
    public sealed class PasswordField : Field
    {
        /// <inheritdoc />
        public override FieldTypes Type => FieldTypes.Password;

        /// <inheritdoc />
        public override bool Contains(string text)
        {
            if (string.IsNullOrWhiteSpace(text)) return false;

            return Text?.Contains(text) == true;
        }

        /// <summary>
        /// Text content of the field.
        /// </summary>
        public string Text { get; set; }
    }
}
