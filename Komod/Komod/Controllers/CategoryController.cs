using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Komod.Data;
using Komod.Ser.ArticleSer;
using Komod.Ser.CategorySer;
using Komod.Ser.ProductSer;
using Komod.Ser.PropertyValCatArtSer;
using Komod.Web.Models;
using Komod.Web.Models.CategoryModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Komod.Web.Controllers
{
    [Authorize(Roles = "admin")]
    public class CategoryController : Controller
    {
        private readonly ICategoryService categoryService;
        private readonly IArticleService articleService;
        private readonly IProductService productService;
        private readonly IPropertyValCatArtService propertyValCatArtService;

        public CategoryController(ICategoryService categoryService, IArticleService articleService, IProductService productService, IPropertyValCatArtService propertyValCatArtService)
        {
            this.categoryService = categoryService;
            this.articleService = articleService;
            this.productService = productService;
            this.propertyValCatArtService = propertyValCatArtService;
        }

        [HttpGet]
        public IActionResult Categories(int page = 1, int sortType = 0, string searchString = null)
        {
            IEnumerable<Category> categories;
            if (searchString == null)
            {
                categories = categoryService.GetCategories();
            }
            else
            {
                searchString = searchString.ToUpper();
                categories = categoryService.GetCategories().Where(s => s.Name.ToUpper().Contains(searchString)
                    || s.AddedDate.ToString().ToUpper().Contains(searchString)
                    || s.ModifiedDate.ToString().ToUpper().Contains(searchString)
                );
            }

            switch (sortType)
            {
                case 0:
                    categories = categories.OrderByDescending(b => b.AddedDate);
                    break;
                case 1:
                    categories = categories.OrderBy(b => b.AddedDate);
                    break;
                case 2:
                    categories = categories.OrderByDescending(b => b.ModifiedDate);
                    break;
                case 3:
                    categories = categories.OrderBy(b => b.ModifiedDate);
                    break;
                case 4:
                    categories = categories.OrderBy(b => b.Name);
                    break;
                case 5:
                    categories = categories.OrderByDescending(b => b.Name);
                    break;
            }
            ViewBag.SortType = sortType;

            int pageSize = 20;

            List<CategoryViewModel> listCategories = new List<CategoryViewModel>();

            var count = categories.Count();
            var items = categories.Skip((page - 1) * pageSize).Take(pageSize).ToList();
            PageViewModel pageViewModel = new PageViewModel(count, page, pageSize);
            items.ForEach(u =>
            {
                CategoryViewModel category = new CategoryViewModel
                {
                    Id = u.Id,
                    Name = u.Name,
                    ParentId = u.ParentId,
                    AddedDate = u.AddedDate,
                    ModifiedDate = u.ModifiedDate,
                    MainPage = u.MainPage
                };
                if (category.ParentId != null) category.ParentCategory = categoryService.GetCategory((long)category.ParentId);
                listCategories.Add(category);
            });

            CategoriesViewModel viewModel = new CategoriesViewModel
            {
                PageViewModel = pageViewModel,
                Categories = listCategories
            };

            return View(viewModel);
        }

        [HttpGet]
        public ActionResult AddCategory()
        {
            CategoryViewModel model = new CategoryViewModel();
            var parentCategories = categoryService.GetCategories().OrderBy(c => c.Name).Select(x => new { Id = x.Id, Value = x.Name });
            model.ParentCategories = new SelectList(parentCategories, "Id", "Value");

            return PartialView("_AddCategory", model);
        }

        [HttpPost]
        public async Task<ActionResult> AddCategory(CategoryViewModel model)
        {
            Category categoryEntity = new Category
            {
                Name = model.Name,
                ParentId = model.ParentId,
                AddedDate = DateTime.Now,
                ModifiedDate = DateTime.Now,
                MainPage = false,
                Title = model.Title,
                TitleDescrition = model.TitleDescription
            };

            if (ModelState.IsValid)
            {
                categoryService.InsertCategory(categoryEntity);
                if (categoryEntity.Id > 0)
                {
                    return RedirectToAction("Categories");
                }
            }
            return PartialView("_AddCategory", model);
        }

        public ActionResult EditCategory(long? id)
        {
            IEnumerable<Category> categories = categoryService.GetCategories();//Получение списка категорий
            List<long> includeIds = new List<long>() { (long)id };//Id детей
            //includeIds.Add((long)id);//Добавление первого родителя

            GetIds((long)id, includeIds, categories);//Вызов на проверку детей

            var parentCategories = categoryService.GetCategories().Where(c => !includeIds.Contains(c.Id)).OrderBy(c => c.Name).Select(x => new { Id = x.Id, Value = x.Name });
            CategoryViewModel model = new CategoryViewModel();
            if (id.HasValue && id != 0)
            {
                Category categoryEntity = categoryService.GetCategory(id.Value);
                model.Name = categoryEntity.Name;
                model.ParentId = categoryEntity.ParentId;
                model.ParentCategories = new SelectList(parentCategories, "Id", "Value", categoryEntity.ParentId);
                model.TitleDescription = categoryEntity.TitleDescrition;
                model.Title = categoryEntity.Title;
            }
            return PartialView("_EditCategory", model);
        }

        private void GetIds(long parentId, List<long> includeIds, IEnumerable<Category> categories)
        {
            foreach (var category in categories)
            {
                if (parentId == category.ParentId)//Если категория кому то отец, то
                {
                    includeIds.Add((long)category.Id);//Добавляем его к списку, чтобы он не мог быть родителем у отца
                    GetIds((long)category.Id, includeIds, categories);//Проверяем ребенка на его детей
                }
            }
        }

        [HttpPost]
        public async Task<ActionResult> EditCategory(CategoryViewModel model)
        {
            Category categoryEntity = categoryService.GetCategory(model.Id);

            categoryEntity.Name = model.Name;
            categoryEntity.ModifiedDate = DateTime.Now;
            categoryEntity.TitleDescrition = model.TitleDescription;
            categoryEntity.Title = model.Title;


            if (ModelState.IsValid)
            {
                if (model.ParentId == 0)
                {
                    categoryEntity.ParentId = null;
                }
                else
                {
                    categoryEntity.ParentId = model.ParentId;
                }


                if (categoryEntity.Id > 0)
                {
                    categoryService.UpdateCategory(categoryEntity);
                    return RedirectToAction("Categories");
                }
            }
            return PartialView("_EditCategory", model);
        }



        [HttpGet]
        public PartialViewResult DeleteCategory(long? id)
        {
            CategoryViewModel model = new CategoryViewModel();
            if (id.HasValue && id != 0)
            {
                Category categoryEntity = categoryService.GetCategory(id.Value);
                model.Name = categoryEntity.Name;
            }
            return PartialView("_DeleteCategory", model);
        }

        [HttpPost]
        public ActionResult DeleteCategory(long id)
        {
            var childCategories = categoryService.GetCategories().Where(c => c.ParentId == id).ToList();
            if (childCategories.Count != 0)
            {
                foreach (var childCategory in childCategories)
                {
                    DeleteChildCategory(childCategory.Id);
                }
            }
            List<PropertyValCatArt> lpvca = propertyValCatArtService.GetAll().Where(p=> p.CategoryId == id).ToList();
            propertyValCatArtService.DeleteSome(lpvca);
            //lpvca.ForEach(p=> propertyValCatArtService.Delete(p));
            List<Article> articles = articleService.GetArticles().Where(a=> lpvca.Select(pvca=> pvca.ArticleId).Contains(a.Id)).ToList();
            articles.ForEach(a=> articleService.DeleteArticle(a.Id));
            List<Product> products = productService.GetProducts().Where(p=> lpvca.Select(pvca=> pvca.ArticleId).Contains(p.Id)).ToList();
            products.ForEach(p=> productService.DeleteProduct(p.Id));
            categoryService.DeleteCategory(id);
            return RedirectToAction("Categories");
        }

        private void DeleteChildCategory(long childId)
        {
            var childCategories = categoryService.GetCategories().Where(c => c.ParentId == childId).ToList();
            if (childCategories.Count != 0)
            {
                foreach (var childCategory in childCategories)
                {
                    DeleteChildCategory(childCategory.Id);
                }
            }
            List<PropertyValCatArt> lpvca = propertyValCatArtService.GetAll().Where(p=> p.CategoryId == childId).ToList();
            lpvca.ForEach(p=> propertyValCatArtService.Delete(p));
            List<Article> articles = articleService.GetArticles().Where(a=> lpvca.Select(pvca=> pvca.ArticleId).Contains(a.Id)).ToList();
            articles.ForEach(a=> articleService.DeleteArticle(a.Id));
            List<Product> products = productService.GetProducts().Where(p=> lpvca.Select(pvca=> pvca.ArticleId).Contains(p.Id)).ToList();
            products.ForEach(p=> productService.DeleteProduct(p.Id));
            categoryService.DeleteCategory(childId);
        }

        public ActionResult SwitchCategoryMainPage(long id)
        {
            Category category = categoryService.GetCategory(id);
            category.MainPage = !category.MainPage;
            category.ModifiedDate = DateTime.Now;
            categoryService.UpdateCategory(category);
            return RedirectToAction("Categories");
        }
    }
}