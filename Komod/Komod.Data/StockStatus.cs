using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Komod.Data
{
    public class StockStatus : BaseEntity
    {
        public List<Article> Articles { get; set; }

        public StockStatus()
        {
            Articles = new List<Article>();
        }
    }
    public class StockStatusMap
    {
        public StockStatusMap(EntityTypeBuilder<StockStatus> entityBuilder)
        {
            entityBuilder.HasKey(t => t.Id);
            entityBuilder.Property(t => t.Name).IsRequired();
        }
    }
}
