using System.Windows.Controls;

namespace Safe.Views
{
    /// <summary>
    /// Interaction logic for ChangePasswordView
    /// </summary>
    public partial class ChangePasswordView : UserControl
    {
        public ChangePasswordView()
        {
            InitializeComponent();

            Loaded += (sender, e) => oldPassword.Focus();
        }
    }
}
