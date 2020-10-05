using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using Safe.Core.Domain;
using Safe.Core.Services;
using Safe.Services;
using System;

namespace Safe.ViewModels
{
    public class ChangePasswordViewModel : BindableBase, INavigationAware
    {
        private readonly IStorage _storage;
        private readonly INavigationService _navigationService;

        private string _oldPassword;
        public string OldPassword
        {
            get { return _oldPassword; }
            set { 
                if(SetProperty(ref _oldPassword, value))
                {
                    RaisePropertyChanged(nameof(OldPasswordIsEmpty));
                }
                OkCommand.RaiseCanExecuteChanged();
            }
        }

        public bool OldPasswordIsEmpty => string.IsNullOrWhiteSpace(OldPassword);

        private string _newPassword1;
        public string NewPassword1
        {
            get { return _newPassword1; }
            set { 
                if(SetProperty(ref _newPassword1, value))
                {
                    RaisePropertyChanged(nameof(NewPasswordIsEmpty));
                    RaisePropertyChanged(nameof(NewPasswordIsNotValid));
                }
                OkCommand.RaiseCanExecuteChanged();
            }
        }

        public bool NewPasswordIsEmpty => string.IsNullOrWhiteSpace(NewPassword1);

        private string _newPassword2;
        public string NewPassword2
        {
            get { return _newPassword2; }
            set { 
                if(SetProperty(ref _newPassword2, value))
                {
                    RaisePropertyChanged(nameof(NewPasswordIsNotValid));
                }
                OkCommand.RaiseCanExecuteChanged();
            }
        }

        public bool NewPasswordIsNotValid => NewPassword1 != NewPassword2;

        public ChangePasswordViewModel(
            IStorage storage,
            INavigationService navigationService)
        {
            _storage = storage ?? throw new ArgumentNullException(nameof(storage));
            _navigationService = navigationService ?? throw new ArgumentNullException(nameof(navigationService));

            OkCommand = new DelegateCommand(ChangePassword, CanChangePassword);
            CancelCommand = new DelegateCommand(Cancel);
        }

        private bool CanChangePassword()
        {
            if (string.IsNullOrEmpty(OldPassword)) return false;
            if (string.IsNullOrEmpty(NewPassword1)) return false;
            if (string.IsNullOrEmpty(NewPassword2)) return false;

            if (NewPassword1 != NewPassword2) return false;

            return true;
        }

        private void ChangePassword()
        {
            try
            {
                _storage.ChangePassword(
                    new Password(OldPassword),
                    new Password(NewPassword1)
                );

                _navigationService.NavigateMainContentTo("ItemsView");
            }
            catch (Exception)
            {
            }
        }

        private void Cancel()
        {
            _navigationService.NavigateMainContentTo("ItemsView");
        }

        public bool IsNavigationTarget(NavigationContext navigationContext) => true;

        public void OnNavigatedFrom(NavigationContext navigationContext) { }

        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            if(!_storage.LoggedIn)
            {
                _navigationService.NavigateMainContentTo("LoginView");
                return;
            }

            OldPassword = string.Empty;
            NewPassword1 = string.Empty;
            NewPassword2 = string.Empty;

            OkCommand.RaiseCanExecuteChanged();
        }

        public DelegateCommand OkCommand { get; }

        public DelegateCommand CancelCommand { get; }
    }
}
