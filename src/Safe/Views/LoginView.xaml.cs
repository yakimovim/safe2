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

        private void OnPasswordKeyUp(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if(e.Key == System.Windows.Input.Key.Enter)
            {
                if (loginButton.Command.CanExecute(null))
                { 
                    loginButton.Command.Execute(null); 
                }
            }
        }
    }
}
