using Komod.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace Komod.Ser.ProductSetSer
{
    public interface IProductSetService
    {
        IEnumerable<ProductSet> GetProductSets();
        ProductSet GetProductSet(long id);
        void InsertProductSet(ProductSet ProductSet);
        void UpdateProductSet(ProductSet ProductSet);
        void DeleteProductSet(long id);
    }
}
