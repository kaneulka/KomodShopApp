using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using Komod.Data;
using System.Linq;

namespace Komod.Repo.CountryRepo
{
    public class CountryRepository : ICountryRepository
    {
        private readonly ApplicationContext context;
        private DbSet<Country> countrys;
        string errorMessage = string.Empty;

        public CountryRepository(ApplicationContext context)
        {
            this.context = context;
            countrys = context.Set<Country>();
        }
        public IEnumerable<Country> GetAll()
        {
            return countrys.AsEnumerable();
        }

        public Country Get(long id)
        {
            return countrys.SingleOrDefault(s => s.Id == id);
        }
        public void Insert(Country entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }
            countrys.Add(entity);
            context.SaveChanges();
        }

        public void Update(Country entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }
            context.SaveChanges();
        }

        public void Delete(Country entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }
            countrys.Remove(entity);
            context.SaveChanges();
        }
        public void Remove(Country entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }
            countrys.Remove(entity);
        }

        public void SaveChanges()
        {
            context.SaveChanges();
        }
    }
}
