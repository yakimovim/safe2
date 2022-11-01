using Prism.Commands;
using Prism.Mvvm;
using Safe.Core.Services;
using Safe.Services;
using System;
using System.Windows;
using Microsoft.Win32;

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

            ExportCommand = new DelegateCommand(Export);

            ExitCommand = new DelegateCommand(Exit);
        }

        private void Export()
        {
            var openDialog = new OpenFileDialog
            {
                CheckFileExists = false,
                CheckPathExists = false,
                AddExtension = true,
                DefaultExt = ".json",
            };

            if (openDialog.ShowDialog() == true)
            {
                _storage.Export(openDialog.FileName);
            }
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
        
        public DelegateCommand ExportCommand { get; }

        public DelegateCommand ExitCommand { get; }
    }
}
