using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Komod.Data
{
    public class Product:BaseEntity
    {
        public long? BrandId { get; set; }
        public Brand Brand { get; set; }
        public long? CountryId { get; set; }
        public Country Country { get; set; }
        public long CategoryId { get; set; }
        public Category Category { get; set; }
        public string Path { get; set; }
        public string Description { get; set; }
        public bool New { get; set; }
        public bool Hit { get; set; }
        public decimal MinProductPrice { get; set; }
        public decimal MaxProductPrice { get; set; }
        public decimal DiscountPercent { get; set; }
        public DayOfWeek? DayOfWeek { get; set; } 
        public bool InStock { get; set; }
        public string Title {get;set;}
        public string TitleDescrition {get;set;}



        public List<WishlistItem> WishlistItems { get; set; }
        public List<Image> Images { get; set; }
        public List<Article> Articles { get; set; }
        public List<PropertyValCatArt> PropertyValCatArts { get; set; }
        public List<EventProduct> EventProducts { get; set; }

        public Product()
        {
            WishlistItems = new List<WishlistItem>();
            Images = new List<Image>();
            Articles = new List<Article>();
            PropertyValCatArts = new List<PropertyValCatArt>();
            EventProducts = new List<EventProduct>();
        }
    }
    public class ProductMap
    {
        public ProductMap(EntityTypeBuilder<Product> entityBuilder)
        {
            entityBuilder.HasKey(t => t.Id);
            entityBuilder.Property(t => t.Name).IsRequired();
            entityBuilder.Property(t => t.Path).IsRequired();
            entityBuilder.Property(t => t.Description).IsRequired();
            entityBuilder.Property(t => t.New);
            entityBuilder.Property(t => t.Hit);
            entityBuilder.Property(t => t.MaxProductPrice);
            entityBuilder.Property(t => t.MinProductPrice);
            entityBuilder.Property(t => t.DiscountPercent);
            entityBuilder.Property(t => t.InStock);
            entityBuilder.Property(t => t.DayOfWeek);
            //entityBuilder.Property(t => t.CountryId);

            entityBuilder.HasOne(o => o.Category).WithMany(c => c.Products).HasForeignKey(o => o.CategoryId);
            entityBuilder.HasOne(o => o.Brand).WithMany(c => c.Products).HasForeignKey(o => o.BrandId);
            entityBuilder.HasOne(o => o.Country).WithMany(c => c.Products).HasForeignKey(o => o.CountryId);
        }
    }
}
