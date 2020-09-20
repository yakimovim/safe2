using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using Safe.Core.Domain;
using Safe.Services;
using Safe.ViewModels.Domain;
using System.Windows;

namespace Safe.ViewModels
{
    public class EditFieldViewModel : BindableBase, INavigationAware
    {
        private IRegionNavigationJournal _journal;
        private IContainer<FieldViewModel> _container;

        private FieldViewModel Item { get; set; }

        private bool _isEditing;
        public bool IsEditing
        {
            get { return _isEditing; }
            set { SetProperty(ref _isEditing, value); }
        }

        private int _type;
        public int Type
        {
            get { return _type; }
            set { SetProperty(ref _type, value); }
        }

        private string _label;
        private readonly IMapper _mapper;
        private readonly INavigationService _navigationService;

        public string Label
        {
            get { return _label; }
            set { SetProperty(ref _label, value); }
        }

        public DelegateCommand OkCommand { get; }

        public DelegateCommand CancelCommand { get; }

        public EditFieldViewModel(
            IMapper mapper,
            INavigationService navigationService)
        {
            _mapper = mapper ?? throw new System.ArgumentNullException(nameof(mapper));
            _navigationService = navigationService ?? throw new System.ArgumentNullException(nameof(navigationService));
            OkCommand = new DelegateCommand(Save);
            CancelCommand = new DelegateCommand(Cancel);
        }

        private void Save()
        {
            if(!IsEditing)
            {
                switch (Type)
                {
                    case 0:
                        {
                            Item = new SingleLineTextFieldViewModel(new SingleLineTextField(), _container, _mapper, _navigationService);
                            break;
                        }
                    case 1:
                        {
                            Item = new PasswordFieldViewModel(new PasswordField(), _container, _mapper, _navigationService);
                            break;
                        }
                    case 2:
                        {
                            Item = new MultiLineTextFieldViewModel(new MultiLineTextField(), _container, _mapper, _navigationService);
                            break;
                        }
                }
            }

            Item.Label = Label;

            Item.FillModel();

            if(!IsEditing)
            {
                Item.Add();
            }

            _journal.GoBack();
        }

        private void Cancel()
        {
            _journal.GoBack();
        }

        public bool IsNavigationTarget(NavigationContext navigationContext) => true;

        public void OnNavigatedFrom(NavigationContext navigationContext) { }

        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            _journal = navigationContext.NavigationService.Journal;

            // This is a back navigation.
            if (!navigationContext.Parameters.ContainsKey("Container")) return;

            _container = navigationContext.Parameters.GetValue<IContainer<FieldViewModel>>("Container");

            if(navigationContext.Parameters.ContainsKey("Item"))
            {
                Item = navigationContext.Parameters.GetValue<FieldViewModel>("Item");
                Type = (int)Item.Model.Type;
            }
            else
            {
                Item = new SingleLineTextFieldViewModel(new SingleLineTextField(), _container, _mapper, _navigationService);
                Type = 0;
            }

            Label = Item.Label;

            IsEditing = navigationContext.Parameters.ContainsKey("IsEditing")
                ? navigationContext.Parameters.GetValue<bool>("IsEditing")
                : false;
        }
    }
}
