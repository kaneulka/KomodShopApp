using Komod.Data;
using System;
using System.Collections.Generic;

namespace Komod.Repo.ProductSetRepo
{
    public interface IProductSetRepository
    {
        IEnumerable<ProductSet> GetAll();
        ProductSet Get(long id);
        void Insert(ProductSet entity);
        void Update(ProductSet entity);
        void Delete(ProductSet entity);
        void Remove(ProductSet entity);
        void SaveChanges();
    }
}
