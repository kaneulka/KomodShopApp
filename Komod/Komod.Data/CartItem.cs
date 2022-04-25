using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Komod.Data
{
    public class CartItem
    {
        public long Id { get; set; }
        public long CartId { get; set; }
        public Cart Cart { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal TotalPrice { get; set; }
        public int Quantity { get; set; }
        public long ArticleId { get; set; }
        public Article Article { get; set; }
    }

    public class CartItemMap
    {
        public CartItemMap(EntityTypeBuilder<CartItem> entityBuilder)
        {
            entityBuilder.HasKey(t => t.Id);
            entityBuilder.Property(t => t.CartId).IsRequired();
            entityBuilder.Property(t => t.UnitPrice).IsRequired();
            entityBuilder.Property(t => t.TotalPrice).IsRequired();
            entityBuilder.Property(t => t.Quantity).IsRequired();
            entityBuilder.Property(t => t.ArticleId).IsRequired();

            entityBuilder.HasOne(o => o.Cart).WithMany(c => c.CartItems).HasForeignKey(o => o.CartId);
        }
    }
}
