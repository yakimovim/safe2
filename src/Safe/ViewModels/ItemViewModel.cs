using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Safe.Core.Domain;
using Safe.Events;
using System;

namespace Safe.ViewModels
{
    public class ItemViewModel : BindableBase
    {
        private readonly IEventAggregator _eventAggregator;

        public Item Item { get; }

        public ItemViewModel(Item item, IEventAggregator eventAggregator)
        {
            _eventAggregator = eventAggregator ?? throw new ArgumentNullException(nameof(eventAggregator));
            Item = item ?? throw new ArgumentNullException(nameof(item));

            DeleteCommand = new DelegateCommand(Delete);
        }

        private void Delete()
        {
            _eventAggregator.GetEvent<DeleteItemEvent>().Publish(this);
        }

        public string Title
        {
            get { return Item.Title; }
            set { Item.Title = value; }
        }

        public string Description
        {
            get { return Item.Description; }
            set { Item.Description = value; }
        }

        public DelegateCommand DeleteCommand { get; }
    }
}
