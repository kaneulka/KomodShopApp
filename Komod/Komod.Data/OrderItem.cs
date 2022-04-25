using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Komod.Data
{
    public class OrderItem
    {
        public long Id { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal TotalPrice { get; set; }

        public long OrderId { get; set; }
        public Order Order { get; set; }
        public long ArticleId { get; set; }
        public Article Article { get; set; }
    }
    public class OrderItemMap
    {
        public OrderItemMap(EntityTypeBuilder<OrderItem> entityBuilder)
        {
            entityBuilder.HasKey(t => t.Id);
            entityBuilder.Property(t => t.Quantity).IsRequired();
            entityBuilder.Property(t => t.UnitPrice).IsRequired();
            entityBuilder.Property(t => t.TotalPrice).IsRequired();

            entityBuilder.HasOne(o => o.Order).WithMany(c => c.OrderItems).HasForeignKey(o => o.OrderId);
        }
    }
}
