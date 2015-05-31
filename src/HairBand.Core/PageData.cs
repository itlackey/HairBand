using System;
using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;

namespace HairBand
{
    public class PageData : DynamicObject //, IDictionary<string, object>
    {

        public PageData()
        {
            this.Content = string.Empty;
            this.Title = string.Empty;
            this.Exceprt = string.Empty;
            this.Url = string.Empty;
            this.Date = DateTime.Now;
            this.Id = string.Empty;
            this.Categories = new List<string>();
            this.Tags = new List<string>();
            this.Path = string.Empty;
            this.Next = null;
            this.Previous = null;

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

        public string Url
        {
            get { return this["url"].ToString(); }
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


        #region Dictionary
        public Dictionary<string, object> _settings = new Dictionary<string, object>();

        public object this[string key]
        {
            get
            {
              
                return this._settings[key.ToLower()];
            }

            set
            {

                if (this._settings.ContainsKey(key.ToLower()))
                    this._settings[key.ToLower()] = value;
                else
                    this._settings.Add(key.ToLower(), value);
            }
        }

        public IDictionary<string, object> ToDictionary()
        {
            return this._settings;
        }

        public override bool TryGetMember(GetMemberBinder binder, out object result)
        {
            if (this._settings.ContainsKey(binder.Name.ToLower()))
            {
                result = this._settings[binder.Name.ToLower()];
                return true;

            }

            result = null;
            return false;
        }


        public override bool TrySetMember(SetMemberBinder binder, object value)
        {


            this._settings.Add(binder.Name.ToLower(), value);

            return true;
        }


        public override bool TryDeleteIndex(DeleteIndexBinder binder, object[] indexes)
        {
            if (indexes.Count() > 0)
            {
                var name = indexes.FirstOrDefault().ToString();

                if (this._settings.ContainsKey(name))
                {
                    this._settings.Remove(name);

                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }


        }


        // Set the property value by index.
        public override bool TrySetIndex(
            SetIndexBinder binder, object[] indexes, object value)
        {
            var index = indexes[0].ToString();

            // If a corresponding property already exists, set the value.
            if (_settings.ContainsKey(index))
                _settings[index] = value;
            else
                // If a corresponding property does not exist, create it.
                _settings.Add(index, value);
            return true;
        }

        // Get the property value by index.
        public override bool TryGetIndex(
            GetIndexBinder binder, object[] indexes, out object result)
        {

            var index = indexes[0].ToString();
            return _settings.TryGetValue(index, out result);
        }
        
        #endregion


        //#region IDictionary

        //public ICollection<string> Keys
        //{
        //    get
        //    {
        //        return this._settings.Keys;
        //    }
        //}

        //public ICollection<object> Values
        //{
        //    get
        //    {
        //        return this._settings.Values;
        //    }
        //}

        //public int Count
        //{
        //    get
        //    {
        //        return this._settings.Count;
        //    }
        //}

        //public bool IsReadOnly
        //{
        //    get
        //    {
        //        return this._settings.IsReadOnly;
        //    }
        //}

        //public object this[string key]
        //{
        //    get
        //    {
        //        return this._settings[key];
        //    }

        //    set
        //    {
        //        this._settings[key] = value;
        //    }
        //}

        //public bool ContainsKey(string key)
        //{
        //    return this._settings.ContainsKey(key);
        //}

        //public void Add(string key, object value)
        //{
        //    this._settings.Add(key, value);
        //}

        //public bool Remove(string key)
        //{
        //    return this._settings.Remove(key);
        //}

        //public bool TryGetValue(string key, out object value)
        //{
        //    return this._settings.TryGetValue(key, out value);
        //}

        //public void Add(KeyValuePair<string, object> item)
        //{
        //    this._settings.Add(item);
        //}

        //public void Clear()
        //{
        //    this._settings.Clear();
        //}

        //public bool Contains(KeyValuePair<string, object> item)
        //{
        //    return this._settings.Contains(item);
        //}

        //public void CopyTo(KeyValuePair<string, object>[] array, int arrayIndex)
        //{
        //    this._settings.CopyTo(array, arrayIndex);
        //}

        //public bool Remove(KeyValuePair<string, object> item)
        //{
        //    return this._settings.Remove(item);
        //}

        //public IEnumerator<KeyValuePair<string, object>> GetEnumerator()
        //{
        //    return this._settings.GetEnumerator();
        //}

        //IEnumerator IEnumerable.GetEnumerator()
        //{
        //    return this._settings.GetEnumerator();
        //}



        //#endregion


    }
}
