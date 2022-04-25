using Komod.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace Komod.Repo.PropertyValueRepo
{
    public interface IPropertyValueRepository
    { 
        IEnumerable<PropertyValue> GetAll();
        PropertyValue Get(long id);
        void Insert(PropertyValue entity);
        void Update(PropertyValue entity);
        void Delete(PropertyValue entity);
        void Remove(PropertyValue entity);
        void SaveChanges();
    }
}
