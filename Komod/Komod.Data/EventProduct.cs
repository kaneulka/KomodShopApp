using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Komod.Data
{
    public class EventProduct
    {
        public long ProductId { get; set; }
        public Product Product { get; set; }
        public long EventPromotionId { get; set; }
        public EventPromotion EventPromotion { get; set; }
    }
    public class EventProductMap
    {
        public EventProductMap(EntityTypeBuilder<EventProduct> entityBuilder)
        {
            entityBuilder.HasKey(t => new { t.ProductId, t.EventPromotionId });

            entityBuilder.HasOne(t => t.EventPromotion).WithMany(t => t.EventProducts).HasForeignKey(t=>t.EventPromotionId);
            entityBuilder.HasOne(t => t.Product).WithMany(t => t.EventProducts).HasForeignKey(t=> t.ProductId);
        }
    }
}
