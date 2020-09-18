using Prism.Commands;
using Prism.Events;
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
using System.Windows;

namespace Safe.ViewModels
{
    public class ItemsViewModel : BindableBase, IContainer<ItemViewModel>
    {
        private readonly IStorage _storage;
        private readonly INavigationService _navigationService;
        private readonly IMapper _mapper;
        private readonly Container _container;
        private readonly List<ItemViewModel> _allItems;

        private string _searchText;

        public string SearchText
        {
            get { return _searchText; }
            set { SetProperty(ref _searchText, value); }
        }

        public ObservableCollection<ItemViewModel> Items { get; } 
            = new ObservableCollection<ItemViewModel>();

        public ItemsViewModel(
            IStorage storage,
            IMapper mapper,
            INavigationService navigationService
            )
        {
            _storage = storage ?? throw new ArgumentNullException(nameof(storage));
            _navigationService = navigationService ?? throw new ArgumentNullException(nameof(navigationService));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));

            _container = _storage.Read();

            Application.Current.Exit += OnApplicationExit;

            _allItems = _container.Items
                .Select(i => new ItemViewModel(i, this, _mapper, _navigationService))
                .ToList();

            Items.AddRange(_allItems);

            CreateNewItemCommand = new DelegateCommand(CreateNewItem);
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

            Items.Add(item);
        }

        public void Delete(ItemViewModel item)
        {
            if (item is null)
            {
                throw new ArgumentNullException(nameof(item));
            }

            _allItems.Remove(item);

            _container.Items.Remove(item.Model);

            Items.Remove(item);
        }

        public bool CanMoveUp(ItemViewModel item) => false;

        public bool CanMoveDown(ItemViewModel item) => false;

        public void MoveUp(ItemViewModel item) { }

        public void MoveDown(ItemViewModel item) { }

        public DelegateCommand CreateNewItemCommand { get; }
    }
}
