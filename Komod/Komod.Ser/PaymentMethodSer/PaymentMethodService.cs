using Komod.Data;
using Komod.Repo.PaymentMethodRepo;
using System;
using System.Collections.Generic;
using System.Text;

namespace Komod.Ser.PaymentMethodSer
{
    public class PaymentMethodService : IPaymentMethodService
    {
        private IPaymentMethodRepository paymentMethodRepository;

        public PaymentMethodService(IPaymentMethodRepository paymentMethodRepository)
        {
            this.paymentMethodRepository = paymentMethodRepository;
        }

        public IEnumerable<PaymentMethod> GetPaymentMethods()
        {
            return paymentMethodRepository.GetAll();
        }

        public PaymentMethod GetPaymentMethod(long id)
        {
            return paymentMethodRepository.Get(id);
        }

        public void InsertPaymentMethod(PaymentMethod paymentMethod)
        {
            paymentMethodRepository.Insert(paymentMethod);
        }
        public void UpdatePaymentMethod(PaymentMethod paymentMethod)
        {
            paymentMethodRepository.Update(paymentMethod);
        }

        public void DeletePaymentMethod(long id)
        {
            PaymentMethod paymentMethod = GetPaymentMethod(id);
            paymentMethodRepository.Remove(paymentMethod);
            paymentMethodRepository.SaveChanges();
        }
    }
}
