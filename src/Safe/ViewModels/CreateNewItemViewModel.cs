using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using Safe.Core.Domain;
using System;

namespace Safe.ViewModels
{
    public class CreateNewItemViewModel : BindableBase, INavigationAware
    {
        private readonly IRegionManager _regionManager;

        private string _title;
        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }

        private string _description;
        public string Description
        {
            get { return _description; }
            set { SetProperty(ref _description, value); }
        }

        public CreateNewItemViewModel(IRegionManager regionManager)
        {
            _regionManager = regionManager ?? throw new ArgumentNullException(nameof(regionManager));

            CreateCommand = new DelegateCommand(Create);
            CancelCommand = new DelegateCommand(Cancel);
        }

        private void Cancel()
        {
            _regionManager.RequestNavigate("ContentRegion", "ItemsView");
        }

        private void Create()
        {
            var item = new Item
            {
                Title = Title,
                Description = Description
            };

            var p = new NavigationParameters();
            p.Add("NewItem", item);

            _regionManager.RequestNavigate("ContentRegion", "ItemsView", p);
        }

        public void OnNavigatedTo(NavigationContext navigationContext) { }

        public bool IsNavigationTarget(NavigationContext navigationContext) => false;

        public void OnNavigatedFrom(NavigationContext navigationContext) { }

        public DelegateCommand CreateCommand { get; }

        public DelegateCommand CancelCommand { get; }
    }
}
