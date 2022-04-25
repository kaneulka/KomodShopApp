using Komod.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace Komod.Ser.PropertySer
{
    public interface IPropertyService
    {
        IEnumerable<Property> GetProperties();
        Property GetProperty(long id);
        void InsertProperty(Property Property);
        void UpdateProperty(Property Property);
        void DeleteProperty(long id);
    }
}
