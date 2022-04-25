using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Komod.Data
{
    //class DeliveryMethod
    //{
    //}
    public class DeliveryMethod : BaseEntity
    {
        public string District { get; set; }
        public int DeliveryPrice { get; set; }
        public int FreeDelivery { get; set; }
        public List<Order> Orders { get; set; }

        public DeliveryMethod()
        {
            Orders = new List<Order>();
        }
    }
    public class DeliveryMethodMap
    {
        public DeliveryMethodMap(EntityTypeBuilder<DeliveryMethod> entityBuilder)
        {
            entityBuilder.HasKey(t => t.Id);
            entityBuilder.Property(t => t.Name).IsRequired();

            entityBuilder.HasMany(t => t.Orders).WithOne(t => t.DeliveryMethod).HasForeignKey(t => t.DeliveryMethodId);
        }
    }
}
