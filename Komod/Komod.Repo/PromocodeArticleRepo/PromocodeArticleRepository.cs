using Komod.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Komod.Repo.PromocodeArticleRepo
{
    public class PromocodeArticleRepository : IPromocodeArticleRepository
    {
        private readonly ApplicationContext context;
        private DbSet<PromocodeArticle> propertyValCatArts;
        string errorMessage = string.Empty;

        public PromocodeArticleRepository(ApplicationContext context)
        {
            this.context = context;
            propertyValCatArts = context.Set<PromocodeArticle>();
        }

        public PromocodeArticle Get(PromocodeArticle entity)
        {
            return propertyValCatArts.SingleOrDefault(s => s.ArticleId == entity.ArticleId && s.PromocodeId == entity.PromocodeId);
        }
        public List<PromocodeArticle> GetAll()
        {
            return propertyValCatArts.ToList();
        }
        public void Insert(PromocodeArticle entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }
            propertyValCatArts.Add(entity);
            context.SaveChanges();
        }
        public void Delete(PromocodeArticle entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }
            propertyValCatArts.Remove(entity);
            context.SaveChanges();
        }
        public void DeleteSome(List<PromocodeArticle> entities)
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
        public void InsertSome(List<PromocodeArticle> entities)
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
        public void Remove(PromocodeArticle entity)
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
