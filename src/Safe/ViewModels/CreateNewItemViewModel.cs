using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using System;

namespace Safe.ViewModels
{
    public class CreateNewItemViewModel : BindableBase
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
            _regionManager.RequestNavigate("ContentRegion", "ItemsView");
        }

        public DelegateCommand CreateCommand { get; }

        public DelegateCommand CancelCommand { get; }
    }
}
