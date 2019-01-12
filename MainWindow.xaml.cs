using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using GalaSoft.MvvmLight.Messaging;
using Shop.Model;
using Shop.ViewModel;
using System.Windows.Forms.DataVisualization.Charting;

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
                    ShopViewModel.userright = Convert.ToInt32(action.Parameter2);
                    ShopViewModel.userid = Convert.ToInt32(action.Parameter);
                    if (ShopViewModel.userright == 2)
                    {
                        this.Page2View.GoodsEdit.Visibility = Visibility.Hidden;
                        this.Page2View.UsersEdit.Visibility = Visibility.Hidden;
                        this.Page2View.Reports.Visibility = Visibility.Hidden;
                    }
                    this.Title = "Shop: " + action.Parameter;
                    if (this.contentControl1.Content != this.Page2View)
                        this.contentControl1.Content = this.Page2View;
                    break;
                case "CreateOrder":
                    MessageBox.Show("Заказ №"+ action.Parameter+" добавлен");
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