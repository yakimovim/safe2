using System.Windows.Controls;

namespace Safe.Views
{
    /// <summary>
    /// Interaction logic for EditItemView
    /// </summary>
    public partial class EditItemView : UserControl
    {
        public EditItemView()
        {
            InitializeComponent();

            Loaded += (sender, e) => titleText.Focus();
        }
    }
}
