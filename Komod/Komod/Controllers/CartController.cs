using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Komod.Data;
using Komod.Ser.ArticleSer;
using Komod.Ser.CartSer;
using Komod.Ser.OrderSer;
using Komod.Ser.ProductSer;
using Komod.Ser.ProductSetSer;
using Komod.Ser.PromocodeArticleSer;
using Komod.Ser.PromocodeSer;
using Komod.Web.Code;
using Komod.Web.Models.CartModels;
using Komod.Web.Models.ProductModels;
using Komod.Web.Models.PromocodeModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Komod.Web.Controllers
{
    public class CartController : Controller
    {
        private readonly UserManager<User> _userManager;
        //private readonly ICartService cartService;
        //private readonly ICartItemService cartItemService;
        private readonly IArticleService articleService;
        private readonly IOrderService orderService;
        private readonly IProductService productService;
        private readonly IPromocodeService promocodeService;
        private readonly IPromocodeArticleService promocodeArticleService;
        private readonly IProductSetService productSetService;
        //
        public CartController(IArticleService articleService, IProductService productService, UserManager<User> userManager,
            IProductSetService productSetService,
            IPromocodeService promocodeService,
            IOrderService orderService)// ICartService cartService, ICartItemService cartItemService,)
        {
            _userManager = userManager;
        //    this.cartService = cartService;
        //    this.cartItemService = cartItemService;
            this.articleService = articleService;
            this.productService = productService;
            this.productSetService = productSetService;
            this.promocodeService = promocodeService;
            this.orderService= orderService;
        }
        //public async Task<IActionResult> Cart()
        //{
        //    if (User.Identity.IsAuthenticated)
        //    {
        //        ClaimsPrincipal currentUser = this.User;
        //        User user = await _userManager.GetUserAsync(currentUser);
        //        long cartId = cartService.GetCartByUser(user.UserName).Id;
        //
        //        List<CartItem> cartItems = cartItemService.GetCartItems().Where(ci => ci.CartId == cartId).ToList();
        //        CartViewModel viewModel = new CartViewModel()
        //        {
        //            Id = cartId,
        //            UserName = user.UserName,
        //            CartItems = cartItems
        //        };
        //        return View(viewModel);
        //    }
        //    else
        //    {
        //        return StatusCode(401);
        //    }
        //}
        //
        //public async Task<IActionResult> AddArticleToCart(long articleId)//??
        //{
        //    if (User.Identity.IsAuthenticated)
        //    {
        //        ClaimsPrincipal currentUser = this.User;
        //        User user = await _userManager.GetUserAsync(currentUser);
        //        long cartId = cartService.GetCartByUser(user.UserName).Id;
        //        Article article = articleService.GetArticle(articleId);
        //        CartItem cartItem = new CartItem()
        //        {
        //            CartId = cartId,
        //            ArticleId = articleId,
        //            Quantity = 1,
        //            UnitPrice = article.Price,
        //            TotalPrice = article.Price
        //        };
        //        cartItemService.InsertCartItem(cartItem);
        //        return StatusCode(201);
        //    }
        //    else
        //    {
        //        return StatusCode(401);
        //    }
        //}
        //
        //public async Task<IActionResult> RemoveProductFromWishlost(long articleId)
        //{
        //    if (User.Identity.IsAuthenticated)
        //    {
        //        ClaimsPrincipal currentUser = this.User;
        //        User user = await _userManager.GetUserAsync(currentUser);
        //        long cartId = cartService.GetCartByUser(user.UserName).Id;
        //        CartItem cartItem = cartItemService.GetCartItems().SingleOrDefault(wi => wi.ArticleId == articleId && wi.CartId == cartId);
        //        cartItemService.DeleteCartItem(cartItem.Id);
        //        return StatusCode(201);
        //    }
        //    else
        //    {
        //        return StatusCode(401);
        //    }
        //}
        //
        //public async Task<IActionResult> IncreaseProductInWishlost(long articleId)
        //{
        //    if (User.Identity.IsAuthenticated)
        //    {
        //        ClaimsPrincipal currentUser = this.User;
        //        User user = await _userManager.GetUserAsync(currentUser);
        //        long cartId = cartService.GetCartByUser(user.UserName).Id;
        //        CartItem cartItem = cartItemService.GetCartItems().SingleOrDefault(wi => wi.ArticleId == articleId && wi.CartId == cartId);
        //        cartItem.Quantity++;
        //        cartItemService.UpdateCartItem(cartItem);
        //        return RedirectToAction("Cart");
        //    }
        //    else
        //    {
        //        return StatusCode(401);
        //    }
        //}
        //
        //public async Task<IActionResult> DecreaseProductInWishlost(long articleId)
        //{
        //    if (User.Identity.IsAuthenticated)
        //    {
        //        ClaimsPrincipal currentUser = this.User;
        //        User user = await _userManager.GetUserAsync(currentUser);
        //        long cartId = cartService.GetCartByUser(user.UserName).Id;
        //        CartItem cartItem = cartItemService.GetCartItems().SingleOrDefault(wi => wi.ArticleId == articleId && wi.CartId == cartId);
        //        cartItem.Quantity--;
        //        if(cartItem.Quantity == 0)
        //        {
        //            cartItemService.DeleteCartItem(cartItem.Id);
        //        }
        //        else
        //        {
        //            cartItemService.UpdateCartItem(cartItem);
        //        }
        //        return RedirectToAction("Cart");
        //    }
        //    else
        //    {
        //        return StatusCode(401);
        //    }
        //}


        public IActionResult Cart()
        {
            var cart = SessionHelper.GetObjectFromJson<List<CartItemViewModel>>(HttpContext.Session, "cart");
            ViewBag.cart = cart;
            if (cart != null)
                ViewBag.total = cart.Sum(item => item.Article.Price * item.ItemQuantity);
            else
                return View();

            List<Product> products = productService.GetProducts().ToList();
            List<ProductSet> prodctSets = productSetService.GetProductSets().ToList();
            List<ProductSet> productSetWithCartElement = new List<ProductSet>();
            List<Product> productsFromCart = new List<Product>();
            List<CartProductSet> cartSets = new List<CartProductSet>();
            foreach(var a in cart)
            {
                Product product = products.SingleOrDefault(p => p.Id == a.Article.ProductId);
                productsFromCart.Add(product);
                List<ProductSet> lps = prodctSets.Where(ps => ps.ProductSetName.Contains(product.Name)).ToList();
                //List<Product> lp = products.Where(p => lps.Select(ps => ps.ProductSetName).Contains(p.Name)).ToList();
                productSetWithCartElement.AddRange(lps);
            }
            productSetWithCartElement.Distinct();
            foreach(var ps in productSetWithCartElement)
            {
                List<Product> productsFromSelectedSet = products.Where(p => ps.ProductSetName.Contains(p.Name)).ToList();
                List<ProductFromSet> lpfs = new List<ProductFromSet>();
                productsFromSelectedSet.ForEach(p =>
                {
                    ProductFromSet pfs = new ProductFromSet()
                    {
                        ProductId = p.Id,
                        ProductName = p.Name,
                    };

                    pfs.IsAlreadyAddToCart = productsFromCart.Contains(p) ? true : false;

                    lpfs.Add(pfs);
                });
                CartProductSet cs = new CartProductSet()
                {
                    Discount = ps.DiscounPercent,
                    ProductFromSets = lpfs
                };
                cartSets.Add(cs);
            }
            ViewBag.CartSets = cartSets;

            return View();
        }

        [HttpPost]
        public JsonResult Add(long id, int quantity)
        {
            Article article = articleService.GetArticle(id);
            ArticleViewModel articleVM = new ArticleViewModel { 
                Id = article.Id,
                Name = article.Name,
                ProductName = article.ProductName,
                Price = article.Price,
                Quantity = article.Quantity
            };
            Product product = productService.GetProduct(article.ProductId);
            if (article.Quantity < quantity) quantity = article.Quantity;

            if (SessionHelper.GetObjectFromJson<List<CartItemViewModel>>(HttpContext.Session, "cart") == null)
            {
                List<CartItemViewModel> cart = new List<CartItemViewModel>();
                CartItemViewModel civm = new CartItemViewModel { Article = articleVM, ItemQuantity = quantity, DayOfWeek = product.DayOfWeek, DiscountPercent = product.DiscountPercent };
                if (article.Quantity == quantity) civm.MaxQuantity = true; else civm.MaxQuantity = false;
                cart.Add(civm);
                SessionHelper.SetObjectAsJson(HttpContext.Session, "cart", cart);
            }
            else
            {
                List<CartItemViewModel> cart = SessionHelper.GetObjectFromJson<List<CartItemViewModel>>(HttpContext.Session, "cart");
                int index = isExist(id);
                if (index != -1)
                {
                    if(cart[index].ItemQuantity + quantity > article.Quantity)
                    {
                        cart[index].ItemQuantity = article.Quantity;
                        cart[index].MaxQuantity = true;
                    }
                    else
                    {
                        cart[index].ItemQuantity += quantity;
                    }
                }
                else
                {
                    CartItemViewModel civm = new CartItemViewModel { Article = articleVM, ItemQuantity = quantity };
                    if (article.Quantity == quantity) civm.MaxQuantity = true; else civm.MaxQuantity = false;
                    cart.Add(civm);
                }
                SessionHelper.SetObjectAsJson(HttpContext.Session, "cart", cart);
            }
            return Json("ok");
        }

        public IActionResult IncreaseArticleQuantity(long id)
        {
            List<CartItemViewModel> cart = SessionHelper.GetObjectFromJson<List<CartItemViewModel>>(HttpContext.Session, "cart");
            int index = isExist(id);
            cart[index].ItemQuantity++;
            if (cart[index].ItemQuantity == cart[index].Article.Quantity)
            {
                cart[index].MaxQuantity = true;
            }
            SessionHelper.SetObjectAsJson(HttpContext.Session, "cart", cart);
            return RedirectToAction("Cart");
        }

        public IActionResult DecreaseArticleQuantity(long id)
        {
            List<CartItemViewModel> cart = SessionHelper.GetObjectFromJson<List<CartItemViewModel>>(HttpContext.Session, "cart");
            int index = isExist(id);
            cart[index].ItemQuantity--;
            if (cart[index].ItemQuantity < cart[index].Article.Quantity)
            {
                cart[index].MaxQuantity = false;
            }
            if (cart[index].ItemQuantity == 0) cart.RemoveAt(index);
            SessionHelper.SetObjectAsJson(HttpContext.Session, "cart", cart);
            return RedirectToAction("Cart");
        }

        public IActionResult Remove(long id)
        {
            List<CartItemViewModel> cart = SessionHelper.GetObjectFromJson<List<CartItemViewModel>>(HttpContext.Session, "cart");
            int index = isExist(id);
            cart.RemoveAt(index);
            SessionHelper.SetObjectAsJson(HttpContext.Session, "cart", cart);
            return RedirectToAction("Cart");
        }

        private int isExist(long id)
        {
            List<CartItemViewModel> cart = SessionHelper.GetObjectFromJson<List<CartItemViewModel>>(HttpContext.Session, "cart");
            for (int i = 0; i < cart.Count; i++)
            {
                if (cart[i].Article.Id.Equals(id))
                {
                    return i;
                }
            }
            return -1;
        }

        
        //Получение Id артикулов привязанных к промокоду
        [HttpGet]
        public async Task<JsonResult> GetPromosArticles(string promocodeName)
        {
            Promocode promocode = promocodeService.GetPromocodes().SingleOrDefault(p => p.Name == promocodeName);//.ToList();
            if (promocode == null)
            {
                return Json("Промокод не существует");
            }
            else 
            {
                if (promocode.EndOfPromocode < DateTime.Now) return Json("Срок действия промокода истек");

                if (User.Identity.IsAuthenticated)
                {
                    ClaimsPrincipal currentUser = this.User;
                    User user = await _userManager.GetUserAsync(currentUser);
                    List<Order> orders = orderService.GetOrders().Where(o=> o.UserName == user.UserName && o.OrderStatus.Name != "Отменен").ToList();
                    if (orders.Select(o=>o.PromoName).Contains(promocodeName)) return Json("Промокод уже был вами использован");
                }
                else
                {
                    return Json("Пользователь не авторизован");
                }
            }
            //List<long> articleIds = promocodeArticleService.GetAll().Where(pa => pa.PromocodeId == promocode.Id).Select(pa=> pa.ArticleId).ToList();
            List<Article> articles = new List<Article>();//articleService.GetArticles().Where(a => articleIds.Contains(a.Id)).ToList();
            foreach(var a in promocode.PromocodeArticles)
            {
                articles.Add(a.Article);
            }

            PromocodeViewModel pvm = new PromocodeViewModel()
            {
                Id = promocode.Id,
                PromocodeName = promocode.Name,
                DiscountPercent = promocode.DiscountPercent,
                PersonalUserPromo = promocode.PersonalUserPromo
            };
            ReturnPromoJson entity = new ReturnPromoJson()
            {
                ArticleNames = articles.Select(a => a.Name).ToList(),
                Promocode = pvm
            };

            
            var cart = SessionHelper.GetObjectFromJson<List<CartItemViewModel>>(HttpContext.Session, "cart");
            foreach(var c in cart)
            {
                c.PromocodeName = promocodeName;
                if (promocode.PromocodeArticles.Select(p => p.ArticleId).Contains(c.Article.Id)) c.IsPromocodeExistForArticle = true;
            }
            SessionHelper.SetObjectAsJson(HttpContext.Session, "cart", cart);

            return Json(entity);
        }

        [HttpGet] //НЕ УЧИТЫВАЮ ЧИСЛО ЗАКАЗОВ
        public async Task<JsonResult> CountTotalPrice()
        {
            List<PriceToCount> entity = new List<PriceToCount>();
            var cart = SessionHelper.GetObjectFromJson<List<CartItemViewModel>>(HttpContext.Session, "cart");
            //Какой промокод
            Promocode promocode = new Promocode();
            if (cart[0].PromocodeName != "" && cart[0].PromocodeName != null)
            {
                promocode = promocodeService.GetPromocodes().SingleOrDefault(p => p.Name == cart[0].PromocodeName);
            }
            //Артикулы из корзины
            List<ArticleViewModel> articleFromCart = cart.Select(c => c.Article).ToList();
            //Продукты артикулов
            List<Product> products = productService.GetProducts().Where(p => articleFromCart.Select(a => a.ProductId).Contains(p.Id)).ToList();
            //Итоговая цена
            decimal totalPrice = articleFromCart.Select(a => a.Price).Sum();
            //Комплекты
            List<ProductSet> allProductsSets = productSetService.GetProductSets().Where(s=> s.ActiveSet == true).ToList();
            List<string> productName = products.Select(p => p.Name).ToList();
            List<ProductSet> productSet = new List<ProductSet>();
            foreach(var set in allProductsSets)
            {
                if(IncludeArrayToArray(set.ProductSetName.Split('|').ToList(), productName))
                {
                    productSet.Add(set);
                }
            }

            //SET DISCOUNT   ----- ПЕРЕПИСАТь
            //if (products.Count != articleFromCart.Count)
            //{
            //    //Если  Продуктов меньше чем Артикулей
            //    if (productSet.Count > 0)
            //    {
            //        foreach (var ps in productSet.OrderBy(ps=> ps.DiscounPercent))
            //        {
            //            var productSetNames = ps.ProductSetName.Split('|').ToList();
            //            List<Product> productFromSet = products.Where(p => productSetNames.Contains(p.Name)).ToList();
            //            List<ArticleViewModel> articlesFromSet = articleFromCart.Where(a => productFromSet.Select(p => p.Id).Contains(a.ProductId)).ToList();
            //            PriceToCount itemIncludedSet = new PriceToCount()
            //            {
            //                WhatDiscount = "Set " + ps.SetName
            //            };
            //            if (productFromSet.Count != articlesFromSet.Count)
            //            {
            //
            //            }
            //            else
            //            {
            //                itemIncludedSet.Articles = articlesFromSet;
            //                itemIncludedSet.TotalPrice = articlesFromSet.Select(a => a.Price).Sum() * ps.DiscounPercent;
            //            }
            //            entity.Add(itemIncludedSet);
            //            articlesFromSet.ForEach(a => articleFromCart.Remove(a));
            //            productFromSet.ForEach(a => products.Remove(a));
            //        }
            //    }
            //}
            //else
            //{
            //    //Если равное число
            //    if (productSet.Count > 0)
            //    {
            //        foreach(var ps in productSet.OrderBy(ps => ps.DiscounPercent))
            //        {
            //            var productSetNames = ps.ProductSetName.Split('|').ToList();
            //            List<Product> productFromSet = products.Where(p => productSetNames.Contains(p.Name)).ToList();
            //            List<ArticleViewModel> articlesFromSet = articleFromCart.Where(a => productFromSet.Select(p => p.Id).Contains(a.ProductId)).ToList();
            //            PriceToCount itemIncludedSet = new PriceToCount()
            //            {
            //                Articles = articlesFromSet,
            //                WhatDiscount = "Set " + ps.SetName,
            //                TotalPrice = 0//articlesFromSet.Select(a=> a.Price).Sum() * ps.DiscounPercent
            //            };
            //            //Подсчет суммы
            //            articlesFromSet.ForEach(a => itemIncludedSet.TotalPrice += a.Price * cart.SingleOrDefault(c => c.Article.Id == a.Id).ItemQuantity);
            //            //Применение скидки
            //            itemIncludedSet.TotalPrice = itemIncludedSet.TotalPrice * (1 - ps.DiscounPercent / 100);
            //
            //            entity.Add(itemIncludedSet);
            //            articlesFromSet.ForEach(a => articleFromCart.Remove(a));
            //            productFromSet.ForEach(a => products.Remove(a));
            //        }
            //    }
            //}
            //Бертуся по максимальной скидке
            foreach (var ps in productSet.OrderBy(ps => ps.DiscounPercent))
            {
                var productSetNames = ps.ProductSetName.Split('|').ToList();
                List<Product> productFromSet = products.Where(p => productSetNames.Contains(p.Name)).ToList();
                List<ArticleViewModel> allArticlesFromSet = articleFromCart.Where(a => productFromSet.Select(p => p.Id).Contains(a.ProductId)).ToList();

                //Найти колличество одного и того же комплекта
                int setItemQuantity = allArticlesFromSet.Where(a => productFromSet[0].Id == a.ProductId).Count();
                foreach (var p in productFromSet)
                {
                    int articleQuantityInProductSet = allArticlesFromSet.Where(a => p.Id == a.ProductId).Count();
                    if (setItemQuantity > articleQuantityInProductSet) setItemQuantity = articleQuantityInProductSet;
                }

                //Подсчет и вывод артикулей итогового комплекта
                List<ArticleViewModel> articlesFromSet = new List<ArticleViewModel>(); 
                foreach (var p in productFromSet)
                {
                    articlesFromSet.AddRange(allArticlesFromSet.Where(a => p.Id == a.ProductId).OrderBy(a=> a.Price).Take(setItemQuantity));
                }

                PriceToCount itemIncludedSet = new PriceToCount()
                {
                    Articles = articlesFromSet,
                    WhatDiscount = "Set " + ps.SetName,
                    TotalPrice = 0//articlesFromSet.Select(a=> a.Price).Sum() * ps.DiscounPercent
                };
                //Подсчет суммы
                articlesFromSet.ForEach(a => itemIncludedSet.TotalPrice += a.Price * cart.SingleOrDefault(c => c.Article.Id == a.Id).ItemQuantity);
                //Применение скидки
                itemIncludedSet.TotalPrice = itemIncludedSet.TotalPrice * (1 - ps.DiscounPercent / 100);
                
                entity.Add(itemIncludedSet);
                articlesFromSet.ForEach(a => articleFromCart.Remove(a));
                productFromSet.ForEach(a => products.Remove(a));
            }


            //DAY DISCOUNT
            List<Product> productWithDayDiscount = products.Where(p => p.DayOfWeek == DateTime.Now.DayOfWeek).ToList();
            if (productWithDayDiscount.Count() > 0)
            {
                List<ArticleViewModel> articleWithDayDiscount = articleFromCart.Where(a => productWithDayDiscount.Select(p => p.Id).Contains(a.ProductId)).ToList();
                PriceToCount itemWithDayDiscount = new PriceToCount()
                {
                    Articles = articleWithDayDiscount,
                    TotalPrice = 0,
                    WhatDiscount = "Day discount"
                };
                foreach(var a in articleWithDayDiscount)
                {
                    itemWithDayDiscount.TotalPrice += a.Price * cart.SingleOrDefault(c=>c.Article.Id == a.Id).ItemQuantity * (1 - productWithDayDiscount.SingleOrDefault(p => p.Id == a.ProductId).DiscountPercent/100);
                }
                
                entity.Add(itemWithDayDiscount);
                articleWithDayDiscount.ForEach(a => articleFromCart.Remove(a));
                productWithDayDiscount.ForEach(a => products.Remove(a));
            }

            //PROMOCODE DISCOUNT
            List<PromocodeArticle> promocodeArticles = promocodeArticleService.GetAll().Where(pa => pa.PromocodeId == promocode.Id).ToList();
            if (promocodeArticles.Count() > 0)
            {
                List<ArticleViewModel> articleWithPromocode = articleFromCart.Where(a => promocodeArticles.Select(p => p.ArticleId).Contains(a.Id)).ToList();
                PriceToCount itemWithPromocode = new PriceToCount()
                {
                    Articles = articleWithPromocode,
                    TotalPrice = 0,//articleWithPromocode.Select(a => a.Price).Sum() * (1-promocode.DiscountPercent/100),
                    WhatDiscount = "Promocode"
                };
                //Подсчет суммы
                articleWithPromocode.ForEach(a => itemWithPromocode.TotalPrice += a.Price * cart.SingleOrDefault(c => c.Article.Id == a.Id).ItemQuantity);
                //Применение скидки
                itemWithPromocode.TotalPrice = itemWithPromocode.TotalPrice * (1 - promocode.DiscountPercent / 100);

                entity.Add(itemWithPromocode);
                articleWithPromocode.ForEach(a => articleFromCart.Remove(a));
                products.Where(p=> articleWithPromocode.Select(a=> a.ProductId).Contains(p.Id)).ToList().ForEach(a => products.Remove(a));//НЕ БУДЕТ ЛИ ОШИБКА???
            }

            //WITHOUT DISCOUNT
            if (articleFromCart.Count() > 0)
            {
                PriceToCount itemWithoutDiscount = new PriceToCount()
                {
                    Articles = articleFromCart,
                    TotalPrice = 0,//articleFromCart.Select(a => a.Price).Sum(),
                    WhatDiscount = "Without Discount"
                };
                articleFromCart.ForEach(a => itemWithoutDiscount.TotalPrice += a.Price * cart.SingleOrDefault(c => c.Article.Id == a.Id).ItemQuantity);
                entity.Add(itemWithoutDiscount);
            }

            return Json(entity);
        }

        public bool IncludeArrayToArray(List<string> first, List<string> second)
        {
            bool ret = true;
            foreach(var i in first)
            {
                if (!second.Contains(i)) ret = false;
            }
            return ret;
        }
    }

    public class PriceToCount
    {
        public decimal TotalPrice { get; set; }
        public List<ArticleViewModel> Articles { get; set; }
        public string WhatDiscount { get; set; }
    }
}