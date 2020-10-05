using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using Safe.Core.Domain;
using Safe.Core.Services;
using Safe.Services;
using System;

namespace Safe.ViewModels
{
    public class LoginViewModel : BindableBase, INavigationAware
    {
        private readonly IStorage _storage;
        private readonly INavigationService _navigationService;

        private string _password;
        public string Password
        {
            get { return _password; }
            set { SetProperty(ref _password, value); }
        }

        private bool _passwordIsIncorrect;
        public bool PasswordIsIncorrect
        {
            get { return _passwordIsIncorrect; }
            set { SetProperty(ref _passwordIsIncorrect, value); }
        }

        public LoginViewModel(
            IStorage storage,
            INavigationService navigationService)
        {
            _storage = storage ?? throw new ArgumentNullException(nameof(storage));
            _navigationService = navigationService ?? throw new ArgumentNullException(nameof(navigationService));

            LoginCommand = new DelegateCommand(Login, CanLogin).ObservesProperty(() => Password);
        }

        private bool CanLogin() => !string.IsNullOrEmpty(_password);

        private void Login()
        {
            var password = new Password(_password);

            if (_storage.Login(password))
            {
                _navigationService.NavigateMainContentTo("ItemsView");
            }
            else
            {
                PasswordIsIncorrect = true;
            }
        }

        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            if(_storage.LoggedIn)
            {
                _navigationService.NavigateMainContentTo("ItemsView");
            }

            PasswordIsIncorrect = false;
            Password = string.Empty;
        }

        public bool IsNavigationTarget(NavigationContext navigationContext) => true;

        public void OnNavigatedFrom(NavigationContext navigationContext) { }

        public DelegateCommand LoginCommand { get; }
    }
}
