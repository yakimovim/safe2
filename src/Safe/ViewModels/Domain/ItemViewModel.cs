using Prism.Commands;
using Prism.Regions;
using Safe.Core.Domain;
using Safe.Services;
using System;
using System.Collections.ObjectModel;

namespace Safe.ViewModels.Domain
{
    public class ItemViewModel : EntityViewModel<Item, ItemViewModel>, IContainer<FieldViewModel>
    {
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
        private readonly IRegionManager _regionManager;

        public string Tags
        {
            get { return _tags; }
            set { SetProperty(ref _tags, value); }
        }

        public ObservableCollection<FieldViewModel> Fields { get; } = new ObservableCollection<FieldViewModel>();

        public DelegateCommand DeleteCommand { get; }

        public DelegateCommand EditCommand { get; }

        public ItemViewModel(
            Item model, 
            IContainer<ItemViewModel> parentContainer, 
            IMapper mapper,
            IRegionManager regionManager) 
            : base(model, parentContainer, mapper)
        {
            _regionManager = regionManager ?? throw new ArgumentNullException(nameof(regionManager));

            DeleteCommand = new DelegateCommand(OnDelete);
            EditCommand = new DelegateCommand(OnEdit);
        }

        private void OnDelete()
        {
            Delete();
        }

        private void OnEdit()
        {
            var p = new NavigationParameters();
            p.Add("Item", this);
            p.Add("IsEditing", true);

            _regionManager.RequestNavigate("ContentRegion", "EditItemView", p);
        }

        public void Add(FieldViewModel item)
        {
            if (item is null)
            {
                throw new ArgumentNullException(nameof(item));
            }

            Fields.Add(item);

            Model.Fields.Add(item.Model);
        }

        public void Delete(FieldViewModel item)
        {
            if (item is null)
            {
                throw new ArgumentNullException(nameof(item));
            }

            Fields.Remove(item);

            Model.Fields.Remove(item.Model);
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

            Model.Fields.RemoveAt(index);

            Model.Fields.Insert(index - 1, item.Model);
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

            Model.Fields.RemoveAt(index);

            Model.Fields.Insert(index + 1, item.Model);
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
