using Komod.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace Komod.Ser.EventProductSer
{
    public interface IEventProductService
    {
        IEnumerable<EventProduct> GetEventProductes();
        EventProduct GetEventProduct(EventProduct eventProduct);
        void InsertEventProduct(EventProduct eventProduct);
        void InsertEventProducts(List<EventProduct> eventProducts);
        void UpdateEventProduct(EventProduct eventProduct);
        void DeleteEventProduct(EventProduct eventProduct);
        void DeleteEventProducts(List<EventProduct> eventProducts);
    }
}
