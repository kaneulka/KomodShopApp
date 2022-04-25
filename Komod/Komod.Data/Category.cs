using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Komod.Data
{
    public class Category : BaseEntity
    {
        public long? ParentId { get; set; }
        public Category ParentCategory { get; set; }
        public List<Product> Products { get; set; }
        public List<PropertyValCatArt> PropertyValCatArts { get; set; }
        public bool MainPage {get;set;}
        public string Title {get;set;}
        public string TitleDescrition {get;set;}

        public Category()
        {
            Products = new List<Product>();
            PropertyValCatArts = new List<PropertyValCatArt>();
        }
    }

    public class CategoryMap
    {
        public CategoryMap(EntityTypeBuilder<Category> entityBuilder)
        {
            entityBuilder.HasKey(t => t.Id);
            entityBuilder.Property(t => t.Name).IsRequired();
        }
    }
}
