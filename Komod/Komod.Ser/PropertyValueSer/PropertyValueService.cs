using Komod.Data;
using Komod.Repo.PropertyValueRepo;
using System;
using System.Collections.Generic;
using System.Text;

namespace Komod.Ser.PropertyValueSer
{
    public class PropertyValueService : IPropertyValueService
    {
        private IPropertyValueRepository propertyValueRepository;

        public PropertyValueService(IPropertyValueRepository propertyValueRepository)
        {
            this.propertyValueRepository = propertyValueRepository;
        }

        public IEnumerable<PropertyValue> GetPropertyValues()
        {
            return propertyValueRepository.GetAll();
        }

        public PropertyValue GetPropertyValue(long id)
        {
            return propertyValueRepository.Get(id);
        }

        public void InsertPropertyValue(PropertyValue propertyValue)
        {
            propertyValueRepository.Insert(propertyValue);
        }
        public void UpdatePropertyValue(PropertyValue propertyValue)
        {
            propertyValueRepository.Update(propertyValue);
        }

        public void DeletePropertyValue(long id)
        {
            PropertyValue propertyValue = GetPropertyValue(id);
            propertyValueRepository.Remove(propertyValue);
            propertyValueRepository.SaveChanges();
        }
    }
}
