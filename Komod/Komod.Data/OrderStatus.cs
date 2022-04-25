using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Komod.Data
{
    public class OrderStatus:BaseEntity
    {
        public List<Order> Orders { get; set; }

        public OrderStatus()
        {
            Orders = new List<Order>();
        }
    }

    public class OrderStatusMap
    {
        public OrderStatusMap(EntityTypeBuilder<OrderStatus> entityBuilder)
        {
            entityBuilder.HasKey(t => t.Id);
            entityBuilder.Property(t => t.Name).IsRequired();

            entityBuilder.HasMany(t => t.Orders).WithOne(t => t.OrderStatus).HasForeignKey(t=> t.OrderStatusId);
        }
    }
}
