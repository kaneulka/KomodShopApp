using Komod.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Komod.Repo.PromocodeRepo
{
    public class PromocodeRepository : IPromocodeRepository
    {
        private readonly ApplicationContext context;
        private DbSet<Promocode> promocodes;
        string errorMessage = string.Empty;

        public PromocodeRepository(ApplicationContext context)
        {
            this.context = context;
            promocodes = context.Set<Promocode>();
        }
        public IEnumerable<Promocode> GetAll()
        {
            return promocodes
                .Include(c => c.PromocodeArticles).ThenInclude(pv => pv.Article)//.ThenInclude(pv=> pv.Property)
                .AsEnumerable();
        }

        public Promocode Get(long id)
        {
            return promocodes
                .Include(c => c.PromocodeArticles).ThenInclude(pv => pv.Article)//.ThenInclude(pv=> pv.Property)
               
                .SingleOrDefault(s => s.Id == id);
        }
        public void Insert(Promocode entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }
            promocodes.Add(entity);
            context.SaveChanges();
        }

        public void Update(Promocode entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }
            context.SaveChanges();
        }

        public void Delete(Promocode entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }
            promocodes.Remove(entity);
            context.SaveChanges();
        }
        public void Remove(Promocode entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }
            promocodes.Remove(entity);
        }

        public void UpdateArray(List<Promocode> entity)//BDController
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
