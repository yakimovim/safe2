using Prism.Commands;
using Prism.Mvvm;
using Safe.Services;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;

namespace Safe.ViewModels
{
    public class MainWindowViewModel : BindableBase
    {
        private readonly INavigationService _navigationService;

        public MainWindowViewModel(INavigationService navigationService)
        {
            _navigationService = navigationService ?? throw new System.ArgumentNullException(nameof(navigationService));

            SettingsCommand = new DelegateCommand(Settings);

            ExitCommand = new DelegateCommand(Exit);
        }

        private void Settings()
        {
            _navigationService.NavigateMainContentTo("SettingsView");
        }

        private void Exit()
        {
            Application.Current.Shutdown();
        }

        public DelegateCommand SettingsCommand { get; }

        public DelegateCommand ExitCommand { get; }
    }
}
