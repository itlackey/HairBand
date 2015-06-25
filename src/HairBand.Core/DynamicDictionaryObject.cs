using System;
using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;
using DotLiquid;

namespace HairBand
{
    public abstract class DynamicDictionaryObject : DynamicObject, IDictionary<string, object>, DotLiquid.ILiquidizable
    {

        public Dictionary<string, object> _properties = new Dictionary<string, object>();


        public DynamicDictionaryObject()
        {

        }

   

        public IDictionary<string, object> ToDictionary()
        {
            return this._properties;
        }

        public void Merge(IDictionary<object, object> input)
        {
            foreach (var item in input)
                this[item.Key.ToString()] = item.Value;

        }
        public void Merge(IDictionary<string, object> input)
        {
            foreach (var item in input)
                this[item.Key] = item.Value;

        }

        #region Liquid
        object ILiquidizable.ToLiquid()
        {
            return ToDictionary();
        }

        #endregion

        #region Dynamic
        public override bool TryGetMember(GetMemberBinder binder, out object result)
        {
            if (this.ContainsKey(binder.Name.ToLower()))
            {
                result = this[binder.Name.ToLower()];
                return true;

            }

            result = null;
            return false;
        }


        public override bool TrySetMember(SetMemberBinder binder, object value)
        {

            this.Add(binder.Name.ToLower(), value);

            return true;
        }


        public override bool TryDeleteIndex(DeleteIndexBinder binder, object[] indexes)
        {
            if (indexes.Count() > 0)
            {
                var name = indexes.FirstOrDefault().ToString();

                if (this.ContainsKey(name))
                {
                    this.Remove(name);

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
            if (this.ContainsKey(index))
                this[index] = value;
            else
                // If a corresponding property does not exist, create it.
                this.Add(index, value);
            return true;
        }

        // Get the property value by index.
        public override bool TryGetIndex(
            GetIndexBinder binder, object[] indexes, out object result)
        {

            var index = indexes[0].ToString();
            return this.TryGetValue(index, out result);
        }

        #endregion

        #region IDictionary

        public ICollection<string> Keys
        {
            get
            {
                return this._properties.Keys;
            }
        }

        public ICollection<object> Values
        {
            get
            {
                return this._properties.Values;
            }
        }

        public int Count
        {
            get
            {
                return this._properties.Count;
            }
        }

        public bool IsReadOnly
        {
            get
            {
                return ((IDictionary<string, object>)_properties).IsReadOnly;
            }
        }


        public object this[string key]
        {
            get
            {

                return this._properties[key.ToLower()];
            }

            set
            {

                if (this._properties.ContainsKey(key.ToLower()))
                    this._properties[key.ToLower()] = value;
                else
                    this._properties.Add(key.ToLower(), value);
            }
        }

        public bool ContainsKey(string key)
        {
            return this._properties.ContainsKey(key.ToLower());
        }

        public void Add(string key, object value)
        {
            this._properties.Add(key.ToLower(), value);
        }

        public bool Remove(string key)
        {
            return this._properties.Remove(key.ToLower());
        }

        public bool TryGetValue(string key, out object value)
        {
            return this._properties.TryGetValue(key.ToLower(), out value);
        }

        public void Add(KeyValuePair<string, object> item)
        {
            this._properties.Add(item.Key, item.Value);
        }

        public void Clear()
        {
            this._properties.Clear();
        }

        public bool Contains(KeyValuePair<string, object> item)
        {
            return this._properties.Contains(item);
        }

        public void CopyTo(KeyValuePair<string, object>[] array, int arrayIndex)
        {
            ((IDictionary<string, object>)_properties).CopyTo(array, arrayIndex);
        }

        public bool Remove(KeyValuePair<string, object> item)
        {
            return this._properties.Remove(item.Key.ToLower());
        }

        public IEnumerator<KeyValuePair<string, object>> GetEnumerator()
        {
            return this._properties.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this._properties.GetEnumerator();
        }



        #endregion
    

    }
}
