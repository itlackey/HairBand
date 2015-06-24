using System;
using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;

namespace HairBand
{
    public class PageData : DynamicDictionaryObject
    {

        public PageData()
        {
            this.Content = string.Empty;
            this.Title = string.Empty;
            this.Exceprt = string.Empty;
            this.Url = new List<string>();
            this.Date = DateTime.Now;
            this.Id = string.Empty;
            this.Categories = new List<string>();
            this.Tags = new List<string>();
            this.Path = string.Empty;
            this.Next = null;
            this.Previous = null;

            this.Order = decimal.MaxValue;
            this.Parent = string.Empty;
            this.ChildrenCount = 0;

        }

        public string Content
        {
            get { return this["content"].ToString(); }
            set { this["content"] = value; }
        }

        public string Title
        {
            get { return this["title"].ToString(); }
            set { this["title"] = value; }
        }

        public string Exceprt
        {
            get { return this["exceprt"].ToString(); }
            set { this["exceprt"] = value; }
        }

        public IEnumerable<string> Url
        {
            get { return this["url"] as IEnumerable<string>; }
            set { this["url"] = value; }
        }

        public DateTime Date {
            get
            {
                return Convert.ToDateTime(this["date"]);
            }
            set { this["date"] = value; }
        }

        public string Id
        {
            get { return this["id"].ToString(); }
            set { this["id"] = value; }
        }

        public ICollection<string> Categories {
            get
            {
                return this["categories"] as ICollection<string>;
            }
            set { this["categories"] = value; }
        }

        public ICollection<string> Tags {
            get
            {
                return this["tags"] as ICollection<string>;
            }
            set { this["tags"] = value; }
        }

        public string Path
        {
            get { return this["path"].ToString(); }
            set { this["path"] = value; }
        }

        public PostData Next
        {
            get
            {
                return this["next"] as PostData;
            }
            set { this["next"] = value; }
        }

        public PostData Previous
        {
            get
            {
                return this["previous"] as PostData;
            }
            set { this["previous"] = value; }
        }



        #region Extended Properties

        public decimal Order
        {
            get
            {
                return Convert.ToDecimal(this["order"]);
            }
            set { this["order"] = value; }
        }


        public string Parent
        {
            get
            {
                return this["parent"].ToString();
            }
            set { this["parent"] = value; }
        }


        public int ChildrenCount
        {
            get
            {
                return Convert.ToInt32(this["children_count"]);
            }
            set { this["children_count"] = value; }
        }

        #endregion


    }
}
