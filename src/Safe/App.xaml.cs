using Prism.Ioc;
using Safe.Core.Services;
using Safe.Services;
using Safe.Views;
using System.Windows;

namespace Safe
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App
    {
        protected override Window CreateShell()
        {
            return Container.Resolve<MainWindow>();
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterSingleton<IMapper, Mapper>();
            containerRegistry.RegisterSingleton<IConfigurationService, ConfigurationService>();
            containerRegistry.RegisterSingleton<IStorageStreamProvider, StorageStreamProvider>();
        }
    }
}
