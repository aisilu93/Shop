using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop.Model
{
    [Table("orders")]
    public class order
    {
        [Key]
        public int id_user
        {
            get;
            private set;
        }
        public int timestamp
        {
            get;
            private set;
        }
    }

    [Table("order_items")]
    class order_item
        {
        [Key]
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
