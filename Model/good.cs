using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Shop.Model
{
    [Table("goods")]
    public class good
    {
        [Key]
        public int id_good
        {
            get;
            set;
        }
        public string name_g
        {
            get;
            set;
        }
        public int cat_g
        {
            get;
            set;
        }
        public int price
        {
            get;
            set;
        }
        public int in_storage
        {
            get;
            set;
        }
    }

    public class good_view
    {
        public int id_good
        {
            get;
            set;
        }
        public string name_g
        {
            get;
            set;
        }
        public int cat_g
        {
            get;
            set;
        }
        public int price
        {
            get;
            set;
        }
        public int in_storage
        {
            get;
             set;
        }
        public int id_gc
        {
            get;
            set;
        }
        public string name_gc
        {
            get;
            set;
        }
        public good ToGood()
        {
            return new good()
            {
                id_good = this.id_good,
                name_g = this.name_g,
                cat_g = this.cat_g,
                price = this.price,
                in_storage = 1
            };
        }
    }

    [Table("goods_categories")]
    public class goods_category
    {
        [Key]
        public int id_gc
        {
            get;
            private set;
        }
        public string name_gc
        {
            get;
            private set;
        }
    }
}