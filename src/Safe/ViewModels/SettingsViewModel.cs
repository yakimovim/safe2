using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using Safe.Core.Domain;
using Safe.Core.Services;
using Safe.Services;
using System;
using System.Collections.Generic;
using System.IO;

namespace Safe.ViewModels
{
    public class SettingsViewModel : BindableBase, INavigationAware
    {
        private readonly IConfigurationService _configurationService;
        private readonly IMapper _mapper;
        private readonly INavigationService _navigationService;
        private readonly IStorage _storage;
        private IRegionNavigationJournal _joirnal;

        public SettingsViewModel(
            IConfigurationService configurationService,
            IMapper mapper,
            INavigationService navigationService,
            IStorage storage)
        {
            _configurationService = configurationService ?? throw new ArgumentNullException(nameof(configurationService));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _navigationService = navigationService ?? throw new ArgumentNullException(nameof(navigationService));
            _storage = storage ?? throw new ArgumentNullException(nameof(storage));

            OkCommand = new DelegateCommand(SaveChanges);
            CancelCommand = new DelegateCommand(Cancel);
        }

        private void Cancel()
        {
            _joirnal.GoBack();
        }

        private void SaveChanges()
        {
            var oldConfiguration = _configurationService.GetConfiguration();

            var newConfiguration = new Configuration();

            _mapper.Map(oldConfiguration, newConfiguration);

            _mapper.Map(this, newConfiguration);

            _configurationService.SaveConfiguration(newConfiguration);

            if(newConfiguration.StoragePath != oldConfiguration.StoragePath)
            {
                if(_storage.LoggedIn)
                {
                    _storage.Logout();
                }

                if(File.Exists(newConfiguration.StoragePath))
                {
                    _navigationService.NavigateMainContentTo("LoginView");
                }
                else
                {
                    _navigationService.NavigateMainContentTo("CreateStorageViewModel");
                }
            }
            else
            {
                _joirnal.GoBack();
            }
        }

        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            _joirnal = navigationContext.NavigationService.Journal;

            var configuration = _configurationService.GetConfiguration();

            _mapper.Map(configuration, this);
        }

        public bool IsNavigationTarget(NavigationContext navigationContext) => true;

        public void OnNavigatedFrom(NavigationContext navigationContext) { }

        private string _storagePath;
        public string StoragePath
        {
            get { return _storagePath; }
            set { SetProperty(ref _storagePath, value); }
        }

        private string _language;

        public string Language
        {
            get { return _language; }
            set { SetProperty(ref _language, value); }
        }

        public IReadOnlyCollection<string> AvailableLanguages { get; }
            = new[] { "en-US", "ru-RU" };

        public DelegateCommand OkCommand { get; }

        public DelegateCommand CancelCommand { get; }
    }
}
