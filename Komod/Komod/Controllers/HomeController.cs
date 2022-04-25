using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Komod.Models;
using Komod.Ser.ProductSer;
using Komod.Ser.ArticleSer;
using Komod.Ser.CategorySer;
using Komod.Ser.PropertySer;
using Komod.Ser.PropertyValueSer;
using Komod.Ser.ImageSer;
using Komod.Ser.StockStatusSer;
using Komod.Ser.BrandSer;
using Komod.Ser.EventPromotionSer;
using Komod.Ser.PropertyValCatArtSer;
using Komod.Web.Models.ProductModels;
using Komod.Data;
using Komod.Ser.EventProductSer;
using Komod.Web.Models.CategoryModels;
using System.Net;
using Komod.Web.Models.EventPromotionModels;
using Komod.Web.Models.CatalogModels;
using Komod.Web.Models.PropertyModels;
using System.Text.Json;
using System.Text.Encodings.Web;
using System.Text.Unicode;
using Komod.Web.Models.BrandModels;
using Komod.Ser.ColorSer;
using Komod.Web.Models.CountryModels;
using Komod.Ser.CountrySer;

namespace Komod.Controllers
{
    public class HomeController : Controller
    {
        private readonly IProductService productService;
        private readonly IArticleService articleService;
        private readonly IPropertyValCatArtService propertyValCatArtService;
        private readonly ICategoryService categoryService;
        private readonly IColorService colorService;
        private readonly IPropertyService propertyService;
        private readonly IPropertyValueService propertyValueService;
        private readonly IImageService imageService;
        private readonly IStockStatusService stockStatusService;
        private readonly IBrandService brandService;
        private readonly IEventPromotionService eventPromotionService;
        private readonly IEventProductService eventProductService;
        private readonly ICountryService countryService;

        public HomeController(IProductService productService,
            IArticleService articleService,
            ICategoryService categoryService,
            IColorService colorService,
            IPropertyValCatArtService propertyValCatArtService,
            IPropertyService propertyService,
            IPropertyValueService propertyValueService,
            IImageService imageService,
            IStockStatusService stockStatusService,
            IBrandService brandService,
            IEventPromotionService eventPromotionService,
            IEventProductService eventProductService,
            ICountryService countryService)
        {
            this.productService = productService;
            this.articleService = articleService;
            this.propertyValCatArtService = propertyValCatArtService;
            this.categoryService = categoryService;
            this.colorService = colorService;
            this.propertyService = propertyService;
            this.propertyValueService = propertyValueService;
            this.imageService = imageService;
            this.stockStatusService = stockStatusService;
            this.brandService = brandService;
            this.eventPromotionService = eventPromotionService;
            this.eventProductService = eventProductService;
            this.countryService = countryService;
        }

        public IActionResult Index()
        {
            //var hitProducts = productService.GetProducts().Where(p => p.Hit == true && p.Category.Name != "No Category" && p.Brand.Name != "No Brand" && p.Images.Count > 0).OrderBy(p => p.ModifiedDate).ToList();
            //List<ProductViewModel> hitList = new List<ProductViewModel>();
            //hitProducts.ForEach(u =>
            //{
            //    ProductViewModel hitProduct = new ProductViewModel
            //    {
            //        Id = u.Id,
            //        Name = u.Name,
            //        Description = u.Description,
            //        AddedDate = u.AddedDate,
            //        MainImgPath = imageService.GetImages().SingleOrDefault(i=> i.MainImg == true && i.ProductId == u.Id).ImgPath,
            //        MinProductPrice = u.MinProductPrice,
            //        MaxProductPrice = u.MaxProductPrice,
            //        New = u.New,
            //        Hit = u.Hit,
            //        CategoryId = u.CategoryId,
            //        BrandId = u.BrandId
            //    };
            //    hitList.Add(hitProduct);
            //});
            //var newProducts = productService.GetProducts().Where(p => p.New == true && p.Category.Name != "No Category" && p.Brand.Name != "No Brand").OrderBy(p => p.ModifiedDate).ToList();
            //List<ProductViewModel> newList = new List<ProductViewModel>();
            //newProducts.ForEach(u =>
            //{
            //    ProductViewModel newProduct = new ProductViewModel
            //    {
            //        Id = u.Id,
            //        Name = u.Name,
            //        Description = u.Description,
            //        AddedDate = u.AddedDate,
            //        MainImgPath = imageService.GetImages().SingleOrDefault(i => i.MainImg == true && i.ProductId == u.Id).ImgPath,
            //        MinProductPrice = u.MinProductPrice,
            //        MaxProductPrice = u.MaxProductPrice,
            //        New = u.New,
            //        Hit = u.Hit,
            //        CategoryId = u.CategoryId,
            //        BrandId = u.BrandId
            //    };
            //
            //    newList.Add(newProduct);
            //});
            //ViewBag.HitList = hitList;
            //ViewBag.NewList = newList;

            List<EventPromotion> lep = eventPromotionService.GetEventPromotions().Where(ep => ep.ActiveEvent == true).OrderBy(ep => ep.StartEvent).Take(4).ToList();
            List<EventPromotionViewModel> lepvm = new List<EventPromotionViewModel>();
            foreach (var ep in lep)
            {
                EventPromotionViewModel epvm = new EventPromotionViewModel
                {
                    Id = ep.Id,
                    Name = ep.Name,
                    ImgPath = ep.ImgPath
                };
                lepvm.Add(epvm);
            }

            ViewBag.Categories = categoryService.GetCategories().Where(c => c.MainPage == true).OrderBy(c => c.ModifiedDate).Take(4).ToList();

            return View(lepvm);
        }


        [HttpPost]
        public IActionResult Search(string searchString)
        {
            List<long> productIds = articleService.GetArticles().Where(a => a.Name.ToUpper().Contains(searchString.ToUpper())).Select(a => a.ProductId).ToList();
            List<Product> products = productService.GetProducts().Where(p => (p.Name.ToUpper().Contains(searchString.ToUpper()) || productIds.Contains(p.Id)) && p.InStock == true).ToList();
            if (products.Count == 0) return RedirectToAction("NoSearch", new { searchString });
            return RedirectToAction("Catalog", new { searchString });
        }

        [HttpGet]
        public IActionResult NoSearch(string searchString)
        {
            return View("NoSearch", new { searchString });
        }

        //поиск детей
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

        //public IActionResult Product111(long id, long articleId = 0)
        //{
        //    Product productEntity = productService.GetProduct(id);
        //    if (productEntity.Category.Name == "No Category" || productEntity.Brand.Name == "No Brand" || productEntity.Images.Count == 0)
        //    {
        //        return StatusCode(423);
        //    }
        //
        //    List<Article> articles = articleService.GetArticles().Where(a => a.ProductId == id && a.Quantity > 0).ToList();
        //
        //    List<PropertyValCatArt> lpvca = propertyValCatArtService.GetAll().Where(i => articles.Select(a => a.Id).Contains(i.ArticleId)).ToList();
        //
        //    List<PropertyValue> lpv = propertyValueService.GetPropertyValues().Where(pv => lpvca.Select(i => i.PropertyValueId).Contains(pv.Id)).ToList();
        //
        //    ProductViewModel model = new ProductViewModel()
        //    {
        //        Id = productEntity.Id,
        //        Name = productEntity.Name,
        //        Description = WebUtility.HtmlDecode(productEntity.Description),
        //        CategoryId = productEntity.CategoryId,
        //        Category = productEntity.Category,
        //        BrandId = productEntity.BrandId,
        //        Brand = productEntity.Brand,
        //        New = productEntity.New,
        //        Hit = productEntity.Hit,
        //        DiscountPercent = productEntity.DiscountPercent,
        //        //Articles = articleService.GetArticles().Where(a => a.ProductId == id && a.StockStatusId == stockStatus.Id && a.Quantity != 0 && a.Quantity != null).ToList()
        //    };
        //
        //    int j = 0;
        //    Category firstCategory = categoryService.GetCategory(productEntity.CategoryId);
        //    List<Category> categoriesBred = new List<Category>() { firstCategory };
        //    //categoriesBred.Add(firstCategory);
        //    if (categoriesBred[0].ParentId != null)
        //    {
        //        while (j != -1)
        //        {
        //            Category nextCategory = categoryService.GetCategories().SingleOrDefault(nc => nc.Id == categoriesBred[j].ParentId);
        //            categoriesBred.Add(nextCategory);
        //            if (nextCategory.ParentId == null) { j = -1; } else { j++; }
        //        }
        //    }
        //    model.CategoriesBred = categoriesBred;
        //
        //    return StatusCode(423);
        //}

        public IActionResult Product(long id)
        {
            //FullProduct model = new FullProduct();
            Product productEntity = productService.GetProduct(id);
            if (productEntity.Category.Name == "No Category" || productEntity.Brand.Name == "No Brand" || productEntity.Images.Count == 0)
            {
                return StatusCode(423);
            }

            //Проверка артикулов
            //List<StockStatus> stockStatuses = stockStatusService.GetStockStatuses().ToList();
            //StockStatus stockStatus = new StockStatus();
            //foreach (var st in stockStatuses)
            //{
            //    if (st.Name == "В наличии")
            //    {
            //        stockStatus = st;
            //        break;
            //    }
            //}
            //StockStatus ss = stockStatusService.GetStockStatuses().SingleOrDefault(ss => ss.Name == "В наличии");

            ProductViewModel model = new ProductViewModel()
            {
                Id = productEntity.Id,
                Name = productEntity.Name,
                Description = WebUtility.HtmlDecode(productEntity.Description),
                CategoryId = productEntity.CategoryId,
                Category = productEntity.Category,
                BrandId = productEntity.BrandId,
                Brand = productEntity.Brand,
                CountryId = productEntity.CountryId,
                Country = productEntity.Country,
                New = productEntity.New,
                Hit = productEntity.Hit,
                DiscountPercent = productEntity.DiscountPercent,
                DayOfWeek = productEntity.DayOfWeek,
                Title = productEntity.Title,
                TitleDescription = productEntity.TitleDescrition
                //Articles = articleService.GetArticles().Where(a => a.ProductId == id && a.StockStatusId == stockStatus.Id && a.Quantity != 0 && a.Quantity != null).ToList()
            };
            List<Article> articles = articleService.GetArticles().Where(a => a.ProductId == id && a.Quantity > 0).ToList();

            if (articles.Count == 0) { return Error(); }//Чтобы не переходить в ручную

            List<Image> images = imageService.GetImages().Where(i => i.ProductId == id).OrderByDescending(a => a.MainImg).ToList();
            model.MainImgPath = images[0].ImgPath;

            //List<string> articleNames = articleService.GetArticles().Where(a => a.ProductId == id).Select(a => a.Name).ToList();
            List<ImageViewModel> livmNotArticle = new List<ImageViewModel>();//Картинки не привязанные к артикулам
            List<ImageViewModel> livmArticle = new List<ImageViewModel>();//Картинки привязанные к артикулам
            foreach (var i in images)
            {
                ImageViewModel ivm = new ImageViewModel
                {
                    Name = i.Name,
                    MainImg = i.MainImg,
                    ImgPath = i.ImgPath,
                    ProductId = i.ProductId
                };
                if (articles.Select(a => a.Name).Contains(ivm.Name.Remove(ivm.Name.LastIndexOf('.'))))
                {
                    ivm.ArticleImage = true;
                    livmArticle.Add(ivm);
                }
                else
                {
                    ivm.ArticleImage = false;
                    livmNotArticle.Add(ivm);
                }
            }
            model.Images = livmNotArticle;
            model.ArticleImages = livmArticle;
            //
            //List<PropertyValCatArt> pvca = propertyValCatArtService.GetAll().Where(p => p.ProductId == productvm.Id).ToList();
            //List<PropertyValueViewModel> propertyValues = new List<PropertyValueViewModel>();
            //List<PropertyViewModel> properties = new List<PropertyViewModel>();
            //foreach (var pv in pvca)
            //{
            //    var propertyValue = propertyValueService.GetPropertyValue(pv.PropertyValueId);
            //    PropertyValueViewModel pvvm = new PropertyValueViewModel() { Id = propertyValue.Id, Value = propertyValue.Value };
            //    if (!propertyValues.Exists(pv => pv.Id == pvvm.Id && pv.Value == pvvm.Value)) propertyValues.Add(pvvm);
            //    PropertyViewModel property = new PropertyViewModel()
            //    {
            //        Id = propertyValue.Property.Id,
            //        Name = propertyValue.Property.Name,
            //        ValueName = propertyValue.Property.ValueName
            //    };
            //    if (!properties.Exists(p => p.Id == property.Id && p.Name == property.Name && p.ValueName == property.ValueName)) properties.Add(property);
            //}
            //model.PropertyValues = propertyValues;
            //model.Properties = properties;



            //var categories = categoryService.GetCategories().ToList();
            //List<CategoryViewModel> modalCategories = new List<CategoryViewModel>();
            //categories.ForEach(u =>
            //{
            //    CategoryViewModel category = new CategoryViewModel
            //    {
            //        Id = u.Id,
            //        Name = u.Name,
            //        ParentId = u.ParentId,
            //        ParentCategory = u.ParentCategory
            //    };
            //    modalCategories.Add(category);
            //});
            //ViewBag.categories = modalCategories;

            //Хлебные крошки
            int j = 0;
            Category firstCategory = categoryService.GetCategory(model.CategoryId);
            List<Category> categoriesBred = new List<Category>() { firstCategory };
            //categoriesBred.Add(firstCategory);
            if (categoriesBred[0].ParentId != null)
            {
                while (j != -1)
                {
                    Category nextCategory = categoryService.GetCategories().SingleOrDefault(nc => nc.Id == categoriesBred[j].ParentId);
                    categoriesBred.Add(nextCategory);
                    if (nextCategory.ParentId == null) { j = -1; } else { j++; }
                }
            }
            model.CategoriesBred = categoriesBred;


            List<ArticleViewModel> lavm = new List<ArticleViewModel>();
            //List<long> articleIds = new List<long>();//Id артикула для свойств
            List<ColorViewModel> lcvm = new List<ColorViewModel>();
            foreach (var a in articles)
            {
                ColorViewModel cvm = new ColorViewModel()
                {
                    Id = a.ColorId,
                    Name = a.Color.Name,
                    ColorCode = a.Color.ColorCode
                };
                if (!lcvm.Exists(c => c.Id == cvm.Id && c.Name == cvm.Name && c.ColorCode == cvm.ColorCode) && a.Color.Name != "Нет цвета") lcvm.Add(cvm);
                ArticleViewModel avm = new ArticleViewModel()
                {
                    Id = a.Id,
                    Name = a.Name,
                    ColorId = a.ColorId,
                    Price = a.Price,
                    Quantity = a.Quantity,
                };
                foreach (var image in images)
                {
                    if (image.Name.Remove(image.Name.LastIndexOf(".")) == a.Name) avm.ImagePath = image.ImgPath;
                }
                lavm.Add(avm);
            }

            List<PropertyValCatArt> lpvca = propertyValCatArtService.GetAll().Where(p => articles.Select(a => a.Id).Contains(p.ArticleId))
                .Select(p => new PropertyValCatArt
                {
                    ArticleId = p.ArticleId,
                    ProductId = p.ProductId,
                    PropertyValueId = p.PropertyValueId,
                    CategoryId = p.CategoryId
                }).ToList();
            List<PropertyValueViewModel> propertyValues = new List<PropertyValueViewModel>();
            List<PropertyViewModel> properties = new List<PropertyViewModel>();
            List<PropertyValue> lpv = propertyValueService.GetPropertyValues().Where(pv => lpvca.Select(pvca => pvca.PropertyValueId).Contains(pv.Id)).ToList();
            foreach (var pv in lpv)
            {
                //var propertyValue = propertyValueService.GetPropertyValue(pv.PropertyValueId);
                PropertyValueViewModel pvvm = new PropertyValueViewModel() { Id = pv.Id, Value = pv.Value, PropertyId = pv.PropertyId };
                if (!propertyValues.Exists(pv => pv.Id == pvvm.Id && pv.Value == pvvm.Value)) propertyValues.Add(pvvm);
                PropertyViewModel property = new PropertyViewModel()
                {
                    Id = pv.Property.Id,
                    Name = pv.Property.Name,
                    ValueName = pv.Property.ValueName
                };
                if (!properties.Exists(p => p.Id == property.Id && p.Name == property.Name && p.ValueName == property.ValueName)) properties.Add(property);
            }
            model.PropertyValues = propertyValues.OrderBy(pv => pv.Value).ToList();
            model.Properties = properties.OrderBy(p => p.Name).ToList();
            model.ArticlesVM = lavm.OrderBy(a => a.Id).ToList();
            model.PropertyValCatArts = lpvca.OrderBy(pvca => pvca.ArticleId).ToList();
            model.Colors = lcvm;

            //model.Product = productvm;
            return View(model);
        }

        [HttpGet]
        public JsonResult GetProductArticle(long? productId)
        {
            FullProduct model = new FullProduct();
            List<Image> images = imageService.GetImages().Where(i => i.ProductId == productId).ToList();

            List<Article> articles = new List<Article>();
            StockStatus ss = stockStatusService.GetStockStatuses().SingleOrDefault(ss => ss.Name == "В наличии");
            articles = articleService.GetArticles().Where(a => a.ProductId == productId && a.StockStatusId == ss.Id && a.Quantity != 0 && a.Quantity >= 0).ToList();
            List<ArticleViewModel> lavm = new List<ArticleViewModel>();
            List<long> articleIds = new List<long>();//Id артикула для свойств
            List<ColorViewModel> lcvm = new List<ColorViewModel>();
            foreach (var a in articles)
            {
                ColorViewModel cvm = new ColorViewModel()
                {
                    Id = a.ColorId,
                    Name = a.Color.Name,
                    ColorCode = a.Color.ColorCode
                };
                if (!lcvm.Exists(c => c.Id == cvm.Id && c.Name == cvm.Name && c.ColorCode == cvm.ColorCode) && a.Color.Name != "Нет цвета") lcvm.Add(cvm);
                ArticleViewModel avm = new ArticleViewModel() {
                    Id = a.Id,
                    Name = a.Name,
                    ColorId = a.ColorId,
                    Price = a.Price,
                    Quantity = a.Quantity,
                    StockStatus = null
                };
                articleIds.Add(a.Id);
                foreach (var image in images)
                {
                    if (image.Name.Remove(image.Name.LastIndexOf(".")) == a.Name) avm.ImagePath = image.ImgPath;
                }
                lavm.Add(avm);
            }

            List<PropertyValCatArt> pvca = propertyValCatArtService.GetAll().Where(p => p.ProductId == productId && articleIds.Contains(p.ArticleId))//Дописал свойство
                .Select(p=> new PropertyValCatArt { 
                    ArticleId = p.ArticleId, 
                    ProductId = p.ProductId, 
                    PropertyValueId = p.PropertyValueId, 
                    CategoryId = p.CategoryId 
                }).ToList();
            List<PropertyValueViewModel> propertyValues = new List<PropertyValueViewModel>();
            List<PropertyViewModel> properties = new List<PropertyViewModel>();
            foreach (var pv in pvca)
            {
                var propertyValue = propertyValueService.GetPropertyValue(pv.PropertyValueId);
                PropertyValueViewModel pvvm = new PropertyValueViewModel() { Id = propertyValue.Id, Value = propertyValue.Value, PropertyId = propertyValue.PropertyId };
                if (!propertyValues.Exists(pv => pv.Id == pvvm.Id && pv.Value == pvvm.Value)) propertyValues.Add(pvvm);
                PropertyViewModel property = new PropertyViewModel()
                {
                    Id = propertyValue.Property.Id,
                    Name = propertyValue.Property.Name,
                    ValueName = propertyValue.Property.ValueName
                };
                if (!properties.Exists(p => p.Id == property.Id && p.Name == property.Name && p.ValueName == property.ValueName)) properties.Add(property);
            }
            model.PropertyValues = propertyValues.OrderBy(pv => pv.Value).ToList();
            model.Properties = properties.OrderBy(p => p.Name).ToList();
            model.Articles = lavm;
            model.PVCA = pvca;
            model.Colors = lcvm;

            return Json(model, new JsonSerializerOptions() { Encoder = JavaScriptEncoder.Create(UnicodeRanges.BasicLatin, UnicodeRanges.Cyrillic) });
        }

        public IActionResult Delivery()
        {
            return View();
        }

        public IActionResult Discounts()
        {
            return View();
        }

        public IActionResult AboutUs()
        {
            return View();
        }

        public IActionResult EventPromotions()
        {
            List<EventPromotion> eventPromotions = eventPromotionService.GetEventPromotions().Where(ep => ep.ActiveEvent == true).ToList();
            List<EventPromotionViewModel> promotions = new List<EventPromotionViewModel>();
            eventPromotions.ForEach(ep =>
            {
                EventPromotionViewModel p = new EventPromotionViewModel
                {
                    Id = ep.Id,
                    Name = ep.Name,
                    Description = ep.Description,
                    ImgPath = ep.ImgPath,
                    StartEvent = ep.StartEvent,
                    EndEvent = ep.EndEvent,
                    Products = new List<Product>()
                };
                List<EventProduct> eventProducts = eventProductService.GetEventProductes().Where(ep => ep.EventPromotionId == p.Id).Take(3).ToList();
                foreach(var eventProduct in eventProducts)
                {
                    p.Products.Add(productService.GetProduct(eventProduct.ProductId));
                }
                promotions.Add(p);
            });
        
            return View(promotions);
        }

        public IActionResult Contacts()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public IActionResult GetCategoriesList()
        {
            var catalog = categoryService.GetCategories().Where(c => c.Name != "No Category").ToList();
            List<CategoryViewModel> model = new List<CategoryViewModel>();
            catalog.ForEach(c => {
                CategoryViewModel category = new CategoryViewModel { Name = c.Name, Id = c.Id, ParentId = c.ParentId };
                model.Add(category);
            });
            return PartialView("_GetCategoriesList", model.OrderBy(c => c.Name).ToList());
        }

        public IActionResult GetSubCategoriesList(long parentId)
        {
            var catalog = categoryService.GetCategories().Where(c => c.ParentId == parentId).ToList();
            List<CategoryViewModel> model = new List<CategoryViewModel>();
            catalog.ForEach(c => {
                CategoryViewModel category = new CategoryViewModel { Name = c.Name, Id = c.Id, ParentId = c.ParentId };
                model.Add(category);
            });
            return PartialView("_GetSubCategoriesList", model.OrderBy(c => c.Name).ToList());
        }

        public IActionResult GetCategoriesListMobile()
        {
            var catalog = categoryService.GetCategories().Where(c => c.Name != "No Category").ToList();
            List<CategoryViewModel> model = new List<CategoryViewModel>();
            catalog.ForEach(c => {
                bool childCategories = GetChildrens(c.Id);
                CategoryViewModel category = new CategoryViewModel { Name = c.Name, Id = c.Id, ParentId = c.ParentId, ChildCategories = childCategories };
                model.Add(category);
            });
            return PartialView("_GetCategoriesListMobile", model.OrderBy(c => c.Name).ToList());
        }

        public IActionResult GetSubCategoriesListMobile(long parentId)
        {
            var catalog = categoryService.GetCategories().Where(c => c.ParentId == parentId).ToList();
            List<CategoryViewModel> model = new List<CategoryViewModel>();
            catalog.ForEach(c => {
                bool childCategories = GetChildrens(c.Id);
                CategoryViewModel category = new CategoryViewModel { Name = c.Name, Id = c.Id, ParentId = c.ParentId, ChildCategories = childCategories };
                model.Add(category);
            });
            return PartialView("_GetSubCategoriesListMobile", model.OrderBy(c => c.Name).ToList());
        }

        private bool GetChildrens(long Id)
        {
            var categories = categoryService.GetCategories().FirstOrDefault(c => c.ParentId == Id);
            if (categories == null)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public IActionResult Catalog(string searchString, long? categoryId)
        {
            if (searchString == null && categoryId == null)
            {
                return RedirectToAction("NoSearch", new { searchString });
            }
            else
            {
                if (searchString != null)
                {
                    CategoriesViewModel cvm = new CategoriesViewModel();
                    return View(cvm);
                }
                if (categoryId != null)
                {
                    Category firstCategory = categoryService.GetCategory((long)categoryId);
                    List<Category> categoriesBred = new List<Category>();
                    categoriesBred.Add(firstCategory);
                    int j = 0;
                    if (categoriesBred[0].ParentId != null)
                    {
                        while (j != -1)
                        {
                            Category nextCategory = categoryService.GetCategories().SingleOrDefault(nc => nc.Id == categoriesBred[j].ParentId);
                            categoriesBred.Add(nextCategory);
                            if (nextCategory.ParentId == null) { j = -1; } else { j++; }
                        }
                    }
                    CategoriesViewModel cvm = new CategoriesViewModel() { CategoriesBred = categoriesBred };
                    return View(cvm);
                }
                return StatusCode(404);
            }
        }

        [HttpGet]
        public JsonResult GetFilter(long? categoryId, string searchString)
        {
            List<PropertyValueViewModel> lpvvm = new List<PropertyValueViewModel>();
            List<PropertyViewModel> lpvm = new List<PropertyViewModel>();
            List<PropertyValue> lpvProduct = new List<PropertyValue>();
            List<ColorViewModel> lcvm = new List<ColorViewModel>();
            List<Color> colors = colorService.GetColors().Where(c=> c.Name != "Нет цвета").ToList();
            //List<long> productIdsWithImages = imageService.GetImages().Select(i => i.ProductId).ToList();
            List<Product> allProducts = productService.GetProducts().ToList();

            decimal minPrice = 0;
            decimal maxPrice = 0;
            List<Brand> brands = new List<Brand>();
            List<Country> countries = new List<Country>();
            List<long> brandIds = new List<long>();
            List<long> countryIds = new List<long>();
            List<PropertyValCatArt> lpvca = propertyValCatArtService.GetAll().ToList();

            List<Category> allCategories = categoryService.GetCategories().ToList();

            List<long> lpvcaArticles = new List<long>();
            List<PropertyValue> lpv = propertyValueService.GetPropertyValues().ToList();

            if (searchString == null)
            {
                //Добавляются категории
                List<Product> products = allProducts//productService.GetProducts()
                    .Where(p => (p.CategoryId == categoryId )
                    && p.InStock == true 
                    && p.Images.Count > 0)
                    //&& productIdsWithImages.Contains(p.Id))
                    .ToList();

                //Добавляются продукты потомка
                List<long> categoriesChildId = categoryService.GetCategories().Where(c => c.ParentId == categoryId).Select(c => c.Id).ToList();
                products.AddRange(productService.GetProducts()
                    .Where(p=> categoriesChildId.Contains(p.CategoryId)  
                    && p.InStock == true
                    && p.Images.Count > 0)
                    //&& productIdsWithImages.Contains(p.Id))
                    .ToList());

                //Получаем Id артикулов
                List<long> productIds = products.Select(p => p.Id).ToList();
                List<Article> articles = articleService.GetArticles().Where(a => productIds.Contains(a.ProductId) && a.Quantity > 0).ToList();

                foreach (var pvca in lpvca.Where(pv => 
                    articles.Select(a=>a.Id).ToList().Contains(pv.ArticleId))) //Добисал условие на выборку по артикулам
                {
                    lpvProduct.Add(lpv.SingleOrDefault(a => a.Id == pvca.PropertyValueId));//lpv.Add(pvca.PropertyValueId);
                    lpvcaArticles.Add(pvca.ArticleId);
                }
                if (products.Count > 0)
                {
                    minPrice = products.Min(p => p.MinProductPrice);
                    maxPrice = products.Max(p => p.MaxProductPrice);
                }
                foreach (var p in products)
                {
                    if (!brandIds.Contains((long)p.BrandId))
                    {
                        brandIds.Add((long)p.BrandId);
                    }
                    if (p.CountryId != null)
                    {
                        if (!countryIds.Contains((long)p.CountryId))
                        {
                            countryIds.Add((long)p.CountryId);
                        }
                    }
                }
            }
            else
            {
                List<Product> products = allProducts//productService.GetProducts()
                    .Where(p => p.Name.ToUpper()
                    .Contains(searchString.ToUpper()) 
                    && p.InStock == true
                    && p.Images.Count > 0)
                    //&& productIdsWithImages.Contains(p.Id))
                    .ToList();
                List<long> productIds = products.Select(p => p.Id).ToList();
                List<Article> articles = articleService.GetArticles().Where(a => (productIds.Contains(a.ProductId)|| a.Name.ToUpper().Contains(searchString.ToUpper())) && a.Quantity > 0).ToList();
                if (!articles.Select(a=> a.ProductId).Distinct().SequenceEqual(products.Select(p=> p.Id)))
                {
                    List<long> otherProductIds = articles.Select(a => a.ProductId).Distinct().Except(products.Select(p => p.Id)).ToList();
                    products.AddRange(allProducts.Where(p => otherProductIds.Contains(p.Id)));
                }
                List<PropertyValCatArt> filtredLPVCA = lpvca.Where(pv =>
                    articles.Select(a => a.Id).ToList().Contains(pv.ArticleId)).ToList();
                foreach (var pvca in filtredLPVCA)
                    //lpvca.Where(pv => 
                    //articles.Select(a => a.Id).ToList().Contains(pv.ArticleId))) //Добисал условие на выборку по артикулам)) 
                {
                    lpvProduct.Add(lpv.SingleOrDefault(a => a.Id == pvca.PropertyValueId));//lpv.Add(pvca.PropertyValueId);
                    lpvcaArticles.Add(pvca.ArticleId);
                }
                if (products.Count > 0)
                {
                    minPrice = products.Min(p => p.MinProductPrice);
                    maxPrice = products.Max(p => p.MaxProductPrice);
                }
                foreach (var p in products)
                {
                    if (!brandIds.Contains((long)p.BrandId))
                    {
                        brandIds.Add((long)p.BrandId);
                    }
                    if (!countryIds.Contains((long)p.BrandId))
                    {
                        countryIds.Add((long)p.CountryId);
                    }
                }
            }

            foreach(var pv in lpvProduct)//lpv)
            {
                if (!pv.Property.TurnOff) //от переключателя
                {
                    PropertyValueViewModel pvvm = new PropertyValueViewModel()
                    {
                        Id = pv.Id,
                        Value = pv.Value,
                        PropertyId = pv.PropertyId
                    };
                    if (!lpvvm.Exists(p => p.Id == pvvm.Id && p.Value == pvvm.Value && p.PropertyId == pvvm.PropertyId)) lpvvm.Add(pvvm);
                    PropertyViewModel pvm = new PropertyViewModel()
                    {
                        Id = pv.Property.Id,
                        ValueName = pv.Property.ValueName,
                        Name = pv.Property.Name
                    };
                    if (!lpvm.Exists(p => p.Id == pvm.Id && p.Name == pvm.Name && p.ValueName == pvm.ValueName)) lpvm.Add(pvm);
                }
            }

            List<BrandViewModel> lbvm = new List<BrandViewModel>();
            brands = brandService.GetBrands().Where(b => brandIds.Contains(b.Id) && b.Name != "No Brand").ToList();
            foreach (var b in brands)
            {
                BrandViewModel bvm = new BrandViewModel() { Id = b.Id, BrandName = b.Name };
                lbvm.Add(bvm);
            }
            List<CountryViewModel> listCountryVM = new List<CountryViewModel>();
            countries = countryService.GetCountries().Where(b => countryIds.Contains(b.Id) && b.Name != "No Country").ToList();
            foreach (var b in countries)
            {
                CountryViewModel bvm = new CountryViewModel() { Id = b.Id, CountryName = b.Name };
                listCountryVM.Add(bvm);
            }

            List<long> colorArticleId = articleService.GetArticles().Where(a=> lpvcaArticles.Contains(a.Id)).Select(a=> a.ColorId).ToList();
            foreach(var colorId in colorArticleId)
            {
                if (colors.Select(c => c.Id).ToList().Contains(colorId))
                {
                    Color color = colors.SingleOrDefault(c => c.Id == colorId);
                    ColorViewModel cvm = new ColorViewModel
                    {
                        Id = color.Id,
                        Name = color.Name,
                        ColorCode = color.ColorCode
                    };
                    if (!lcvm.Exists(c => c.Id == cvm.Id && c.Name == cvm.Name && c.ColorCode == cvm.ColorCode)) lcvm.Add(cvm);
                }
            }

            FilterViewModel fvm = new FilterViewModel() { Properties = lpvm, PropertyValues = lpvvm, MinPrice = minPrice, MaxPrice = maxPrice, Brands = lbvm, Colors = lcvm, Countries = listCountryVM};

            return Json(fvm, new JsonSerializerOptions() { Encoder = JavaScriptEncoder.Create(UnicodeRanges.BasicLatin, UnicodeRanges.Cyrillic) });
        }

        [HttpGet]
        public JsonResult GetCatalog(long? categoryId, string searchString)
        {
            List<FullProduct> products = new List<FullProduct>();
            List<Product> onlyProduct = new List<Product>();
            List<long> productIdsWithImages = imageService.GetImages().Select(i => i.ProductId).ToList();
            List<PropertyValCatArt> pvca = propertyValCatArtService.GetAll().ToList();
            List<Image> images = imageService.GetImages().ToList();
            List<PropertyValue> propertyValues = propertyValueService.GetPropertyValues().ToList();
            List<Color> colors = colorService.GetColors().Where(c => c.Name != "Нет цвета").ToList();
            List<Article> articles = articleService.GetArticles().ToList();

            List<long> categoriesIds = new List<long>();
            if (categoryId != null) 
            {
                List<Category> categoriesChild = categoryService.GetCategories().Where(c=> c.ParentId == categoryId).ToList();
                categoriesIds = categoriesChild.Select(c=> c.Id).ToList();
                categoriesIds.Add((long)categoryId);
            }

            if (searchString == null)
            {
                onlyProduct = productService.GetProducts().Where(p => categoriesIds.Contains(p.CategoryId) && p.InStock == true && productIdsWithImages.Contains(p.Id)).OrderBy(p => p.MinProductPrice).ToList();
            }
            else
            {
                List<long> productIds = articles.Where(p => p.Name.ToUpper().Contains(searchString.ToUpper())).Select(a=> a.ProductId).ToList();
                onlyProduct  = productService.GetProducts().Where(p => (p.Name.ToUpper().Contains(searchString.ToUpper())|| productIds.Contains(p.Id)) && p.InStock == true && p.Images.Count > 0).OrderBy(p => p.MinProductPrice).ToList();
            }

            foreach (var op in onlyProduct)
            {
                ProductViewModel pvm = new ProductViewModel()
                {
                    Id = op.Id,
                    Name = op.Name,
                    MainImgPath = images.SingleOrDefault(i => i.ProductId == op.Id && i.MainImg == true).ImgPath,
                    MinProductPrice = op.MinProductPrice,
                    MaxProductPrice = op.MaxProductPrice,
                    DiscountPercent = op.DiscountPercent,
                    BrandId = op.BrandId,
                    CountryId = op.CountryId,
                    DayOfWeek = op.DayOfWeek
                };
                List<PropertyValCatArt> pvcaProduct = pvca.Where(p => p.ProductId == pvm.Id).ToList();
                List<PropertyValueViewModel> propertyValuesProduct = new List<PropertyValueViewModel>();
                foreach (var pv in pvcaProduct)
                {
                    var propertyValue = propertyValues.SingleOrDefault(a => a.Id == pv.PropertyValueId);
                    PropertyValueViewModel pvvm = new PropertyValueViewModel() { Id = propertyValue.Id, Value = propertyValue.Value };
                    if (!propertyValuesProduct.Exists(pval => pval.Id == pvvm.Id && pval.Value == pvvm.Value)) propertyValuesProduct.Add(pvvm);
                }

                List<Article> articleColros = articles.Where(a=> a.ProductId == pvm.Id ).ToList();
                List<long> colorArticleId = articleColros/*.Where(a=> lpvcaArticles.Contains(a.Id))*/.Select(a=> a.ColorId).ToList();
                List<ColorViewModel> lcvm = new List<ColorViewModel>();
                foreach(var colorId in colorArticleId)
                {
                    if (colors.Select(c=>c.Id).ToList().Contains(colorId))
                    {
                        Color color = colors.SingleOrDefault(c=> c.Id == colorId);
                        ColorViewModel cvm = new ColorViewModel
                        {
                            Id = color.Id,
                            Name = color.Name,
                            ColorCode = color.ColorCode
                        };
                        if (!lcvm.Exists(c => c.Id == cvm.Id && c.Name == cvm.Name && c.ColorCode == cvm.ColorCode)) lcvm.Add(cvm);
                    }
                }
                pvm.Colors = lcvm;

                products.Add(new FullProduct { Product = pvm, PropertyValues = propertyValuesProduct });
            }
            return Json(products, new JsonSerializerOptions() { Encoder = JavaScriptEncoder.Create(UnicodeRanges.BasicLatin, UnicodeRanges.Cyrillic) });
        }


        [HttpGet]
        public JsonResult GetData(long? categoryId, string searchString)
        {
            List<PropertyValueViewModel> lpvvm = new List<PropertyValueViewModel>();
            List<PropertyViewModel> lpvm = new List<PropertyViewModel>();
            List<PropertyValue> lpvProduct = new List<PropertyValue>();
            List<ColorViewModel> lcvm = new List<ColorViewModel>();
            List<Color> colors = colorService.GetColors().Where(c => c.Name != "Нет цвета").ToList();
            List<long> productIdsWithImages = imageService.GetImages().Select(i => i.ProductId).ToList();

            decimal minPrice;
            decimal maxPrice;
            List<Brand> brands = new List<Brand>();
            List<long> brandIds = new List<long>();
            List<PropertyValCatArt> lpvca = new List<PropertyValCatArt>();//propertyValCatArtService.GetAll().ToList();

            //List<Category> allCategories = new List<Category>();//categoryService.GetCategories().ToList();

            List<long> lpvcaArticles = new List<long>();
            List<PropertyValue> lpv = new List<PropertyValue>();//propertyValueService.GetPropertyValues().ToList();

            List<Product> products = new List<Product>();
            List<Article> articles = new List<Article>();

            if (searchString == null)
            {
                //Добавляются продукты потомка
                List<long> categoriesChildId = categoryService.GetCategories().Where(c => c.ParentId == categoryId ).Select(c => c.Id).ToList();
                categoriesChildId.Add(categoryId.Value);

                //Добавляются категории
                products = productService.GetProducts()
                    .Where(p => categoriesChildId.Contains(p.CategoryId)//(p.CategoryId == categoryId)
                    && p.InStock == true && productIdsWithImages.Contains(p.Id)).ToList();

                //products.AddRange(productService.GetProducts().Where(p => categoriesChildId.Contains(p.CategoryId) && p.InStock == true && productIdsWithImages.Contains(p.Id)).ToList());

                //Получаем Id артикулов
                List<long> productIds = products.Select(p => p.Id).ToList();
                articles = articleService.GetArticles().Where(a => productIds.Contains(a.ProductId) && a.Quantity > 0).ToList();

                foreach (var pvca in lpvca.Where(pv =>
                    articles.Select(a => a.Id).ToList().Contains(pv.ArticleId))) //Добисал условие на выборку по артикулам
                {
                    lpvProduct.Add(lpv.SingleOrDefault(a => a.Id == pvca.PropertyValueId));//lpv.Add(pvca.PropertyValueId);
                    lpvcaArticles.Add(pvca.ArticleId);
                }
                minPrice = products.Min(p => p.MinProductPrice);
                maxPrice = products.Max(p => p.MaxProductPrice);
                //foreach (var p in products)
                //{
                //    if (!brandIds.Contains(p.BrandId))
                //    {
                //        brandIds.Add(p.BrandId);
                //    }
                //}
            }
            else
            {
                products = productService.GetProducts().Where(p => p.Name.ToUpper().Contains(searchString.ToUpper()) && p.InStock == true && productIdsWithImages.Contains(p.Id)).ToList();
                List<long> productIds = products.Select(p => p.Id).ToList();
                articles = articleService.GetArticles().Where(a => productIds.Contains(a.ProductId) && a.Quantity > 0).ToList();
                foreach (var pvca in lpvca.Where(pv =>
                    articles.Select(a => a.Id).ToList().Contains(pv.ArticleId))) //Добисал условие на выборку по артикулам)) 
                {
                    lpvProduct.Add(lpv.SingleOrDefault(a => a.Id == pvca.PropertyValueId));//lpv.Add(pvca.PropertyValueId);
                    lpvcaArticles.Add(pvca.ArticleId);
                }
                minPrice = products.Min(p => p.MinProductPrice);
                maxPrice = products.Max(p => p.MaxProductPrice);
                //foreach (var p in products)
                //{
                //    if (!brandIds.Contains(p.BrandId))
                //    {
                //        brandIds.Add(p.BrandId);
                //    }
                //}
            }

            foreach (var pv in lpvProduct)
            {
                if (!pv.Property.TurnOff) //от переключателя
                {
                    PropertyValueViewModel pvvm = new PropertyValueViewModel()
                    {
                        Id = pv.Id,
                        Value = pv.Value,
                        PropertyId = pv.PropertyId
                    };
                    if (!lpvvm.Exists(p => p.Id == pvvm.Id && p.Value == pvvm.Value && p.PropertyId == pvvm.PropertyId)) lpvvm.Add(pvvm);
                    PropertyViewModel pvm = new PropertyViewModel()
                    {
                        Id = pv.Property.Id,
                        ValueName = pv.Property.ValueName,
                        Name = pv.Property.Name
                    };
                    if (!lpvm.Exists(p => p.Id == pvm.Id && p.Name == pvm.Name && p.ValueName == pvm.ValueName)) lpvm.Add(pvm);
                }
            }


            List<long> colorArticleId = articles.Where(a => lpvcaArticles.Contains(a.Id)).Select(a => a.ColorId).ToList();
            //foreach (var colorId in colorArticleId.Distinct())
            //{
            //    if (colors.Select(c => c.Id).ToList().Contains(colorId))
            //    {
            //        Color color = colors.SingleOrDefault(c => c.Id == colorId);
            //        ColorViewModel cvm = new ColorViewModel
            //        {
            //            Id = color.Id,
            //            Name = color.Name,
            //            ColorCode = color.ColorCode
            //        };
            //        if (!lcvm.Exists(c => c.Id == cvm.Id && c.Name == cvm.Name && c.ColorCode == cvm.ColorCode)) lcvm.Add(cvm);
            //    }
            //}




            List<FullProduct> fullProducts = new List<FullProduct>();
            List<Image> images = imageService.GetImages().Where(i=> i.MainImg == true && products.Select(p=> p.Id).Contains(i.ProductId)).ToList();

            foreach (var op in products)
            {
                ProductViewModel pvm = new ProductViewModel()
                {
                    Id = op.Id,
                    Name = op.Name,
                    MainImgPath = images.SingleOrDefault(i => i.ProductId == op.Id && i.MainImg == true).ImgPath,
                    MinProductPrice = op.MinProductPrice,
                    MaxProductPrice = op.MaxProductPrice,
                    DiscountPercent = op.DiscountPercent,
                    BrandId = op.BrandId
                }; 
                if (!brandIds.Contains((long)op.BrandId))
                {
                    brandIds.Add((long)op.BrandId);
                }
                List<PropertyValCatArt> pvcaProduct = lpvca.Where(p => p.ProductId == pvm.Id).ToList();
                List<PropertyValueViewModel> propertyValuesProduct = new List<PropertyValueViewModel>();
                foreach (var pv in pvcaProduct)
                {
                    var propertyValue = lpvProduct.SingleOrDefault(a => a.Id == pv.PropertyValueId);
                    PropertyValueViewModel pvvm = new PropertyValueViewModel() { Id = propertyValue.Id, Value = propertyValue.Value };
                    if (!lpvProduct.Exists(pv => pv.Id == pvvm.Id && pv.Value == pvvm.Value)) propertyValuesProduct.Add(pvvm);
                }

                List<Article> articleColors = articles.Where(a => a.ProductId == pvm.Id).ToList();
                //List<long> colorArticleId = articleColors/*.Where(a=> lpvcaArticles.Contains(a.Id))*/.Select(a => a.ColorId).ToList();
                //List<ColorViewModel> lcvm = new List<ColorViewModel>();
                foreach (var colorId in colorArticleId.Distinct())
                {
                    if (colors.Select(c => c.Id).ToList().Contains(colorId))
                    {
                        Color color = colors.SingleOrDefault(c => c.Id == colorId);
                        ColorViewModel cvm = new ColorViewModel
                        {
                            Id = color.Id,
                            Name = color.Name,
                            ColorCode = color.ColorCode
                        };
                        if (!lcvm.Exists(c => c.Id == cvm.Id && c.Name == cvm.Name && c.ColorCode == cvm.ColorCode)) lcvm.Add(cvm);
                    }
                }
                pvm.Colors = lcvm;

                fullProducts.Add(new FullProduct { Product = pvm, PropertyValues = propertyValuesProduct });
            }



            List<BrandViewModel> lbvm = new List<BrandViewModel>();
            brands = brandService.GetBrands().Where(b => brandIds.Contains(b.Id)).ToList();
            foreach (var b in brands)
            {
                BrandViewModel bvm = new BrandViewModel() { Id = b.Id, BrandName = b.Name };
                lbvm.Add(bvm);
            }

            FilterViewModel fvm = new FilterViewModel() { Properties = lpvm, PropertyValues = lpvvm, MinPrice = minPrice, MaxPrice = maxPrice, Brands = lbvm, Colors = lcvm };

            var dataJson = new { fvm, fullProducts };

            return Json(dataJson, new JsonSerializerOptions() { Encoder = JavaScriptEncoder.Create(UnicodeRanges.BasicLatin, UnicodeRanges.Cyrillic) });
        }
    }
}
