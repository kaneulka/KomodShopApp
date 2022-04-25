using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Komod.Data
{
    public class Cart
    {
        public long Id { get; set; }
        public string UserName { get; set; }
        public List<CartItem> CartItems { get; set; }

        public Cart()
        {
            CartItems = new List<CartItem>();
        }
    }

    public class CartMap
    {
        public CartMap(EntityTypeBuilder<Cart> entityBuilder)
        {
            entityBuilder.HasKey(t => t.Id);
            entityBuilder.Property(t => t.UserName).IsRequired();
        }
    }
}
