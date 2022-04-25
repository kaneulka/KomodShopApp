using Komod.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Komod.Repo.ProductRepo
{
    public class ProductRepository : IProductRepository
    {
        private readonly ApplicationContext context;
        private DbSet<Product> products;
        string errorMessage = string.Empty;

        public ProductRepository(ApplicationContext context)
        {
            this.context = context;
            products = context.Set<Product>();
        }
        public IEnumerable<Product> GetAll()
        {
            return products
                //.Include(p => p.Articles)???
                .Include(p => p.Category)
                .Include(p => p.Brand)
                .Include(p => p.Images)
                .Include(p => p.Country)
                .AsEnumerable();
        }

        public Product Get(long id)
        {
            return products
                .Include(p => p.Category)
                .Include(p => p.Brand)
                .Include(p => p.Country)
                .Include(p => p.Images)//Почему этого не было???
                .SingleOrDefault(s => s.Id == id);
        }
        public void Insert(Product entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }
            products.Add(entity);
            context.SaveChanges();
        }

        public void Update(Product entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }
            context.SaveChanges();
        }

        public void Delete(Product entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }
            products.Remove(entity);
            context.SaveChanges();
        }
        public void Remove(Product entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }
            products.Remove(entity);
        }

        public void UpdateArray(List<Product> entity)
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
