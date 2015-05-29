using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HairBand
{
    public class PageData : IDictionary<string, object>
    {
        public string Content { get; set; }

        public string  Title { get; set; }

        public string Exceprt { get; set; }

        public string Url { get; set; }

        public DateTime Date { get; set; }


        public string Id { get; set; }

        public IEnumerable<string> Categories { get; set; }

        public IEnumerable<string> Tags { get; set; }

        public string Path { get; set; }


        public PostData Next { get; set; }

        public PostData Previous { get; set; }




        public IDictionary<string, object> Settings { get; set; }

        public dynamic Metadata { get; set; }

        #region IDictionary

        public ICollection<string> Keys
        {
            get
            {
                return this.Settings.Keys;
            }
        }

        public ICollection<object> Values
        {
            get
            {
                return this.Settings.Values;
            }
        }

        public int Count
        {
            get
            {
                return this.Settings.Count;
            }
        }

        public bool IsReadOnly
        {
            get
            {
                return this.Settings.IsReadOnly;
            }
        }

        public object this[string key]
        {
            get
            {
                return this.Settings[key];
            }

            set
            {
                this.Settings[key] = value;
            }
        }

        public bool ContainsKey(string key)
        {
            return this.Settings.ContainsKey(key);
        }

        public void Add(string key, object value)
        {
            this.Settings.Add(key, value);
        }

        public bool Remove(string key)
        {
            return this.Settings.Remove(key);
        }

        public bool TryGetValue(string key, out object value)
        {
            return this.Settings.TryGetValue(key, out value);
        }

        public void Add(KeyValuePair<string, object> item)
        {
            this.Settings.Add(item);
        }

        public void Clear()
        {
            this.Settings.Clear();
        }

        public bool Contains(KeyValuePair<string, object> item)
        {
            return this.Settings.Contains(item);
        }

        public void CopyTo(KeyValuePair<string, object>[] array, int arrayIndex)
        {
            this.Settings.CopyTo(array, arrayIndex);
        }

        public bool Remove(KeyValuePair<string, object> item)
        {
            return this.Settings.Remove(item);
        }

        public IEnumerator<KeyValuePair<string, object>> GetEnumerator()
        {
            return this.Settings.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.Settings.GetEnumerator();
        }

  

        #endregion


    }
}
