using System.Windows.Controls;

namespace Safe.Views
{
    /// <summary>
    /// Interaction logic for ItemsView.xaml
    /// </summary>
    public partial class ItemsView : UserControl
    {
        public ItemsView()
        {
            InitializeComponent();

            Loaded += (sender, e) => searchTextBox.Focus();
        }
    }
}
