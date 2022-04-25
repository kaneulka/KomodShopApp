using Komod.Data;
using Komod.Repo.EventPromotionRepo;
using System;
using System.Collections.Generic;
using System.Text;

namespace Komod.Ser.EventPromotionSer
{
    public class EventPromotionService : IEventPromotionService
    {
        private IEventPromotionRepository eventRepository;

        public EventPromotionService(IEventPromotionRepository eventRepository)
        {
            this.eventRepository = eventRepository;
        }

        public IEnumerable<EventPromotion> GetEventPromotions()
        {
            return eventRepository.GetAll();
        }

        public EventPromotion GetEventPromotion(long id)
        {
            return eventRepository.Get(id);
        }

        public void InsertEventPromotion(EventPromotion eventAction)
        {
            eventRepository.Insert(eventAction);
        }
        public void UpdateEventPromotion(EventPromotion eventAction)
        {
            eventRepository.Update(eventAction);
        }

        public void DeleteEventPromotion(long id)
        {
            EventPromotion eventAction = GetEventPromotion(id);
            eventRepository.Remove(eventAction);
            eventRepository.SaveChanges();
        }
    }
}
