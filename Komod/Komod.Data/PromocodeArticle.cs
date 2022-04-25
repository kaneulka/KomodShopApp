using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Komod.Data
{
    public class PromocodeArticle
    {
        public long ArticleId { get; set; }
        public Article Article { get; set; }
        public long PromocodeId { get; set; }
        public Promocode Promocode { get; set; }
    }
    public class PromocodeArticleMap
    {
        public PromocodeArticleMap(EntityTypeBuilder<PromocodeArticle> entityBuilder)
        {
            entityBuilder.HasKey(t => new { t.ArticleId, t.PromocodeId });
            entityBuilder.Property(t => t.ArticleId).IsRequired();
            entityBuilder.Property(t => t.PromocodeId).IsRequired();

            entityBuilder.HasOne(t => t.Promocode).WithMany(t => t.PromocodeArticles).HasForeignKey(t => t.PromocodeId).OnDelete(DeleteBehavior.ClientCascade);
            entityBuilder.HasOne(t => t.Article).WithMany(t => t.PromocodeArticles).HasForeignKey(t => t.ArticleId).OnDelete(DeleteBehavior.ClientCascade);
        }
    }
}
