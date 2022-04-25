using Komod.Data;
using Komod.Repo.BrandRepo;
using System;
using System.Collections.Generic;
using System.Text;

namespace Komod.Ser.BrandSer
{
    public class BrandService : IBrandService
    {
        private IBrandRepository brandRepository;

        public BrandService(IBrandRepository brandRepository)
        {
            this.brandRepository = brandRepository;
        }

        public IEnumerable<Brand> GetBrands()
        {
            return brandRepository.GetAll();
        }

        public Brand GetBrand(long id)
        {
            return brandRepository.Get(id);
        }

        public void InsertBrand(Brand brand)
        {
            brandRepository.Insert(brand);
        }
        public void UpdateBrand(Brand brand)
        {
            brandRepository.Update(brand);
        }

        public void DeleteBrand(long id)
        {
            Brand brand = GetBrand(id);
            brandRepository.Remove(brand);
            brandRepository.SaveChanges();
        }
    }
}
