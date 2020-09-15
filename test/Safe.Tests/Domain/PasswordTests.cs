using AutoFixture.Xunit2;
using Safe.Core.Domain;
using System;
using Xunit;

namespace Safe.Tests.Domain
{
    public class PasswordTests
    {
        [Fact]
        public void Password_cant_be_null()
        {
            Assert.Throws<ArgumentNullException>(() => new Password(null));
        }

        [Fact]
        public void Password_cant_be_empty()
        {
            Assert.Throws<ArgumentNullException>(() => new Password(""));
        }

        [Theory]
        [AutoData]
        public void Password_should_be_convertable_to_string(string expectedPassword)
        {
            // Arrange

            var password = new Password(expectedPassword);

            // Act

            string actualPassword = password;

            // Assert

            Assert.Equal(expectedPassword, actualPassword);
        }
    }
}
