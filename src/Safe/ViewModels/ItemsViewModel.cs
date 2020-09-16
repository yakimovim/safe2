using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Prism.Regions;
using Safe.Core.Domain;
using Safe.Core.Services;
using Safe.Events;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Safe.ViewModels
{
    public class ItemsViewModel : BindableBase, INavigationAware
    {
        private readonly IStorage _storage;
        private readonly IRegionManager _regionManager;
        private readonly IEventAggregator _eventAggregator;
        private readonly Container _container;
        private readonly List<ItemViewModel> _allItems;

        private string _searchText;

        public string SearchText
        {
            get { return _searchText; }
            set { SetProperty(ref _searchText, value); }
        }

        public ObservableCollection<ItemViewModel> Items { get; } = new ObservableCollection<ItemViewModel>();

        public ItemsViewModel(
            IStorage storage,
            IRegionManager regionManager,
            IEventAggregator eventAggregator
            )
        {
            _storage = storage ?? throw new ArgumentNullException(nameof(storage));
            _regionManager = regionManager ?? throw new ArgumentNullException(nameof(regionManager));
            _eventAggregator = eventAggregator ?? throw new ArgumentNullException(nameof(eventAggregator));
            _container = _storage.Read();

            _allItems = _container.Items.Select(i => new ItemViewModel(i, _eventAggregator)).ToList();

            Items.AddRange(_allItems);

            CreateNewItemCommand = new DelegateCommand(CreateNewItem);

            _eventAggregator.GetEvent<DeleteItemEvent>().Subscribe(OnDelete);
        }

        private void OnDelete(ItemViewModel itemViewModel)
        {
            _allItems.Remove(itemViewModel);

            Items.Remove(itemViewModel);

            _container.Items.Remove(itemViewModel.Item);

            _storage.Save(_container);
        }

        private void CreateNewItem()
        {
            _regionManager.RequestNavigate("ContentRegion", "CreateNewItemView");
        }

        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            if(navigationContext.Parameters.ContainsKey("NewItem"))
            {
                var newItem = (Item)navigationContext.Parameters["NewItem"];

                var newItemViewModel = new ItemViewModel(newItem, _eventAggregator);

                _allItems.Add(newItemViewModel);

                Items.Add(newItemViewModel);

                _container.Items.Add(newItem);

                _storage.Save(_container);
            }
        }

        public bool IsNavigationTarget(NavigationContext navigationContext) => true;

        public void OnNavigatedFrom(NavigationContext navigationContext)
        {
        }

        public DelegateCommand CreateNewItemCommand { get; }
    }
}
