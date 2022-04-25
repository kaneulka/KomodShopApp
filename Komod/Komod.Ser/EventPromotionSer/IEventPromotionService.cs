using Komod.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace Komod.Ser.EventPromotionSer
{
    public interface IEventPromotionService
    {
        IEnumerable<EventPromotion> GetEventPromotions();
        EventPromotion GetEventPromotion(long id);
        void InsertEventPromotion(EventPromotion eventAction);
        void UpdateEventPromotion(EventPromotion eventAction);
        void DeleteEventPromotion(long id);
    }
}
