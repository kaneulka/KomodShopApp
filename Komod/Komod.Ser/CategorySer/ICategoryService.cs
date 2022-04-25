using Komod.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace Komod.Ser.CategorySer
{
    public interface ICategoryService
    {
        IEnumerable<Category> GetCategories();
        Category GetCategory(long id);
        void InsertCategory(Category category);
        void UpdateCategory(Category category);
        void DeleteCategory(long id);
    }
}
