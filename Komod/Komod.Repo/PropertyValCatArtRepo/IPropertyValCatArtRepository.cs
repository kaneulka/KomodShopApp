using Komod.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace Komod.Repo.PropertyValCatArtRepo
{
    public interface IPropertyValCatArtRepository
    {
        PropertyValCatArt Get(PropertyValCatArt entity);
        List<PropertyValCatArt> GetAll();
        void Insert(PropertyValCatArt entity);
        void Delete(PropertyValCatArt entity);
        void DeleteSome(List<PropertyValCatArt> entities);
        void InsertSome(List<PropertyValCatArt> entities);
        void Remove(PropertyValCatArt entity);
        void SaveChanges();
    }
}
