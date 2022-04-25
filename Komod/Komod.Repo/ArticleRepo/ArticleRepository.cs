using Komod.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Komod.Repo.ArticleRepo
{
    public class ArticleRepository : IArticleRepository
    {
        private readonly ApplicationContext context;
        private DbSet<Article> articles;
        string errorMessage = string.Empty;

        public ArticleRepository(ApplicationContext context)
        {
            this.context = context;
            articles = context.Set<Article>();
        }
        public IEnumerable<Article> GetAll()
        {
            return articles
                //.Include(a => a.Product)
                .Include(c=> c.Color)
                .Include(p => p.StockStatus)
                .Include(c => c.PropertyValCatArts).ThenInclude(pv => pv.PropertyValue).ThenInclude(pv=> pv.Property)
                .AsEnumerable();
        }

        public Article Get(long id)
        {
            return articles
                //.Include(a => a.Product)
                .Include(c => c.Color)
                .Include(p => p.StockStatus)
                .Include(c => c.PropertyValCatArts).ThenInclude(pv => pv.PropertyValue).ThenInclude(pv => pv.Property)
                .SingleOrDefault(s => s.Id == id);
        }
        public void Insert(Article entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }
            articles.Add(entity);
            context.SaveChanges();
        }

        public void Update(Article entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }
            context.SaveChanges();
        }

        public void Delete(Article entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }
            articles.Remove(entity);
            context.SaveChanges();
        }
        public void Remove(Article entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }
            articles.Remove(entity);
        }

        public void UpdateArray(List<Article> entity)//BDController
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }
            context.SaveChanges();
        }

        public void SaveChanges()
        {
            context.SaveChanges();
        }
    }
}
