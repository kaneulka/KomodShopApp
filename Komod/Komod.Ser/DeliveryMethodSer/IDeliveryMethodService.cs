using Komod.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace Komod.Ser.DeliveryMethodSer
{
    public interface IDeliveryMethodService
    {
        IEnumerable<DeliveryMethod> GetDeliveryMethods();
        DeliveryMethod GetDeliveryMethod(long id);
        void InsertDeliveryMethod(DeliveryMethod DeliveryMethod);
        void UpdateDeliveryMethod(DeliveryMethod DeliveryMethod);
        void DeleteDeliveryMethod(long id);
    }
}
