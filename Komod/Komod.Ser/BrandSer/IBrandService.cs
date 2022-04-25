using Komod.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace Komod.Ser.BrandSer
{
    public interface IBrandService
    {
        IEnumerable<Brand> GetBrands();
        Brand GetBrand(long id);
        void InsertBrand(Brand Brand);
        void UpdateBrand(Brand Brand);
        void DeleteBrand(long id);
    }
}
