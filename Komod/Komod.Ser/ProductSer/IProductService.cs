using Komod.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace Komod.Ser.ProductSer
{
    public interface IProductService
    {
        IEnumerable<Product> GetProducts();
        Product GetProduct(long id);
        void InsertProduct(Product entity);
        void UpdateProduct(Product entity);
        void UpdateProducts(List<Product> entitys);
        void DeleteProduct(long id);
    }
}
