using Prism.Commands;
using Prism.Mvvm;
using Safe.Core.Services;
using Safe.Services;
using System;
using System.Windows;

namespace Safe.ViewModels
{
    public class MainWindowViewModel : BindableBase
    {
        private readonly INavigationService _navigationService;
        private readonly IStorage _storage;

        public MainWindowViewModel(
            INavigationService navigationService,
            IStorage storage)
        {
            _navigationService = navigationService ?? throw new ArgumentNullException(nameof(navigationService));
            _storage = storage ?? throw new ArgumentNullException(nameof(storage));

            _storage.LoggedInChanged += OnLoggedInChanged;

            GeneratePasswordCommand = new DelegateCommand(GeneratePassword);

            ChangePasswordCommand = new DelegateCommand(ChangePassword)
                .ObservesCanExecute(() => CanChangePassword);

            SettingsCommand = new DelegateCommand(Settings);

            ExitCommand = new DelegateCommand(Exit);
        }

        private void GeneratePassword()
        {
            _navigationService.ShowPasswordGenerationDialog();
        }

        private void OnLoggedInChanged(object sender, EventArgs e)
        {
            RaisePropertyChanged(nameof(CanChangePassword));
        }

        private void ChangePassword()
        {
            _navigationService.NavigateMainContentTo("ChangePasswordView");
        }

        public bool CanChangePassword => _storage.LoggedIn;

        private void Settings()
        {
            _navigationService.NavigateMainContentTo("SettingsView");
        }

        private void Exit()
        {
            Application.Current.Shutdown();
        }

        public DelegateCommand GeneratePasswordCommand { get; }

        public DelegateCommand ChangePasswordCommand { get; }

        public DelegateCommand SettingsCommand { get; }

        public DelegateCommand ExitCommand { get; }
    }
}
