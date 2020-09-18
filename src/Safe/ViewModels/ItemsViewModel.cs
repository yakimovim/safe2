using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Prism.Regions;
using Safe.Core.Domain;
using Safe.Core.Services;
using Safe.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Safe.ViewModels
{
    public class ItemsViewModel : BindableBase, INavigationAware, IContainer<Domain.ItemViewModel>
    {
        private readonly IStorage _storage;
        private readonly INavigationService _navigationService;
        private readonly IEventAggregator _eventAggregator;
        private readonly IMapper _mapper;
        private readonly Container _container;
        private readonly List<Domain.ItemViewModel> _allItems;

        private string _searchText;

        public string SearchText
        {
            get { return _searchText; }
            set { SetProperty(ref _searchText, value); }
        }

        public ObservableCollection<Domain.ItemViewModel> Items { get; } 
            = new ObservableCollection<Domain.ItemViewModel>();

        public ItemsViewModel(
            IStorage storage,
            INavigationService navigationService,
            IEventAggregator eventAggregator,
            IMapper mapper
            )
        {
            _storage = storage ?? throw new ArgumentNullException(nameof(storage));
            _navigationService = navigationService ?? throw new ArgumentNullException(nameof(navigationService));
            _eventAggregator = eventAggregator ?? throw new ArgumentNullException(nameof(eventAggregator));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));

            _container = _storage.Read();

            _allItems = _container.Items
                .Select(i => new Domain.ItemViewModel(i, this, _mapper, _navigationService))
                .ToList();

            Items.AddRange(_allItems);

            CreateNewItemCommand = new DelegateCommand(CreateNewItem);
        }

        private void CreateNewItem()
        {
            var p = new NavigationParameters();
            p.Add("Item", new Domain.ItemViewModel(new Item(), this, _mapper, _navigationService));
            p.Add("IsEditing", false);

            _navigationService.NavigateMainContentTo("EditItemView", p);
        }

        public void OnNavigatedTo(NavigationContext navigationContext)
        {
        }

        public bool IsNavigationTarget(NavigationContext navigationContext) => true;

        public void OnNavigatedFrom(NavigationContext navigationContext)
        {
        }

        public void Add(Domain.ItemViewModel item)
        {
            if (item is null)
            {
                throw new ArgumentNullException(nameof(item));
            }

            _allItems.Add(item);

            _container.Items.Add(item.Model);

            Items.Add(item);
        }

        public void Delete(Domain.ItemViewModel item)
        {
            if (item is null)
            {
                throw new ArgumentNullException(nameof(item));
            }

            _allItems.Remove(item);

            _container.Items.Remove(item.Model);

            Items.Remove(item);
        }

        public bool CanMoveUp(Domain.ItemViewModel item) => false;

        public bool CanMoveDown(Domain.ItemViewModel item) => false;

        public void MoveUp(Domain.ItemViewModel item) { }

        public void MoveDown(Domain.ItemViewModel item) { }

        public DelegateCommand CreateNewItemCommand { get; }
    }
}
