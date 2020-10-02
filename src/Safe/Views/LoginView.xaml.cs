using System.Windows.Controls;

namespace Safe.Views
{
    /// <summary>
    /// Interaction logic for LoginView.xaml
    /// </summary>
    public partial class LoginView : UserControl
    {
        public LoginView()
        {
            InitializeComponent();

            Loaded += (sender, e) => password.Focus();
        }
    }
}
