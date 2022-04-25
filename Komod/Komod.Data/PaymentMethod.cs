using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Komod.Data
{
    public class PaymentMethod:BaseEntity
    {
        public List<Order> Orders { get; set; }

        public PaymentMethod()
        {
            Orders = new List<Order>();
        }
    }
    public class PaymentMethodMap
    {
        public PaymentMethodMap(EntityTypeBuilder<PaymentMethod> entityBuilder)
        {
            entityBuilder.HasKey(t => t.Id);
            entityBuilder.Property(t => t.Name).IsRequired();

            entityBuilder.HasMany(t => t.Orders).WithOne(t => t.PaymentMethod).HasForeignKey(t => t.PaymentMethodId);
        }
    }
}
