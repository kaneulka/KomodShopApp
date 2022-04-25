using Komod.Data;
using Komod.Repo.CategoryRepo;
using System;
using System.Collections.Generic;
using System.Text;

namespace Komod.Ser.CategorySer
{
    public class CategoryService : ICategoryService
    {
        private ICategoryRepository categoryRepository;

        public CategoryService(ICategoryRepository categoryRepository)
        {
            this.categoryRepository = categoryRepository;
        }

        public IEnumerable<Category> GetCategories()
        {
            return categoryRepository.GetAll();
        }

        public Category GetCategory(long id)
        {
            return categoryRepository.Get(id);
        }

        public void InsertCategory(Category category)
        {
            categoryRepository.Insert(category);
        }
        public void UpdateCategory(Category category)
        {
            categoryRepository.Update(category);
        }

        public void DeleteCategory(long id)
        {
            Category category = GetCategory(id);
            categoryRepository.Remove(category);
            categoryRepository.SaveChanges();
        }
    }
}
