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

            Loaded += (sender, e) => password.Focus();
        }

        private void OnPasswordKeyUp(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == System.Windows.Input.Key.Enter)
            {
                if (createButton.Command.CanExecute(null))
                {
                    createButton.Command.Execute(null);
                }
            }
        }
    }
}
