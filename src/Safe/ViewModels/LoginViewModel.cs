using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using Safe.Core.Domain;
using Safe.Core.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Safe.ViewModels
{
    public class LoginViewModel : BindableBase
    {
        private string _password;
        private readonly IStorage _storage;
        private readonly IRegionManager _regionManager;

        public void SetPassword(string password)
        {
            _password = password;

            LoginCommand.RaiseCanExecuteChanged();
        }
        public LoginViewModel(
            IStorage storage,
            IRegionManager regionManager)
        {
            _storage = storage ?? throw new ArgumentNullException(nameof(storage));
            _regionManager = regionManager ?? throw new ArgumentNullException(nameof(regionManager));

            LoginCommand = new DelegateCommand(Login, CanLogin);
        }

        private bool CanLogin() => !string.IsNullOrEmpty(_password);

        private void Login()
        {
            var password = new Password(_password);

            if (_storage.Login(password))
            {
                _regionManager.RequestNavigate("ContentRegion", "ItemsView");
            }
        }

        public DelegateCommand LoginCommand { get; }

    }
}
