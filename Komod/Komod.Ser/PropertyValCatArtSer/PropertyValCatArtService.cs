using Komod.Data;
using Komod.Repo.PropertyRepo;
using Komod.Repo.PropertyValCatArtRepo;
using System;
using System.Collections.Generic;
using System.Text;

namespace Komod.Ser.PropertyValCatArtSer
{
    public class PropertyValCatArtService : IPropertyValCatArtService
    {
        private IPropertyValCatArtRepository propertyValCatArtRepository;

        public PropertyValCatArtService(IPropertyValCatArtRepository propertyValCatArtRepository)
        {
            this.propertyValCatArtRepository = propertyValCatArtRepository;
        }

        public PropertyValCatArt Get(PropertyValCatArt entity)
        {
            return propertyValCatArtRepository.Get(entity);
        }
        public List<PropertyValCatArt> GetAll()
        {
            return propertyValCatArtRepository.GetAll();
        }

        public void Insert(PropertyValCatArt propertyValCatArt)
        {
            propertyValCatArtRepository.Insert(propertyValCatArt);
        }

        public void Delete(PropertyValCatArt entity)
        {
            propertyValCatArtRepository.Delete(entity);
            propertyValCatArtRepository.SaveChanges();
        }
        public void DeleteSome(List<PropertyValCatArt> entities)
        {
            propertyValCatArtRepository.DeleteSome(entities);
            propertyValCatArtRepository.SaveChanges();
        }
        public void InsertSome(List<PropertyValCatArt> entities)
        {
            propertyValCatArtRepository.InsertSome(entities);
            propertyValCatArtRepository.SaveChanges();
        }
    }
}
