using Safe.ViewModels;
using System.Windows;
using System.Windows.Controls;

namespace Safe.Views
{
    /// <summary>
    /// Interaction logic for CreateStorageView.xaml
    /// </summary>
    public partial class CreateStorageView : UserControl
    {
        public CreateStorageView()
        {
            InitializeComponent();
        }

        private void PasswordChanged(object sender, RoutedEventArgs e)
        {
            var viewModel = DataContext as CreateStorageViewModel;
            if (viewModel == null) return;

            viewModel.SetPassword(pwdBox.Password);
        }
    }
}
