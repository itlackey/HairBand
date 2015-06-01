using System;
using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;

namespace HairBand
{
    public class SiteData : DynamicDictionaryObject
    {

        public SiteData()
        {
            this.Name = string.Empty;
            this.Time = DateTime.Now;
            this.Pages = new List<PageData>();
            this.Posts = new List<PostData>();
            this.RelatedPosts = new List<PostData>();
            this.StaticFiles = new List<FileData>();
            this.HtmlPages = new List<string>();
            this.Data = new List<object>();
            this.Documents = new List<object>();
            this.Categories = new Dictionary<string, PostData>();
            this.Tags = new Dictionary<string, PostData>();
            this.RootPath = string.Empty;
        }

        public string Name
        {
            get { return this["name"].ToString(); }
            set { this["name"] = value; }
        }

        public DateTime Time
        {
            get
            {
                return Convert.ToDateTime(this["time"]);
            }
            set { this["time"] = value; }
        }

        public ICollection<PageData> Pages
        {
            get
            {
                return this["pages"] as ICollection<PageData>;
            }
            set { this["pages"] = value; }
        }

        public ICollection<PostData> Posts
        {
            get
            {
                return this["posts"] as ICollection<PostData>;
            }
            set { this["posts"] = value; }
        }

        public ICollection<PostData> RelatedPosts
        {
            get
            {
                return this["related_posts"] as ICollection<PostData>;
            }
            set { this["related_posts"] = value; }
        }

        public ICollection<FileData> StaticFiles
        {
            get
            {
                return this["static_files"] as ICollection<FileData>;
            }
            set { this["static_files"] = value; }
        }

        public ICollection<string> HtmlPages
        {
            get
            {
                return this["related_posts"] as ICollection<string>;
            }
            set { this["related_posts"] = value; }
        }


        //public ICollection<ICollection> Collections { get; set; }

        public ICollection<object> Data
        {
            get
            {
                return this["data"] as ICollection<object>;
            }
            set { this["data"] = value; }
        }

        public ICollection<object> Documents
        {
            get
            {
                return this["documents"] as ICollection<object>;
            }
            set { this["documents"] = value; }
        }

        public IDictionary<string, PostData> Categories
        {
            get
            {
                return this["categories"] as IDictionary<string, PostData>;
            }
            set { this["categories"] = value; }
        }

        public IDictionary<string, PostData> Tags
        {
            get
            {
                return this["tags"] as IDictionary<string, PostData>;
            }
            set { this["tags"] = value; }
        }



        #region Extended Properties

        public string RootPath
        {
            get { return this["root_path"].ToString(); }
            set { this["root_path"] = value; }
        }

        #endregion


    }
}
