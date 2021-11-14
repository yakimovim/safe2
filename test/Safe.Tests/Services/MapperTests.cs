using AutoFixture.Xunit2;
using Moq;
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
        private readonly INavigationService _navigationService
            = new Mock<INavigationService>().Object;
        private readonly Mapper _mapper;

        public MapperTests()
        {
            _mapper = new Mapper(_navigationService);
        }

        [Theory]
        [AutoData]
        public void From_configuration_to_settings(string storagePath)
        {
            // Arrange

            var settings = new Settings();

            var configuration = new Configuration
            {
                Salt = Encoding.ASCII.GetBytes(_salt),
                StoragePath = storagePath
            };

            // Act

            _mapper.Map(configuration, settings);

            // Assert

            Assert.Equal(storagePath, settings.StoragePath);
            Assert.Equal(_salt, settings.Salt);
        }

        [Theory]
        [AutoData]
        public void From_settings_to_configuration(string storagePath)
        {
            // Arrange

            var settings = new Settings
            {
                StoragePath = storagePath,
                Salt = _salt
            };

            var configuration = new Configuration();

            // Act

            _mapper.Map(settings, configuration);

            // Assert

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

            var viewModel = new SingleLineTextFieldViewModel(field, null, _mapper, _navigationService);

            // Act

            viewModel.RefreshFromModel();

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

            var viewModel = new SingleLineTextFieldViewModel(field, null, _mapper, _navigationService)
            {
                Label = label,
                Text = text
            };

            // Act

            viewModel.FillModel();

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

            var viewModel = new MultiLineTextFieldViewModel(field, null, _mapper, _navigationService);

            // Act

            viewModel.RefreshFromModel();

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

            var viewModel = new MultiLineTextFieldViewModel(field, null, _mapper, _navigationService)
            {
                Label = label,
                Text = text
            };

            // Act

            viewModel.FillModel();

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

            var viewModel = new PasswordFieldViewModel(field, null, _mapper, _navigationService);

            // Act

            viewModel.RefreshFromModel();

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

            var viewModel = new PasswordFieldViewModel(field, null, _mapper, _navigationService)
            {
                Label = label,
                Text = text
            };

            // Act

            viewModel.FillModel();

            // Assert

            Assert.Equal(label, field.Label);
            Assert.Equal(text, field.Text);
        }

        [Theory]
        [AutoData]
        public void From_item_to_view_model(string title, string description, string label)
        {
            // Arrange

            var item = new Item
            {
                Title = title,
                Description = description,
                Tags = { "1", "2", "3" },
                Fields = {
                    new SingleLineTextField
                    {
                        Label = label
                    }
                }
            };

            var viewModel = new ItemViewModel(item, null, _mapper, _navigationService);

            // Act

            viewModel.RefreshFromModel();

            // Assert

            Assert.Equal(title, viewModel.Title);
            Assert.Equal(description, viewModel.Description);
            Assert.Equal("1, 2, 3", viewModel.Tags);

            var field = Assert.Single(viewModel.Fields);
            Assert.Equal(FieldTypes.SingleLineText, field.Type);
            Assert.Equal(label, field.Label);
        }

        [Theory]
        [AutoData]
        public void From_view_model_to_item(string title, string description, string label)
        {
            // Arrange

            var item = new Item();

            var viewModel = new ItemViewModel(item, null, _mapper, _navigationService)
            {
                Title = title,
                Description = description,
                Tags = "1, 2, 3",
                Fields =
                {
                    new SingleLineTextFieldViewModel(new SingleLineTextField(), null, _mapper, _navigationService)
                    {
                        Label = label
                    }
                }
            };

            // Act

            viewModel.FillModel();

            // Assert

            Assert.Equal(title, item.Title);
            Assert.Equal(description, item.Description);
            Assert.Equal(new[] { "1", "2", "3" }, item.Tags);

            var field = Assert.Single(item.Fields);

            Assert.Equal(FieldTypes.SingleLineText, field.Type);
            Assert.Equal(label, field.Label);
        }
    }
}
