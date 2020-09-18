using Prism.Regions;
using Prism.Services.Dialogs;
using System;

namespace Safe.Services
{
    public interface INavigationService
    {
        void ShowYesNoDialog(string message, Action<IDialogResult> closeAction, string title = null);

        void NavigateMainContentTo(string target, NavigationParameters navigationParameters = null);
    }

    public sealed class NavigationService : INavigationService
    {
        private readonly IDialogService _dialogService;
        private readonly IRegionManager _regionManager;

        public NavigationService(
            IDialogService dialogService,
            IRegionManager regionManager)
        {
            _dialogService = dialogService ?? throw new ArgumentNullException(nameof(dialogService));
            _regionManager = regionManager ?? throw new ArgumentNullException(nameof(regionManager));
        }

        public void NavigateMainContentTo(string target, NavigationParameters navigationParameters = null)
        {
            _regionManager.RequestNavigate("ContentRegion", target, navigationParameters);
        }

        public void ShowYesNoDialog(string message, Action<IDialogResult> closeAction, string title = null)
        {
            var p = new DialogParameters();
            p.Add("Message", message);
            if(!string.IsNullOrEmpty(title))
            {
                p.Add("Title", title);
            }

            _dialogService.ShowDialog("YesNoDialog", p, closeAction);
        }
    }
}
