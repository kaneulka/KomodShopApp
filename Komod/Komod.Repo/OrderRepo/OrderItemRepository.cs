using Komod.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Komod.Repo.OrderRepo
{
    public class OrderItemRepository : IOrderItemRepository
    {
        private readonly ApplicationContext context;
        private DbSet<OrderItem> entities;
        string errorMessage = string.Empty;

        public OrderItemRepository(ApplicationContext context)
        {
            this.context = context;
            entities = context.Set<OrderItem>();
        }
        public IEnumerable<OrderItem> GetAll()
        {
            return entities.AsEnumerable();
        }

        public OrderItem Get(long id)
        {
            return entities.SingleOrDefault(s => s.Id == id);
        }
        public void Insert(OrderItem entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }
            entities.Add(entity);
            context.SaveChanges();
        }

        public void Update(OrderItem entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }
            context.SaveChanges();
        }

        public void Delete(OrderItem entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }
            entities.Remove(entity);
            context.SaveChanges();
        }
        public void Remove(OrderItem entity)
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
