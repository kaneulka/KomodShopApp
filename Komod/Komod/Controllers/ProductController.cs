using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Text.Unicode;
using System.Threading.Tasks;
using Komod.Data;
using Komod.Ser.ArticleSer;
using Komod.Ser.BrandSer;
using Komod.Ser.CategorySer;
using Komod.Ser.ColorSer;
using Komod.Ser.CountrySer;
using Komod.Ser.ImageSer;
using Komod.Ser.ProductSer;
using Komod.Ser.ProductSetSer;
using Komod.Ser.PropertySer;
using Komod.Ser.PropertyValCatArtSer;
using Komod.Ser.PropertyValueSer;
using Komod.Ser.StockStatusSer;
using Komod.Web.Models;
using Komod.Web.Models.ProductModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Komod.Web.Controllers
{
    [Authorize(Roles = "admin")]
    public class ProductController : Controller
    {
        private readonly IProductService productService;
        private readonly IProductSetService productSetService;
        private readonly IArticleService articleService;
        private readonly ICategoryService categoryService;
        private readonly IBrandService brandService;
        private readonly ICountryService countryService;
        private readonly IPropertyService propertyService;
        private readonly IPropertyValueService propertyValueService;
        private readonly IPropertyValCatArtService propertyValCatArtService;
        private readonly IImageService imageService;
        private readonly IColorService colorService;
        private readonly IStockStatusService stockStatusService;
        IWebHostEnvironment _appEnvironment;
        
        public ProductController
        (IWebHostEnvironment appEnvironment,
            IProductService productService,
            IArticleService articleService,
            ICategoryService categoryService,
            IBrandService brandService,
            IPropertyService propertyService,
            IPropertyValCatArtService propertyValCatArtService,
            IStockStatusService stockStatusService,
            IImageService imageService,
            IPropertyValueService propertyValueService,
            IColorService colorService,
            ICountryService countryService,
            IProductSetService productSetService)
        {
            _appEnvironment = appEnvironment;
            this.productService = productService;
            this.productSetService = productSetService;
            this.articleService = articleService;
            this.categoryService = categoryService;
            this.brandService = brandService;
            this.propertyService = propertyService;
            this.propertyValueService = propertyValueService;
            this.propertyValCatArtService = propertyValCatArtService;
            this.stockStatusService = stockStatusService;
            this.imageService = imageService;
            this.colorService = colorService;
            this.countryService = countryService;
        }

        [HttpGet]
        public IActionResult Products(int page = 1, int sortType = 0, string searchString = null)
        {
            List<Product> products;
            if (searchString == null)
            {
                products = productService.GetProducts().ToList();
            }
            else
            {
                searchString = searchString.ToUpper();
                List<long> articlesId = articleService.GetArticles().Where(a => a.Name.ToUpper().Contains(searchString)).Select(a => a.ProductId).ToList();
                if (articlesId.Count() > 0)
                {
                    products = productService.GetProducts().Where(a => articlesId.Contains(a.Id)).ToList();
                }
                else
                {
                    products = productService.GetProducts().Where(s => s.Name.ToUpper().Contains(searchString)
                        || s.Category.Name.ToString().ToUpper().Contains(searchString)
                        || s.Brand.Name.ToString().ToUpper().Contains(searchString)
                        || s.AddedDate.ToString().ToUpper().Contains(searchString)
                        || s.ModifiedDate.ToString().ToUpper().Contains(searchString)
                      ).ToList();
                }
            }

            switch (sortType)
            {
                case 0:
                    products = products.OrderByDescending(b => b.AddedDate).ToList();
                    break;
                case 1:
                    products = products.OrderBy(b => b.AddedDate).ToList();
                    break;
                case 2:
                    products = products.OrderByDescending(b => b.ModifiedDate).ToList();
                    break;
                case 3:
                    products = products.OrderBy(b => b.ModifiedDate).ToList();
                    break;
                case 4:
                    products = products.OrderBy(b => b.Name).ToList();
                    break;
                case 5:
                    products = products.OrderByDescending(b => b.Name).ToList();
                    break;
                case 6:
                    products = products.OrderBy(b => b.Category.Name).ToList();
                    break;
                case 7:
                    products = products.OrderByDescending(b => b.Category.Name).ToList();
                    break;
                case 8:
                    products = products.OrderBy(b => b.Brand.Name).ToList();
                    break;
                case 9:
                    products = products.OrderByDescending(b => b.Brand.Name).ToList();
                    break;
            }
            ViewBag.SortType = sortType;

            int pageSize = 20;

            List<ProductViewModel> listProducts = new List<ProductViewModel>();

            var count = products.Count();
            var items = products.Skip((page - 1) * pageSize).Take(pageSize).ToList();
            PageViewModel pageViewModel = new PageViewModel(count, page, pageSize);
            items.ForEach(u =>
            {
                ProductViewModel product = new ProductViewModel
                {
                    Id = u.Id,
                    Name = u.Name,
                    Description = u.Description,
                    AddedDate = u.AddedDate,
                    ModifiedDate = u.ModifiedDate,
                    New = u.New,
                    Hit = u.Hit,
                    CategoryId = u.CategoryId,
                    BrandId = u.BrandId,
                    Category = u.Category,
                    Brand = u.Brand,
                    Country = u.Country,
                    DiscountPercent = u.DiscountPercent,
                    DayOfWeek = u.DayOfWeek
                };
                listProducts.Add(product);
            });

            ProductsViewModel viewModel = new ProductsViewModel
            {
                PageViewModel = pageViewModel,
                Products = listProducts,
                searchString = searchString,
                sortType = sortType
            };
            return View(viewModel);
        }

        [HttpGet]
        public ActionResult AddProduct(string returnurl)
        {
            ProductViewModel model = new ProductViewModel();
            var categories = categoryService.GetCategories().OrderBy(c => c.Name).Select(c => new { c.Id, Value = c.Name });
            var brands = brandService.GetBrands().OrderBy(c => c.Name).Select(c => new { c.Id, Value = c.Name });
            var countries = countryService.GetCountries().OrderBy(c => c.Name).Select(c => new { c.Id, Value = c.Name });
            model.Categories = new SelectList(categories, "Id", "Value");
            model.Brands = new SelectList(brands, "Id", "Value");
            model.Countries = new SelectList(countries, "Id", "Value");

            ViewBag.ReturnUrl = returnurl;

            return PartialView("_AddProduct", model);
        }

        [HttpPost]
        public ActionResult AddProduct(ProductViewModel model, string returnurl)
        {
            Product productEntity = new Product
            {
                Name = model.Name,
                Description = model.Description,
                AddedDate = DateTime.Now,
                New = false,
                Hit = false,
                CategoryId = model.CategoryId,
                BrandId = model.BrandId == 0 ? brandService.GetBrands().SingleOrDefault(b=> b.Name == "No Brand").Id : model.BrandId,
                CountryId = model.CountryId == 0 ? countryService.GetCountries().SingleOrDefault(c=> c.Name == "No Country").Id : model.CountryId,
                ModifiedDate = DateTime.Now,
                Title = model.Title,
                TitleDescrition = model.TitleDescription,
                DiscountPercent = model.DiscountPercent,
                DayOfWeek = model.DayOfWeek.ToString() == "" ? null : model.DayOfWeek
            };

            if (ModelState.IsValid)
            {
                string path = _appEnvironment.WebRootPath + "/Files/" + productEntity.Name;
                DirectoryInfo di = new DirectoryInfo(path);
                if (Directory.Exists(path) == false)
                    di.Create();
                productEntity.Path = path;

                productService.InsertProduct(productEntity);
                if (productEntity.Id > 0)
                {
                    return Redirect(returnurl);
                }
            }
            return PartialView("_AddProduct", model);
        }


        [HttpGet]
        public ActionResult EditProduct(long? id, string returnurl)
        {
            ProductViewModel model = new ProductViewModel();
            if (id.HasValue && id != 0)
            {
                Product productEntity = productService.GetProduct(id.Value);
                model.Name = productEntity.Name;
                model.Description = productEntity.Description;
                model.New = productEntity.New;
                model.Hit = productEntity.Hit;
                model.CategoryId = productEntity.CategoryId;
                model.BrandId = productEntity.BrandId;
                model.CountryId = productEntity.CountryId;
                var categories = categoryService.GetCategories().OrderBy(c => c.Name).Select(c => new { c.Id, Value = c.Name });
                var brands = brandService.GetBrands().OrderBy(c => c.Name).Select(c => new { c.Id, Value = c.Name });
                var countries = countryService.GetCountries().OrderBy(c => c.Name).Select(c => new { c.Id, Value = c.Name });
                model.Categories = new SelectList(categories, "Id", "Value", productEntity.CategoryId);
                model.Brands = new SelectList(brands, "Id", "Value", productEntity.BrandId);
                model.Countries = new SelectList(countries, "Id", "Value", productEntity.CountryId);
                model.Title = productEntity.Title;
                model.TitleDescription = productEntity.TitleDescrition;
                model.DiscountPercent = productEntity.DiscountPercent;
                model.DayOfWeek = productEntity.DayOfWeek;
            }

            ViewBag.ReturnUrl = returnurl;

            return PartialView("_EditProduct", model);
        }

        [HttpPost]
        public ActionResult EditProduct(ProductViewModel model, string returnurl)
        {
            Product productEntity = productService.GetProduct(model.Id);

            productEntity.Name = model.Name;
            productEntity.Description = model.Description;
            productEntity.New = model.New;
            productEntity.Hit = model.Hit;
            productEntity.CategoryId = model.CategoryId;

            productEntity.BrandId = model.BrandId == 0 ? brandService.GetBrands().SingleOrDefault(b=> b.Name == "No Brand").Id : model.BrandId;
            productEntity.CountryId = model.CountryId == 0 ? countryService.GetCountries().SingleOrDefault(c=> c.Name == "No Country").Id : model.CountryId;
            productEntity.ModifiedDate = DateTime.Now;
            productEntity.Title = model.Title;
            productEntity.TitleDescrition = model.TitleDescription;
            productEntity.DayOfWeek = model.DayOfWeek.ToString() == "" ? null : model.DayOfWeek;
            productEntity.DiscountPercent = model.DiscountPercent;

            if (ModelState.IsValid)
            {
                //Переименование папки
                string newPath = _appEnvironment.WebRootPath + "/Files/" + productEntity.Name;
                DirectoryInfo di = new DirectoryInfo(productEntity.Path);
                if (Directory.Exists(productEntity.Path) == true && newPath != productEntity.Path)
                    di.MoveTo(newPath);
                productEntity.Path = newPath;
                //переименование пути у картинок в бд
                imageService.GetImages().Where(p => p.ProductId == productEntity.Id).ToList().ForEach(p =>
                {
                    Image image = imageService.GetImage(p.Id);
                    {
                        p.ImgPath = "/Files/" + productEntity.Name + "/" + p.Name;
                        p.ModifiedDate = DateTime.Now;
                    };
                    imageService.UpdateImage(image);
                });

                if (productEntity.Id > 0)
                {
                    productService.UpdateProduct(productEntity);
                    return Redirect(returnurl);
                }
            }
            return PartialView("_EditProduct", model);
        }

        [HttpPost]
        public ActionResult OnMainNew(long Id, string returnurl)
        {
            Product productEntity = productService.GetProduct(Id);
            productEntity.New = !productEntity.New;
            productService.UpdateProduct(productEntity);
            return Redirect(returnurl);
        }

        [HttpPost]
        public ActionResult OnMainHit(long Id, string returnurl)
        {
            Product productEntity = productService.GetProduct(Id);
            productEntity.Hit = !productEntity.Hit;
            productService.UpdateProduct(productEntity);
            return Redirect(returnurl);
        }

        [HttpGet]
        public PartialViewResult DeleteProduct(int? id, string returnurl)
        {
            ProductViewModel model = new ProductViewModel();
            if (id.HasValue && id != 0)
            {
                Product productEntity = productService.GetProduct(id.Value);
                model.Name = productEntity.Name;
            }

            ViewBag.ReturnUrl = returnurl;

            return PartialView("_DeleteProduct", model);
        }

        [HttpPost]
        public ActionResult DeleteProduct(long id, string returnurl)
        {
            //Удаление папки
            string productPath = productService.GetProduct(id).Path;
            DirectoryInfo di = new DirectoryInfo(productPath);
            if (Directory.Exists(productPath) == true)
                di.Delete(true);


            List<PropertyValCatArt> lpvca = propertyValCatArtService.GetAll().Where(p=> p.ProductId == id).ToList();
            propertyValCatArtService.DeleteSome(lpvca);
            
            productService.DeleteProduct(id);
            return Redirect(returnurl);
        }


        //Список картинок
        [HttpGet]
        public IActionResult ImageList(long productId, bool mainImg, string returnurl)
        {
            List<ImageViewModel> model = new List<ImageViewModel>();
            List<string> articleNames = articleService.GetArticles().Where(a => a.ProductId == productId).Select(a => a.Name).ToList();//Получить все имена артикулов для поиска картинок
            imageService.GetImages().Where(p => p.ProductId == productId).ToList().ForEach(p =>
            {
                ImageViewModel image = new ImageViewModel
                {
                    Id = p.Id,
                    Name = p.Name,
                    ImgPath = p.ImgPath,
                    ProductId = p.ProductId,
                    AddedDate = p.AddedDate,
                    MainImg = p.MainImg
                };

                //ПРоверка на то, привязана ли картинка к артикулу 
                if (!(articleNames.Contains(image.Name.Remove(image.Name.LastIndexOf('.'))))) model.Add(image);//image.ArticleImage = true; else image.ArticleImage = false;
            });
            ViewBag.mainImg = mainImg;
            ViewBag.productId = productId;
            ViewBag.ReturnUrl = returnurl;

            return View(model);
        }

        //Добавить картинку
        [HttpPost]
        public async Task<IActionResult> AddImage(IFormFileCollection uploadedFiles, long productId, string returnurl)
        {
            bool status = false;
            Product product = productService.GetProduct(productId);
            foreach (var uploadedFile in uploadedFiles)
            {
                if (uploadedFile != null)
                {
                    string fileName = uploadedFile.FileName.Substring(uploadedFile.FileName.LastIndexOf('\\') + 1);
                    string path = "/Files/" + product.Name + "/" + fileName;
                    // сохраняем файл в папку Files в каталоге wwwroot
                    using (var fileStream = new FileStream(product.Path + "/" + fileName, FileMode.Create))
                    {
                        await uploadedFile.CopyToAsync(fileStream);
                    }
                    Image image = new Image
                    {
                        Name = fileName,//uploadedFile.FileName,
                        ImgPath = path,
                        ProductId = productId,
                        AddedDate = DateTime.Now,
                        ModifiedDate = DateTime.Now,
                        Product = productService.GetProduct(productId),
                        MainImg = false
                    };

                    imageService.InsertImage(image);

                    //Проверка на наличие главной картинки
                    List<bool> imagesMain = imageService.GetImages().Where(p => p.ProductId == productId).Select(a => a.MainImg).ToList();
                    bool mainImgExist = false;
                    if (imagesMain.Contains(true))
                    {
                        mainImgExist = true;
                    }

                    //Если нет, то сделать первую картинку главной
                    if (!mainImgExist)
                    {
                        image.MainImg = true;
                        imageService.UpdateImage(image);
                    }
                    status = true;
                }
            }
            if (status == true)
                return RedirectToAction("ImageList", "Product", new { productId, returnurl });
            else
                return RedirectToAction("Products", "Product");
        }

        [HttpPost]
        public IActionResult DeleteImage(long Id, string returnurl)
        {
            Image image = imageService.GetImage(Id);
            long productId = image.ProductId;
            FileInfo fi = new FileInfo(_appEnvironment.WebRootPath + "/" + image.ImgPath);
            int imagesCount = imageService.GetImages().Where(i => i.ProductId == productId).Count();
            fi.Delete();
            if (image.MainImg)
            {
                imageService.DeleteImage(image.Id);
                if (imagesCount > 1)
                {
                    Image imageFirst = imageService.GetImages().FirstOrDefault(i => i.ProductId == productId);
                    imageFirst.MainImg = true;
                    imageService.UpdateImage(imageFirst);
                }
            }
            else
            {
                imageService.DeleteImage(image.Id);
            }
            return RedirectToAction("ImageList", "Product", new { productId = image.ProductId, returnurl });

        }

        //Сделать картину главной
        [HttpPost]
        public ActionResult DoImageMain(long Id, bool MainImg, long productId, string returnurl)
        {
            Image imageEntity = imageService.GetImage(Id);
            int imagesCount = imageService.GetImages().Where(i => i.ProductId == productId).Count();
            if (imagesCount > 0)
            {
                Image oldMainImage = imageService.GetImages().SingleOrDefault(i => i.MainImg == true && i.ProductId == productId);
                oldMainImage.MainImg = false;
                imageService.UpdateImage(oldMainImage);
                imageEntity.MainImg = true;
                imageService.UpdateImage(imageEntity);
            }

            return RedirectToAction("ImageList", "Product", new { productId, mainImg = false, returnurl });
        }

        [HttpGet]
        public IActionResult AllDiscount()
        {
            return View("_AllDiscount");
        }

        [HttpPost]
        public IActionResult AllDiscount(bool giveDiscountAllProducts, decimal numberDiscount = 0)
        {
            List<Product> products = new List<Product>();
            //List<Article> articles = new List<Article>();
            if (giveDiscountAllProducts)
            {
                products = productService.GetProducts().ToList();
            }
            else
            {
                products = productService.GetProducts().Where(p => p.DiscountPercent == 0).ToList();
            }

            foreach (var product in products)
            {
                product.DiscountPercent = numberDiscount;
                UpdateProduct(product.Id);
            }
            return RedirectToAction("AllDiscount");
        }

        //уточнить с борей
        //[HttpPost]
        //public ActionResult DoDiscount(List<long> discounts, long productId, bool isDiscountPrice, double numberDiscount = 0)
        //{
        //    //if (numberDiscount != 0)
        //    //{
        //    double discount = (numberDiscount / 100);
        //    foreach (var d in discounts)
        //    {
        //        Article articleEntity = articleService.GetArticle(d);
        //        articles.Add(articleEntity);
        //    }
        //    Product product = productService.GetProduct(productId);
        //
        //    foreach (var a in articles)
        //    {
        //        //double discountPrice = 0;
        //        if (isDiscountPrice)
        //        {
        //            a.DiscountPercent = (1 - (numberDiscount / a.Price)) * 100;
        //        }
        //        else
        //        {
        //            a.DiscountPercent = numberDiscount;
        //        }
        //
        //
        //        articleService.UpdateArticle(a);
        //        UpdateProduct(product.Id);
        //    }
        //    return RedirectToAction("Articles", new { productId = productId });
        //}
        //
        //[HttpPost]
        //public ActionResult DeleteDiscount(long productId)
        //{
        //    Product product = productService.GetProduct(productId);
        //    List<Article> articles = articleService.GetArticles().Where(a => a.ProductId == productId).ToList();
        //    foreach (var a in articles)
        //    {
        //        a.DiscountPercent = 0;
        //        articleService.UpdateArticle(a);
        //    }
        //    product.IsProductDiscount = false;
        //    UpdateProduct(product.Id);
        //
        //    return RedirectToAction("Articles", new { productId = productId });
        //}

        [HttpGet]
        public IActionResult Articles(long productId, string returnurl)
        {
            List<ArticleViewModel> model = new List<ArticleViewModel>();
            var product = productService.GetProduct(productId);
            List<Image> images = imageService.GetImages().Where(i => i.ProductId == product.Id).ToList();
            articleService.GetArticles().Where(p => p.ProductId == productId).ToList().ForEach(p =>
            {
                ArticleViewModel article = new ArticleViewModel
                {
                    Id = p.Id,
                    Name = p.Name,
                    Price = p.Price,
                    AddedDate = p.AddedDate,
                    ModifiedDate = p.ModifiedDate,
                    StockStatus = p.StockStatus,
                    Product = p.Product,
                    Quantity = p.Quantity
                };
                foreach (var i in images)
                {
                    if (i.Name.Remove(i.Name.LastIndexOf('.')) == article.Name) article.ImagePath = i.ImgPath;
                }
                model.Add(article);
            });
            ViewBag.Product = product;
            ViewBag.ReturnUrl = returnurl;

            return View(model.OrderBy(a => a.Name));
        }

        [HttpGet]
        public ActionResult AddArticle(long productId, string returnurl)
        {
            ArticleViewModel model = new ArticleViewModel();
            var product = productService.GetProduct(productId);
            var category = categoryService.GetCategory(product.CategoryId);
            var stockStatuses = stockStatusService.GetStockStatuses().OrderBy(c => c.Name).Select(c => new { c.Id, Value = c.Name });
            var properties = propertyService.GetProperties().ToList();
            model.StockStatuses = new SelectList(stockStatuses, "Id", "Value");
            model.Properties = new SelectList(properties, "Id", "Name");
            //model.PropertyValues = new SelectList(propertyValues, "Id", "Value");
            model.Product = product;
            ViewBag.ReturnUrl = returnurl;
            model.Colors = colorService.GetColors().ToList();

            return PartialView("_AddArticle", model);
        }

        [HttpPost]
        public async Task<ActionResult> AddArticle(ArticleViewModel model, List<long> propertyValues, string returnurl, IFormFile uploadedFile, long colorId)
        {
            Article articleEntity = new Article
            {
                Name = model.Name,
                AddedDate = DateTime.Now,
                ModifiedDate = DateTime.Now,
                Quantity = model.Quantity,
                Price = model.Price,
                StockStatusId = model.Quantity > 0 ? stockStatusService.GetStockStatuses().SingleOrDefault(ss=> ss.Name =="В наличии").Id : stockStatusService.GetStockStatuses().SingleOrDefault(ss => ss.Name == "Нет в наличии").Id, //model.StockStatusId,
                ProductId = model.ProductId,
                ProductName = model.ProductName,
                ColorId = colorId,
            };

            long categoryId = productService.GetProduct(model.ProductId).CategoryId;

            if (ModelState.IsValid)
            {
                //Проверка цвета
                //Color color = new Color
                //{
                //    Name = model.ColorName,
                //    AddedDate = DateTime.Now,
                //    ModifiedDate = DateTime.Now,
                //    ColorCode = model.ColorCode
                //};
                //if (!colorService.IsColorExist(color))
                //{
                //    colorService.InsertColor(color);
                //    articleEntity.ColorId = color.Id;
                //}
                //else
                //    articleEntity.ColorId = colorService.GetColors().SingleOrDefault(c => c.Name == color.Name && c.ColorCode == color.ColorCode).Id;

                //Работа с артикулом
                articleService.InsertArticle(articleEntity);
                if (articleEntity.Id > 0)
                {
                    List<PropertyValCatArt> pvcaToadd = new List<PropertyValCatArt>();
                    foreach (var pv in propertyValues)
                    {
                        PropertyValCatArt articlePropertyValCat = new PropertyValCatArt()
                        {
                            ArticleId = articleEntity.Id,
                            ProductId = articleEntity.ProductId,
                            CategoryId = categoryId,
                            PropertyValueId = pv
                        };
                        pvcaToadd.Add(articlePropertyValCat);
                    }
                    propertyValCatArtService.InsertSome(pvcaToadd);
                    UpdateProduct(model.ProductId);

                    //загрузка картинки
                    if (uploadedFile != null)
                    {
                        Product product = productService.GetProduct(articleEntity.ProductId);
                        string fileName = articleEntity.Name + uploadedFile.FileName.Substring(uploadedFile.FileName.LastIndexOf('.'));//uploadedFile.FileName.Substring(uploadedFile.FileName.LastIndexOf('\\') + 1);
                        string path = "/Files/" + product.Name + "/" + fileName;//"/Files/" + product.Name + "/"+ articleEntity.Name + "-" + fileName;
                                                                                // сохраняем файл в папку Files в каталоге wwwroot
                        using (var fileStream = new FileStream(product.Path + "/" + fileName, FileMode.Create))
                        {
                            await uploadedFile.CopyToAsync(fileStream);
                        }
                        Image image = new Image
                        {
                            Name = fileName,//uploadedFile.FileName,
                            ImgPath = path,
                            ProductId = product.Id,
                            AddedDate = DateTime.Now,
                            ModifiedDate = DateTime.Now,
                            Product = product,
                            MainImg = false
                        };

                        imageService.InsertImage(image);
                    }


                    return RedirectToAction("Articles", new { productId = articleEntity.ProductId, returnurl });
                }
            }
            return PartialView("_AddArticle", model);
        }

        [HttpGet]
        public JsonResult GetPropertyValues(long propertyId)
        {
            List<PropertyValue> propertyValues = propertyValueService.GetPropertyValues().Where(pv => pv.PropertyId == propertyId).Select(pv => new PropertyValue { Id = pv.Id, Value = pv.Value }).OrderBy(pv => pv.Value).ToList();
            return Json(propertyValues, new JsonSerializerOptions() { Encoder = JavaScriptEncoder.Create(UnicodeRanges.BasicLatin, UnicodeRanges.Cyrillic) });
        }

        [HttpGet]
        public JsonResult GetColors()
        {
            List<Color> colors = colorService.GetColors().Select(c => new Color { Id = c.Id, ColorCode = c.ColorCode, Name = c.Name }).ToList();
            return Json(colors, new JsonSerializerOptions() { Encoder = JavaScriptEncoder.Create(UnicodeRanges.BasicLatin, UnicodeRanges.Cyrillic) });
        }


        [HttpGet]
        public ActionResult EditArticle(long? id, long productId, string returnurl)
        {
            ArticleViewModel model = new ArticleViewModel();
            if (id.HasValue && id != 0)
            {
                Article articleEntity = articleService.GetArticle(id.Value);
                model.Id = articleEntity.Id;
                model.Name = articleEntity.Name;
                model.Price = articleEntity.Price;
                model.Quantity = articleEntity.Quantity;
                //model.StockStatusId = articleEntity.StockStatusId;
                model.Quantity = articleEntity.Quantity;
                model.ColorId = articleEntity.ColorId;

                var product = productService.GetProduct(productId);
                var category = categoryService.GetCategory(product.CategoryId);
                //var stockStatuses = stockStatusService.GetStockStatuses().OrderBy(c => c.Name).Select(c => new { c.Id, Value = c.Name });
                var properties = propertyService.GetProperties().ToList();//category.CategoryProperties.Select(p => p.Property).OrderBy(c => c.Name).ToList();
                var propertyValues = propertyValueService.GetPropertyValues().OrderBy(c => c.Value);

                //model.StockStatuses = new SelectList(stockStatuses, "Id", "Value");
                model.AllProperties = properties;
                model.Product = product;
                model.PropertyValuesChecked = new List<PropertyValue>();

                List<PropertyValCatArt> checkedProperties = propertyValCatArtService.GetAll().Where(cp => cp.ArticleId == model.Id).ToList();

                List<Property> listProp = new List<Property>();
                foreach (var cp in checkedProperties)
                {
                    var propVal = propertyValueService.GetPropertyValue(cp.PropertyValueId);
                    var prop = propertyService.GetProperties().SingleOrDefault(p => p.Id == propVal.PropertyId);
                    if (!listProp.Contains(prop))
                        listProp.Add(prop);
                    model.PropertyValuesChecked.Add(propVal);
                }
                model.PropertyChecked = listProp;
                model.Colors = colorService.GetColors().ToList();

                //Color color = colorService.GetColor(model.ColorId);
                //model.ColorName = color.Name;
                //model.ColorCode = color.ColorCode;
            }
            ViewBag.ReturnUrl = returnurl;
            return PartialView("_EditArticle", model);
        }

        
        [AcceptVerbs("Get", "Post")]
        public IActionResult CheckArticleNames(string name, long id)
        {
            List<Article> articleNames = articleService.GetArticles().ToList();
            string oldArticleName = "";
            if (id > 0)
            {
                Article article = articleNames.SingleOrDefault(a => a.Id == id);
                oldArticleName = article.Name;
                articleNames.Remove(article);
            }
            if (oldArticleName == name) return Json(true);

            if (!articleNames.Select(a => a.Name).Contains(name))
            {
                return Json(true);
            }
            else
            {
                return Json(false);
            }

        }

        [HttpPost]
        public async Task<ActionResult> EditArticle(ArticleViewModel model, List<long> propertyValues, string returnurl, long colorId, IFormFile uploadedFile)
        {
            Article articleEntity = articleService.GetArticle(model.Id);

            //Имя для картинки пряжи
            string oldArticleName = articleEntity.Name;

            articleEntity.Name = model.Name;
            articleEntity.Price = model.Price;
            articleEntity.StockStatusId = model.Quantity > 0 ? stockStatusService.GetStockStatuses().SingleOrDefault(ss => ss.Name == "В наличии").Id : stockStatusService.GetStockStatuses().SingleOrDefault(ss => ss.Name == "Нет в наличии").Id;//model.StockStatusId;
            articleEntity.ModifiedDate = DateTime.Now.Date;
            articleEntity.Quantity = model.Quantity;
            articleEntity.ColorId = colorId;
            
            if (ModelState.IsValid && articleEntity.Id > 0)
                //if (ModelState.IsValid )
            {
                //Color color = new Color
                //{
                //    Name = colorName,
                //    AddedDate = DateTime.Now,
                //    ModifiedDate = DateTime.Now,
                //    ColorCode = colorCode
                //};
                //if (!colorService.IsColorExist(color))
                //    colorService.InsertColor(color);
                //else
                //{
                //    color.Id = colorService.GetColors().SingleOrDefault(c => c.Name == color.Name && c.ColorCode == color.ColorCode).Id;
                //    if (color.Id != articleEntity.ColorId) articleEntity.ColorId = color.Id;
                //}

                articleService.UpdateArticle(articleEntity);

                List<PropertyValCatArt> propertyValCatArtsOld = propertyValCatArtService.GetAll().Where(pv => pv.ArticleId == articleEntity.Id).ToList();
                List<PropertyValCatArt> propertyValCatArtsNewToAdd = new List<PropertyValCatArt>();

                long categoryId = productService.GetProduct(model.ProductId).CategoryId;

                foreach (var pv in propertyValues)
                {
                    PropertyValCatArt articlePropertyValCat = new PropertyValCatArt()
                    {
                        PropertyValueId = pv,
                        ArticleId = articleEntity.Id,
                        ProductId = articleEntity.ProductId,
                        CategoryId = categoryId,
                    };
                    if (propertyValCatArtsOld.Exists(a =>
                        a.ArticleId == articlePropertyValCat.ArticleId &&
                        a.CategoryId == articlePropertyValCat.CategoryId &&
                        a.ProductId == articlePropertyValCat.ProductId &&
                        a.PropertyValueId == articlePropertyValCat.PropertyValueId))
                    {
                        PropertyValCatArt pvca = propertyValCatArtsOld.SingleOrDefault(a =>
                            a.ArticleId == articlePropertyValCat.ArticleId &&
                            a.CategoryId == articlePropertyValCat.CategoryId &&
                            a.ProductId == articlePropertyValCat.ProductId &&
                            a.PropertyValueId == articlePropertyValCat.PropertyValueId);
                        propertyValCatArtsOld.Remove(pvca);
                    }
                    else
                    {
                        propertyValCatArtsNewToAdd.Add(articlePropertyValCat);
                    }
                }
                if (propertyValCatArtsOld.Count != 0) propertyValCatArtService.DeleteSome(propertyValCatArtsOld);
                if (propertyValCatArtsNewToAdd.Count != 0) propertyValCatArtService.InsertSome(propertyValCatArtsNewToAdd);

                UpdateProduct(model.ProductId);

                //Замена картинки
                if (uploadedFile != null)
                {
                    //удаление старой картинки
                    Image oldImage = imageService.GetImages().SingleOrDefault(i =>
                        i.ProductId == model.ProductId &&
                        (i.Name.Remove(i.Name.LastIndexOf(".")) == oldArticleName)
                    );
                    if (oldImage != null)
                    {
                        long productId = oldImage.ProductId;
                        FileInfo fi = new FileInfo(_appEnvironment.WebRootPath + "/" + oldImage.ImgPath);
                        int imagesCount = imageService.GetImages().Where(i => i.ProductId == productId).Count();
                        fi.Delete();
                        if (oldImage.MainImg)
                        {
                            imageService.DeleteImage(oldImage.Id);
                            if (imagesCount > 1)
                            {
                                Image imageFirst = imageService.GetImages().FirstOrDefault(i => i.ProductId == productId);
                                imageFirst.MainImg = true;
                                imageService.UpdateImage(imageFirst);
                            }
                        }
                        else
                        {
                            imageService.DeleteImage(oldImage.Id);
                        }
                    }

                    //загрузка новой картинки
                    Product product = productService.GetProduct(articleEntity.ProductId);
                    string fileName = articleEntity.Name + uploadedFile.FileName.Substring(uploadedFile.FileName.LastIndexOf('.'));//uploadedFile.FileName.Substring(uploadedFile.FileName.LastIndexOf('\\') + 1);
                    string path = "/Files/" + product.Name + "/" + fileName;//"/Files/" + product.Name + "/"+ articleEntity.Name + "-" + fileName;
                    // сохраняем файл в папку Files в каталоге wwwroot
                    using (var fileStream = new FileStream(product.Path + "/" + fileName, FileMode.Create))
                    {
                        await uploadedFile.CopyToAsync(fileStream);
                    }
                    Image image = new Image
                    {
                        Name = fileName,//uploadedFile.FileName,
                        ImgPath = path,
                        ProductId = product.Id,
                        AddedDate = DateTime.Now,
                        ModifiedDate = DateTime.Now,
                        Product = product,
                        MainImg = false
                    };

                    imageService.InsertImage(image);

                    //Проверка на наличие главной картинки
                    List<bool> imagesMain = imageService.GetImages().Where(p => p.ProductId == product.Id).Select(a => a.MainImg).ToList();
                    bool mainImgExist = false;
                    if (imagesMain.Contains(true))
                    {
                        mainImgExist = true;
                    }

                    //Если нет, то сделать первую картинку главной
                    if (!mainImgExist)
                    {
                        image.MainImg = true;
                        imageService.UpdateImage(image);
                    }
                }

                return RedirectToAction("Articles", new { productId = articleEntity.Product.Id, returnurl });
            }
            return PartialView("_EditArticle", model);
        }


        [HttpGet]
        public PartialViewResult DeleteArticle(int? id, string returnurl)
        {
            ArticleViewModel model = new ArticleViewModel();
            if (id.HasValue && id != 0)
            {
                Article articleEntity = articleService.GetArticle(id.Value);
                model.Name = articleEntity.Name;
                model.Product = productService.GetProduct(articleEntity.ProductId);
                model.ProductId = articleEntity.ProductId;
            }
            ViewBag.ReturnUrl = returnurl;
            return PartialView("_DeleteArticle", model);
        }

        [HttpPost]
        public ActionResult DeleteArticle(Article model, string returnurl)
        {
            string imageName = model.Name;
            long productId = model.ProductId;

            articleService.DeleteArticle(model.Id);
            UpdateProduct(model.ProductId);

            //Удаление картинки
            Image oldImage = imageService.GetImages().SingleOrDefault(i =>
                i.ProductId == model.ProductId &&
                (i.Name.Remove(i.Name.LastIndexOf(".")) == imageName)
            );
            if (oldImage != null)
            {
                FileInfo fi = new FileInfo(_appEnvironment.WebRootPath + "/" + oldImage.ImgPath);
                int imagesCount = imageService.GetImages().Where(i => i.ProductId == productId).Count();
                fi.Delete();
                if (oldImage.MainImg)
                {
                    imageService.DeleteImage(oldImage.Id);
                    if (imagesCount > 1)
                    {
                        Image imageFirst = imageService.GetImages().FirstOrDefault(i => i.ProductId == productId);
                        imageFirst.MainImg = true;
                        imageService.UpdateImage(imageFirst);
                    }
                }
                else
                {
                    imageService.DeleteImage(oldImage.Id);
                }
            }
            
            return RedirectToAction("Articles", new { productId = model.ProductId, returnurl });
        }


        

        //Идет дозапись (удалить старые строки категорий и продуктов)
        public void UpdateYML()
        {
            string newYML = "";
            using (StreamReader sr = new StreamReader(_appEnvironment.WebRootPath + "/YMLFile.xml", System.Text.Encoding.Default))
            {
                string line;
                while ((line = sr.ReadLine()) != null)
                {
                    newYML += line + Environment.NewLine;
                }
            }
            newYML = "<yml_catalog date='" + DateTime.Today + "'>" + Environment.NewLine +
                "<shop>" + Environment.NewLine +
                "   <name>Набитый Комод</name>" + Environment.NewLine +
                "   <company>Набитый Комод</company>" + Environment.NewLine +
                "   <url>https://komod-tlt.ru</url>" + Environment.NewLine +
                "   <currencies>" + Environment.NewLine +
                "       <currency id='RUR' rate='1'/>" + Environment.NewLine +
                "   </currencies>" + Environment.NewLine +
                "   <categories>" + Environment.NewLine;


            //Добавить категории
            List<Category> categories = categoryService.GetCategories().ToList();
            foreach (var c in categories)
            {
                newYML += "     <category id='" + c.Id + "'>" + c.Name + "</category>" + Environment.NewLine;
            }
            newYML += " </categories>" + Environment.NewLine +
                "   <delivery-options>" + Environment.NewLine +
                "       <option cost='300' days='2-5' order-before='12'/>" + Environment.NewLine +
                "   </delivery-options>" + Environment.NewLine +
                "   <offers>" + Environment.NewLine;


            //Добавить продукты
            List<Product> products = productService.GetProducts().Where(p => p.InStock && p.Articles.Count > 0).ToList();
            foreach (var product in products)
            {
                newYML += "     <offer id='" + product.Id + "' type='vendor.model'>" + Environment.NewLine +
                    "           <typePrefix>" + product.Category.Name + "</typePrefix>" + Environment.NewLine +
                    "           <vendor>" + product.Brand.Name + "</vendor>" + Environment.NewLine +
                    "           <model>" + product.Name + "</model>" + Environment.NewLine +
                    "           <url>https://komod-tlt.ru/Home/Product/" + product.Id + "</url>" + Environment.NewLine;

                if (product.DiscountPercent > 0)
                {
                    newYML +=
                    "           <price from='true'>" + product.MinProductPrice + "</price>" + Environment.NewLine;
                }
                else
                {
                    newYML += "         <price from='true'>" + product.MinProductPrice + "</price>" + Environment.NewLine;
                }

                string mainImgPath = imageService.GetImages().SingleOrDefault(i => i.MainImg == true && i.ProductId == product.Id).ImgPath;

                newYML += "         <currencyId>RUR</currencyId>" + Environment.NewLine +
                    "           <categoryId>" + product.CategoryId + "</categoryId>" + Environment.NewLine +
                    "           <picture>https://komod-tlt.ru" + mainImgPath + "</picture>" + Environment.NewLine +
                    "           <store>true</store>" + Environment.NewLine +
                    "           <pickup>true</pickup>" + Environment.NewLine +
                    "           <delivery>true</delivery>" + Environment.NewLine +
                    "           <delivery-options>" + Environment.NewLine +
                    "              <option cost= '200' days='2-5' order-before='12'/>" + Environment.NewLine +
                    "           </delivery-options>" + Environment.NewLine +
                    "           <description>" + Environment.NewLine +
                    "           <![CDATA[" + Environment.NewLine +
                    "             <h3>" + product.Category.Name + " " + product.Name + " " + product.Brand.Name + "</h3>" + Environment.NewLine +
                    "             <p>" + product.Description + "</p>" + Environment.NewLine +
                    "           ]]>" + Environment.NewLine +
                    "           </description>" + Environment.NewLine;

                List<PropertyValCatArt> pv = propertyValCatArtService.GetAll().Where(ap => ap.Article.ProductId == product.Id).ToList();
                foreach (var pve in pv)
                {
                    PropertyValue propertyValue = propertyValueService.GetPropertyValue(pve.PropertyValueId);
                    newYML += "          <param name='" + propertyValue.Property.Name + "'>" + Environment.NewLine +
                                    propertyValue.Value + Environment.NewLine +
                    "           </param>" + Environment.NewLine;
                }

                newYML += "           <manufacturer_warranty>true</manufacturer_warranty>" + Environment.NewLine +
                "     </offer>" + Environment.NewLine;
            }

            newYML +=
                    "   </offers>" + Environment.NewLine +
                    "</shop>" + Environment.NewLine +
                    "</yml_catalog> " + Environment.NewLine;

            using (StreamWriter sw = new StreamWriter(_appEnvironment.WebRootPath + "/YMLFile.xml", false, System.Text.Encoding.Default))
            {
                sw.Write(newYML);
            }
        }


        private void UpdateProduct(long productId)
        {
            Product product = productService.GetProduct(productId);
            StockStatus stockStatus = stockStatusService.GetStockStatuses().SingleOrDefault(ss => ss.Name == "В наличии");

            var articleCount = 0;
            List<Article> articles = articleService.GetArticles().Where(a => a.ProductId == product.Id).ToList();
            if (articles.Count == articleCount)
            {
                product.InStock = false;
                product.MinProductPrice = 0;
                product.MaxProductPrice = 0;
                productService.UpdateProduct(product);
                return;
            }
            decimal minPrice = articles[0].Price;
            decimal maxPrice = articles[0].Price;
            foreach (var article in articles)
            {
                if (article.Quantity > 0)
                {
                    if (minPrice > article.Price) minPrice = article.Price;
                    if (maxPrice < article.Price) maxPrice = article.Price;
                    articleCount++;
                }
            }

            if (articleCount != 0)
            {
                //Обновить Sitemap, если продукта нет (product.IsProductInStock был равен false) добавить его
                string sitemap = "";
                using (StreamReader sr = new StreamReader(_appEnvironment.WebRootPath + "/Sitemap.txt", System.Text.Encoding.Default))
                {
                    string line;
                    while ((line = sr.ReadLine()) != null)
                    {
                        sitemap += line + Environment.NewLine;
                    }
                }
                string addString = "https://komod-tlt.ru/Home/Product/" + product.Id + Environment.NewLine;
                string addStringCatalog = "https://komod-tlt.ru/Home/Catalog?categoryId=" + product.CategoryId + Environment.NewLine;
                if (!sitemap.Contains(addString))
                {
                    using (StreamWriter sw = new StreamWriter(_appEnvironment.WebRootPath + "/Sitemap.txt", true, System.Text.Encoding.Default))
                    {
                        sw.Write(addString);
                    }
                }
                if (!sitemap.Contains(addStringCatalog))
                {
                    using (StreamWriter sw = new StreamWriter(_appEnvironment.WebRootPath + "/Sitemap.txt", true, System.Text.Encoding.Default))
                    {
                        sw.Write(addStringCatalog);
                    }
                }

                product.MinProductPrice = minPrice;
                product.MaxProductPrice = maxPrice;
                product.InStock = true;
            }
            else
            {
                //Обновить Sitemap, если продукт был (product.IsProductInStock был равен true) удалить его
                string sitemap = "";
                using (StreamReader sr = new StreamReader(_appEnvironment.WebRootPath + "/Sitemap.txt", System.Text.Encoding.Default))
                {
                    string line;
                    while ((line = sr.ReadLine()) != null)
                    {
                        sitemap += line + Environment.NewLine;
                    }
                }
                string addString = "https://komod-tlt.ru/Home/Product/" + product.Id + Environment.NewLine;
                if (sitemap.Contains(addString))
                {
                    using (StreamWriter sw = new StreamWriter(_appEnvironment.WebRootPath + "/Sitemap.txt", false, System.Text.Encoding.Default))
                    {
                        sw.Write(sitemap.Replace(addString, ""));
                    }
                }

                product.InStock = false;
                product.MinProductPrice = 0;
                product.MaxProductPrice = 0;
                product.DiscountPercent = 0;
            }

            productService.UpdateProduct(product);
            return;
        }

        /*public void UpdateDatabase()
        {
                List<StockStatusArticle> stockStatuses = stockStatusService.GetStockStatuses().ToList();
                StockStatusArticle stockStatus = new StockStatusArticle();
                foreach (var st in stockStatuses)
                {
                    if (st.Name == "В наличии")
                    {
                        stockStatus = st;
                        break;
                    }
                }

            List<Product> products = productService.GetProducts().ToList();
            foreach(Product product in products)
            {

                var articleCount = 0;
                List<Article> articles = articleService.GetArticles().Where(a => a.ProductId == product.Id).ToList();
                if (articles.Count == articleCount)
                {
                    product.IsProductInStock = false;
                    product.MinProductPrice = 0;
                    product.MaxProductPrice = 0;
                    product.IsProductDiscount = false;
                    productService.UpdateProduct(product);
                    return;
                }
                double minPrice = articles[0].Price;
                double maxPrice = articles[0].Price;
                bool isDiscount = false;
                foreach (var article in articles)
                {
                    if (article.StockStatusArticleId != stockStatus.Id || !(article.Count > 0))
                    {
                        continue;
                    }
                    if (article.DiscountPercent > 0)
                    {
                        var price = article.Price * (1 - article.DiscountPercent);
                        if (minPrice > price) minPrice = price;
                        if (maxPrice < price) maxPrice = price;
                        isDiscount = true;
                    }
                    else
                    {
                        if (minPrice > article.Price) minPrice = article.Price;
                        if (maxPrice < article.Price) maxPrice = article.Price;
                    }
                    articleCount++;
                }

                if (articleCount != 0)
                {
                    product.MinProductPrice = minPrice;
                    product.MaxProductPrice = maxPrice;
                    product.IsProductDiscount = isDiscount;
                    product.IsProductInStock = true;
                }
                else
                {
                    product.IsProductInStock = false;
                    product.MinProductPrice = 0;
                    product.MaxProductPrice = 0;
                    product.IsProductDiscount = false;
                }

                //Проверка на наличие главной картинки
                List<Image> images = imageService.GetImages().Where(i=>i.ProductId == product.Id).ToList();
                if (images.Count > 0)
                {
                    bool mainImgExist = false;
                    foreach (var img in images)
                        if (img.MainImg)
                        {
                            mainImgExist = true;
                            product.MainImgPath = img.Path;
                            break;
                        }
                    //Если нет, то сделать первую картинку главной
                    if (!mainImgExist)
                    {
                        images[0].MainImg = true;
                        product.MainImgPath = images[0].Path;
                    }

                }
                else product.MainImgPath = null;
                productService.UpdateProduct(product);
            }
        }*/


        /*
        //Идет дозапись (удалить старые строки категорий и продуктов)
        public void UpdateSitemap()
        {
            string newSitemap = "";
            using (StreamReader sr = new StreamReader(_appEnvironment.WebRootPath + "/Sitemap.txt", System.Text.Encoding.Default))
            {
                string line;
                while ((line = sr.ReadLine()) != null)
                {
                    newSitemap += line + Environment.NewLine;
                }
            }

            //Добавить категории
            List<Category> categories = categoryService.GetCategories().ToList();
            foreach(var c in categories)
            {
                newSitemap += "https://komod-tlt.ru/Home/Catalog?categoryId=" + c.Id + Environment.NewLine;
            }
            //Добавить продукты
            List<Product> products = productService.GetProducts().Where(p => p.IsProductInStock && p.Articles.Count > 0).ToList();
            foreach (var product in products)
            {
                newSitemap += "https://komod-tlt.ru/Home/Product/" + product.Id + Environment.NewLine;
            }

            using (StreamWriter sw = new StreamWriter(_appEnvironment.WebRootPath + "/Sitemap.txt", false, System.Text.Encoding.Default))
            {
                sw.Write(newSitemap);
            }
        }*/





        [HttpGet]
        public IActionResult ProductSets(int page = 1, int sortType = 0, string searchString = null)
        {
            IEnumerable<ProductSet> productSets;
            if (searchString == null)
            {
                productSets = productSetService.GetProductSets();
            }
            else
            {
                searchString = searchString.ToUpper();
                productSets = productSetService.GetProductSets().Where(s => s.ProductSetName.ToUpper().Contains(searchString)
                );
            }

            //int pageSize = 20;
            //
            List<ProductSetViewModel> listProductSets = new List<ProductSetViewModel>();
            //
            //var count = productSets.Count();
            //var items = productSets.Skip((page - 1) * pageSize).Take(pageSize).ToList();
            //PageViewModel pageViewModel = new PageViewModel(count, page, pageSize);
            productSets.ToList().ForEach(u =>
            {
                ProductSetViewModel productSet = new ProductSetViewModel
                {
                    Id = u.Id,
                    DiscounPercent = u.DiscounPercent,
                    ProductsInAdmin = productService.GetProducts().Where(p=> u.ProductSetName.Contains(p.Name)).ToList()

                };
                //List<Product> products = productService.GetProducts().Where(p => u.ProductSetName.Contains(p.Name)).ToList();


                listProductSets.Add(productSet);
            });

            //ProductSetsViewModel viewModel = new ProductSetsViewModel
            //{
            //    PageViewModel = pageViewModel,
            //    ProductSets = listProductSets
            //};

            return View(listProductSets);
        }

        [HttpGet]
        public ActionResult AddProductSet()
        {
            ProductSetViewModel model = new ProductSetViewModel();
            ViewBag.AllProducts = productService.GetProducts().ToList();

            return PartialView("_AddProductSet", model);
        }

        [HttpPost]
        public ActionResult AddProductSet(ProductSetViewModel model, List<long> productIds)
        {
            ProductSet productSetEntity = new ProductSet
            {
                DiscounPercent = model.DiscounPercent
            };
            List<Product> product = productService.GetProducts().Where(p => productIds.Contains(p.Id)).ToList();
            foreach(var p in product)
            {
                productSetEntity.ProductSetName += p.Name + "||";
            }
            if (ModelState.IsValid)
            {
                productSetService.InsertProductSet(productSetEntity);
                if (productSetEntity.Id > 0)
                {
                    return RedirectToAction("ProductSets");
                }
            }
            return PartialView("_AddProductSet", model);
        }

        public ActionResult EditProductSet(int? id)
        {
            ProductSetViewModel model = new ProductSetViewModel();
            if (id.HasValue && id != 0)
            {
                ProductSet productSetEntity = productSetService.GetProductSet(id.Value);
                model.DiscounPercent = productSetEntity.DiscounPercent;
                model.ProductsInAdmin = productService.GetProducts().Where(p => productSetEntity.ProductSetName.Contains(p.Name)).ToList();
            }
            ViewBag.AllProducts = productService.GetProducts().ToList();
            return PartialView("_EditProductSet", model);
        }

        [HttpPost]
        public ActionResult EditProductSet(ProductSetViewModel model, List<long> productIds)
        {
            ProductSet productSetEntity = productSetService.GetProductSet(model.Id);
            productSetEntity.DiscounPercent = model.DiscounPercent;
            List<Product> product = productService.GetProducts().Where(p => productIds.Contains(p.Id)).ToList();
            foreach (var p in product)
            {
                productSetEntity.ProductSetName += p.Name + "||";
            }
            if (ModelState.IsValid)
            {
                productSetService.UpdateProductSet(productSetEntity);
                if (productSetEntity.Id > 0)
                {
                    return RedirectToAction("ProductSets");
                }
            }
            return PartialView("_EditProductSet", model);
        }

        [HttpGet]
        public PartialViewResult DeleteProductSet(long? id)
        {
            ProductSetViewModel model = new ProductSetViewModel();
            if (id.HasValue && id != 0)
            {
                ProductSet productSetEntity = productSetService.GetProductSet(id.Value);
                ViewBag.ProductSetNames = productSetEntity.ProductSetName;
            }
            return PartialView("_DeleteProductSet", model);
        }

        [HttpPost]
        public ActionResult DeleteProductSet(long id)
        {
            productSetService.DeleteProductSet(id);
            return RedirectToAction("ProductSets");
        }


        
        [AcceptVerbs("Get", "Post")]
        public IActionResult CheckProductName(string Name)
        {
            var product = productService.GetProducts().SingleOrDefault(u => u.Name == Name);

            return Json(product == null ? true : false);
        }
    }
}