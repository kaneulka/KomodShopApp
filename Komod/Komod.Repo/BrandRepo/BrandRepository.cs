using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using Komod.Data;
using System.Linq;

namespace Komod.Repo.BrandRepo
{
    public class BrandRepository : IBrandRepository
    {
        private readonly ApplicationContext context;
        private DbSet<Brand> brands;
        string errorMessage = string.Empty;

        public BrandRepository(ApplicationContext context)
        {
            this.context = context;
            brands = context.Set<Brand>();
        }
        public IEnumerable<Brand> GetAll()
        {
            return brands.AsEnumerable();
        }

        public Brand Get(long id)
        {
            return brands.SingleOrDefault(s => s.Id == id);
        }
        public void Insert(Brand entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }
            brands.Add(entity);
            context.SaveChanges();
        }

        public void Update(Brand entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }
            context.SaveChanges();
        }

        public void Delete(Brand entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }
            brands.Remove(entity);
            context.SaveChanges();
        }
        public void Remove(Brand entity)
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
