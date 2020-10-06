using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using Safe.Core.Domain;
using Safe.Core.Services;
using Safe.Services;
using System;

namespace Safe.ViewModels
{
    public class CreateStorageViewModel : BindableBase, INavigationAware
    {
        private readonly IStorage _storage;
        private readonly INavigationService _navigationService;

        private string _password;
        public string Password
        {
            get { return _password; }
            set 
            { 
                if(SetProperty(ref _password, value))
                {
                    RaisePropertyChanged(nameof(PasswordIsEmpty));
                }
            }
        }

        public bool PasswordIsEmpty => string.IsNullOrWhiteSpace(Password);

        public CreateStorageViewModel(
            IStorage storage,
            INavigationService navigationService)
        {
            _storage = storage ?? throw new ArgumentNullException(nameof(storage));
            _navigationService = navigationService ?? throw new ArgumentNullException(nameof(navigationService));
            
            CreateStorageCommand = new DelegateCommand(CreateStorage, CanCreateStorage)
                .ObservesProperty(() => Password);
        }

        private bool CanCreateStorage() => !string.IsNullOrEmpty(_password);

        private void CreateStorage()
        {
            var password = new Password(_password);

            _storage.Create(password);

            _storage.Login(password);

            _navigationService.NavigateMainContentTo("ItemsView");
        }

        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            if(_storage.Exists)
            {
                _navigationService.NavigateMainContentTo("LoginView");
            }

            Password = string.Empty;
        }

        public bool IsNavigationTarget(NavigationContext navigationContext) => true;

        public void OnNavigatedFrom(NavigationContext navigationContext) { }

        public DelegateCommand CreateStorageCommand { get; }
    }
}
