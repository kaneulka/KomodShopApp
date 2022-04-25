using Komod.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Komod.Repo.EventPromotionRepo
{
    public class EventPromotionRepository : IEventPromotionRepository
    {
        private readonly ApplicationContext context;
        private DbSet<EventPromotion> events;
        string errorMessage = string.Empty;

        public EventPromotionRepository(ApplicationContext context)
        {
            this.context = context;
            events = context.Set<EventPromotion>();
        }
        public IEnumerable<EventPromotion> GetAll()
        {
            return events.AsEnumerable();
        }

        public EventPromotion Get(long id)
        {
            return events.SingleOrDefault(s => s.Id == id);
        }
        public void Insert(EventPromotion entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }
            events.Add(entity);
            context.SaveChanges();
        }

        public void Update(EventPromotion entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }
            context.SaveChanges();
        }

        public void Delete(EventPromotion entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }
            events.Remove(entity);
            context.SaveChanges();
        }
        public void Remove(EventPromotion entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }
            events.Remove(entity);
        }

        public void SaveChanges()
        {
            context.SaveChanges();
        }
    }
}
