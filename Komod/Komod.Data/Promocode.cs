using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Komod.Data
{
    public class Promocode : BaseEntity
    {
        public DateTime EndOfPromocode { get; set; } 
        public decimal DiscountPercent { get; set; }
        public List<PromocodeArticle> PromocodeArticles { get; set; }
        public string PersonalUserPromo { get; set; }
        public int Count { get; set; }

        public Promocode()
        {
            PromocodeArticles = new List<PromocodeArticle>();
        }
    }

    public class PromocodeMap
    {
        public PromocodeMap(EntityTypeBuilder<Promocode> entityBuilder)
        {
            entityBuilder.HasKey(t => t.Id);
            entityBuilder.Property(t => t.Name).IsRequired();
        }
    }
}
