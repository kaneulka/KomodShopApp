using Komod.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Komod.Repo.DeliveryMethodRepo
{
    public class DeliveryMethodRepository : IDeliveryMethodRepository
    {
        private readonly ApplicationContext context;
        private DbSet<DeliveryMethod> deliveryMethod;
        string errorMessage = string.Empty;

        public DeliveryMethodRepository(ApplicationContext context)
        {
            this.context = context;
            deliveryMethod = context.Set<DeliveryMethod>();
        }
        public IEnumerable<DeliveryMethod> GetAll()
        {
            return deliveryMethod.AsEnumerable();
        }

        public DeliveryMethod Get(long id)
        {
            return deliveryMethod.SingleOrDefault(s => s.Id == id);
        }
        public void Insert(DeliveryMethod entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }
            deliveryMethod.Add(entity);
            context.SaveChanges();
        }

        public void Update(DeliveryMethod entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }
            context.SaveChanges();
        }

        public void Delete(DeliveryMethod entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }
            deliveryMethod.Remove(entity);
            context.SaveChanges();
        }
        public void Remove(DeliveryMethod entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }
            deliveryMethod.Remove(entity);
        }

        public void SaveChanges()
        {
            context.SaveChanges();
        }
    }
}
