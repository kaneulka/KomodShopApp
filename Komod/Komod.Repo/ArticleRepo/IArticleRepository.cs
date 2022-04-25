using Komod.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace Komod.Repo.ArticleRepo
{
    public interface IArticleRepository : IRepository<Article>
    {
        void UpdateArray(List<Article> entity);//BdController
    }
}
