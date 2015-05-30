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
        public IDictionary<string, object> _settings;

        public PageData()
        {
            this._settings = new Dictionary<string, object>();
            this.Date = DateTime.Now;
        }

        public string Content { get; set; }

        public string Title { get; set; }

        public string Exceprt { get; set; }

        public string Url { get; set; }

        public DateTime Date { get; set; }

        public string Id { get; set; }

        public IEnumerable<string> Categories { get; set; }

        public IEnumerable<string> Tags { get; set; }

        public string Path { get; set; }

        public PostData Next { get; set; }

        public PostData Previous { get; set; }


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
