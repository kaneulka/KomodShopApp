using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Komod.Data
{
    public class ProductSet
    {
        public long Id { get; set; }
        public string SetName { get; set; }
        public string ProductSetName { get; set; }
        public decimal DiscounPercent { get; set; }
        public bool ActiveSet { get; set; }
    }
    public class ProductSetMap
    {
        public ProductSetMap(EntityTypeBuilder<ProductSet> entityBuilder)
        {
            entityBuilder.HasKey(t => t.Id);
            entityBuilder.Property(t => t.ProductSetName).IsRequired();
            entityBuilder.Property(t => t.DiscounPercent).IsRequired();
        }
    }
}
