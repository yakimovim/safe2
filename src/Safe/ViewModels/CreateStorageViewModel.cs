using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using Safe.Core.Domain;
using Safe.Core.Services;
using System;

namespace Safe.ViewModels
{
    public class CreateStorageViewModel : BindableBase
    {
        private string _password;
        private readonly IStorage _storage;
        private readonly IRegionManager _regionManager;

        public void SetPassword(string password)
        {
            _password = password;

            CreateStorageCommand.RaiseCanExecuteChanged();
        }

        public CreateStorageViewModel(
            IStorage storage,
            IRegionManager regionManager)
        {
            _storage = storage ?? throw new ArgumentNullException(nameof(storage));
            _regionManager = regionManager ?? throw new ArgumentNullException(nameof(regionManager));
            
            CreateStorageCommand = new DelegateCommand(CreateStorage, CanCreateStorage);
        }

        private bool CanCreateStorage() => !string.IsNullOrEmpty(_password);

        private void CreateStorage()
        {
            var password = new Password(_password);

            _storage.Create(password);

            _regionManager.RequestNavigate("ContentRegion", "ItemsView");
        }

        public DelegateCommand CreateStorageCommand { get; }
    }
}
