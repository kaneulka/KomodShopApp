using Komod.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace Komod.Ser.PropertyValueSer
{
    public interface IPropertyValueService
    {
        IEnumerable<PropertyValue> GetPropertyValues();
        PropertyValue GetPropertyValue(long id);
        void InsertPropertyValue(PropertyValue PropertyValue);
        void UpdatePropertyValue(PropertyValue PropertyValue);
        void DeletePropertyValue(long id);
    }
}
