using Komod.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Komod.Repo.EventProductRepo
{
    public class EventProductRepository : IEventProductRepository
    {
        private readonly ApplicationContext context;
        private DbSet<EventProduct> entities;
        string errorMessage = string.Empty;

        public EventProductRepository(ApplicationContext context)
        {
            this.context = context;
            entities = context.Set<EventProduct>();
        }
        public IEnumerable<EventProduct> GetAll()
        {
            return entities.AsEnumerable();
        }

        public EventProduct Get(EventProduct entity)
        {
            return entities.SingleOrDefault(s => s.ProductId == entity.ProductId && s.EventPromotionId == entity.EventPromotionId);
        }
        public void Insert(EventProduct entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }
            entities.Add(entity);
            context.SaveChanges();
        }
        public void InsertAll(List<EventProduct> insertEntities)
        {
            if (insertEntities == null)
            {
                throw new ArgumentNullException("entity");
            }
            foreach (var ie in insertEntities)
            {
                entities.Add(ie);
            }
            context.SaveChanges();
        }

        public void Update(EventProduct entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }
            context.SaveChanges();
        }

        public void Delete(EventProduct entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }
            entities.Remove(entity);
            context.SaveChanges();
        }
        public void DeleteAll(List<EventProduct> insertEntities)
        {
            if (insertEntities == null)
            {
                throw new ArgumentNullException("entity");
            }
            foreach (var ie in insertEntities)
            {
                entities.Remove(ie);
            }
            context.SaveChanges();
        }
        public void Remove(EventProduct entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }
            entities.Remove(entity);
        }

        public void SaveChanges()
        {
            context.SaveChanges();
        }
    }
}
