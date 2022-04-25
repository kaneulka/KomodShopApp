using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Komod.Data
{
    public class Order
    {
        public long Id { get; set; }
        public string UserName { get; set; }
        public string OrderNumber { get; set; }
        public decimal TotalPrice { get; set; }
        public DateTime AddedDate { get; set; }
        public string Comment { get; set; }
        //public string DeliveryMethod { get; set; }
        public long PaymentMethodId { get; set; }
        public PaymentMethod PaymentMethod { get; set; }
        public long DeliveryMethodId { get; set; }
        public DeliveryMethod DeliveryMethod { get; set; }
        public long OrderStatusId { get; set; }
        public OrderStatus OrderStatus { get; set; }
        public string ClientFIO { get; set; }
        public string ClientPhone { get; set; }
        public string ClientOtherPhone { get; set; }
        public string ClientEmail { get; set; }
        public string ClientAddress { get; set; }
        public decimal DeliveryPrice { get; set; }
        public string DiscountType { get; set; }
        public decimal DiscountPercent { get; set; }
        public string PromoName { get; set; }

        public List<OrderItem> OrderItems { get; set; }

        public Order()
        {   
            OrderItems = new List<OrderItem>();
        }
    }
    public class OrderMap
    {
        public OrderMap(EntityTypeBuilder<Order> entityBuilder)
        {
            entityBuilder.HasKey(t => t.Id);
            entityBuilder.Property(t => t.UserName).IsRequired();
        }
    }
}
