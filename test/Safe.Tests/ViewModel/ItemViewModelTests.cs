using Moq;
using Safe.Core.Domain;
using Safe.Services;
using Safe.ViewModels.Domain;
using System.Linq;
using Xunit;

namespace Safe.Tests.ViewModel
{
    public class ItemViewModelTests
    {
        private readonly INavigationService _navigationService
            = new Mock<INavigationService>().Object;
        private readonly IMapper _mapper;

        public ItemViewModelTests()
        {
            _mapper = new Mapper(_navigationService);
        }

        [Fact]
        public void Add_fields()
        {
            // Arrange

            var item = new Item();

            var itemViewModel = new ItemViewModel(item, null, _mapper, _navigationService);

            var fieldViewModel = new SingleLineTextFieldViewModel(new SingleLineTextField(), itemViewModel, _mapper, _navigationService);

            // Act

            itemViewModel.Add(fieldViewModel);

            // Assert

            Assert.Same(
                fieldViewModel,
                Assert.Single(itemViewModel.Fields)
            );

            Assert.Empty(item.Fields);
        }

        [Fact]
        public void Add_fields_and_save_to_model()
        {
            // Arrange

            var item = new Item();

            var itemViewModel = new ItemViewModel(item, null, _mapper, _navigationService);

            var field = new SingleLineTextField();

            var fieldViewModel = new SingleLineTextFieldViewModel(field, itemViewModel, _mapper, _navigationService);

            // Act

            itemViewModel.Add(fieldViewModel);

            itemViewModel.FillModel();

            // Assert

            Assert.Same(
                fieldViewModel,
                Assert.Single(itemViewModel.Fields)
            );

            Assert.Same(
                field,
                Assert.Single(item.Fields)
            );
        }

        [Fact]
        public void Delete_fields()
        {
            // Arrange

            var item = new Item();

            item.Fields.Add(new SingleLineTextField());

            var itemViewModel = new ItemViewModel(item, null, _mapper, _navigationService);

            // Act

            itemViewModel.Delete(itemViewModel.Fields.Single());

            // Assert

            Assert.Empty(itemViewModel.Fields);

            Assert.Single(item.Fields);
        }

        [Fact]
        public void Delete_fields_and_save()
        {
            // Arrange

            var item = new Item();

            item.Fields.Add(new SingleLineTextField());

            var itemViewModel = new ItemViewModel(item, null, _mapper, _navigationService);

            // Act

            itemViewModel.Delete(itemViewModel.Fields.Single());

            itemViewModel.FillModel();

            // Assert

            Assert.Empty(itemViewModel.Fields);

            Assert.Empty(item.Fields);
        }

        [Fact]
        public void Move_field_up()
        {
            // Arrange

            var item = new Item();

            item.Fields.Add(new SingleLineTextField());
            item.Fields.Add(new PasswordField());

            var itemViewModel = new ItemViewModel(item, null, _mapper, _navigationService);

            // Act

            itemViewModel.MoveUp(itemViewModel.Fields[1]);

            // Assert

            Assert.IsType<PasswordFieldViewModel>(itemViewModel.Fields[0]);
            Assert.IsType<SingleLineTextFieldViewModel>(itemViewModel.Fields[1]);

            Assert.IsType<SingleLineTextField>(item.Fields[0]);
            Assert.IsType<PasswordField>(item.Fields[1]);
        }

        [Fact]
        public void Move_field_up_and_save()
        {
            // Arrange

            var item = new Item();

            item.Fields.Add(new SingleLineTextField());
            item.Fields.Add(new PasswordField());

            var itemViewModel = new ItemViewModel(item, null, _mapper, _navigationService);

            // Act

            itemViewModel.MoveUp(itemViewModel.Fields[1]);

            itemViewModel.FillModel();

            // Assert

            Assert.IsType<PasswordFieldViewModel>(itemViewModel.Fields[0]);
            Assert.IsType<SingleLineTextFieldViewModel>(itemViewModel.Fields[1]);

            Assert.IsType<PasswordField>(item.Fields[0]);
            Assert.IsType<SingleLineTextField>(item.Fields[1]);
        }

        [Fact]
        public void Move_field_down()
        {
            // Arrange

            var item = new Item();

            item.Fields.Add(new SingleLineTextField());
            item.Fields.Add(new PasswordField());

            var itemViewModel = new ItemViewModel(item, null, _mapper, _navigationService);

            // Act

            itemViewModel.MoveDown(itemViewModel.Fields[0]);

            // Assert

            Assert.IsType<PasswordFieldViewModel>(itemViewModel.Fields[0]);
            Assert.IsType<SingleLineTextFieldViewModel>(itemViewModel.Fields[1]);

            Assert.IsType<SingleLineTextField>(item.Fields[0]);
            Assert.IsType<PasswordField>(item.Fields[1]);
        }

        [Fact]
        public void Move_field_down_and_save()
        {
            // Arrange

            var item = new Item();

            item.Fields.Add(new SingleLineTextField());
            item.Fields.Add(new PasswordField());

            var itemViewModel = new ItemViewModel(item, null, _mapper, _navigationService);

            // Act

            itemViewModel.MoveDown(itemViewModel.Fields[0]);

            itemViewModel.FillModel();

            // Assert

            Assert.IsType<PasswordFieldViewModel>(itemViewModel.Fields[0]);
            Assert.IsType<SingleLineTextFieldViewModel>(itemViewModel.Fields[1]);

            Assert.IsType<PasswordField>(item.Fields[0]);
            Assert.IsType<SingleLineTextField>(item.Fields[1]);
        }
    }
}
