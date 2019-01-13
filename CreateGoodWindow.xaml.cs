using Shop.Model;
using Shop.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Shop
{
    /// <summary>
    /// Логика взаимодействия для CreateGoodWindow.xaml
    /// </summary>
    public partial class CreateGoodWindow : Window
    {
        ShopViewModel shopViewModel;
        public CreateGoodWindow()
        {
            InitializeComponent();
        }

        public CreateGoodWindow(ShopViewModel shopViewModel)
        {
            InitializeComponent();
            this.shopViewModel = shopViewModel;
            this.category_id.ItemsSource = shopViewModel.goods_categories;
            this.category_id.DisplayMemberPath = "name_gc";
            this.category_id.SelectedValuePath = "id_gc";
            this.category_id.SelectedIndex = 0;

            this.in_storage.Text = "0";
            this.good_price.Text = "0";
        }

        private void ApplyChanges(object sender, RoutedEventArgs e)
        {
            int cat_id = (int)this.category_id.SelectedValue;

            int price;
            if (!Int32.TryParse(this.good_price.Text.Trim(), out price)) { price = 0; }

            int storage;
            if (!Int32.TryParse(this.in_storage.Text.Trim(), out storage)) { storage = 0; }

            shopViewModel.AddNewGood(this.good_name.Text, price, cat_id, storage);
            this.Close();
        }
        private void Cancel(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
