using System.Collections.Generic;
using GalaSoft.MvvmLight;
using Shop.Model;
using System.Data.Entity;
using System.Collections.ObjectModel;
using GalaSoft.MvvmLight.CommandWpf;
using System;
using System.Windows;
using System.Linq;
using System.Windows.Input;
using System.Windows.Controls;
using GalaSoft.MvvmLight.Messaging;

namespace Shop.ViewModel
{
    /// <summary>
    /// This class contains properties that the main View can data bind to.
    /// <para>
    /// See http://www.mvvmlight.net
    /// </para>
    /// </summary>
    public class ShopViewModel : ViewModelBase
    {
        private readonly IDataService _dataService;
        public List<good> goods_base { get; set; }
        public List<good> order { get; set; }
        public string InfoTitlePageShop
        {
            get
            {
                return "Shop Window";
            }
        }
        public RelayCommand HelloCommand {
            get;
            set;
        }

        private object AddingGood()
        {
            var msg = new GoToPageMessage() { PageName = "AddGood" };
            Messenger.Default.Send<GoToPageMessage>(msg);
            return null;
        }
        /// <summary>
        /// Initializes a new instance of the MainViewModel class.
        /// </summary>
        public ShopViewModel(IDataService dataService)
        {
            HelloCommand = new RelayCommand(() => AddingGood());
            _dataService = dataService;
            _dataService.GetData(
                (user item, Exception error) =>
                {
                    if (error != null)
                    {
                        // Report error here
                        return;
                    }
                    DbClient db = new DbClient();
                    db.goods.Load();
                    goods_base = new List<good>();
                    goods_base=db.goods.ToList<good>();
                    //foreach (var i in db.goods.ToList<good>()) { goods_base.Add(i.name_g); }
                    //WelcomeTitle2 = string.Join(" ", item.lst);
                });
        }
        ////public override void Cleanup()
        ////{
        ////    // Clean up if needed

        ////    base.Cleanup();
        ////}
    }
}