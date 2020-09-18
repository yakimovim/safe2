using Prism.Commands;
using Prism.Regions;
using Safe.Core.Domain;
using Safe.Services;
using System;

namespace Safe.ViewModels.Domain
{
    public abstract class FieldViewModel : EntityViewModel<Field, FieldViewModel>
    {
        private readonly INavigationService _navigationService;

        public DelegateCommand DeleteCommand { get; }

        public DelegateCommand EditCommand { get; }

        protected FieldViewModel(
            Field model, 
            IContainer<FieldViewModel> parentContainer, 
            IMapper mapper,
            INavigationService navigationService) 
            : base(model, parentContainer, mapper)
        {
            _navigationService = navigationService ?? throw new System.ArgumentNullException(nameof(navigationService));

            DeleteCommand = new DelegateCommand(OnDelete);
            EditCommand = new DelegateCommand(OnEdit);
        }

        private void OnDelete()
        {
            _navigationService.ShowYesNoDialog("Do you want to delete this field?", res => {
                if (res.Result == Prism.Services.Dialogs.ButtonResult.Yes)
                {
                    Delete();
                }
            });
        }

        private void OnEdit()
        {
            var p = new NavigationParameters();
            p.Add("Container", ParentContainer);
            p.Add("Item", this);
            p.Add("IsEditing", true);

            _navigationService.NavigateMainContentTo("EditFieldView", p);
        }

        public FieldTypes Type => Model.Type;

        private string _label;

        public string Label
        {
            get { return _label; }
            set { SetProperty(ref _label, value); }
        }

        internal static FieldViewModel Create(
            Field f, 
            IContainer<FieldViewModel> container, 
            IMapper mapper, 
            INavigationService navigationService)
        {
            switch(f.Type)
            {
                case FieldTypes.SingleLineText:
                    return new SingleLineTextFieldViewModel((SingleLineTextField)f, container, mapper, navigationService);
                case FieldTypes.MultiLineText:
                    return new MultiLineTextFieldViewModel((MultiLineTextField)f, container, mapper, navigationService);
                case FieldTypes.Password:
                    return new PasswordFieldViewModel((PasswordField)f, container, mapper, navigationService);
                default:
                    throw new ArgumentException();
            }
        }
    }

    public sealed class SingleLineTextFieldViewModel : FieldViewModel
    {
        public SingleLineTextFieldViewModel(
            SingleLineTextField model, 
            IContainer<FieldViewModel> parentContainer, 
            IMapper mapper,
            INavigationService navigationService) 
            : base(model, parentContainer, mapper, navigationService)
        { }

        private string _text;
        public string Text
        {
            get { return _text; }
            set { SetProperty(ref _text, value); }
        }
    }

    public sealed class MultiLineTextFieldViewModel : FieldViewModel
    {
        public MultiLineTextFieldViewModel(
            MultiLineTextField model,
            IContainer<FieldViewModel> parentContainer,
            IMapper mapper,
            INavigationService navigationService)
            : base(model, parentContainer, mapper, navigationService)
        { }

        private string _text;
        public string Text
        {
            get { return _text; }
            set { SetProperty(ref _text, value); }
        }
    }

    public sealed class PasswordFieldViewModel : FieldViewModel
    {
        public PasswordFieldViewModel(
            PasswordField model,
            IContainer<FieldViewModel> parentContainer,
            IMapper mapper,
            INavigationService navigationService)
            : base(model, parentContainer, mapper, navigationService)
        { }

        private string _text;
        public string Text
        {
            get { return _text; }
            set { SetProperty(ref _text, value); }
        }
    }
}
