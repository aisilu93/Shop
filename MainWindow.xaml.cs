using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using GalaSoft.MvvmLight.Messaging;
using Shop.Model;
using Shop.ViewModel;

namespace Shop
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        /// <summary>
        /// Initializes a new instance of the MainWindow class.
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
            Closing += (s, e) => ViewModelLocator.Cleanup();

            Messenger.Default.Register<GoToPageMessage>
                (
                    this,
                    (action) => ReceiveMessage(action)
                );
        }
        private ShopWindow _page2View;
        private ShopWindow Page2View
        {
            get
            {
                if (_page2View == null)
                    _page2View = new ShopWindow();
                return _page2View;
            }
        }
        private object ReceiveMessage(GoToPageMessage action)
        {
            //            this.contentControl1.Content = this.Page2View;
            //this shows what pagename property is!!
            switch (action.PageName)
            {
                case "ShopWindow":
                    if (this.contentControl1.Content != this.Page2View)
                        this.contentControl1.Content = this.Page2View;
                    break;
                case "AddGood":
                    //good a = action.Parameter as good;
                    ViewModelLocator loc = new ViewModelLocator();
                    List<good_view> a = Page2View.GoodsTable.ItemsSource as List<good_view>;
                    good_view selected_item = a.Find(x => x.id_good == Convert.ToInt32(action.Parameter));
                    good order_item = loc.ShopWind.order
                                                  .Where(x => x.id_good == selected_item.id_good).FirstOrDefault();
                    if (order_item!=null)
                    {
                        if (order_item.in_storage < selected_item.in_storage)
                        {
                            good i = order_item;
                            i.in_storage++;
                            loc.ShopWind.order.Remove(order_item);
                            loc.ShopWind.order.Add(i);
                        }
                        break;
                    }
                    good j = selected_item.ToGood();
                    loc.ShopWind.order.Add(j);
                    /*order_list.Add(a.Find(x => x.id_good == Convert.ToInt32(action.Parameter)));
                    Page2View.CurrentOrder.ItemsSource = order_list;*/
                    break;
                default:
                    break;
            }

            //            string testII = action.PageName.ToString();
            //           System.Windows.MessageBox.Show("You were successful switching to " + testII + ".");

            return null;
        }
    }
}