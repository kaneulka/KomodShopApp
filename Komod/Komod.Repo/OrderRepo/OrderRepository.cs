using Komod.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Komod.Repo.OrderRepo
{
    public class OrderRepository : IOrderRepository
    {
        private readonly ApplicationContext context;
        private DbSet<Order> entities;
        string errorMessage = string.Empty;

        public OrderRepository(ApplicationContext context)
        {
            this.context = context;
            entities = context.Set<Order>();
        }
        public IEnumerable<Order> GetAll()
        {
            return entities.Include(o => o.OrderStatus)
                .Include(o=>o.DeliveryMethod)
                .Include(o=>o.OrderItems).
                AsEnumerable();
        }

        public Order Get(long id)
        {
            return entities.Include(o => o.OrderItems)
                .Include(o=>o.DeliveryMethod)
                .SingleOrDefault(s => s.Id == id);
        }
        public void Insert(Order entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }
            entities.Add(entity);
            context.SaveChanges();
        }

        public void Update(Order entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }
            context.SaveChanges();
        }

        public void Delete(Order entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }
            entities.Remove(entity);
            context.SaveChanges();
        }
        public void Remove(Order entity)
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
