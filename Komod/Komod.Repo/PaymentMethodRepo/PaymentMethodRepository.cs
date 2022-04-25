using Komod.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Komod.Repo.PaymentMethodRepo
{
    public class PaymentMethodRepository : IPaymentMethodRepository
    {
        private readonly ApplicationContext context;
        private DbSet<PaymentMethod> paymentMethod;
        string errorMessage = string.Empty;

        public PaymentMethodRepository(ApplicationContext context)
        {
            this.context = context;
            paymentMethod = context.Set<PaymentMethod>();
        }
        public IEnumerable<PaymentMethod> GetAll()
        {
            return paymentMethod.AsEnumerable();
        }

        public PaymentMethod Get(long id)
        {
            return paymentMethod.SingleOrDefault(s => s.Id == id);
        }
        public void Insert(PaymentMethod entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }
            paymentMethod.Add(entity);
            context.SaveChanges();
        }

        public void Update(PaymentMethod entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }
            context.SaveChanges();
        }

        public void Delete(PaymentMethod entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }
            paymentMethod.Remove(entity);
            context.SaveChanges();
        }
        public void Remove(PaymentMethod entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }
            paymentMethod.Remove(entity);
        }

        public void SaveChanges()
        {
            context.SaveChanges();
        }
    }
}
