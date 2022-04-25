using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Komod.Data;
using Komod.Ser.ArticleSer;
using Komod.Ser.BrandSer;
using Komod.Ser.CategorySer;
using Komod.Ser.ColorSer;
using Komod.Ser.ImageSer;
using Komod.Ser.ProductSer;
using Komod.Ser.PropertySer;
using Komod.Ser.PropertyValCatArtSer;
using Komod.Ser.PropertyValueSer;
using Komod.Ser.StockStatusSer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Komod.Web.Controllers
{
    [Authorize(Roles = "admin")]
    public class TestController : Controller
    {
        private readonly IProductService productService;
        private readonly IArticleService articleService;
        private readonly ICategoryService categoryService;
        private readonly IBrandService brandService;
        private readonly IPropertyService propertyService;
        private readonly IPropertyValueService propertyValueService;
        private readonly IPropertyValCatArtService propertyValCatArtService;
        private readonly IStockStatusService stockStatusService;
        private readonly IImageService imageService;
        private readonly IColorService colorService;
        IWebHostEnvironment _appEnvironment;

        public TestController(IWebHostEnvironment appEnvironment,
            IProductService productService,
            IArticleService articleService,
            ICategoryService categoryService,
            IBrandService brandService,
            IPropertyService propertyService,
            IPropertyValCatArtService propertyValCatArtService,
            IStockStatusService stockStatusService,
            IImageService imageService,
            IPropertyValueService propertyValueService,
            IColorService colorService)
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
        }


        public async void Create()
        {
            for(var i = 1; i< 11; i++)
            {
                Brand brand = new Brand
                {
                    Name = "test " + i,
                    AddedDate = DateTime.Now,
                    ModifiedDate = DateTime.Now
                };
                brandService.InsertBrand(brand);
            }
            Color red = new Color
            {
                Name = "red",
                ColorCode = "#ff0000",
                AddedDate = DateTime.Now,
                ModifiedDate = DateTime.Now
            };
            colorService.InsertColor(red);
            Color green = new Color
            {
                Name = "green",
                ColorCode = "#00ff00",
                AddedDate = DateTime.Now,
                ModifiedDate = DateTime.Now
            };
            colorService.InsertColor(green);
            Color blue = new Color
            {
                Name = "blue",
                ColorCode = "#0000ff",
                AddedDate = DateTime.Now,
                ModifiedDate = DateTime.Now
            };
            colorService.InsertColor(blue);

            List<Category> categories = categoryService.GetCategories().ToList();
            List<Brand> brands = brandService.GetBrands().ToList();
            List<Color> colors = colorService.GetColors().ToList();
            List<PropertyValue> propertyValues = propertyValueService.GetPropertyValues().ToList();
            string ProductName = "Product ";
            string ArticleName = "Artice ";
            int counter = 1;
            StockStatus stockStatus = stockStatusService.GetStockStatuses().SingleOrDefault(ss => ss.Name == "В наличии"); 
            Random rnd = new Random();
            foreach (var category in categories)
            {
                foreach (var brand in brands)
                {
                    foreach(var color in colors)
                    {
                        for(var j=0; j<5; j++)
                        {
                            Product product = new Product
                            {
                                Name = ProductName + counter,
                                AddedDate = DateTime.Now,
                                ModifiedDate = DateTime.Now,
                                BrandId = brand.Id,
                                CategoryId = category.Id,
                                Description = "sdfsdfsdf sdf sd sd fsdf sdfs dfasdfw efw ef",
                                Hit = false,
                                New = false,
                                InStock = true,
                                Path = _appEnvironment.WebRootPath + "/"
                            };
                            productService.InsertProduct(product);
                            List<Article> articles = new List<Article>();
                            for (var a=1; a<4; a++)
                            {
                                Article article = new Article
                                {
                                    Name = ArticleName + ProductName + a,
                                    AddedDate = DateTime.Now,
                                    ModifiedDate = DateTime.Now,
                                    Quantity = 5,
                                    Price = rnd.Next(100, 10000),
                                    StockStatusId = stockStatus.Id,
                                    ProductId = product.Id,
                                    ProductName = product.Name,
                                    ColorId = color.Id
                                };
                                articleService.InsertArticle(article);
                                PropertyValCatArt pvca1 = new PropertyValCatArt
                                {
                                    ProductId = product.Id,
                                    ArticleId = article.Id,
                                    CategoryId = category.Id,
                                    PropertyValueId = propertyValues[rnd.Next(1, 4)].Id
                                };
                                propertyValCatArtService.Insert(pvca1);
                                //bool notExist = false;
                                //PropertyValCatArt pvca2 = new PropertyValCatArt
                                //{
                                //    ProductId = product.Id,
                                //    ArticleId = article.Id,
                                //    CategoryId = category.Id
                                //};
                                //do
                                //{
                                //    pvca2.PropertyValueId = propertyValues[rnd.Next(1, 4)].Id;
                                //    if (pvca1 != pvca2) notExist = true;
                                //}
                                //while (notExist);
                                //List<PropertyValCatArt> lpvca = new List<PropertyValCatArt>();
                                //lpvca.Add(pvca1);
                                //lpvca.Add(pvca2);
                                //propertyValCatArtService.InsertSome(lpvca);
                                articles.Add(article);

                            }

                            //List<Article> articles = articleService.GetArticles().Where(a => a.ProductId == product.Id).ToList();
                            decimal minPrice = articles[0].Price;
                            decimal maxPrice = articles[0].Price;
                            foreach (var article in articles)
                            {
                                if (article.Quantity > 0)
                                {
                                    if (minPrice > article.Price) minPrice = article.Price;
                                    if (maxPrice < article.Price) maxPrice = article.Price;
                                }
                            }
                            product.MinProductPrice = minPrice;
                            product.MaxProductPrice = maxPrice;
                            productService.UpdateProduct(product);

                            Image image = new Image
                            {
                                Name = "1.jpg",
                                ImgPath = "/1.jpg",
                                ProductId = product.Id,
                                AddedDate = DateTime.Now,
                                ModifiedDate = DateTime.Now,
                                MainImg = true
                            };
                            imageService.InsertImage(image);


                            counter++;
                        }
                    }
                }
            }

        }
    }
}

//1.jpg