using Komod.Data;
using Komod.Repo.PropertyRepo;
using System;
using System.Collections.Generic;
using System.Text;

namespace Komod.Ser.PropertySer
{
    public class PropertyService : IPropertyService
    {
        private IPropertyRepository propertyRepository;

        public PropertyService(IPropertyRepository propertyRepository)
        {
            this.propertyRepository = propertyRepository;
        }

        public IEnumerable<Property> GetProperties()
        {
            return propertyRepository.GetAll();
        }

        public Property GetProperty(long id)
        {
            return propertyRepository.Get(id);
        }

        public void InsertProperty(Property property)
        {
            propertyRepository.Insert(property);
        }
        public void UpdateProperty(Property property)
        {
            propertyRepository.Update(property);
        }

        public void DeleteProperty(long id)
        {
            Property property = GetProperty(id);
            propertyRepository.Remove(property);
            propertyRepository.SaveChanges();
        }
    }
}
