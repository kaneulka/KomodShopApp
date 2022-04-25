using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;
using System.Threading.Tasks;
using Komod.Data;
using Komod.Ser.ArticleSer;
using Komod.Ser.CategorySer;
using Komod.Ser.OrderSer;
using Komod.Ser.ProductSer;
using Komod.Ser.PromocodeArticleSer;
using Komod.Ser.PromocodeSer;
using Komod.Ser.PropertyValCatArtSer;
using Komod.Web.Models;
using Komod.Web.Models.PromocodeModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NPOI.OpenXmlFormats.Dml;
using NPOI.XSSF.UserModel;

namespace Komod.Web.Controllers
{
    [Authorize(Roles = "admin")]
    public class PromocodeController : Controller
    {
        private readonly IPromocodeService promocodeService;
        private readonly IProductService productService;
        private readonly IPropertyValCatArtService propertyValCatArtService;
        private readonly IPromocodeArticleService promocodeArticleService;
        private readonly IArticleService articleService;
        private readonly ICategoryService categoryService;
        private readonly IOrderService orderService;
        private readonly UserManager<User> _userManager;

        public PromocodeController(IPromocodeService promocodeService,
            IPromocodeArticleService promocodeArticleService,
            IProductService productService,
            IPropertyValCatArtService propertyValCatArtService,
            IArticleService articleService,
            ICategoryService categoryService,
            IOrderService orderService,
            UserManager<User> _userManager)
        {
            this.categoryService = categoryService;
            this.promocodeService = promocodeService;
            this.promocodeArticleService = promocodeArticleService;
            this.productService = productService;
            this.propertyValCatArtService = propertyValCatArtService;
            this.articleService = articleService;
            this.orderService = orderService;
            this._userManager = _userManager;
        }

        [HttpGet]
        public IActionResult Promocodes(int page = 1, int sortType = 0, string searchString = null)
        {
            IEnumerable<Promocode> promocodes;
            if (searchString == null)
            {
                promocodes = promocodeService.GetPromocodes();
            }
            else
            {
                searchString = searchString.ToUpper();
                promocodes = promocodeService.GetPromocodes().Where(s => s.Name.ToUpper().Contains(searchString)
                    || s.AddedDate.ToString().ToUpper().Contains(searchString)
                    || s.ModifiedDate.ToString().ToUpper().Contains(searchString)
                );
            }

            switch (sortType)
            {
                case 0:
                    promocodes = promocodes.OrderByDescending(b => b.AddedDate);
                    break;
                case 1:
                    promocodes = promocodes.OrderBy(b => b.AddedDate);
                    break;
                case 2:
                    promocodes = promocodes.OrderByDescending(b => b.ModifiedDate);
                    break;
                case 3:
                    promocodes = promocodes.OrderBy(b => b.ModifiedDate);
                    break;
                case 4:
                    promocodes = promocodes.OrderBy(b => b.Name);
                    break;
                case 5:
                    promocodes = promocodes.OrderByDescending(b => b.Name);
                    break;
            }
            ViewBag.SortType = sortType;

            int pageSize = 20;

            List<PromocodeViewModel> listPromocodes = new List<PromocodeViewModel>();

            var count = promocodes.Count();
            var items = promocodes.Skip((page - 1) * pageSize).Take(pageSize).ToList();
            PageViewModel pageViewModel = new PageViewModel(count, page, pageSize);
            items.ForEach(u =>
            {
                PromocodeViewModel promocode = new PromocodeViewModel
                {
                    Id = u.Id,
                    PromocodeName = u.Name,
                    AddedDate = u.AddedDate,
                    ModifiedDate = u.ModifiedDate,
                    EndOfPromocode = u.EndOfPromocode,
                    DiscountPercent = u.DiscountPercent,
                    PersonalUserPromo = u.PersonalUserPromo,
                    Count = u.Count
                };
                listPromocodes.Add(promocode);
            });

            PromocodesViewModel viewModel = new PromocodesViewModel
            {
                PageViewModel = pageViewModel,
                Promocodes = listPromocodes
            };

            return View(viewModel);
        }

        [HttpGet]
        public IActionResult AddPromocode()
        {
            PromocodeViewModel model = new PromocodeViewModel();

            return PartialView("_AddPromocode", model);
        }

        [HttpPost]
        public IActionResult AddPromocode(PromocodeViewModel model)
        {
            Promocode promocodeEntity = new Promocode
            {
                Name = model.PromocodeName,
                AddedDate = DateTime.Now,
                ModifiedDate = DateTime.Now,
                EndOfPromocode = model.EndOfPromocode,
                DiscountPercent = model.DiscountPercent,
                PersonalUserPromo = model.PersonalUserPromo,
                Count = model.Count
            };
            if (ModelState.IsValid)
            {
                promocodeService.InsertPromocode(promocodeEntity);
                if (promocodeEntity.Id > 0)
                {
                    return RedirectToAction("Promocodes");
                }
            }
            return PartialView("_AddPromocode", model);
        }

        public IActionResult EditPromocode(int? id)
        {
            PromocodeViewModel model = new PromocodeViewModel();
            if (id.HasValue && id != 0)
            {
                Promocode promocodeEntity = promocodeService.GetPromocode(id.Value);
                model.PromocodeName = promocodeEntity.Name;
                model.EndOfPromocode = promocodeEntity.EndOfPromocode;
                model.DiscountPercent = promocodeEntity.DiscountPercent;
                model.PersonalUserPromo = promocodeEntity.PersonalUserPromo;
                model.Count = promocodeEntity.Count;
            }
            return PartialView("_EditPromocode", model);
        }

        [HttpPost]
        public IActionResult EditPromocode(PromocodeViewModel model)
        {
            Promocode promocodeEntity = promocodeService.GetPromocode(model.Id);
            promocodeEntity.Name = model.PromocodeName;
            promocodeEntity.ModifiedDate = DateTime.Now;
            promocodeEntity.EndOfPromocode = model.EndOfPromocode;
            promocodeEntity.DiscountPercent = model.DiscountPercent;
            promocodeEntity.PersonalUserPromo = model.PersonalUserPromo;
            promocodeEntity.Count = model.Count;
            if (ModelState.IsValid)
            {
                promocodeService.UpdatePromocode(promocodeEntity);
                if (promocodeEntity.Id > 0)
                {
                    return RedirectToAction("Promocodes");
                }
            }
            return PartialView("_EditPromocode", model);
        }

        [HttpGet]
        public PartialViewResult DeletePromocode(long? id)
        {
            PromocodeViewModel model = new PromocodeViewModel();
            if (id.HasValue && id != 0)
            {
                Promocode promocodeEntity = promocodeService.GetPromocode(id.Value);
                model.PromocodeName = promocodeEntity.Name;
            }
            return PartialView("_DeletePromocode", model);
        }

        [HttpPost]
        public IActionResult DeletePromocode(long id)
        {
            promocodeService.DeletePromocode(id);
            return RedirectToAction("Promocodes");
        }

        //Получить промо при добавлении у артикула/продукта/категории
        [HttpGet]
        public JsonResult GetPromo()
        {
            List<Promocode> lp = promocodeService.GetPromocodes().ToList();
            List<PromocodeViewModel> lpvm = new List<PromocodeViewModel>();
            foreach (var p in lp)
            {
                PromocodeViewModel pvm = new PromocodeViewModel
                {
                    Id = p.Id,
                    PromocodeName = p.Name,
                    EndOfPromocode = p.EndOfPromocode,
                    DiscountPercent = p.DiscountPercent
                };
                lpvm.Add(pvm);
            }
            return Json(lpvm);
        }

        //Изменить промо у артикула
        [HttpGet]
        public IActionResult ChangePromoToArticle(long articleId)
        {
            List<PromocodeArticle> lpa = promocodeArticleService.GetAll().ToList();
            Article article = articleService.GetArticle(articleId);
            List<Promocode> promocodes = promocodeService.GetPromocodes().ToList();
            EditPromoArticleModel model = new EditPromoArticleModel() { Article = article, PromocodeArticles = lpa, Promocodes = promocodes };
            return PartialView("_ChangePromoToArticle", model);
        }

        [HttpPost]
        public IActionResult ChangePromoToArticle(List<long> promocodeIds, long articleId)
        {
            List<PromocodeArticle> lpa = promocodeArticleService.GetAll().Where(pa=> pa.ArticleId == articleId).ToList();

            List<long> newToAdd = promocodeIds.Where(i => !lpa.Select(pa => pa.PromocodeId).Contains(i)).ToList();
            List<long> olderToRemove = lpa.Select(pa => pa.PromocodeId).Where(pa => !promocodeIds.Contains(pa)).ToList();

            List<PromocodeArticle> toAdd = new List<PromocodeArticle>();
            foreach (var i in newToAdd)
            {
                PromocodeArticle pa = new PromocodeArticle { PromocodeId = i, ArticleId = articleId };
                toAdd.Add(pa);
            }
            promocodeArticleService.InsertSome(toAdd);

            List<PromocodeArticle> toRemove = new List<PromocodeArticle>();
            foreach (var i in olderToRemove)
            {
                PromocodeArticle pa = new PromocodeArticle { PromocodeId = i, ArticleId = articleId };
                toRemove.Add(pa);
            }
            promocodeArticleService.DeleteSome(toRemove);

            return RedirectToAction("Articles", "Article");
        }

        //Изменить промо у продукта
        [HttpGet]
        public IActionResult ChangePromoToProduct(long productId)
        {
            List<PromocodeArticle> lpa = promocodeArticleService.GetAll().ToList();
            Article article = articleService.GetArticle(productId);
            List<Promocode> promocodes = promocodeService.GetPromocodes().ToList();
            EditPromoArticleModel model = new EditPromoArticleModel() { Article = article, PromocodeArticles = lpa, Promocodes = promocodes };
            return PartialView("_ChangePromoToProduct", model);
        }

        [HttpPost]
        public IActionResult ChangePromoToProduct(List<long> promocodeIds, long articleId)
        {
            List<PromocodeArticle> lpa = promocodeArticleService.GetAll().Where(pa => pa.ArticleId == articleId).ToList();

            List<long> newToAdd = promocodeIds.Where(i => !lpa.Select(pa => pa.PromocodeId).Contains(i)).ToList();
            List<long> olderToRemove = lpa.Select(pa => pa.PromocodeId).Where(pa => !promocodeIds.Contains(pa)).ToList();

            List<PromocodeArticle> toAdd = new List<PromocodeArticle>();
            foreach (var i in newToAdd)
            {
                PromocodeArticle pa = new PromocodeArticle { PromocodeId = i, ArticleId = articleId };
                toAdd.Add(pa);
            }
            promocodeArticleService.InsertSome(toAdd);

            List<PromocodeArticle> toRemove = new List<PromocodeArticle>();
            foreach (var i in olderToRemove)
            {
                PromocodeArticle pa = new PromocodeArticle { PromocodeId = i, ArticleId = articleId };
                toRemove.Add(pa);
            }
            promocodeArticleService.DeleteSome(toRemove);

            return RedirectToAction("Articles", "Article");
        }

        //Изменить промо у категории
        [HttpGet]
        public IActionResult ChangePromoToCategory(long articleId)
        {
            List<PromocodeArticle> lpa = promocodeArticleService.GetAll().ToList();
            Article article = articleService.GetArticle(articleId);
            List<Promocode> promocodes = promocodeService.GetPromocodes().ToList();
            EditPromoArticleModel model = new EditPromoArticleModel() { Article = article, PromocodeArticles = lpa, Promocodes = promocodes };
            return PartialView("_ChangePromoToCategory", model);
        }

        [HttpPost]
        public IActionResult ChangePromoToCategory(List<long> promocodeIds, long articleId)
        {
            List<PromocodeArticle> lpa = promocodeArticleService.GetAll().Where(pa => pa.ArticleId == articleId).ToList();

            List<long> newToAdd = promocodeIds.Where(i => !lpa.Select(pa => pa.PromocodeId).Contains(i)).ToList();
            List<long> olderToRemove = lpa.Select(pa => pa.PromocodeId).Where(pa => !promocodeIds.Contains(pa)).ToList();

            List<PromocodeArticle> toAdd = new List<PromocodeArticle>();
            foreach (var i in newToAdd)
            {
                PromocodeArticle pa = new PromocodeArticle { PromocodeId = i, ArticleId = articleId };
                toAdd.Add(pa);
            }
            promocodeArticleService.InsertSome(toAdd);

            List<PromocodeArticle> toRemove = new List<PromocodeArticle>();
            foreach (var i in olderToRemove)
            {
                PromocodeArticle pa = new PromocodeArticle { PromocodeId = i, ArticleId = articleId };
                toRemove.Add(pa);
            }
            promocodeArticleService.DeleteSome(toRemove);

            return RedirectToAction("Articles", "Article");
        }

        //Удалить промокоды у товаров
        //[HttpGet]
        //public IActionResult DeletePromoFromSome(long id, string url, string entityName)
        //{
        //    PromocodeToDelete model = new PromocodeToDelete { Id = id , EntityName = entityName, URL = url}; 
        //    if (model.EntityName == "Category")
        //    {
        //        List<long> articleIds = propertyValCatArtService.GetAll().Where(pvca => pvca.CategoryId == model.Id).Select(pvca => pvca.ArticleId).ToList();
        //        List<PromocodeArticle> lpa = promocodeArticleService.GetAll().Where(pa => articleIds.Contains(pa.ArticleId)).ToList();
        //        model.Promocodes = promocodeService.GetPromocodes().Where(p => lpa.Select(pa => pa.PromocodeId).Contains(p.Id)).ToList();
        //    }
        //    if (model.EntityName == "Product")
        //    {
        //        List<long> articleIds = propertyValCatArtService.GetAll().Where(pvca => pvca.CategoryId == model.Id).Select(pvca => pvca.ArticleId).ToList();
        //        List<PromocodeArticle> lpa = promocodeArticleService.GetAll().Where(pa => articleIds.Contains(pa.ArticleId)).ToList();
        //        model.Promocodes = promocodeService.GetPromocodes().Where(p => lpa.Select(pa => pa.PromocodeId).Contains(p.Id)).ToList();
        //    }
        //    if (model.EntityName == "Article")
        //    {
        //        List<PromocodeArticle> lpa = promocodeArticleService.GetAll().Where(pa => pa.ArticleId == id).ToList();
        //        model.Promocodes = promocodeService.GetPromocodes().Where(p => lpa.Select(pa => pa.PromocodeId).Contains(p.Id)).ToList();
        //    }
        //    return PartialView("_DeletePromoFromSome", model);
        //}
        //
        //[HttpPost]
        //public IActionResult DeletePromoFromSome(PromocodeToDelete model)
        //{
        //    List<PromocodeArticle> lpa = new List<PromocodeArticle>();// promocodeArticleService.GetAll().Where
        //    if (model.EntityName == "Category")
        //    {
        //        List<long> articleIds = propertyValCatArtService.GetAll().Where(pvca => pvca.CategoryId == model.Id).Select(pvca => pvca.ArticleId).ToList();
        //        lpa.AddRange(promocodeArticleService.GetAll().Where(pa => pa.PromocodeId == model.Promocode.Id && articleIds.Contains(pa.ArticleId)));
        //    }
        //    if (model.EntityName == "Product")
        //    {
        //        List<long> articleIds = propertyValCatArtService.GetAll().Where(pvca => pvca.ProductId == model.Id).Select(pvca => pvca.ArticleId).ToList();
        //        lpa.AddRange(promocodeArticleService.GetAll().Where(pa => pa.PromocodeId == model.Promocode.Id && articleIds.Contains(pa.ArticleId)));
        //    }
        //    if (model.EntityName == "Article")
        //    {
        //        lpa.Add(promocodeArticleService.Get(new PromocodeArticle { ArticleId = model.Id, PromocodeId = model.Promocode.Id }));
        //    }
        //    promocodeArticleService.DeleteSome(lpa);
        //
        //    return Redirect(model.URL);
        //}


        //Добавить артикулы к промо?
        [HttpGet]
        public IActionResult AddArticleToProduct(long id, string url)
        {
            Promocode promocode = promocodeService.GetPromocode(id);
            PromocodeArticleModel model = new PromocodeArticleModel()
            {
                PromocodeId = promocode.Id,
                PromocodeName = promocode.Name,
                Categories = categoryService.GetCategories().ToList(),
                Products = productService.GetProducts().ToList(),
                Articles = articleService.GetArticles().ToList(),
                ArticleIds = promocodeArticleService.GetAll().Where(pa=> pa.PromocodeId == id).Select(pa=> pa.ArticleId).ToList()
            };
            ViewBag.ReturnUrl = url;
            return View(model);
        }

        [HttpPost]
        public IActionResult AddArticleToProduct(PromocodeArticleModel model, List<long> articleIds)
        {
            List<PromocodeArticle> lpa = promocodeArticleService.GetAll().Where(pa => pa.PromocodeId == model.PromocodeId).ToList();
            List<long> newToAdd = articleIds.Where(i => !lpa.Select(pa => pa.ArticleId).Contains(i)).ToList();
            List<long> olderToRemove = lpa.Select(pa => pa.ArticleId).Where(pa => !articleIds.Contains(pa)).ToList();

            List<PromocodeArticle> toAdd = new List<PromocodeArticle>();
            foreach(var i in newToAdd)
            {
                PromocodeArticle pa = new PromocodeArticle { ArticleId = i, PromocodeId = model.PromocodeId };
                toAdd.Add(pa);
            }
            promocodeArticleService.InsertSome(toAdd);

            List<PromocodeArticle> toRemove = new List<PromocodeArticle>();
            foreach (var i in olderToRemove)
            {
                PromocodeArticle pa = new PromocodeArticle { ArticleId = i, PromocodeId = model.PromocodeId };
                toRemove.Add(pa);
            }
            promocodeArticleService.DeleteSome(toRemove);

            return RedirectToAction("Promocodes");
        }

        [HttpGet]
        public IActionResult AddPromocodeToProduct(long id, string url)
        {
            PromocodeArticleModel model = new PromocodeArticleModel()
            {
                PromocodeId = id,
                Categories = categoryService.GetCategories().ToList(),
                //Products = productService.GetProducts().ToList(),
                //Articles = articleService.GetArticles().ToList(),
                //ArticleIds = promocodeArticleService.GetAll().Where(pa => pa.PromocodeId == id).Select(pa => pa.ArticleId).ToList()
            };
            return PartialView("_AddPromocodeToProduct", model);
        }

        [HttpPost]
        public IActionResult AddPromocodeToProduct(PromocodeArticleModel model, List<long> articleIds, List<long> productIds, List<long> categoryIds)
        {
            List<PromocodeArticle> lpa = promocodeArticleService.GetAll().Where(pa => pa.PromocodeId == model.PromocodeId).ToList();
            //Получить ID продуктов из категории
            List<long> productIdsFromCategoryList = productService.GetProducts().Where(p => categoryIds.Contains(p.CategoryId)).Select(p => p.Id).ToList();
            productIds.AddRange(productIdsFromCategoryList);
            //Получить Id артикулов из продуктов
            articleIds.AddRange(articleService.GetArticles().Where(a => productIds.Contains(a.ProductId)).Select(a=> a.Id));

            List<long> newToAdd = articleIds.Where(i => !lpa.Select(pa => pa.ArticleId).Contains(i)).ToList();
            List<long> olderToRemove = lpa.Select(pa => pa.ArticleId).Where(pa => !articleIds.Contains(pa)).ToList();

            List<PromocodeArticle> toAdd = new List<PromocodeArticle>();
            foreach (var i in newToAdd)
            {
                PromocodeArticle pa = new PromocodeArticle { ArticleId = i, PromocodeId = model.PromocodeId };
                toAdd.Add(pa);
            }
            promocodeArticleService.InsertSome(toAdd);

            List<PromocodeArticle> toRemove = new List<PromocodeArticle>();
            foreach (var i in olderToRemove)
            {
                PromocodeArticle pa = new PromocodeArticle { ArticleId = i, PromocodeId = model.PromocodeId };
                toRemove.Add(pa);
            }
            promocodeArticleService.DeleteSome(toRemove);

            return RedirectToAction("Promocodes");
        }

        //Получение продуктов по категории
        [HttpGet]
        public JsonResult GetProductValues(long categoryId)
        {
            List<ProductValue> product = productService.GetProducts().Where(p => p.CategoryId == categoryId).Select(p => new ProductValue { Id = p.Id, ProductName = p.Name }).OrderBy(p => p.ProductName).ToList();
            return Json(product, new JsonSerializerOptions() { Encoder = JavaScriptEncoder.Create(UnicodeRanges.BasicLatin, UnicodeRanges.Cyrillic) });
        }

        //Получение фартикулов по продукту
        [HttpGet]
        public JsonResult GetArticleValues(long productId)
        {
            List<ArticleValue> articles = articleService.GetArticles().Where(p => p.ProductId == productId).Select(p => new ArticleValue { Id = p.Id, ArticleName = p.Name }).OrderBy(p => p.ArticleName).ToList();
            return Json(articles, new JsonSerializerOptions() { Encoder = JavaScriptEncoder.Create(UnicodeRanges.BasicLatin, UnicodeRanges.Cyrillic) });
        }

        [HttpGet]
        public JsonResult GetProductList(long[] categoryIds)
        {
            List<Category> categories = categoryService.GetCategories().ToList();//.Where(c => categoryIds.Contains(c.Id) || categoryIds.Contains(c.ParentId)).ToList();
            List<long> categoryIdsNew = categoryIds.ToList();
            foreach (var c in categories)
            {
                if (c.ParentId != null)
                    if (categoryIds.Contains((long)c.ParentId))
                        categoryIdsNew.Add(c.Id);
            }

            List<ProductValue> products = productService.GetProducts().Where(p => categoryIdsNew.Contains(p.CategoryId)).Select(p => new ProductValue { Id = p.Id, ProductName = p.Name, CategoryName = p.Category.Name }).OrderBy(p => p.ProductName).ToList();
            return Json(products, new JsonSerializerOptions() { Encoder = JavaScriptEncoder.Create(UnicodeRanges.BasicLatin, UnicodeRanges.Cyrillic) });
        }

        [HttpGet]
        public JsonResult GetArticleList(long[] productIds)
        {
            List<ArticleValue> articles = articleService.GetArticles().Where(p => productIds.Contains(p.ProductId)).Select(p => new ArticleValue { Id = p.Id, ArticleName = p.Name }).OrderBy(p => p.ArticleName).ToList();
            return Json(articles, new JsonSerializerOptions() { Encoder = JavaScriptEncoder.Create(UnicodeRanges.BasicLatin, UnicodeRanges.Cyrillic) });
        }



        //Получение Id артикулов привязанных к промокоду
        //[HttpGet]
        //public async Task<JsonResult> GetPromosArticles(string promocodeName)
        //{
        //    Promocode promocode = promocodeService.GetPromocodes().SingleOrDefault(p => p.Name == promocodeName);//.ToList();
        //    if (promocode == null)
        //    {
        //        return Json("Промокод не существует");
        //    }
        //    else 
        //    {
        //        if (promocode.EndOfPromocode < DateTime.Now) return Json("Срок действия промокода истек");
//
        //        if (User.Identity.IsAuthenticated)
        //        {
        //            ClaimsPrincipal currentUser = this.User;
        //            User user = await _userManager.GetUserAsync(currentUser);
        //            List<Order> orders = orderService.GetOrders().Where(o=> o.UserName == user.UserName).ToList();
        //            if (orders.Select(o=>o.PromoName).Contains(promocodeName)) return Json("Промокод уже был вами использован");
        //        }
        //        else
        //        {
        //            return Json("Пользователь не авторизован");
        //        }
        //    }
        //    List<long> articleIds = promocodeArticleService.GetAll().Where(pa => pa.PromocodeId == promocode.Id).Select(pa=> pa.ArticleId).ToList();
        //    List<Article> articles = articleService.GetArticles().Where(a => articleIds.Contains(a.Id)).ToList();
//
        //    PromocodeViewModel pvm = new PromocodeViewModel()
        //    {
        //        Id = promocode.Id,
        //        PromocodeName = promocode.Name,
        //        DiscountPercent = promocode.DiscountPercent,
        //        PersonalUserPromo = promocode.PersonalUserPromo
        //    };
        //    ReturnPromoJson entity = new ReturnPromoJson()
        //    {
        //        ArticleNames = articles.Select(a => a.Name).ToList(),
        //        Promocode = pvm
        //    };
//
        //    return Json(entity);
        //}
    }
}