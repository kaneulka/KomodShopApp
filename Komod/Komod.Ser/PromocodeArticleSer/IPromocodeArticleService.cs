using Komod.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace Komod.Ser.PromocodeArticleSer
{
    public interface IPromocodeArticleService
    {
        PromocodeArticle Get(PromocodeArticle entity);
        List<PromocodeArticle> GetAll();
        void Insert(PromocodeArticle entity);
        void Delete(PromocodeArticle entity);
        void DeleteSome(List<PromocodeArticle> entities);
        void InsertSome(List<PromocodeArticle> entities);
    }
}
