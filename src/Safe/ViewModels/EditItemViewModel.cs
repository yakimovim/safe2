using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using System;

namespace Safe.ViewModels
{
    public class EditItemViewModel : BindableBase, INavigationAware
    {
        private readonly IRegionManager _regionManager;

        private string _returnUrl;

        public bool IsEditing { get; private set; }

        private Domain.ItemViewModel itemViewModel;
        public Domain.ItemViewModel Item
        {
            get { return itemViewModel; }
            set { SetProperty(ref itemViewModel, value); }
        }

        public DelegateCommand OkCommand { get; }

        public DelegateCommand CancelCommand { get; }

        public EditItemViewModel(
            IRegionManager regionManager
            )
        {
            _regionManager = regionManager ?? throw new ArgumentNullException(nameof(regionManager));

            OkCommand = new DelegateCommand(Save);
            CancelCommand = new DelegateCommand(Cancel);
        }

        public bool IsNavigationTarget(NavigationContext navigationContext) => true;

        public void OnNavigatedFrom(NavigationContext navigationContext)
        {
        }

        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            Item = navigationContext.Parameters.GetValue<Domain.ItemViewModel>("Item");

            if (Item == null) throw new InvalidOperationException();

            IsEditing = navigationContext.Parameters.ContainsKey("IsEditing")
                ? navigationContext.Parameters.GetValue<bool>("IsEditing")
                : false;

            _returnUrl = navigationContext.Parameters.ContainsKey("ReturnUrl")
                ? navigationContext.Parameters.GetValue<string>("ReturnUrl")
                : "ItemsView";
        }

        private void Cancel()
        {
            Item.RefreshFromModel();

            _regionManager.RequestNavigate("ContentRegion", _returnUrl);
        }

        private void Save()
        {
            Item.FillModel();

            if(!IsEditing)
            {
                Item.Add();
            }

            _regionManager.RequestNavigate("ContentRegion", _returnUrl);
        }

    }
}
