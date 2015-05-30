using System.Collections.Generic;
using System.Dynamic;

namespace HairBand
{
    public class PageSettings : DynamicObject
    {
        public PageSettings()
        {
        }

        public void AddProperty(string name, object value)
        {
            this._properties.Add(name, value);
        }

        private Dictionary<string, object> _properties = new Dictionary<string, object>();

        public override bool TryGetMember(GetMemberBinder binder, out object result)
        {
            return _properties.TryGetValue(binder.Name, out result);
        }

        public override bool TrySetMember(SetMemberBinder binder, object value)
        {
            _properties[binder.Name] = value;
            return true;
        }


    }
}