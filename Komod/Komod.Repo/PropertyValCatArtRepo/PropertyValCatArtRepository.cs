using Komod.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Komod.Repo.PropertyValCatArtRepo
{
    public class PropertyValCatArtRepository : IPropertyValCatArtRepository
    {
        private readonly ApplicationContext context;
        private DbSet<PropertyValCatArt> propertyValCatArts;
        string errorMessage = string.Empty;

        public PropertyValCatArtRepository(ApplicationContext context)
        {
            this.context = context;
            propertyValCatArts = context.Set<PropertyValCatArt>();
        }

        public PropertyValCatArt Get(PropertyValCatArt entity)
        {
            return propertyValCatArts.SingleOrDefault(s => s.ArticleId == entity.ArticleId && s.ProductId == entity.ProductId && s.PropertyValueId == entity.PropertyValueId && s.CategoryId == entity.CategoryId);
        }
        public List<PropertyValCatArt> GetAll()
        {
            return propertyValCatArts.ToList();
        }
        public void Insert(PropertyValCatArt entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }
            propertyValCatArts.Add(entity);
            context.SaveChanges();
        }
        public void Delete(PropertyValCatArt entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }
            propertyValCatArts.Remove(entity);
            context.SaveChanges();
        }
        public void DeleteSome(List<PropertyValCatArt> entities)
        {
            if (entities == null)
            {
                throw new ArgumentNullException("entity");
            }
            foreach (var entity in entities)
            {
                propertyValCatArts.Remove(entity);
            }
            context.SaveChanges();
        }
        public void InsertSome(List<PropertyValCatArt> entities)
        {
            if (entities == null)
            {
                throw new ArgumentNullException("entity");
            }
            foreach (var entity in entities)
            {
                propertyValCatArts.Add(entity);
            }
            context.SaveChanges();
        }
        public void Remove(PropertyValCatArt entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }
            propertyValCatArts.Remove(entity);
        }

        public void SaveChanges()
        {
            context.SaveChanges();
        }
    }
}
