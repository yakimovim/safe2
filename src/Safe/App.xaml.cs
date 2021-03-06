﻿using Prism.Ioc;
using Safe.Core.Services;
using Safe.Dialogs;
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
            containerRegistry.RegisterSingleton<IEncryptionService, EncryptionService>();
            containerRegistry.RegisterSingleton<IStorage, Storage>();
            containerRegistry.RegisterSingleton<IPasswordGenerator, PasswordGenerator>();
            containerRegistry.RegisterSingleton<INavigationService, NavigationService>();

            containerRegistry.RegisterForNavigation<LoginView>();
            containerRegistry.RegisterForNavigation<CreateStorageView>();
            containerRegistry.RegisterForNavigation<ItemsView>();
            containerRegistry.RegisterForNavigation<EditItemView>();
            containerRegistry.RegisterForNavigation<SettingsView>();
            containerRegistry.RegisterForNavigation<ChangePasswordView>();
            containerRegistry.RegisterForNavigation<AddFieldsView>();

            containerRegistry.RegisterDialog<YesNoDialog, YesNoDialogViewModel>();
            containerRegistry.RegisterDialog<PasswordGenerationDialog, PasswordGenerationDialogViewModel>();
        }
    }
}
