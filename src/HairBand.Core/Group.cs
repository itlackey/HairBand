using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HairBand
{
    public class Group : DynamicDictionaryObject
    {
        public Group()
        {
            this.Order = decimal.MaxValue;

        }

        public decimal Order
        {
            get
            {
                return Convert.ToDecimal(this["order"]);
            }
            set { this["order"] = value; }
        }


        public string Name
        {
            get
            {
                return this["name"].ToString();
            }
            set { this["name"] = value; }
        }

        public ICollection<PageData> Pages
        {
            get
            {
                return this["pages"] as ICollection<PageData>;
            }
            set { this["pages"] = value; }
        }

    }
}
