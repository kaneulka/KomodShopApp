using Komod.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace Komod.Ser.PropertyValCatArtSer
{
    public interface IPropertyValCatArtService
    {
        PropertyValCatArt Get(PropertyValCatArt entity);
        List<PropertyValCatArt> GetAll();
        void Insert(PropertyValCatArt entity);
        void Delete(PropertyValCatArt entity);
        void DeleteSome(List<PropertyValCatArt> entities);
        void InsertSome(List<PropertyValCatArt> entities);
    }
}
