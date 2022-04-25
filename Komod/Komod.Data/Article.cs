using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Komod.Data
{
    public class Article:BaseEntity
    {
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public long StockStatusId { get; set; }
        public StockStatus StockStatus { get; set; }
        public long ProductId { get; set; }
        public Product Product { get; set; }
        public string ProductName { get; set; }
        public long ColorId { get; set; }
        public Color Color { get; set; }

        public List<OrderItem> OrderItems { get; set; }
        public List<PropertyValCatArt> PropertyValCatArts { get; set; }
        public List<PromocodeArticle> PromocodeArticles { get; set; }
        public Article()
        {
            PropertyValCatArts = new List<PropertyValCatArt>();
            PromocodeArticles = new List<PromocodeArticle>();
            OrderItems = new List<OrderItem>();
        }
    }
    public class ArticleMap
    {
        public ArticleMap(EntityTypeBuilder<Article> entityBuilder)
        {
            entityBuilder.HasKey(t => t.Id);
            entityBuilder.Property(t => t.Name).IsRequired();
            entityBuilder.Property(t => t.Price).IsRequired();
            entityBuilder.Property(t => t.ProductName).IsRequired();
            entityBuilder.Property(t => t.Quantity).IsRequired();

            entityBuilder.HasMany(o => o.OrderItems).WithOne(c => c.Article).HasForeignKey(o => o.ArticleId);
            entityBuilder.HasOne (o=> o.Product).WithMany(c => c.Articles).HasForeignKey(o => o.ProductId);
            entityBuilder.HasOne(o => o.StockStatus).WithMany(c => c.Articles).HasForeignKey(o => o.StockStatusId).OnDelete(DeleteBehavior.Cascade);
            entityBuilder.HasOne(o => o.Color).WithMany(c => c.Articles).HasForeignKey(o => o.ColorId);
        }
    }
}
