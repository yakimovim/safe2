using AutoFixture.Xunit2;
using Safe.Core.Domain;
using Safe.Services;
using Safe.ViewModels.Domain;
using System.Text;
using Xunit;

namespace Safe.Tests.Services
{
    public class MapperTests
    {
        private readonly string _salt = "0C9F222E-426A-409C-9026-5D15E49A5E0B";
        private readonly Mapper _mapper = new Mapper();

        [Theory]
        [AutoData]
        public void From_configuration_to_settings(string language, string storagePath)
        {
            // Arrange

            var settings = new Settings();

            var configuration = new Configuration
            {
                Salt = Encoding.ASCII.GetBytes(_salt),
                Language = language,
                StoragePath = storagePath
            };

            // Act

            _mapper.Map(configuration, settings);

            // Assert

            Assert.Equal(language, settings.Language);
            Assert.Equal(storagePath, settings.StoragePath);
            Assert.Equal(_salt, settings.Salt);
        }

        [Theory]
        [AutoData]
        public void From_settings_to_configuration(string language, string storagePath)
        {
            // Arrange

            var settings = new Settings
            {
                Language = language,
                StoragePath = storagePath,
                Salt = _salt
            };

            var configuration = new Configuration();

            // Act

            _mapper.Map(settings, configuration);

            // Assert

            Assert.Equal(language, configuration.Language);
            Assert.Equal(storagePath, configuration.StoragePath);
            Assert.Equal(Encoding.ASCII.GetBytes(_salt), configuration.Salt);
        }

        [Theory]
        [AutoData]
        public void From_text_field_to_view_model(string label, string text)
        {
            // Arrange

            var field = new SingleLineTextField
            {
                Label = label,
                Text = text
            };

            var viewModel = new SingleLineTextFieldViewModel(field, null, _mapper);

            // Act

            _mapper.Map(field, viewModel);

            // Assert

            Assert.Equal(label, viewModel.Label);
            Assert.Equal(text, viewModel.Text);
        }

        [Theory]
        [AutoData]
        public void From_view_model_to_text_field(string label, string text)
        {
            // Arrange

            var field = new SingleLineTextField();

            var viewModel = new SingleLineTextFieldViewModel(field, null, _mapper)
            {
                Label = label,
                Text = text
            };

            // Act

            _mapper.Map(viewModel, field);

            // Assert

            Assert.Equal(label, field.Label);
            Assert.Equal(text, field.Text);
        }

        [Theory]
        [AutoData]
        public void From_multiline_text_field_to_view_model(string label, string text)
        {
            // Arrange

            var field = new MultiLineTextField
            {
                Label = label,
                Text = text
            };

            var viewModel = new MultiLineTextFieldViewModel(field, null, _mapper);

            // Act

            _mapper.Map(field, viewModel);

            // Assert

            Assert.Equal(label, viewModel.Label);
            Assert.Equal(text, viewModel.Text);
        }

        [Theory]
        [AutoData]
        public void From_view_model_to_multiline_text_field(string label, string text)
        {
            // Arrange

            var field = new MultiLineTextField();

            var viewModel = new MultiLineTextFieldViewModel(field, null, _mapper)
            {
                Label = label,
                Text = text
            };

            // Act

            _mapper.Map(viewModel, field);

            // Assert

            Assert.Equal(label, field.Label);
            Assert.Equal(text, field.Text);
        }

        [Theory]
        [AutoData]
        public void From_password_field_to_view_model(string label, string text)
        {
            // Arrange

            var field = new PasswordField
            {
                Label = label,
                Text = text
            };

            var viewModel = new PasswordFieldViewModel(field, null, _mapper);

            // Act

            _mapper.Map(field, viewModel);

            // Assert

            Assert.Equal(label, viewModel.Label);
            Assert.Equal(text, viewModel.Text);
        }

        [Theory]
        [AutoData]
        public void From_view_model_to_password_field(string label, string text)
        {
            // Arrange

            var field = new PasswordField();

            var viewModel = new PasswordFieldViewModel(field, null, _mapper)
            {
                Label = label,
                Text = text
            };

            // Act

            _mapper.Map(viewModel, field);

            // Assert

            Assert.Equal(label, field.Label);
            Assert.Equal(text, field.Text);
        }
    }
}
