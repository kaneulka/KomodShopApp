using Komod.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Komod.Repo.OrderStatusRepo
{
    public class OrderStatusRepository : IOrderStatusRepository
    {
        private readonly ApplicationContext context;
        private DbSet<OrderStatus> orderStatuses;
        string errorMessage = string.Empty;

        public OrderStatusRepository(ApplicationContext context)
        {
            this.context = context;
            orderStatuses = context.Set<OrderStatus>();
        }
        public IEnumerable<OrderStatus> GetAll()
        {
            return orderStatuses.AsEnumerable();
        }

        public OrderStatus Get(long id)
        {
            return orderStatuses.SingleOrDefault(s => s.Id == id);
        }
        public void Insert(OrderStatus entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }
            orderStatuses.Add(entity);
            context.SaveChanges();
        }

        public void Update(OrderStatus entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }
            context.SaveChanges();
        }

        public void Delete(OrderStatus entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }
            orderStatuses.Remove(entity);
            context.SaveChanges();
        }
        public void Remove(OrderStatus entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }
            orderStatuses.Remove(entity);
        }

        public void SaveChanges()
        {
            context.SaveChanges();
        }
    }
}
