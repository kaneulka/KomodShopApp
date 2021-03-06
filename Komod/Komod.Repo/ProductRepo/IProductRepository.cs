using Komod.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace Komod.Repo.ProductRepo
{
    public interface IProductRepository : IRepository<Product>
    {
        void UpdateArray(List<Product> entity);
    }
}
