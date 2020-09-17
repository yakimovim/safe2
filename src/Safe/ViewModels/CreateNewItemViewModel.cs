using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using Prism.Services.Dialogs;
using Safe.Core.Domain;
using System;
using System.Collections.ObjectModel;
using System.Linq;

namespace Safe.ViewModels
{
    public class CreateNewItemViewModel : BindableBase, INavigationAware
    {
        private readonly IRegionManager _regionManager;
        private readonly IDialogService _dialogService;

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

        private string _tags;
        public string Tags
        {
            get { return _tags; }
            set { SetProperty(ref _tags, value); }
        }

        public ObservableCollection<Field> Fields { get; } = new ObservableCollection<Field>();

        public CreateNewItemViewModel(
            IRegionManager regionManager,
            IDialogService dialogService)
        {
            _regionManager = regionManager ?? throw new ArgumentNullException(nameof(regionManager));
            _dialogService = dialogService ?? throw new ArgumentNullException(nameof(dialogService));

            CreateCommand = new DelegateCommand(Create);
            CancelCommand = new DelegateCommand(Cancel);
            AddFieldCommand = new DelegateCommand(AddField);
        }

        private void AddField()
        {
            var p = new DialogParameters();

            _dialogService.ShowDialog("AddFieldsDialog", p, r => { 
                if(r.Result == ButtonResult.OK)
                {
                    var field = r.Parameters.GetValue<Field>("Field");

                    Fields.Add(field);
                }
            });
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

            if(!string.IsNullOrWhiteSpace(Tags))
            {
                var tags = Tags.Split(',', StringSplitOptions.RemoveEmptyEntries)
                    .Where(t => !string.IsNullOrWhiteSpace(t))
                    .ToArray();

                foreach (var tag in tags)
                {
                    item.Tags.Add(tag.Trim());
                }
            }

            var p = new NavigationParameters();
            p.Add("NewItem", item);

            _regionManager.RequestNavigate("ContentRegion", "ItemsView", p);
        }

        public void OnNavigatedTo(NavigationContext navigationContext) { }

        public bool IsNavigationTarget(NavigationContext navigationContext) => false;

        public void OnNavigatedFrom(NavigationContext navigationContext) { }

        public DelegateCommand CreateCommand { get; }

        public DelegateCommand CancelCommand { get; }

        public DelegateCommand AddFieldCommand { get; }
    }
}
