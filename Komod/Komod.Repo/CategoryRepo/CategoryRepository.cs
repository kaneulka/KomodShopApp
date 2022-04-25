using Komod.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Komod.Repo.CategoryRepo
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly ApplicationContext context;
        private DbSet<Category> categories;
        string errorMessage = string.Empty;

        public CategoryRepository(ApplicationContext context)
        {
            this.context = context;
            categories = context.Set<Category>();
        }
        public IEnumerable<Category> GetAll()
        {
            return categories.AsEnumerable();//.Include(c => c.ParentCategory).Include(c => c.CategoryProperties).ThenInclude(cp => cp.Property).AsEnumerable();
        }

        public Category Get(long id)
        {
            return categories.SingleOrDefault(s => s.Id == id);//.Include(c => c.CategoryProperties).ThenInclude(cp => cp.Property).SingleOrDefault(s => s.Id == id);
        }
        public void Insert(Category entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }
            categories.Add(entity);
            context.SaveChanges();
        }

        public void Update(Category entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }
            context.SaveChanges();
        }

        public void Delete(Category entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }
            categories.Remove(entity);
            context.SaveChanges();
        }
        public void Remove(Category entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }
            categories.Remove(entity);
        }

        public void SaveChanges()
        {
            context.SaveChanges();
        }
    }
}
