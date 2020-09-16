using Prism.Events;
using Safe.ViewModels;

namespace Safe.Events
{
    /// <summary>
    /// Event about deleting of item.
    /// </summary>
    public class DeleteItemEvent : PubSubEvent<ItemViewModel> { }
}
