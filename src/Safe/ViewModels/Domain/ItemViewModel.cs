using Prism.Commands;
using Prism.Regions;
using Safe.Core.Domain;
using Safe.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Safe.ViewModels.Domain
{
    public class ItemViewModel : EntityViewModel<Item, ItemViewModel>, IContainer<FieldViewModel>
    {
        private string _title;
        public string Title
        {
            get { return _title; }
            set 
            { 
                if(SetProperty(ref _title, value))
                {
                    RaisePropertyChanged(nameof(TitleIsNotValid));
                    RaisePropertyChanged(nameof(IsValid));
                }
            }
        }

        public bool TitleIsNotValid => string.IsNullOrWhiteSpace(Title);

        private string _description;
        public string Description
        {
            get { return _description; }
            set 
            { 
                if(SetProperty(ref _description, value))
                {
                    HasDescription = !string.IsNullOrWhiteSpace(_description);
                }
            }
        }

        private bool _hasDescription;
        public bool HasDescription
        {
            get { return _hasDescription; }
            private set { SetProperty(ref _hasDescription, value); }
        }

        private string _tags;
        private readonly INavigationService _navigationService;

        public string Tags
        {
            get { return _tags; }
            set { SetProperty(ref _tags, value); }
        }

        public IReadOnlyCollection<string> TagsCollection => Model.Tags.ToArray();

        public ObservableCollection<FieldViewModel> Fields { get; } = new ObservableCollection<FieldViewModel>();

        public bool HasNoFields => Fields.Count == 0;

        public bool IsValid => !TitleIsNotValid && !HasNoFields;

        public DelegateCommand DeleteCommand { get; }

        public DelegateCommand EditCommand { get; }

        public ItemViewModel(
            Item model, 
            IContainer<ItemViewModel> parentContainer, 
            IMapper mapper,
            INavigationService navigationService)
            : base(model, parentContainer, mapper)
        {
            _navigationService = navigationService ?? throw new ArgumentNullException(nameof(navigationService));

            DeleteCommand = new DelegateCommand(OnDelete);
            EditCommand = new DelegateCommand(OnEdit);

            Fields.CollectionChanged += (sender, e) => {
                RaisePropertyChanged(nameof(HasNoFields));
                RaisePropertyChanged(nameof(IsValid));
            };
        }

        public override void FillModel()
        {
            base.FillModel();

            foreach (var field in Fields)
            {
                field.FillModel();
            }

            RaisePropertyChanged(nameof(TagsCollection));
        }

        public override void RefreshFromModel()
        {
            base.RefreshFromModel();

            foreach (var field in Fields)
            {
                field.RefreshFromModel();
            }
        }

        private void OnDelete()
        {
            _navigationService.ShowYesNoDialog("Do you want to delete this item?", res => { 
                if(res.Result == Prism.Services.Dialogs.ButtonResult.Yes)
                {
                    Delete();
                }
            });
        }

        private void OnEdit()
        {
            var p = new NavigationParameters();
            p.Add("Item", this);
            p.Add("IsEditing", true);

            _navigationService.NavigateMainContentTo("EditItemView", p);
        }

        public void Add(FieldViewModel item)
        {
            if (item is null)
            {
                throw new ArgumentNullException(nameof(item));
            }

            Fields.Add(item);
        }

        public void Delete(FieldViewModel item)
        {
            if (item is null)
            {
                throw new ArgumentNullException(nameof(item));
            }

            Fields.Remove(item);
        }

        public void MoveUp(FieldViewModel item)
        {
            if (item is null)
            {
                throw new ArgumentNullException(nameof(item));
            }

            var index = Fields.IndexOf(item);

            if (index < 0) return;

            if (index == 0) return;

            Fields.RemoveAt(index);

            Fields.Insert(index - 1, item);

            foreach (var field in Fields)
            {
                field.RefreshPosition();
            }
        }

        public void MoveDown(FieldViewModel item)
        {
            if (item is null)
            {
                throw new ArgumentNullException(nameof(item));
            }

            var index = Fields.IndexOf(item);

            if (index < 0) return;

            if (index == Fields.Count - 1) return;

            Fields.RemoveAt(index);

            Fields.Insert(index + 1, item);

            foreach (var field in Fields)
            {
                field.RefreshPosition();
            }
        }

        bool IContainer<FieldViewModel>.CanMoveUp(FieldViewModel item)
        {
            if (item is null)
            {
                throw new ArgumentNullException(nameof(item));
            }

            var index = Fields.IndexOf(item);

            if (index < 0) return false;

            return index > 0;
        }

        bool IContainer<FieldViewModel>.CanMoveDown(FieldViewModel item)
        {
            if (item is null)
            {
                throw new ArgumentNullException(nameof(item));
            }

            var index = Fields.IndexOf(item);

            if (index < 0) return false;

            return index < Fields.Count - 1;
        }
    }
}
