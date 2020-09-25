using Prism.Commands;
using Prism.Regions;
using Safe.Core.Domain;
using Safe.Services;
using System;
using System.Windows;

namespace Safe.ViewModels.Domain
{
    public abstract class FieldViewModel : EntityViewModel<Field, FieldViewModel>
    {
        private readonly INavigationService _navigationService;

        public DelegateCommand DeleteCommand { get; }

        public DelegateCommand MoveUpCommand { get; }

        public DelegateCommand MoveDownCommand { get; }

        protected FieldViewModel(
            Field model, 
            IContainer<FieldViewModel> parentContainer, 
            IMapper mapper,
            INavigationService navigationService) 
            : base(model, parentContainer, mapper)
        {
            _navigationService = navigationService ?? throw new ArgumentNullException(nameof(navigationService));

            DeleteCommand = new DelegateCommand(OnDelete);

            MoveUpCommand = new DelegateCommand(MoveUp, () => CanMoveUp);
            MoveDownCommand = new DelegateCommand(MoveDown, () => CanMoveDown);
        }

        public void RefreshPosition()
        {
            MoveUpCommand.RaiseCanExecuteChanged();
            MoveDownCommand.RaiseCanExecuteChanged();
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
        {
            CopyCommand = new DelegateCommand(CopyText);
        }

        public DelegateCommand CopyCommand { get; }

        private void CopyText()
        {
            Clipboard.SetText(Text ?? string.Empty);
        }

        private SingleLineTextField Field => (SingleLineTextField) Model;

        public string Text
        {
            get { return Field.Text; }
            set 
            { 
                if(Field.Text != value)
                {
                    Field.Text = value;
                    RaisePropertyChanged();
                }
            }
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
        {
            CopyCommand = new DelegateCommand(CopyText);
        }

        public DelegateCommand CopyCommand { get; }

        private void CopyText()
        {
            Clipboard.SetText(Text ?? string.Empty);
        }

        private MultiLineTextField Field => (MultiLineTextField)Model;

        public string Text
        {
            get { return Field.Text; }
            set
            {
                if (Field.Text != value)
                {
                    Field.Text = value;
                    RaisePropertyChanged();
                }
            }
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
        {
            CopyCommand = new DelegateCommand(CopyText);
        }

        public DelegateCommand CopyCommand { get; }

        private void CopyText()
        {
            Clipboard.SetText(Text ?? string.Empty);
        }

        private PasswordField Field => (PasswordField)Model;

        public string Text
        {
            get { return Field.Text; }
            set
            {
                if (Field.Text != value)
                {
                    Field.Text = value;
                    RaisePropertyChanged();
                }
            }
        }
    }
}
