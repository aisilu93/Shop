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
                    this.Title = "Shop: " + action.Parameter;
                    ShopViewModel.userid = Convert.ToInt32(action.Parameter);
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