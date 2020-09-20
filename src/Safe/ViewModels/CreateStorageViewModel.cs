using Prism.Commands;
using Prism.Mvvm;
using Safe.Core.Domain;
using Safe.Core.Services;
using Safe.Services;
using System;

namespace Safe.ViewModels
{
    public class CreateStorageViewModel : BindableBase
    {
        private readonly IStorage _storage;
        private readonly INavigationService _navigationService;

        private string _password;
        public string Password
        {
            get { return _password; }
            set { SetProperty(ref _password, value); }
        }

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

        public DelegateCommand CreateStorageCommand { get; }
    }
}
