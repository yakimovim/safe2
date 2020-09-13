using Safe.Core.Domain;
using System.Collections.Generic;
using Xunit;

namespace Safe.Tests.Domain
{
    public class ItemTests
    {
        [Theory]
        [MemberData(nameof(GetTestData))]
        public void Item_contains_text_in_title(string title, string text, bool expectedContains)
        {
            // Arrange

            var item = new Item
            {
                Title = title
            };

            // Act

            var actualContains = item.Contains(text);

            // Assert

            Assert.Equal(expectedContains, actualContains);
        }

        [Theory]
        [MemberData(nameof(GetTestData))]
        public void Item_contains_text_in_description(string description, string text, bool expectedContains)
        {
            // Arrange

            var item = new Item
            {
                Description = description
            };

            // Act

            var actualContains = item.Contains(text);

            // Assert

            Assert.Equal(expectedContains, actualContains);
        }

        [Theory]
        [MemberData(nameof(GetTestData))]
        public void Item_contains_text_in_tag(string tag, string text, bool expectedContains)
        {
            if (string.IsNullOrWhiteSpace(tag)) return;

            // Arrange

            var item = new Item();
            item.Tags.Add(tag);

            // Act

            var actualContains = item.Contains(text);

            // Assert

            Assert.Equal(expectedContains, actualContains);
        }

        [Theory]
        [MemberData(nameof(GetTestData))]
        public void Item_contains_text_in_field(string fieldText, string text, bool expectedContains)
        {
            // Arrange

            var item = new Item();
            item.Fields.Add(new SingleLineTextField { 
                Label = "Text",
                Text = fieldText
            });

            // Act

            var actualContains = item.Contains(text);

            // Assert

            Assert.Equal(expectedContains, actualContains);
        }

        public static IEnumerable<object[]> GetTestData()
        {
            yield return new object[] { "Google", "Google", true };
            yield return new object[] { "Google", "google", true };
            yield return new object[] { "Google", null, false };
            yield return new object[] { "Google", "", false };
            yield return new object[] { "Google", "   ", false };
            yield return new object[] { null, "Google", false };
        }
    }
}
