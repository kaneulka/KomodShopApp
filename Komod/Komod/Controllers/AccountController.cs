using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Komod.Data;
using Komod.Ser;
using Komod.Ser.ArticleSer;
using Komod.Ser.DeliveryMethodSer;
using Komod.Ser.OrderSer;
using Komod.Ser.OrderStatusSer;
using Komod.Ser.PaymentMethodSer;
using Komod.Ser.ProductSer;
using Komod.Ser.PropertyValCatArtSer;
using Komod.Ser.PropertyValueSer;
using Komod.Ser.StockStatusSer;
using Komod.Web.Models.AccountModels;
using Komod.Web.Models.OrderItemModels;
using Komod.Web.Models.PropertyModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Komod.Web.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IOrderService orderService;
        private readonly IArticleService articleService;
        private readonly IOrderItemService orderItemService;
        private readonly IOrderStatusService orderStatusService; 
        private readonly IPaymentMethodService paymentMethodService;
        private readonly IDeliveryMethodService deliveryMethodService;
        private readonly IProductService productService;
        private readonly IPropertyValueService propertyValueService;
        private readonly IPropertyValCatArtService propertyValCatArtValueService;
        private readonly IStockStatusService stockStatusService;
        IWebHostEnvironment _appEnvironment;

        public AccountController(UserManager<User> userManager, 
            SignInManager<User> signInManager, 
            IArticleService articleService,
            IOrderService orderService, 
            IOrderItemService orderItemService,
            IOrderStatusService orderStatusService,
            IPaymentMethodService paymentMethodService,
            IPropertyValueService propertyValueService,
            IPropertyValCatArtService propertyValCatArtValueService,
            IProductService productService,
            IStockStatusService stockStatusService,
            IWebHostEnvironment appEnvironment,
            IDeliveryMethodService deliveryMethodService
        )
        {
            _userManager = userManager;
            _signInManager = signInManager;
            this.orderService = orderService;
            this.orderItemService = orderItemService;
            this.orderStatusService = orderStatusService;
            this.articleService = articleService;
            this.productService = productService;
            this.paymentMethodService = paymentMethodService;
            this.propertyValueService = propertyValueService;
            this.propertyValCatArtValueService = propertyValCatArtValueService;
            this.stockStatusService = stockStatusService;
            _appEnvironment = appEnvironment;
            this.deliveryMethodService = deliveryMethodService;
        }

        [AcceptVerbs("Get", "Post")]
        public IActionResult CheckEmail(string email)
        {
            var user = _userManager.Users.SingleOrDefault(u => u.Email == email);

            return Json(user == null ? true : false);
        }
        public IActionResult CheckEmailNotConfirmOrNotExist(string email)
        {
            var user = _userManager.Users.SingleOrDefault(u => u.Email == email);

            if (user == null) return Json(false);

            return Json(user.EmailConfirmed == true ? true : false);
        }

        [AcceptVerbs("Get", "Post")]
        public IActionResult CheckPassword(string password)
        {
            bool checkedPwd = true;

            Regex regex = new Regex(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^\da-zA-Z]).{8,}$");

            MatchCollection matches = regex.Matches(password);

            if (matches.Count <= 0)
            {
                checkedPwd = false;
            }

            return Json(checkedPwd);
        }

        [HttpGet]
        public IActionResult Register()
        {
            RegisterViewModel model = new RegisterViewModel();
            return PartialView("_Register", model);
        }
        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                User user = new User
                {
                    Email = model.Email,
                    UserName = model.UserName,
                    UserFIO = ""
                };

                // добавляем пользователя
                var result = await _userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    var callbackUrl = Url.Action(
                        "ConfirmEmail",
                        "Account",
                        new { userId = user.Id, code },
                        protocol: HttpContext.Request.Scheme
                        );
                    await _userManager.AddToRoleAsync(user, "user");
                    EmailService emailService = new EmailService();
                    string message = $"<tr>" +
                                                        $"<td>" +
                                                            $"<table cellspacing='0' cellpadding='0' width='820' align='center' border='0'>" +
                                                                $"<tbody>" +
                                                                    $"<tr>" +
                                                                        $"<td style='FONT-SIZE:18px;FONT-FAMILY:Trebuchet MS;FONT-WEIGHT:bold;COLOR:#212121;LINE-HEIGHT:20px;PADDING-TOP:28px;'>" +
                                                                            $"Подтвердите ваш Email <a href='{callbackUrl}' style='FONT-SIZE:18px;FONT-WEIGHT:bold;COLOR:#D80000;'>перейдя по этой ссылке</a>" +
                                                                        $"</td>" +
                                                                    $"</tr>" +
                                                                $"</tbody>" +
                                                            $"</table>";
                    await emailService.SendEmailAsync(model.Email, "Набитый комод - подтверждение email", message);
                    //await SendConfirmationEmail(user);
                    return RedirectToAction("Index", "Home", "open-confirm-send-modal");
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
            }
            return View("_Register", model);
        }

        public async Task SendConfirmationEmail(User user)
        {
            var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            var callbackUrl = Url.Action(
                "ConfirmEmail",
                "Account",
                new { userId = user.Id, code },
                protocol: HttpContext.Request.Scheme
                );
            await _userManager.AddToRoleAsync(user, "user");
            EmailService emailService = new EmailService();
            await emailService.SendEmailAsync(user.Email, "Confirm your account", $"Подтвердите ваш Email <a href='{callbackUrl}'>перейдя по этой ссылке</a>");
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> ConfirmEmail(string userId, string code)
        {
            if (userId == null || code == null)
            {
                return RedirectToAction("Index", "Home");
            }
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return View("Error");
            }
            var result = await _userManager.ConfirmEmailAsync(user, code);
            if (result.Succeeded)
            {
                return RedirectToAction("Index", "Home", "open-confirm-email-modal");
            }
            else
            {
                return StatusCode(503);
            }
            //return View(result.Succeeded ? "ConfirmEmail" : "Error");
        }

        [HttpGet]
        public ActionResult Login()
        {
            LoginViewModel model = new LoginViewModel();
            return PartialView("_Login", model);
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                model.RememberMe = true;

                var user = await _userManager.FindByEmailAsync(model.Email);
                if (user != null)
                {
                    // проверяем, подтвержден ли email
                    if (!await _userManager.IsEmailConfirmedAsync(user))
                    {
                        ModelState.AddModelError(string.Empty, "Вы не подтвердили свой email");
                        return View("_Login", model);
                    }
                }

                var result = await _signInManager.PasswordSignInAsync(user.UserName, model.Password, model.RememberMe, false);
                if (result.Succeeded)
                {
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ModelState.AddModelError("", "Неправильный логин и (или) пароль");
                }
            }
            return Json(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> LogOff(string returnUrl)
        {
            // удаляем аутентификационные куки
            await _signInManager.SignOutAsync();
            if (!string.IsNullOrEmpty(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }


        [HttpGet]
        public async Task<IActionResult> YourAccount()
        {
            if (User.Identity.IsAuthenticated)
            {
                ClaimsPrincipal currentUser = this.User;
                User user = await _userManager.GetUserAsync(currentUser);
                AccountViewModel model = new AccountViewModel
                {
                    Id = user.Id,
                    Email = user.Email,
                    UserFIO = user.UserFIO,
                    PhoneNumber = user.PhoneNumber
                };
                return View(model);
            }
            else
            {
                return StatusCode(401);
            }
        }
        [HttpPost]
        public async Task<IActionResult> YourAccount(AccountViewModel model)
        {
            ClaimsPrincipal currentUser = this.User;
            User user = await _userManager.GetUserAsync(currentUser);
            //List<Order> orders = orderService.GetOrders().Where(o => o.UserName == user.UserName).ToList();//Если измениться имя, изменить его в имени заказа
            user.UserFIO = model.UserFIO;
            user.Email = model.Email;
            user.PhoneNumber = model.PhoneNumber;
            //user.PasswordHash = model.NewPassword.PasswordHash;
            if (ModelState.IsValid)
            {
                if (model.NewPassword != null && model.OldPassword != null)
                {
                    IdentityResult result = await _userManager.ChangePasswordAsync(user, model.OldPassword, model.NewPassword);
                    if (result.Succeeded)
                    {
                        await _userManager.UpdateAsync(user);
                        return RedirectToAction("YourAccount", "Account");
                    }
                    else
                    {
                        foreach (var error in result.Errors)
                        {
                            if (error.Code == "PasswordMismatch")
                            {
                                ModelState.AddModelError("OldPassword", "Неправильно введен старый пароль!");
                            }
                            if (error.Code == "PasswordRequiresNonAlphanumeric")
                            {
                                ModelState.AddModelError("NewPassword", "Пароль должен состоять из A-Z, a-z, 0-9 и должен иметь один не алфавитно-цифровой символ");
                            }
                            ModelState.AddModelError(string.Empty, error.Description);
                        }

                    }
                }
                else
                {
                    await _userManager.UpdateAsync(user);
                    return RedirectToAction("YourAccount", "Account");
                }
            }
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> YourOrders()
        {
            if (User.Identity.IsAuthenticated)
            {
                ClaimsPrincipal currentUser = this.User;
                User user = await _userManager.GetUserAsync(currentUser);
                List<OrderViewModel> model = new List<OrderViewModel>();

                List<Order> orders = orderService.GetOrders().Where(o=> o.UserName == user.UserName).ToList();
                foreach(var o in orders)
                {
                    OrderViewModel order = new OrderViewModel
                    {
                        Id = o.Id,
                        TotalPrice = o.TotalPrice,
                        AddedDate = o.AddedDate,
                        OrderStatusName = orderStatusService.GetOrderStatus(o.OrderStatusId).Name,
                        OrderNumber = o.OrderNumber
                    };
                    model.Add(order);
                }
                model = model.OrderBy(m=> m.AddedDate).ToList();
                
                return View(model);
            }
            else
            {
                return StatusCode(401);
            }
        }

        [HttpGet]
        public IActionResult CancelOrder(long orderId)
        {
            Order order = orderService.GetOrder(orderId);
            ViewBag.OrderId = order.Id;
            ViewBag.OrderNumber = order.OrderNumber;
            return PartialView("_CancelOrder");
        }


        [HttpPost]
        public async Task<IActionResult> CancelOrderPost(long orderId)
        {
            OrderStatus orderStatus = orderStatusService.GetOrderStatuses().SingleOrDefault(os => os.Name == "Отменен");
            Order order = orderService.GetOrder(orderId);
            ClaimsPrincipal currentUser = this.User;
            User user = await _userManager.GetUserAsync(currentUser);

            List<string> productNames = new List<string>();
            decimal totalOrderPrice = 0;
            if (order.UserName == user.UserName)
            {
                order.OrderStatusId = orderStatus.Id;
                orderService.UpdateOrder(order);
                List<OrderItem> orderItems = orderItemService.GetOrderItems().Where(oi => oi.OrderId == orderId).ToList();
                List<Article> articles = articleService.GetArticles().Where(a => orderItems.Select(oi => oi.ArticleId).ToList().Contains(a.Id)).ToList();
                List<Article> articlesToUpdate = new List<Article>();
                List<long> productIds = new List<long>();
                List<StockStatus> ss = stockStatusService.GetStockStatuses().ToList();
                foreach (var a in articles)
                {
                    a.Quantity = orderItems.SingleOrDefault(oi => oi.ArticleId == a.Id).Quantity + a.Quantity;
                    if (a.StockStatusId == ss.SingleOrDefault(ss => ss.Name == "Нет в наличии").Id) a.StockStatusId = ss.SingleOrDefault(ss => ss.Name == "В наличии").Id;
                    totalOrderPrice += orderItems.SingleOrDefault(oi => oi.ArticleId == a.Id).Quantity * a.Price;
                    productNames.Add(a.ProductName);

                    articlesToUpdate.Add(a);
                    if (!productIds.Contains(a.ProductId)) productIds.Add(a.ProductId);
                }
                articleService.UpdateArticles(articlesToUpdate);
                foreach (var pi in productIds)
                {
                    UpdateProduct(pi);
                }


                //Письмо с заказом

                var message =                       $"<tr>" +
                                                        $"<td>" +
                                                            $"<table cellspacing='0' cellpadding='0' width='820' align='center' border='0'>" +
                                                                $"<tbody>" +
                                                                    $"<tr>" +
                                                                        $"<td style='FONT-SIZE:18px;FONT-FAMILY:Trebuchet MS;FONT-WEIGHT:bold;COLOR:#212121;LINE-HEIGHT:20px;PADDING-TOP:28px;'>" +
                                                                            order.ClientFIO + $", Ваш заказ <span style='FONT-SIZE:18px;FONT-WEIGHT:bold;COLOR:#D80000;'>№ " + order.OrderNumber + "</span> был отменен!" +
                                                                        $"</td>" +
                                                                    $"</tr>" +
                                                                    $"<tr>" +
                                                                        $"<td style='FONT-SIZE:18px;FONT-FAMILY:Trebuchet MS;COLOR:#212121;LINE-HEIGHT:20px;PADDING-TOP:12px;'>" +
                                                                            $"Новый статус - " + order.OrderStatus.Name +
                                                                        $"</td>" +
                                                                    $"</tr>" +
                                                                $"</tbody>" +
                                                            $"</table>";
                //Письмо об отмене зкаказа
                EmailService emailService = new EmailService();
                await emailService.SendEmailAsync(user.Email, "Ваш заказ №" + order.OrderNumber + " был отменен!", message);//$"Подтвердите ваш Email <a href=''>перейдя по этой ссылке</a>");


                //Письмо для жести
                var newMessage = $"<H1>Заказ №" + order.OrderNumber + $" пользователя" + user.UserName + $"!</H1>";
                await emailService.SendEmailAsync("Nabitiy.Komod.tlt@yandex.ru", "Заказ №" + order.OrderNumber + " отменен", newMessage);

                return RedirectToAction("YourOrders");
            }
            return StatusCode(403);
        }
        
        [HttpGet]
        public async Task<IActionResult> OrderInfo(long orderId)
        {
            Order order = orderService.GetOrder(orderId);
            ClaimsPrincipal currentUser = this.User;
            User user = await _userManager.GetUserAsync(currentUser);

            if (order.UserName == user.UserName)
            {
                //DeliveryMethod dm = deliveryMethodService.GetDeliveryMethod(order.DeliveryMethodId);
                OrderViewModel ovm = new OrderViewModel
                {
                    Id = order.Id,
                    OrderNumber = order.OrderNumber,
                    AddedDate = order.AddedDate,
                    TotalPrice = order.TotalPrice,
                    Comment = order.Comment,
                    PaymentMethodId = order.PaymentMethodId,
                    PaymentMethodName = paymentMethodService.GetPaymentMethod(order.PaymentMethodId).Name,
                    OrderStatusId = order.OrderStatusId,
                    OrderStatusName = orderStatusService.GetOrderStatus(order.OrderStatusId).Name,
                    DeliveryMethodName = order.DeliveryMethod.Name,
                    UserName = order.UserName,
                    DeliveryPrice = order.
                    DeliveryPrice = order.DeliveryPrice
                };

                List<OrderItem> orderItems = orderItemService.GetOrderItems().Where(oi => oi.OrderId == ovm.Id).ToList();
                List<OrderItemViewModel> loivm = new List<OrderItemViewModel>();
                List<PropertyValue> propertyValues = propertyValueService.GetPropertyValues().ToList();
                List<PropertyValCatArt> pvca = propertyValCatArtValueService.GetAll().ToList();

                foreach (var oi in orderItems)
                {
                    OrderItemViewModel oivm = new OrderItemViewModel
                    {
                        Id = oi.Id,
                        Quantity = oi.Quantity,
                        UnitPrice = oi.UnitPrice,
                        TotalPrice = oi.TotalPrice
                    };
                    Article article = articleService.GetArticle(oi.ArticleId);
                    oivm.ArticleName = article.Name;
                    oivm.ProductName = productService.GetProduct(article.ProductId).Name;
                    List<PropertyValCatArt> articlePVCA = pvca.Where(p => p.ArticleId == article.Id).ToList();
                    List<long> pvcaIds = new List<long>();
                    foreach (var p in articlePVCA)
                    {
                        pvcaIds.Add(p.PropertyValueId);
                    }
                    List<PropertyValue> articlePV = propertyValues.Where(pv => pvcaIds.Contains(pv.Id)).ToList();
                    List<PropertyValueViewModel> lpvvm = new List<PropertyValueViewModel>();
                    foreach (var pv in articlePV)
                    {
                        PropertyValueViewModel pvvm = new PropertyValueViewModel
                        {
                            Value = pv.Value,
                            PropertyName = pv.Property.Name
                        };
                    }
                    oivm.Properties = lpvvm;
                    loivm.Add(oivm);
                }
                OrderInfo model = new OrderInfo
                {
                    Order = ovm,
                    OrderItems = loivm
                };
                return PartialView("_OrderInfo", model);
            }
            return StatusCode(403);
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

        //[HttpPost]
        //public async Task<IActionResult> YourOrders(AccountViewModel model)
        //{
        //    ClaimsPrincipal currentUser = this.User;
        //    User user = await _userManager.GetUserAsync(currentUser);
        //    user.UserFIO = model.UserFIO;
        //    user.Email = model.Email;
        //    user.PhoneNumber = model.PhoneNumber;
        //    //user.PasswordHash = model.NewPassword.PasswordHash;
        //    if (model.NewPassword != null)
        //    {
        //        var _passwordValidator =
        //            HttpContext.RequestServices.GetService(typeof(IPasswordValidator<User>)) as IPasswordValidator<User>;
        //        var _passwordHasher =
        //            HttpContext.RequestServices.GetService(typeof(IPasswordHasher<User>)) as IPasswordHasher<User>;
        //
        //        IdentityResult result = await _passwordValidator.ValidateAsync(_userManager, user, model.NewPassword);
        //        if (result.Succeeded)
        //        {
        //            user.PasswordHash = _passwordHasher.HashPassword(user, model.NewPassword);
        //            await _userManager.UpdateAsync(user);
        //        }
        //    }
        //    else
        //    {
        //        await _userManager.UpdateAsync(user);
        //    }
        //    return RedirectToAction("YourAccount", "Account");
        //}

        //[HttpGet]
        //public IActionResult YourBonuses()
        //{
        //    if (User.Identity.IsAuthenticated)
        //    {
        //        return View();
        //    }
        //    else
        //    {
        //        return StatusCode(401);
        //    }
        //}
        //[HttpPost]
        //public async Task<IActionResult> YourBonuses(AccountViewModel model)
        //{
        //    ClaimsPrincipal currentUser = this.User;
        //    User user = await _userManager.GetUserAsync(currentUser);
        //    user.UserFIO = model.UserFIO;
        //    user.Email = model.Email;
        //    user.PhoneNumber = model.PhoneNumber;
        //    //user.PasswordHash = model.NewPassword.PasswordHash;
        //    if (model.NewPassword != null)
        //    {
        //        var _passwordValidator =
        //            HttpContext.RequestServices.GetService(typeof(IPasswordValidator<User>)) as IPasswordValidator<User>;
        //        var _passwordHasher =
        //            HttpContext.RequestServices.GetService(typeof(IPasswordHasher<User>)) as IPasswordHasher<User>;
        //
        //        IdentityResult result = await _passwordValidator.ValidateAsync(_userManager, user, model.NewPassword);
        //        if (result.Succeeded)
        //        {
        //            user.PasswordHash = _passwordHasher.HashPassword(user, model.NewPassword);
        //            await _userManager.UpdateAsync(user);
        //        }
        //    }
        //    else
        //    {
        //        await _userManager.UpdateAsync(user);
        //    }
        //    return RedirectToAction("YourAccount", "Account");
        //}

    }
}