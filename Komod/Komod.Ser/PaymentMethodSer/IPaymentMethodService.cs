using Komod.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace Komod.Ser.PaymentMethodSer
{
    public interface IPaymentMethodService
    {
        IEnumerable<PaymentMethod> GetPaymentMethods();
        PaymentMethod GetPaymentMethod(long id);
        void InsertPaymentMethod(PaymentMethod PaymentMethod);
        void UpdatePaymentMethod(PaymentMethod PaymentMethod);
        void DeletePaymentMethod(long id);
    }
}
