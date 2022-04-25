using Komod.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace Komod.Ser.ArticleSer
{
    public interface IArticleService
    {
        IEnumerable<Article> GetArticles();
        Article GetArticle(long id);
        void InsertArticle(Article article);
        void UpdateArticle(Article article);
        void UpdateArticles(List<Article> articles);
        void DeleteArticle(long id);
    }
}
