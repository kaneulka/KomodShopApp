using Komod.Data;
using Microsoft.AspNetCore.DataProtection.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Komod.Repo
{
    public class ApplicationContext : IdentityDbContext<User>, IDataProtectionKeyContext
    {

        public DbSet<StockStatus> StockStatus { get; set; }
        public DbSet<OrderStatus> OrderStatus { get; set; }
        public DbSet<PaymentMethod> PaymentMethod { get; set; }
        public DbSet<DataProtectionKey> DataProtectionKeys { get; set; }
        public DbSet<Color> Color { get; set; }
        public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            new ArticleMap(modelBuilder.Entity<Article>());
            new BrandMap(modelBuilder.Entity<Brand>());
            new CartMap(modelBuilder.Entity<Cart>());
            new CartItemMap(modelBuilder.Entity<CartItem>());
            new CategoryMap(modelBuilder.Entity<Category>());
            new DeliveryMethodMap(modelBuilder.Entity<DeliveryMethod>());
            new EventProductMap(modelBuilder.Entity<EventProduct>());
            new EventPromotionMap(modelBuilder.Entity<EventPromotion>());
            new ImageMap(modelBuilder.Entity<Image>());
            new OrderMap(modelBuilder.Entity<Order>());
            new OrderItemMap(modelBuilder.Entity<OrderItem>());
            new OrderStatusMap(modelBuilder.Entity<OrderStatus>());
            new PaymentMethodMap(modelBuilder.Entity<PaymentMethod>());
            new ProductMap(modelBuilder.Entity<Product>());
            new PropertyMap(modelBuilder.Entity<Property>());
            new PropertyValCatArtMap(modelBuilder.Entity<PropertyValCatArt>());
            new PropertyValueMap(modelBuilder.Entity<PropertyValue>());
            new StockStatusMap(modelBuilder.Entity<StockStatus>());
            new UserAddressMap(modelBuilder.Entity<UserAddress>());
            new WishlistMap(modelBuilder.Entity<Wishlist>());
            new WishlistItemMap(modelBuilder.Entity<WishlistItem>());
            new ColorMap(modelBuilder.Entity<Color>());
            new CountryMap(modelBuilder.Entity<Country>());
            new PromocodeMap(modelBuilder.Entity<Promocode>());
            new PromocodeArticleMap(modelBuilder.Entity<PromocodeArticle>());
            new ProductSetMap(modelBuilder.Entity<ProductSet>());
        }
    }
}