using Komod.Data;
using Komod.Repo.EventProductRepo;
using System;
using System.Collections.Generic;
using System.Text;

namespace Komod.Ser.EventProductSer
{
    public class EventProductService : IEventProductService
    {
        private IEventProductRepository eventProductRepository;

        public EventProductService(IEventProductRepository eventProductRepository)
        {
            this.eventProductRepository = eventProductRepository;
        }

        public IEnumerable<EventProduct> GetEventProductes()
        {
            return eventProductRepository.GetAll();
        }

        public EventProduct GetEventProduct(EventProduct eventProduct)
        {
            return eventProductRepository.Get(eventProduct);
        }

        public void InsertEventProduct(EventProduct eventProduct)
        {
            eventProductRepository.Insert(eventProduct);
        }
        public void InsertEventProducts(List<EventProduct> eventProducts)
        {
            eventProductRepository.InsertAll(eventProducts);
        }
        public void UpdateEventProduct(EventProduct eventProduct)
        {
            eventProductRepository.Update(eventProduct);
        }

        public void DeleteEventProduct(EventProduct eventProduct)
        {
            eventProductRepository.Delete(eventProduct);
        }
        public void DeleteEventProducts(List<EventProduct> eventProducts)
        {
            eventProductRepository.DeleteAll(eventProducts);
        }
    }
}
