using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop.Model
{
    public class order
    {
        public int id_ord
        {
            get;
            set;
        }
        public int id_user
        {
            get;
            set;
        }
        public string login
        {
            get;
            set;
        }
        public int timestamp
        {
            get;
            set;
        }
    }

    
    public class order_item
        {
        public int id
        {
            get;
            private set;
        }
        public int id_order
        {
            get;
            private set;
        }
        public int id_good
        {
            get;
            private set;
        }
        public string name_g
        {
            get;
            private set;
        }
        public int amount
        {
            get;
            private set;
        }
        public int price
        {
            get;
            private set;
        }
    }
}
