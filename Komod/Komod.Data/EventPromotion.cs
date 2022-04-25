using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Komod.Data
{
    public class EventPromotion:BaseEntity
    {
        public DateTime StartEvent { get; set; }
        public DateTime EndEvent { get; set; }
        public string Description { get; set; }
        public string ImgPath { get; set; }
        public bool ActiveEvent { get; set; }
        public decimal DiscountPercent { get; set; }

        public List<EventProduct> EventProducts { get; set; }
        public EventPromotion()
        {
            EventProducts = new List<EventProduct>();
        }
    }
    public class EventPromotionMap
    {
        public EventPromotionMap(EntityTypeBuilder<EventPromotion> entityBuilder)
        {
            entityBuilder.HasKey(t => t.Id);
            entityBuilder.Property(t => t.Name).IsRequired();
            entityBuilder.Property(t => t.Description);
            entityBuilder.Property(t => t.ImgPath);
            entityBuilder.Property(t => t.ActiveEvent);
        }
    }
}
