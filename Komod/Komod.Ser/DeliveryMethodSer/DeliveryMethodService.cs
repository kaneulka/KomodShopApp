using Komod.Data;
using Komod.Repo.DeliveryMethodRepo;
using System;
using System.Collections.Generic;
using System.Text;

namespace Komod.Ser.DeliveryMethodSer
{
    public class DeliveryMethodService : IDeliveryMethodService
    {
        private IDeliveryMethodRepository deliveryMethodRepository;

        public DeliveryMethodService(IDeliveryMethodRepository deliveryMethodRepository)
        {
            this.deliveryMethodRepository = deliveryMethodRepository;
        }

        public IEnumerable<DeliveryMethod> GetDeliveryMethods()
        {
            return deliveryMethodRepository.GetAll();
        }

        public DeliveryMethod GetDeliveryMethod(long id)
        {
            return deliveryMethodRepository.Get(id);
        }

        public void InsertDeliveryMethod(DeliveryMethod deliveryMethod)
        {
            deliveryMethodRepository.Insert(deliveryMethod);
        }
        public void UpdateDeliveryMethod(DeliveryMethod deliveryMethod)
        {
            deliveryMethodRepository.Update(deliveryMethod);
        }

        public void DeleteDeliveryMethod(long id)
        {
            DeliveryMethod deliveryMethod = GetDeliveryMethod(id);
            deliveryMethodRepository.Remove(deliveryMethod);
            deliveryMethodRepository.SaveChanges();
        }
    }
}
