using Komod.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Komod.Repo.PropertyRepo
{
    public class PropertyRepository : IPropertyRepository
    {
        private readonly ApplicationContext context;
        private DbSet<Property> properties;
        string errorMessage = string.Empty;

        public PropertyRepository(ApplicationContext context)
        {
            this.context = context;
            properties = context.Set<Property>();
        }
        public IEnumerable<Property> GetAll()
        {
            return properties.Include(pv => pv.PropertyValues).AsEnumerable();//.Include(c => c.CategoryProperties).ThenInclude(cp => cp.Category).AsEnumerable();
        }

        public Property Get(long id)
        {
            return properties.Include(pv => pv.PropertyValues).SingleOrDefault(s => s.Id == id);//.Include(c => c.CategoryProperties).ThenInclude(cp => cp.Category).SingleOrDefault(s => s.Id == id);
        }
        public void Insert(Property entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }
            properties.Add(entity);
            context.SaveChanges();
        }

        public void Update(Property entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }
            context.SaveChanges();
        }

        public void Delete(Property entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }
            properties.Remove(entity);
            context.SaveChanges();
        }
        public void Remove(Property entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }
            properties.Remove(entity);
        }

        public void SaveChanges()
        {
            context.SaveChanges();
        }
    }
}
