using Safe.Core.Domain;
using Xunit;

namespace Safe.Tests.Domain
{
    public class FieldTests
    {
        [Theory]
        [InlineData("URL:", "www.ivany.com", "ivany", true)]
        [InlineData("URL:", "www.ivany.com", "IvanY", true)]
        [InlineData("URL:", "www.ivany.com", "url", false)]
        [InlineData("URL:", "www.ivany.com", null, false)]
        [InlineData("URL:", "www.ivany.com", "", false)]
        [InlineData("URL:", "www.ivany.com", "  ", false)]
        [InlineData("URL:", null, "ivan", false)]
        public void Single_line_text_field_contains_given_text(
            string label,
            string text,
            string searchText,
            bool expectedContains)
        {
            // Arrange

            var field = new SingleLineTextField
            {
                Label = label,
                Text = text
            };

            // Act

            var actualContains = field.Contains(searchText);

            // Assert

            Assert.Equal(expectedContains, actualContains);
        }

        [Theory]
        [InlineData("Description:", "This\nis\nthe\ntext", "This", true)]
        [InlineData("Description:", "This\nis\nthe\ntext", "this", true)]
        [InlineData("Description:", "This\nis\nthe\ntext", "Description", false)]
        [InlineData("Description:", "This\nis\nthe\ntext", null, false)]
        [InlineData("Description:", "This\nis\nthe\ntext", "", false)]
        [InlineData("Description:", "This\nis\nthe\ntext", "  ", false)]
        [InlineData("Description:", null, "ivan", false)]
        public void Multi_line_text_field_contains_given_text(
            string label,
            string text,
            string searchText,
            bool expectedContains)
        {
            // Arrange

            var field = new MultiLineTextField
            {
                Label = label,
                Text = text
            };

            // Act

            var actualContains = field.Contains(searchText);

            // Assert

            Assert.Equal(expectedContains, actualContains);
        }

        [Theory]
        [InlineData("Password:", "sj3jfbsj4fb", "jfbs", true)]
        [InlineData("Password:", "sj3jfbsj4fb", "jFBs", false)]
        [InlineData("Password:", "sj3jfbsj4fb", "Password", false)]
        [InlineData("Password:", "sj3jfbsj4fb", null, false)]
        [InlineData("Password:", "sj3jfbsj4fb", "", false)]
        [InlineData("Password:", "sj3jfbsj4fb", "  ", false)]
        [InlineData("Password:", null, "jfbs", false)]
        public void Password_field_contains_given_text(
            string label,
            string text,
            string searchText,
            bool expectedContains)
        {
            // Arrange

            var field = new PasswordField
            {
                Label = label,
                Text = text
            };

            // Act

            var actualContains = field.Contains(searchText);

            // Assert

            Assert.Equal(expectedContains, actualContains);
        }
    }
}
