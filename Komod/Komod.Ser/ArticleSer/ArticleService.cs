using Komod.Data;
using Komod.Repo.ArticleRepo;
using System;
using System.Collections.Generic;
using System.Text;

namespace Komod.Ser.ArticleSer
{
    public class ArticleService : IArticleService
    {
        private IArticleRepository articleRepository;

        public ArticleService(IArticleRepository articleRepository)
        {
            this.articleRepository = articleRepository;
        }

        public IEnumerable<Article> GetArticles()
        {
            return articleRepository.GetAll();
        }

        public Article GetArticle(long id)
        {
            return articleRepository.Get(id);
        }

        public void InsertArticle(Article article)
        {
            articleRepository.Insert(article);
        }
        public void UpdateArticle(Article article)
        {
            articleRepository.Update(article);
        }

        public void UpdateArticles(List<Article> articles)
        {
            articleRepository.UpdateArray(articles);
        }
        public void DeleteArticle(long id)
        {
            Article article = GetArticle(id);
            articleRepository.Remove(article);
            articleRepository.SaveChanges();
        }
    }
}
