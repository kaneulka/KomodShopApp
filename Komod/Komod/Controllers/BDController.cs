using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Komod.Data;
using Komod.Ser.ArticleSer;
using Komod.Ser.BrandSer;
using Komod.Ser.CategorySer;
using Komod.Ser.ColorSer;
using Komod.Ser.ImageSer;
using Komod.Ser.CountrySer;
using Komod.Ser.ProductSer;
using Komod.Ser.PropertySer;
using Komod.Ser.PropertyValCatArtSer;
using Komod.Ser.PropertyValueSer;
using Komod.Ser.StockStatusSer;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NPOI.HSSF.UserModel;
using NPOI.SS.Formula.Functions;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using Microsoft.AspNetCore.Authorization;

namespace Komod.Web.Controllers
{
    [Authorize(Roles = "admin")]
    public class BDController : Controller
    {
        private readonly IProductService productService;
        private readonly IArticleService articleService;
        private readonly ICategoryService categoryService;
        private readonly IColorService colorService;
        private readonly IBrandService brandService;
        private readonly IPropertyService propertyService;
        private readonly IPropertyValueService propertyValueService;
        private readonly IPropertyValCatArtService propertyValCatArtService;
        private readonly IStockStatusService stockStatusService;
        private readonly IImageService imageService;
        private readonly ICountryService countryService;
        IWebHostEnvironment _appEnvironment;

        public BDController(
            IProductService productService,
            IArticleService articleService,
            ICategoryService categoryService,
            IBrandService brandService,
            IPropertyService propertyService,
            IPropertyValueService propertyValueService,
            IPropertyValCatArtService propertyValCatArtService,
            IStockStatusService stockStatusService,
            IImageService imageService,
            IWebHostEnvironment appEnvironment,
            IColorService colorService,
            ICountryService countryService
        )
        {
            _appEnvironment = appEnvironment;
            this.productService = productService;
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

        public IActionResult UploadDB()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> UploadDB (IFormFile uploadedFile)
        {
            if (uploadedFile != null)
            {
                Color color = colorService.GetColors().FirstOrDefault();


                // сохраняем файл в папку Files в каталоге wwwroot
                string path = _appEnvironment.WebRootPath + "/Files/BDUpdateXLS";// +  uploadedFile.FileName;
                using (var fileStream = new FileStream(path, FileMode.Create))
                {
                    await uploadedFile.CopyToAsync(fileStream);
                }
                using (var stream = new FileStream(path, FileMode.Create))
                {
                    string sFileExtension = Path.GetExtension(uploadedFile.FileName).ToLower();
                    ISheet sheet;
                    uploadedFile.CopyTo(stream);
                    stream.Position = 0;
                    if (sFileExtension == ".xls")
                    {
                        HSSFWorkbook hssfwb = new HSSFWorkbook(stream); //This will read the Excel 97-2000 formats  
                        sheet = hssfwb.GetSheetAt(0); //get first sheet from workbook  
                    }
                    else
                    {
                        XSSFWorkbook hssfwb = new XSSFWorkbook(stream); //This will read 2007 Excel format  
                        sheet = hssfwb.GetSheetAt(0); //get first sheet from workbook   
                    }
                    var i = 0;//Какая строка
                    var ending = sheet.PhysicalNumberOfRows; //Число строк
                    //StockStatusArticle stockStatusAvailable = stockStatusService.GetStockStatuses().SingleOrDefault(ss => ss.Name == "В наличии");//Модель есть ли в налиции
                    //StockStatusArticle stockStatusNotAvailable = stockStatusService.GetStockStatuses().SingleOrDefault(ss => ss.Name == "Нет в наличии");//Модель нет ли в наличии

                    List<Product> products = productService.GetProducts().ToList();
                    List<Article> articles = articleService.GetArticles().ToList();
                    List<StockStatus> stockStatuses = stockStatusService.GetStockStatuses().ToList();
                    List<Property> properties = propertyService.GetProperties().ToList();
                    List<PropertyValue> propertyValues = propertyValueService.GetPropertyValues().ToList();
                    List<Category> categories = categoryService.GetCategories().ToList();
                    List<Brand> brands = brandService.GetBrands().ToList();

                    List<PropertyValCatArt> lpvca = propertyValCatArtService.GetAll().Where(p=> products.Select(prd=> prd.Id).Contains(p.ProductId) ).ToList();
                    do
                    {
                        i++;
                        IRow row = sheet.GetRow(i);
                        string articleName = "";
                        if (i < ending)//Если строк больше нет, то закончит цикл
                        {
                            articleName = row.GetCell(0).ToString();
                            if (articleName == "Итого" || articleName == "") break;
                        }
                        else
                        {
                            break;
                        }
                        string articlePrice = row.GetCell(1).ToString();            //  +
                        string articleStockStatus = row.GetCell(2).ToString();      //  +
                        string articleQuantity = row.GetCell(3).ToString();         //  +
                        string categoryName = row.GetCell(4).ToString();            //  +
                        string parentCategoryName = row.GetCell(5).ToString();      //  +
                        string productName = row.GetCell(6).ToString();             //  +
                        string productPath = row.GetCell(7).ToString();             //  +
                        string brandName = row.GetCell(8).ToString();               //  +
                        string productDescriprion = row.GetCell(9).ToString();      //  +
                        string productHit = row.GetCell(10).ToString();             //  +
                        string productNew = row.GetCell(11).ToString();             //  +
                        string productImages = row.GetCell(12).ToString();          //  +
                        string propertiesDB = row.GetCell(13).ToString();           //

                        //Проверка Бренда
                        Brand brand = new Brand();
                        if(brands.Select(b=> b.Name).Contains(brandName))
                        {
                            brand = brands.SingleOrDefault(b => b.Name == brandName);
                        }
                        else
                        {
                            brand.Name = brandName;
                            brand.AddedDate = DateTime.Now;
                            brand.ModifiedDate = DateTime.Now;
                            brandService.InsertBrand(brand);
                            brands.Add(brand);
                        }

                        //Categody Test
                        Category category = new Category();
                        if(categories.Select(b => b.Name).Contains(categoryName))
                        {
                            category = categories.SingleOrDefault(b => b.Name == categoryName);
                        }
                        else
                        {
                            //создание категории
                            category.Name = categoryName;
                            category.AddedDate = DateTime.Now;
                            category.ModifiedDate = DateTime.Now;
                            //Проверка родителя
                            Category parentCategory = new Category();
                            if (parentCategoryName != "")
                            {
                                if (categories.Select(b => b.Name).Contains(parentCategoryName))
                                {
                                    parentCategory = categories.SingleOrDefault(b => b.Name == parentCategoryName);
                                }
                                else
                                {
                                    parentCategory.Name = parentCategoryName;
                                    parentCategory.AddedDate = DateTime.Now;
                                    parentCategory.ModifiedDate = DateTime.Now;
                                    categoryService.InsertCategory(parentCategory);
                                    categories.Add(parentCategory);
                                }
                                category.ParentId = parentCategory.Id;
                            }
                            categoryService.InsertCategory(category);
                            categories.Add(category);
                        }

                        //Product 
                        Product product = new Product();
                        if (products.Select(b => b.Name).Contains(productName))
                        {
                            product = products.SingleOrDefault(b => b.Name == productName);
                        }
                        else
                        {
                            product.Name = productName;
                            product.AddedDate = DateTime.Now;
                            product.ModifiedDate = DateTime.Now;
                            product.BrandId = brand.Id;
                            product.CategoryId = category.Id;
                            product.Description = productDescriprion;
                            product.Hit = productHit.ToUpper() == "TRUE" ? true : false;
                            product.New = productNew.ToUpper() == "TRUE" ? true : false;
                            product.InStock = false;

                            //Создание папки
                            string pathInApp = _appEnvironment.WebRootPath + "/Files/" + product.Name;
                            DirectoryInfo di = new DirectoryInfo(pathInApp);
                            if (Directory.Exists(pathInApp) == false)
                                di.Create();
                            product.Path = pathInApp;

                            productService.InsertProduct(product);
                            products.Add(product);



                            //Images - если уже есть продукт, то получается уже заливал картинки
                            List<string> newImagesString = productImages.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries).ToList();
                            foreach(var newImage in newImagesString)
                            {
                                List<string> imageElement = newImage.Split(new char[] { '%' }, StringSplitOptions.RemoveEmptyEntries).ToList();
                                Image image = new Image
                                {
                                    Name = imageElement[0],
                                    AddedDate = DateTime.Now,
                                    ModifiedDate = DateTime.Now,
                                    ProductId = product.Id,
                                    ImgPath = imageElement[1],
                                    MainImg = imageElement[2].ToUpper() == "TRUE" ? true : false
                                };
                                imageService.InsertImage(image);
                            }
                        }

                        Article article = new Article();
                        //Article
                        if (!articles.Select(a => a.Name).Contains(articleName))
                        {
                            article.Name = articleName;
                            article.AddedDate = DateTime.Now;
                            article.ModifiedDate = DateTime.Now;
                            article.Quantity = Convert.ToInt32(articleQuantity);
                            article.Price = Convert.ToInt32(articlePrice);
                            article.ProductId = product.Id;
                            article.StockStatusId = stockStatuses.SingleOrDefault(ss => ss.Name == articleStockStatus).Id;
                            article.ProductName = product.Name;
                            article.ColorId = color.Id;
                            articleService.InsertArticle(article);
                            articles.Add(article);
                        }
                        else
                        {
                            article = articles.SingleOrDefault(a => a.Name == articleName);
                        }

                        //Обновление продукта
                        var articleCount = 0;
                        List<Article> productArticles = articles.Where(a => a.ProductId == product.Id).ToList();
                        if (productArticles.Count == articleCount)
                        {
                            product.InStock = false;
                            product.MinProductPrice = 0;
                            product.MaxProductPrice = 0;
                            goto finishUpdateProduct;
                        }
                        decimal minPrice = productArticles[0].Price;
                        decimal maxPrice = productArticles[0].Price;
                        foreach (var productArticle in productArticles)
                        {
                            if (productArticle.Quantity > 0)
                            {
                                if (minPrice > productArticle.Price) minPrice = productArticle.Price;
                                if (maxPrice < productArticle.Price) maxPrice = productArticle.Price;
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


                    finishUpdateProduct:
                        productService.UpdateProduct(product);

                        //Свойства
                        List<string> articleProperties = propertiesDB.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries).ToList();
                        List<PropertyValue> articlePVs = new List<PropertyValue>();//Для добавление в PVCA
                        foreach(var ap in articleProperties)
                        {
                            List<string> articlePropertyElem = ap.Split(new char[] { '/' }, StringSplitOptions.RemoveEmptyEntries).ToList();
                            Property property = new Property();

                            if (articlePropertyElem[0] == "Элементы комплекта")
                            {
                                List<string> apSubElems = articlePropertyElem[2].Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries).ToList();
                                foreach(var apse in apSubElems)
                                {
                                    if (apse.Substring(1) == "") continue;
                                    string changeAPSE = apse.Substring(1);
                                    List<string> cAPSE = changeAPSE.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries).ToList();
                                    string pName = cAPSE[0].Substring(0, cAPSE[0].Length - 1);
                                    if (properties.Select(b => b.Name).Contains(pName))
                                    {
                                        property = properties.SingleOrDefault(b => b.Name == pName);
                                        PropertyValue propertyValue = new PropertyValue();
                                        if (propertyValues.Exists(pv => pv.Value == cAPSE[1] && pv.PropertyId == property.Id))
                                        {
                                            propertyValue = propertyValues.SingleOrDefault(pv => pv.Value == cAPSE[1] && pv.PropertyId == property.Id);
                                        }
                                        else
                                        {
                                            propertyValue.Value = cAPSE[1];
                                            propertyValue.PropertyId = property.Id;
                                            propertyValueService.InsertPropertyValue(propertyValue);
                                            propertyValues.Add(propertyValue);
                                        }
                                        articlePVs.Add(propertyValue);
                                    }
                                    else
                                    {
                                        property = new Property();
                                        property.Name = pName;
                                        property.AddedDate = DateTime.Now;
                                        property.ModifiedDate = DateTime.Now;
                                        property.ValueName = "см";
                                        property.TurnOff = true;//articlePropertyElem[0] == "Номер цвета" || articlePropertyElem[0] == "Цвет" ? true : false;
                                        propertyService.InsertProperty(property);
                                        properties.Add(property);
                                        PropertyValue propertyValue = new PropertyValue
                                        {
                                            Value = cAPSE[1],
                                            PropertyId = property.Id
                                        };
                                        propertyValueService.InsertPropertyValue(propertyValue);
                                        propertyValues.Add(propertyValue);
                                        articlePVs.Add(propertyValue);
                                    }
                                }
                            }
                            else
                            {
                                if (properties.Select(b => b.Name).Contains(articlePropertyElem[0]))
                                {
                                    property = properties.SingleOrDefault(b => b.Name == articlePropertyElem[0]);
                                    PropertyValue propertyValue = new PropertyValue();
                                    if (propertyValues.Exists(pv => pv.Value == articlePropertyElem[2] && pv.PropertyId == property.Id))
                                    {
                                        propertyValue = propertyValues.SingleOrDefault(pv => pv.Value == articlePropertyElem[2] && pv.PropertyId == property.Id);
                                    }
                                    else
                                    {
                                        propertyValue.Value = articlePropertyElem[2];
                                        propertyValue.PropertyId = property.Id;
                                        propertyValueService.InsertPropertyValue(propertyValue);
                                        propertyValues.Add(propertyValue);
                                    }
                                    articlePVs.Add(propertyValue);
                                }
                                else
                                {
                                    property.Name = articlePropertyElem[0];
                                    property.AddedDate = DateTime.Now;
                                    property.ModifiedDate = DateTime.Now;
                                    property.ValueName = articlePropertyElem[1];
                                    property.TurnOff = articlePropertyElem[0] == "Номер цвета" || articlePropertyElem[0] == "Цвет" ? true : false;
                                    propertyService.InsertProperty(property);
                                    properties.Add(property);
                                    PropertyValue propertyValue = new PropertyValue
                                    {
                                        Value = articlePropertyElem[2],
                                        PropertyId = property.Id
                                    };
                                    propertyValueService.InsertPropertyValue(propertyValue);
                                    propertyValues.Add(propertyValue);
                                    articlePVs.Add(propertyValue);
                                }
                            }

                            //Добавление связи артикулов, свойств, продуктов и категорий
                            foreach(var articlePV in articlePVs)
                            {
                                if(!lpvca.Exists(p => p.ArticleId == article.Id 
                                    && p.CategoryId == category.Id 
                                    && p.ProductId == product.Id
                                    && p.PropertyValueId == articlePV.Id))
                                {
                                    PropertyValCatArt pvca = new PropertyValCatArt
                                    {
                                        ArticleId = article.Id,
                                        CategoryId = category.Id,
                                        PropertyValueId = articlePV.Id,
                                        ProductId = product.Id
                                    };
                                    propertyValCatArtService.Insert(pvca);
                                    lpvca.Add(pvca);
                                }
                            }
                        }

                        //    //Проверяем продукт
                        //    Product product = new Product();
                        //    string productName = "";
                        //    string productPath = "";
                        //    bool articleExist = false;
                        //    Article articleCopy = new Article();
                        //    bool productExist = false;
                        //    Product productCopy = new Product();
                        //
                        //    //Получаем название продукта
                        //    if (B.Contains(" арт"))
                        //    {
                        //        productName = B.Substring(0, B.IndexOf(" арт"));
                        //    }
                        //    else
                        //    {
                        //        if (B.Contains(","))
                        //        {
                        //            productName = B.Substring(0, B.IndexOf(","));
                        //        }
                        //        else
                        //        {
                        //            productName = B;
                        //        }
                        //        //articleNumber = A;
                        //    }
                        //    productName = productName.Replace("\"", String.Empty);
                        //
                        //    if (productNames.Contains(productName))
                        //    {
                        //        product = productService.GetProducts().SingleOrDefault(p => p.Name == productName);
                        //        productExist = true;
                        //    }
                        //    else
                        //    {
                        //        product.Name = productName;
                        //        product.Description = "";
                        //        product.AddedDate = DateTime.Now;
                        //        product.New = false;
                        //        product.Hit = false;
                        //        product.CategoryId = categoryId;
                        //        product.BrandId = brandId;
                        //        product.ModifiedDate = DateTime.Now;
                        //        product.OrderCount = 0;
                        //        product.MainImgPath = null;
                        //
                        //        productPath = _appEnvironment.WebRootPath + "/Files/" + product.Name;
                        //        DirectoryInfo di = new DirectoryInfo(productPath);
                        //        if (Directory.Exists(path) == false)
                        //            di.Create();
                        //        product.Path = productPath;
                        //
                        //        productService.InsertProduct(product);
                        //
                        //        //Добавить имя в список, чтобы избежать дубликатов
                        //        productNames.Add(product.Name);
                        //    }
                        //
                        //    //ArticleNumberIsUsed: //???
                        //
                        //    Article article = new Article();
                        //    if (productArticleNumbers.Contains((articleNumber, article.ProductId)))
                        //    {
                        //        goto ArticleIsExist;
                        //    }
                        //    else
                        //    {
                        //        goto ArticleIsNotExist;
                        //
                        //    }
                        //
                        ////Если артикул есть
                        //ArticleIsExist:
                        //
                        //    article = articleService.GetArticles().SingleOrDefault(p => p.Name == articleNumber && p.ProductId == product.Id);
                        //    var oldArticleCount = article.Count;
                        //    //Делаем копию артикуля, чтобы обновить его, если это необходимо
                        //    articleCopy = article;
                        //    //Обновляем артикуль
                        //    if (Convert.ToInt32(count) >= 0 && article.Count != Convert.ToInt32(count))
                        //    {
                        //        article.Count = Convert.ToInt32(count);
                        //        article.ModifiedDate = DateTime.Now;
                        //    }
                        //    if (oldArticleCount != article.Count)
                        //    {
                        //        if (article.Count > 1)
                        //            article.StockStatusArticleId = stockStatusAvailable.Id;
                        //        else
                        //            article.StockStatusArticleId = stockStatusNotAvailable.Id;
                        //        article.ModifiedDate = DateTime.Now;
                        //    }
                        //    if (article.Price != Convert.ToInt32(price))
                        //    {
                        //        article.Price = Convert.ToInt32(price);
                        //        article.ModifiedDate = DateTime.Now;
                        //    }
                        //    articlesUpdete.Add(article);
                        //    productArticleNumbers.Remove((articleNumber, article.ProductId)); //Если удалятся из бд значение, то знать что удалять
                        //
                        //    articleExist = true;
                        //
                        //    if (product.IsProductInStock == true && article.StockStatusArticleId == stockStatusNotAvailable.Id)
                        //    {
                        //        if (product.Articles.Count() > 1)
                        //        {
                        //            List<double> productPrices = new List<double>();
                        //            foreach (var a in product.Articles)
                        //            {
                        //                if (a.StockStatusArticleId == stockStatusAvailable.Id) productPrices.Add(a.Price);
                        //            }
                        //            if (productPrices.Count > 0)
                        //            {
                        //                productPrices.Sort();
                        //                product.MinProductPrice = productPrices.First();
                        //                product.MaxProductPrice = productPrices.Last();
                        //            }
                        //            else
                        //            {
                        //                product.IsProductInStock = false;
                        //            }
                        //        }
                        //        else
                        //        {
                        //            product.IsProductInStock = false;
                        //            product.MaxProductPrice = product.MinProductPrice = article.Price;
                        //        }
                        //        goto SkipProductPricesChanges;
                        //    }
                        //
                        //    goto EndOfArticleBegining;
                        //
                        ////Артикул не существует
                        //ArticleIsNotExist:
                        //
                        //    article.Name = articleNumber;
                        //    article.Price = Convert.ToDouble(price);// model.Description,
                        //    article.AddedDate = DateTime.Now;
                        //    article.OrderCount = 0;
                        //    article.ProductId = product.Id;
                        //    article.ModifiedDate = DateTime.Now;
                        //    if (Convert.ToInt32(count) >= 0) article.Count = Convert.ToInt32(count); else article.Count = null;
                        //    if (article.Count != 0)
                        //        article.StockStatusArticleId = stockStatusAvailable.Id;
                        //    else
                        //        article.StockStatusArticleId = stockStatusNotAvailable.Id;
                        //    articleService.InsertArticle(article);
                        //
                        ////Выход из изменения 
                        //EndOfArticleBegining:
                        //
                        //    //Редактирование цены при артикуле если новый артикул или изменение старого с наличием его в магазине
                        //    if (article.StockStatusArticleId == stockStatusAvailable.Id)
                        //    {
                        //        if (product.IsProductInStock == true)
                        //        {
                        //            if (product.MaxProductPrice == product.MinProductPrice)
                        //            {
                        //                if (product.MaxProductPrice < article.Price) product.MaxProductPrice = article.Price;
                        //                else product.MinProductPrice = article.Price;
                        //            }
                        //            else
                        //            {
                        //                if (product.MaxProductPrice < article.Price) product.MaxProductPrice = article.Price;
                        //                if (product.MinProductPrice > article.Price) product.MinProductPrice = article.Price;
                        //            }
                        //        }
                        //        else
                        //        {
                        //            product.MaxProductPrice = article.Price;
                        //            product.MinProductPrice = article.Price;
                        //            product.IsProductInStock = true;
                        //        }
                        //    }
                        //
                        //SkipProductPricesChanges:
                        //
                        //    if (articleExist && article != articleCopy)
                        //    {
                        //        articlesUpdete.Add(article);
                        //    }
                        //    if (productExist && product != productCopy)
                        //    {
                        //        productsUpdete.Add(product);
                        //    }
                        //
                        //
                    } while (true);
                        //articleService.UpdateArticles(articlesUpdete);
                        //productService.UpdateProducts(productsUpdete);
                    }
            }
            return RedirectToAction("UploadDB");
        }

        [HttpPost]
        public async Task<IActionResult> UpdateDB(IFormFile uploadedFile, string priceColumn = "C", string quantityColumn = "B")
        {
            if (uploadedFile != null)
            {
                // сохраняем файл в папку Files в каталоге wwwroot
                string path = _appEnvironment.WebRootPath + "/Files/BDUpdateXLS";// +  uploadedFile.FileName;
                using (var fileStream = new FileStream(path, FileMode.Create))
                {
                    await uploadedFile.CopyToAsync(fileStream);
                }
                
                int priceColumnInt = ReturnColumn(priceColumn);
                int quantityColumnInt = ReturnColumn(quantityColumn);

                bool isDBUpdate = false;//Была ли обновлена база
                List<long> productIdsToUpdate = new List<long>();
                List<Product> products = productService.GetProducts().ToList();
                List<Article> articles = articleService.GetArticles().ToList();
                List<StockStatus> stockStatuses = stockStatusService.GetStockStatuses().ToList();

                using (var stream = new FileStream(path, FileMode.Create))
                {
                    string sFileExtension = Path.GetExtension(uploadedFile.FileName).ToLower();
                    ISheet sheet;
                    uploadedFile.CopyTo(stream);
                    stream.Position = 0;
                    if (sFileExtension == ".xls")
                    {
                        HSSFWorkbook hssfwb = new HSSFWorkbook(stream); //This will read the Excel 97-2000 formats  
                        sheet = hssfwb.GetSheetAt(0); //get first sheet from workbook  
                    }
                    else
                    {
                        XSSFWorkbook hssfwb = new XSSFWorkbook(stream); //This will read 2007 Excel format  
                        sheet = hssfwb.GetSheetAt(0); //get first sheet from workbook   
                    }
                    var i = 2;//Какая строка
                    var ending = sheet.PhysicalNumberOfRows; //Число строк


                    do
                    {
                        i++;
                        IRow row = sheet.GetRow(i);
                        string articleName = "";
                        if (i < ending)//Если строк больше нет, то закончит цикл
                        {
                            articleName = row.GetCell(0).ToString();
                            if (articleName == "Итого" || articleName == "") break;
                        }
                        else
                        {
                            break;
                        }

                        Article article = new Article();
                        
                        if (articles.Select(a=> a.Name).Contains(articleName))

                        {
                            article = articles.SingleOrDefault(a => a.Name == articleName);
                        }
                        else
                        {
                            continue;
                        }
                        string articleQuantity = row.GetCell(quantityColumnInt).ToString();
                        string articlePrice = row.GetCell(priceColumnInt).ToString();

                        //Article changingArticle = new Article
                        //{
                        //    Id = article.Id,
                        //    Name = article.Name,
                        //    Price = article.Price,
                        //    Quantity = article.Quantity,
                        //    StockStatusId = article.StockStatusId,
                        //    ProductId = article.ProductId,
                        //    ProductName = article.ProductName,
                        //    ColorId = article.ColorId,
                        //    AddedDate = article.AddedDate,
                        //    ModifiedDate = article.ModifiedDate
                        //};
                        article.Quantity = Convert.ToInt32(articleQuantity) > 0 ? Convert.ToInt32(articleQuantity) : 0;
                        article.Price = Convert.ToInt32(articlePrice);

                        //if (article != changingArticle)
                        //{
                        //    isDBUpdate = true;
//
                        //    //Замена старого артикула на новый
                        //    articles.Remove(article);
                        //    articles.Add(changingArticle);

                            productIdsToUpdate.Add(article.ProductId);//Какие продукты были изменены

                            if (article.Quantity == 0)
                            {
                                article.StockStatusId = stockStatuses.SingleOrDefault(ss => ss.Name == "Нет в наличии").Id;
                            }
                            else
                            {
                                article.StockStatusId = stockStatuses.SingleOrDefault(ss => ss.Name == "В наличии").Id;
                            }
                            articleService.UpdateArticle(article);
                        //}
                    } while (true);
                }

                foreach (var product in products.Where(p => productIdsToUpdate.Contains(p.Id)))
                {//Обновление продукта
                    List<Article> productArticles = articles.Where(a => a.ProductId == product.Id).ToList();

                    decimal minPrice = productArticles[0].Price;
                    decimal maxPrice = productArticles[0].Price;
                    var articleCount = 0;
                    foreach (var productArticle in productArticles)
                    {
                        if (productArticle.Quantity > 0)
                        {
                            if (minPrice > productArticle.Price) minPrice = productArticle.Price;
                            if (maxPrice < productArticle.Price) maxPrice = productArticle.Price;
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
                }
            }
            return RedirectToAction("UploadDB");
        }

        public int ReturnColumn(string columnName)
        {
            int priceColumnInt = 0;
                switch (columnName.ToUpper())
                {
                    case "A":
                        priceColumnInt = 0;
                        break;
                    case "B":
                        priceColumnInt = 1;
                        break;
                    case "C":
                        priceColumnInt = 2;
                        break;
                    case "D":
                        priceColumnInt = 3;
                        break;
                    case "E":
                        priceColumnInt = 4;
                        break;
                    case "F":
                        priceColumnInt = 5;
                        break;
                    case "G":
                        priceColumnInt = 6;
                        break;
                    case "H":
                        priceColumnInt = 7;
                        break;
                    case "I":
                        priceColumnInt = 8;
                        break;
                    case "J":
                        priceColumnInt = 9;
                        break;
                    case "K":
                        priceColumnInt = 10;
                        break;
                    case "L":
                        priceColumnInt = 11;
                        break;
                    case "M":
                        priceColumnInt = 12;
                        break;
                    case "N":
                        priceColumnInt = 13;
                        break;
                    case "O":
                        priceColumnInt = 14;
                        break;
                    case "P":
                        priceColumnInt = 15;
                        break;
                    default: 
                        priceColumnInt = Convert.ToInt32(columnName) - 1;
                        break;
                }
                return priceColumnInt;
        }

        
        [HttpGet]
        public ActionResult RemoveSymbolPV()
        {
            List<PropertyValue> lpv = propertyValueService.GetPropertyValues().ToList();
            foreach(var pv in lpv)
            {
                if ( pv.Value.IndexOf(" ;") > 0)
                {
                    pv.Value = pv.Value.Substring(0, pv.Value.IndexOf(" ;"));
                }
                propertyValueService.UpdatePropertyValue(pv);
            }
            return StatusCode(200);
        }
        [HttpGet]
        public ActionResult DeletePV()
        {
            List<PropertyValue> lpv = propertyValueService.GetPropertyValues().ToList();
            foreach(var pv in lpv)
            {
                if ( pv.Value.IndexOf(" ;") > 0)
                {
                    List<PropertyValCatArt> lpvca = propertyValCatArtService.GetAll().Where(p=> p.PropertyValueId == pv.Id).ToList();
                    propertyValCatArtService.DeleteSome(lpvca);
                    propertyValueService.DeletePropertyValue(pv.Id);
                    //pv.Value = pv.Value.Substring(0, pv.Value.IndexOf(" ;"));
                }
            }
            return StatusCode(200);
        }
        [HttpGet]
        public ActionResult FixArticles()
        {
            List<Article> articles = articleService.GetArticles().ToList();
            List<StockStatus> ss = stockStatusService.GetStockStatuses().ToList();
            foreach(var a in articles)
            {
                if (a.Quantity > 0)
                {
                    a.StockStatusId = ss.SingleOrDefault(ss => ss.Name == "В наличии").Id;
                }
                else
                {
                    a.StockStatusId = ss.SingleOrDefault(ss => ss.Name == "Нет в наличии").Id;
                }
            }
            articleService.UpdateArticles(articles);
            return StatusCode(200);
        }
        [HttpGet]
        public ActionResult FixProductDirectoryName()
        {
            string directoryPath = _appEnvironment.WebRootPath + "/Files/";
            DirectoryInfo di = new DirectoryInfo(directoryPath);
            DirectoryInfo[] rgFiles = di.GetDirectories();
            foreach(var d in rgFiles)
            {
                List<string> newName = d.Name.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries).ToList();
                string newPath = _appEnvironment.WebRootPath + "/NEWFILES/";
                foreach(var n in newName)
                {
                    newPath += n + " ";
                }
                newPath = newPath.Trim();
                string oldPath = _appEnvironment.WebRootPath + "/Files/" + d.Name;
                if (newPath != oldPath)
                {
                    d.MoveTo(newPath);
                }
            }
            return StatusCode(200);
        }
        [HttpGet]
        public ActionResult AddCountryToProduct()
        {
            List<Product> products = productService.GetProducts().ToList();
            Country country = countryService.GetCountries().SingleOrDefault(c=> c.Name == "No Country");
            foreach(var p in products)
            {
                if (p.CountryId == null) p.CountryId = country.Id;
            }
            productService.UpdateProducts(products);
            return StatusCode(200);
        }

        [HttpGet]
        public ActionResult AddTitleAndDescToCategory()
        {
            List<Category> categories = categoryService.GetCategories().ToList();
            foreach (var сat in categories)
            {
                if (сat.Title == null)
                {
                    сat.Title = сat.Name;
                    сat.TitleDescrition = сat.Name;
                    categoryService.UpdateCategory(сat);
                }
            }
           
            return StatusCode(200);
        }

        [HttpGet]
        public ActionResult TrimNamePropertyValues()
        {
            List<PropertyValue> propertyValues = propertyValueService.GetPropertyValues().ToList();
            foreach (var pv in propertyValues)
            {
                pv.Value = pv.Value.Trim();
                propertyValueService.UpdatePropertyValue(pv);
            }

            return StatusCode(200);
        }
    }
}