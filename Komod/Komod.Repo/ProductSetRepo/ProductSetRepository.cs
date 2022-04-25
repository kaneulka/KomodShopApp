using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using Komod.Data;
using System.Linq;

namespace Komod.Repo.ProductSetRepo
{
    public class ProductSetRepository : IProductSetRepository
    {
        private readonly ApplicationContext context;
        private DbSet<ProductSet> brands;
        string errorMessage = string.Empty;

        public ProductSetRepository(ApplicationContext context)
        {
            this.context = context;
            brands = context.Set<ProductSet>();
        }
        public IEnumerable<ProductSet> GetAll()
        {
            return brands.AsEnumerable();
        }

        public ProductSet Get(long id)
        {
            return brands.SingleOrDefault(s => s.Id == id);
        }
        public void Insert(ProductSet entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }
            brands.Add(entity);
            context.SaveChanges();
        }

        public void Update(ProductSet entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }
            context.SaveChanges();
        }

        public void Delete(ProductSet entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }
            brands.Remove(entity);
            context.SaveChanges();
        }
        public void Remove(ProductSet entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }
            brands.Remove(entity);
        }

        public void SaveChanges()
        {
            context.SaveChanges();
        }
    }
}
