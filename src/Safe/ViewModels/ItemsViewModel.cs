﻿using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using Safe.Core.Domain;
using Safe.Core.Services;
using Safe.Services;
using Safe.ViewModels.Domain;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Windows;

namespace Safe.ViewModels
{
    public class ItemsViewModel 
        : BindableBase, INavigationAware, IContainer<ItemViewModel>
    {
        private readonly IStorage _storage;
        private readonly INavigationService _navigationService;
        private readonly IConfigurationService _configurationService;
        private readonly IMapper _mapper;

        private string _storagePath;
        private Container _container;
        private List<ItemViewModel> _allItems;

        private string _searchText;
        private readonly Subject<string> _searchSubject = new Subject<string>();

        public string SearchText
        {
            get { return _searchText; }
            set 
            { 
                if(SetProperty(ref _searchText, value))
                {
                    _searchSubject.OnNext(_searchText);
                }
            }
        }

        public ObservableCollection<ItemViewModel> Items { get; } 
            = new ObservableCollection<ItemViewModel>();

        public ItemsViewModel(
            IStorage storage,
            IMapper mapper,
            INavigationService navigationService,
            IConfigurationService configurationService
            )
        {
            _storage = storage ?? throw new ArgumentNullException(nameof(storage));
            _navigationService = navigationService ?? throw new ArgumentNullException(nameof(navigationService));
            _configurationService = configurationService ?? throw new ArgumentNullException(nameof(configurationService));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));

            Application.Current.Exit += OnApplicationExit;

            CreateNewItemCommand = new DelegateCommand(CreateNewItem);

            _searchSubject
                .Throttle(TimeSpan.FromSeconds(1))
                .ObserveOnDispatcher()
                .Subscribe(searchText =>
                {
                    UpdateVisibleItems();
                });
        }

        private void UpdateVisibleItems()
        {
            var visibleItems = _allItems.AsEnumerable();

            if(!string.IsNullOrWhiteSpace(SearchText))
            {
                visibleItems = visibleItems.Where(i => i.Model.Contains(SearchText));
            }

            Items.Clear();
            Items.AddRange(visibleItems.Take(20));
        }

        private void OnApplicationExit(object sender, ExitEventArgs e)
        {
            _storage.Save(_container);
        }

        private void CreateNewItem()
        {
            var p = new NavigationParameters();
            p.Add("Item", new ItemViewModel(new Item(), this, _mapper, _navigationService));
            p.Add("IsEditing", false);

            _navigationService.NavigateMainContentTo("EditItemView", p);
        }

        public void Add(ItemViewModel item)
        {
            if (item is null)
            {
                throw new ArgumentNullException(nameof(item));
            }

            _allItems.Add(item);

            _container.Items.Add(item.Model);

            UpdateVisibleItems();
        }

        public void Delete(ItemViewModel item)
        {
            if (item is null)
            {
                throw new ArgumentNullException(nameof(item));
            }

            _allItems.Remove(item);

            _container.Items.Remove(item.Model);

            UpdateVisibleItems();
        }

        public bool CanMoveUp(ItemViewModel item) => false;

        public bool CanMoveDown(ItemViewModel item) => false;

        public void MoveUp(ItemViewModel item) { }

        public void MoveDown(ItemViewModel item) { }

        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            if(!_storage.Exists)
            {
                _navigationService.NavigateMainContentTo("CreateStorageView");
                return;
            }

            if(!_storage.LoggedIn)
            {
                _navigationService.NavigateMainContentTo("LoginView");
                return;
            }

            var configuration = _configurationService.GetConfiguration();

            if(_storagePath != configuration.StoragePath)
            {
                _storagePath = configuration.StoragePath;

                _container = _storage.Read();

                _allItems = _container.Items
                    .Select(i => new ItemViewModel(i, this, _mapper, _navigationService))
                    .ToList();
            }

            UpdateVisibleItems();
        }

        public bool IsNavigationTarget(NavigationContext navigationContext) => true;

        public void OnNavigatedFrom(NavigationContext navigationContext) { }

        public DelegateCommand CreateNewItemCommand { get; }
    }
}
