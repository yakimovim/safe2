using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Safe.ViewModels
{
    public class ItemsViewModel : BindableBase
    {
        private string _searchText;
        private readonly IRegionManager _regionManager;

        public string SearchText
        {
            get { return _searchText; }
            set { SetProperty(ref _searchText, value); }
        }

        public ItemsViewModel(
            IRegionManager regionManager
            )
        {
            _regionManager = regionManager ?? throw new ArgumentNullException(nameof(regionManager));

            CreateNewItemCommand = new DelegateCommand(CreateNewItem);
        }

        private void CreateNewItem()
        {
            _regionManager.RequestNavigate("ContentRegion", "CreateNewItemView");
        }

        public DelegateCommand CreateNewItemCommand { get; }
    }
}
