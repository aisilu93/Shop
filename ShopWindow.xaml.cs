using System.Windows;
using System.Windows.Controls;
using Shop.ViewModel;

namespace Shop
{
    /// <summary>
    /// Interaction logic for ShopWindow.xaml
    /// </summary>
    public partial class ShopWindow : UserControl
    {
        public ShopWindow()
        {
            InitializeComponent();
            //Closing += (s, e) => ViewModelLocator.Cleanup();
        }
    }
}
