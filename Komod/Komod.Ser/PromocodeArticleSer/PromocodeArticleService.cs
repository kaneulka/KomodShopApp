using Komod.Data;
using Komod.Repo.PropertyRepo;
using Komod.Repo.PromocodeArticleRepo;
using System;
using System.Collections.Generic;
using System.Text;

namespace Komod.Ser.PromocodeArticleSer
{
    public class PromocodeArticleService : IPromocodeArticleService
    {
        private IPromocodeArticleRepository propertyValCatArtRepository;

        public PromocodeArticleService(IPromocodeArticleRepository propertyValCatArtRepository)
        {
            this.propertyValCatArtRepository = propertyValCatArtRepository;
        }

        public PromocodeArticle Get(PromocodeArticle entity)
        {
            return propertyValCatArtRepository.Get(entity);
        }
        public List<PromocodeArticle> GetAll()
        {
            return propertyValCatArtRepository.GetAll();
        }

        public void Insert(PromocodeArticle propertyValCatArt)
        {
            propertyValCatArtRepository.Insert(propertyValCatArt);
        }

        public void Delete(PromocodeArticle entity)
        {
            propertyValCatArtRepository.Delete(entity);
            propertyValCatArtRepository.SaveChanges();
        }
        public void DeleteSome(List<PromocodeArticle> entities)
        {
            propertyValCatArtRepository.DeleteSome(entities);
            propertyValCatArtRepository.SaveChanges();
        }
        public void InsertSome(List<PromocodeArticle> entities)
        {
            propertyValCatArtRepository.InsertSome(entities);
            propertyValCatArtRepository.SaveChanges();
        }
    }
}
