using Komod.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Komod.Repo.PropertyValueRepo
{
    public class PropertyValueRepository : IPropertyValueRepository
    {
        private readonly ApplicationContext context;
        private DbSet<PropertyValue> propertyValues;
        string errorMessage = string.Empty;

        public PropertyValueRepository(ApplicationContext context)
        {
            this.context = context;
            propertyValues = context.Set<PropertyValue>();
        }
        public IEnumerable<PropertyValue> GetAll()
        {
            return propertyValues.Include(pv => pv.Property).AsEnumerable();
        }

        public PropertyValue Get(long id)
        {
            return propertyValues.Include(pv => pv.Property).SingleOrDefault(s => s.Id == id);
        }
        public void Insert(PropertyValue entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }
            propertyValues.Add(entity);
            context.SaveChanges();
        }

        public void Update(PropertyValue entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }
            context.SaveChanges();
        }

        public void Delete(PropertyValue entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }
            propertyValues.Remove(entity);
            context.SaveChanges();
        }
        public void Remove(PropertyValue entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }
            propertyValues.Remove(entity);
        }

        public void SaveChanges()
        {
            context.SaveChanges();
        }
    }
}
