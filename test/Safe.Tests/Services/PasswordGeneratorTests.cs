using AutoFixture.Xunit2;
using Safe.Core.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Xunit;

namespace Safe.Tests.Services
{
    public class PasswordGeneratorTests
    {
        private readonly PasswordGenerator _generator = new PasswordGenerator();

        [Fact]
        public void Dont_allow_non_positive_length()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() =>
                _generator.Generate(0, true, true, true)
            );
        }

        [Fact]
        public void Some_set_of_symbols_should_be_used()
        {
            Assert.Throws<ArgumentException>(() =>
                _generator.Generate(10, false, false, false)
            );
        }

        [Theory]
        [AutoData]
        public void Generated_password_should_be_of_requested_length(uint length)
        {
            // Act

            var password = _generator.Generate(length, true, true, true);

            // Assert

            Assert.Equal((int) length, password.Length);
        }

        [Theory]
        [InlineData(true, true, true)]
        [InlineData(true, true, false)]
        [InlineData(true, false, true)]
        [InlineData(false, true, true)]
        [InlineData(true, false, false)]
        [InlineData(false, false, true)]
        [InlineData(false, true, false)]
        public void Generated_password_must_contain_only_requested_symbols(
            bool useLetters,
            bool useDigits,
            bool usePunctuation
            )
        {
            // Act

            var password = _generator.Generate(10, useLetters, useDigits, usePunctuation);

            // Assert

            if(!useLetters)
            {
                Assert.DoesNotMatch("[a-zA-Z]", password);
            }
            if (!useDigits)
            {
                Assert.DoesNotMatch("[0-9]", password);
            }
            if (!usePunctuation)
            {
                Assert.DoesNotMatch("[!?@#$%&*]", password);
            }
        }
    }
}
