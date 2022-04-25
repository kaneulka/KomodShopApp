using Komod.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Komod.Repo.StockStatusRepo
{
    public class StockStatusRepository : IStockStatusRepository
    {
        private readonly ApplicationContext context;
        private DbSet<StockStatus> stockStatuses;
        string errorMessage = string.Empty;

        public StockStatusRepository(ApplicationContext context)
        {
            this.context = context;
            stockStatuses = context.Set<StockStatus>();
        }
        public IEnumerable<StockStatus> GetAll()
        {
            return stockStatuses.AsEnumerable();
        }

        public StockStatus Get(long id)
        {
            return stockStatuses.SingleOrDefault(s => s.Id == id);
        }
        public void Insert(StockStatus entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }
            stockStatuses.Add(entity);
            context.SaveChanges();
        }

        public void Update(StockStatus entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }
            context.SaveChanges();
        }

        public void Delete(StockStatus entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }
            stockStatuses.Remove(entity);
            context.SaveChanges();
        }
        public void Remove(StockStatus entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }
            stockStatuses.Remove(entity);
        }

        public void SaveChanges()
        {
            context.SaveChanges();
        }
    }
}
