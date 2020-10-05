using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using Safe.Services;
using System;
using System.ComponentModel;

namespace Safe.ViewModels
{
    public class EditItemViewModel : BindableBase, INavigationAware
    {
        private IRegionNavigationJournal _journal;

        public bool IsEditing { get; private set; }

        private Domain.ItemViewModel itemViewModel;
        private readonly INavigationService _navigationService;

        public Domain.ItemViewModel Item
        {
            get { return itemViewModel; }
            set { SetProperty(ref itemViewModel, value); }
        }

        public bool IsItemValid => Item?.IsValid ?? false;

        public DelegateCommand OkCommand { get; }

        public DelegateCommand CancelCommand { get; }

        public DelegateCommand AddFieldsCommand { get; }

        public EditItemViewModel(INavigationService navigationService)
        {
            _navigationService = navigationService ?? throw new ArgumentNullException(nameof(navigationService));

            OkCommand = new DelegateCommand(Save)
                .ObservesCanExecute(() => IsItemValid);
            CancelCommand = new DelegateCommand(Cancel);
            AddFieldsCommand = new DelegateCommand(AddFields);
        }

        public bool IsNavigationTarget(NavigationContext navigationContext) => true;

        public void OnNavigatedFrom(NavigationContext navigationContext) { }

        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            _journal = navigationContext.NavigationService.Journal;

            RaisePropertyChanged(nameof(IsItemValid));

            // If this is just a back navigation do not change Item and IsEditing
            if (!navigationContext.Parameters.ContainsKey("Item")) return;

            Item = navigationContext.Parameters.GetValue<Domain.ItemViewModel>("Item");

            if (Item == null) throw new InvalidOperationException();

            IsEditing = navigationContext.Parameters.ContainsKey("IsEditing")
                ? navigationContext.Parameters.GetValue<bool>("IsEditing")
                : false;
        }

        private void Cancel()
        {
            Item.RefreshFromModel();

            _journal.GoBack();
        }

        private void Save()
        {
            Item.FillModel();

            if(!IsEditing)
            {
                Item.Add();
            }

            _journal.GoBack();
        }

        private void AddFields()
        {
            var p = new NavigationParameters();
            //p.Add("IsEditing", false);
            p.Add("Container", Item);

            //_navigationService.NavigateMainContentTo("EditFieldView", p);
            _navigationService.NavigateMainContentTo("AddFieldsView", p);
        }
    }
}
