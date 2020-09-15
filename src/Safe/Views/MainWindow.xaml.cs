using Prism.Regions;
using Safe.Core.Services;
using System.Windows;

namespace Safe.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow(IStorage storage,
            IRegionManager regionManager)
        {
            InitializeComponent();

            Loaded += (sender, e) => {
                if (storage.Exists)
                {
                    regionManager.RequestNavigate("ContentRegion", "LoginView");
                }
                else
                {
                    regionManager.RequestNavigate("ContentRegion", "CreateStorageView");
                }
            };
        }
    }
}
