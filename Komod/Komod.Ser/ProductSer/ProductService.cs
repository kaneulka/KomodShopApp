using Komod.Data;
using Komod.Repo.ProductRepo;
using System;
using System.Collections.Generic;
using System.Text;

namespace Komod.Ser.ProductSer
{
    public class ProductService : IProductService
    {
        private IProductRepository productRepository;

        public ProductService(IProductRepository productRepository)
        {
            this.productRepository = productRepository;
        }

        public IEnumerable<Product> GetProducts()
        {
            return productRepository.GetAll();
        }

        public Product GetProduct(long id)
        {
            return productRepository.Get(id);
        }

        public void InsertProduct(Product product)
        {
            productRepository.Insert(product);
        }
        public void UpdateProduct(Product product)
        {
            productRepository.Update(product);
        }
        public void UpdateProducts(List<Product> products)
        {
            productRepository.UpdateArray(products);
        }

        public void DeleteProduct(long id)
        {
            Product product = GetProduct(id);
            productRepository.Remove(product);
            productRepository.SaveChanges();
        }
    }
}
