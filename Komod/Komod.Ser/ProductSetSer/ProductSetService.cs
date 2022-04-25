using Komod.Data;
using Komod.Repo.ProductSetRepo;
using System;
using System.Collections.Generic;
using System.Text;

namespace Komod.Ser.ProductSetSer
{
    public class ProductSetService : IProductSetService
    {
        private IProductSetRepository productSetRepository;

        public ProductSetService(IProductSetRepository productSetRepository)
        {
            this.productSetRepository = productSetRepository;
        }

        public IEnumerable<ProductSet> GetProductSets()
        {
            return productSetRepository.GetAll();
        }

        public ProductSet GetProductSet(long id)
        {
            return productSetRepository.Get(id);
        }

        public void InsertProductSet(ProductSet productSet)
        {
            productSetRepository.Insert(productSet);
        }
        public void UpdateProductSet(ProductSet productSet)
        {
            productSetRepository.Update(productSet);
        }

        public void DeleteProductSet(long id)
        {
            ProductSet productSet = GetProductSet(id);
            productSetRepository.Remove(productSet);
            productSetRepository.SaveChanges();
        }
    }
}
