using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Komod.Data;
using Komod.Ser;
using Komod.Ser.ArticleSer;
using Komod.Ser.OrderSer;
using Komod.Ser.OrderStatusSer;
using Komod.Ser.PaymentMethodSer;
using Komod.Ser.ProductSer;
using Komod.Ser.PropertyValCatArtSer;
using Komod.Ser.PropertyValueSer;
using Komod.Ser.StockStatusSer;
using Komod.Web.Code;
using Komod.Web.Models.CartModels;
using Komod.Web.Models.OrderItemModels;
using Komod.Web.Models.PropertyModels;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Komod.Ser.DeliveryMethodSer;
using Komod.Web.Models.Methods;
using NPOI.SS.Formula.Functions;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Komod.Ser.PromocodeSer;
using Komod.Ser.ProductSetSer;
using Komod.Ser.PromocodeArticleSer;
using Komod.Web.Models.PromocodeModels;

namespace Komod.Web.Controllers
{
    public class OrderController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IArticleService articleService;
        private readonly IProductService productService;
        private readonly IPropertyValueService propertyValueService;
        private readonly IPropertyValCatArtService propertyValCatArtValueService;
        private readonly IOrderService orderService;
        private readonly IOrderItemService orderItemService;
        private readonly IOrderStatusService orderStatusService;
        private readonly IPaymentMethodService paymentMethodService;
        private readonly IStockStatusService stockStatusService;
        private readonly IDeliveryMethodService deliveryMethodService;
        private readonly IPromocodeService promocodeService;
        private readonly IPromocodeArticleService promocodeArticleService;
        private readonly IProductSetService productSetService;


        public OrderController(UserManager<User> userManager,
            SignInManager<User> signInManager,
            IArticleService articleService,
            IDeliveryMethodService deliveryMethodService,
            IProductService productService,
            IPropertyValueService propertyValueService,
            IPropertyValCatArtService propertyValCatArtValueService,
            IOrderService orderService,
            IPaymentMethodService paymentMethodService,
            IOrderStatusService orderStatusService,
            IOrderItemService orderItemService,
            IStockStatusService stockStatusService,
            IProductSetService productSetService,
            IPromocodeService promocodeService,
            IPromocodeArticleService promocodeArticleService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            this.productSetService = productSetService;
            this.articleService = articleService;
            this.deliveryMethodService = deliveryMethodService;
            this.productService = productService;
            this.propertyValueService = propertyValueService;
            this.propertyValCatArtValueService = propertyValCatArtValueService;
            this.orderService = orderService;
            this.orderItemService = orderItemService;
            this.orderStatusService = orderStatusService;
            this.paymentMethodService = paymentMethodService;
            this.stockStatusService = stockStatusService;
            this.promocodeService = promocodeService;
            this.promocodeArticleService = promocodeArticleService;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<ActionResult> OrderPage()
        {
            var cart = SessionHelper.GetObjectFromJson<List<CartItemViewModel>>(HttpContext.Session, "cart");
            ClaimsPrincipal currentUser = this.User;
            User user = await _userManager.GetUserAsync(currentUser);

            //Поиск промокода
            Promocode promocode = promocodeService.GetPromocodes().SingleOrDefault(p => p.Name == cart[0].PromocodeName);
            if (promocode == null)
            {
                return Json("Промокод не существует");
            }
            else 
            {
                if (promocode.EndOfPromocode < DateTime.Now) return Json("Срок действия промокода истек");

                if (User.Identity.IsAuthenticated)
                {
                    List<Order> orders = orderService.GetOrders().Where(o=> o.UserName == user.UserName).ToList();
                    if (orders.Select(o=>o.PromoName).Contains(promocode.Name)) return Json("Промокод уже был вами использован");
                }
                else
                {
                    return Json("Пользователь не авторизован");
                }
            }
            //List<long> articleIds = promocodeArticleService.GetAll().Where(pa => pa.PromocodeId == promocode.Id).Select(pa=> pa.ArticleId).ToList();
            //List<Article> articles = articleService.GetArticles().Where(a => articleIds.Contains(a.Id)).ToList();
            List<Article> articles = new List<Article>();
            foreach (var a in promocode.PromocodeArticles)
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

            List<CartItemViewModel> orderItems = new List<CartItemViewModel>();
            foreach(var c in cart) 
            {
                if (entity.ArticleNames.Contains(c.Article.Name) )
                {
                    c.IsPromocodeExistForArticle = true;
                    c.DiscountPercent = entity.Promocode.DiscountPercent;
                }
                else 
                {
                    c.IsPromocodeExistForArticle = false;
                    c.DiscountPercent = 0;
                }
                orderItems.Add(c);
            }


            ViewBag.cart = orderItems;
            ViewBag.total = orderItems.Sum(item => (item.Article.Price * (1 - item.DiscountPercent/100)) * item.ItemQuantity);
            ViewBag.totalWithoutDisc = orderItems.Sum(item => (item.Article.Price * item.ItemQuantity));


            List<DeliveryMethod> ldm = deliveryMethodService.GetDeliveryMethods().ToList();
            List<DeliveryMethodViewModel> ldmvm = new List<DeliveryMethodViewModel>();
            foreach (var dm in ldm)
            {
                DeliveryMethodViewModel dmvm = new DeliveryMethodViewModel
                {
                    Id = dm.Id,
                    Name = dm.Name,
                    DeliveryPrice = dm.DeliveryPrice,
                    District = dm.District,
                    FreeDelivery = dm.FreeDelivery
                };
                ldmvm.Add(dmvm);
            }
            ViewBag.DeliveryMethods = ldmvm;
            ViewBag.UserOrder = new User { Email = user.Email, PhoneNumber = user.PhoneNumber, UserFIO = user.UserFIO };
            return View("OrderPage");
        }

        //ПОСЧИТАТЬ НОВУЮ ЦЕНУ СОС КИДКОЙ ПРИ ОФОРМЛЕНИИ ПИСЬМА ПОСЛЕ СОЗДАНИЯ
        [HttpPost]
        public async Task<ActionResult> CreateOrder(OrderInfo model, int totalOrderPrice)
        {
            var cart = SessionHelper.GetObjectFromJson<List<CartItemViewModel>>(HttpContext.Session, "cart");
            ClaimsPrincipal currentUser = this.User;
            User user = await _userManager.GetUserAsync(currentUser);
            PaymentMethod pm = paymentMethodService.GetPaymentMethods().SingleOrDefault(pm => pm.Name.ToUpper() == model.Order.PaymentMethodName.ToUpper());
            DeliveryMethod dm = deliveryMethodService.GetDeliveryMethods().SingleOrDefault(d => d.Name.ToUpper() == model.Order.DeliveryMethodName.ToUpper());
            if (ModelState.IsValid)
            {
                Order order = new Order
                {
                    UserName = user.UserName,
                    PaymentMethodId = pm.Id,
                    OrderStatusId = orderStatusService.GetOrderStatuses().SingleOrDefault(os => os.Name == "Обрабатывается").Id, //Ну или как там назвать его
                    AddedDate = DateTime.Now,
                    DeliveryMethodId = dm.Id,
                    Comment = model.Order.Comment,
                    ClientFIO = model.Order.ClientFIO,
                    ClientPhone = model.Order.ClientPhone,
                    ClientOtherPhone = model.Order.ClientOtherPhone,
                    ClientEmail = model.Order.ClientEmail,
                    ClientAddress = model.Order.ClientAddress,
                    DeliveryPrice = dm.DeliveryPrice,
                    DiscountType = model.Order.DiscountType,
                    DiscountPercent = model.Order.DiscountPercent,
                    PromoName = model.Order.PromoName
                };
                orderService.InsertOrder(order);

                //если промо
                if (order.DiscountType == "Promocode")
                {
                    Promocode promo = promocodeService.GetPromocodes().SingleOrDefault(o => order.PromoName == o.Name);
                    if (!(promo.Count < 0))
                    {
                        promo.Count--;
                        promocodeService.UpdatePromocode(promo);
                    }
                }

                order.OrderNumber = DateTime.Now.Day.ToString() + "-" + DateTime.Now.Month.ToString() + "-" + DateTime.Now.Year.ToString() + "-" + order.Id;
                decimal orderTotalPrice = new decimal();

                List<string> productNames = new List<string>();
                List<OrderItem> orderItems = new List<OrderItem>();
                List<long> articleIds = new List<long>();
                List<int> articleQuantity = new List<int>();
                foreach (var cartItem in cart)
                {
                    OrderItem orderItem = new OrderItem
                    {
                        Quantity = cartItem.ItemQuantity,
                        ArticleId = cartItem.Article.Id,
                        UnitPrice = cartItem.Article.Price,
                        TotalPrice = cartItem.ItemQuantity * cartItem.Article.Price,
                        OrderId = order.Id
                    };
                    articleIds.Add(cartItem.Article.Id);
                    articleQuantity.Add(cartItem.ItemQuantity);
                    productNames.Add(cartItem.Article.ProductName);
                    orderItems.Add(orderItem);
                    orderTotalPrice += orderItem.TotalPrice;
                }
                orderItemService.InsertOrderItems(orderItems);
                order.TotalPrice = orderTotalPrice;
                if (order.TotalPrice >= dm.FreeDelivery) order.DeliveryPrice = 0;
                orderService.UpdateOrder(order);

                ///
                List<Article> articlesToChange = articleService.GetArticles().Where(a => articleIds.Contains(a.Id)).ToList();
                for (var i = 0; i < articlesToChange.Count; i++)
                {
                    articlesToChange[i].Quantity = articlesToChange[i].Quantity - articleQuantity[i];
                    if (articlesToChange[i].Quantity <= 0)
                    {
                        articlesToChange[i].StockStatusId = stockStatusService.GetStockStatuses().SingleOrDefault(ss => ss.Name == "Нет в наличии").Id;
                    }
                }
                articleService.UpdateArticles(articlesToChange);

                //Письмо с заказом

                var message =                       $"<tr>" +
                                                        $"<td>" +
                                                            $"<table cellspacing='0' cellpadding='0' width='820' align='center' border='0'>" +
                                                                $"<tbody>" +
                                                                    $"<tr>" +
                                                                        $"<td style='FONT-SIZE:18px;FONT-FAMILY:Trebuchet MS;FONT-WEIGHT:bold;COLOR:#212121;LINE-HEIGHT:20px;PADDING-TOP:28px;'>" +
                                                                            order.ClientFIO + $", Ваш заказ <span style='FONT-SIZE:18px;FONT-WEIGHT:bold;COLOR:#D80000;'>№ " + order.OrderNumber + "</span> находится в обработке, с вами скоро свяжется наш менеджер" +
                                                                        $"</td>" +
                                                                    $"</tr>" +
                                                                    $"<tr>" +
                                                                        $"<td style='FONT-SIZE:18px;FONT-FAMILY:Trebuchet MS;COLOR:#212121;LINE-HEIGHT:20px;PADDING-TOP:12px;'>" +
                                                                            $"Новый статус - " + order.OrderStatus.Name +
                                                                        $"</td>" +
                                                                    $"</tr>" +
                                                                    $"<tr>" +
                                                                        $"<td style='FONT-SIZE:18px;FONT-FAMILY:Trebuchet MS;FONT-WEIGHT:bold;COLOR:#212121;LINE-HEIGHT:20px;PADDING-TOP:17px;'>" +
                                                                            $"Подробнее о заказе" +
                                                                        $"</td>" +
                                                                    $"</tr>" +
                                                                $"</tbody>" +
                                                            $"</table>" +
                                                            $"<table style='BORDER-BOTTOM:#212121 2px solid;' cellspacing='0' cellpadding='0' width='704' align='center' border='0'>" +
                                                                $"<tbody>" +
                                                                    $"<tr>" +
                                                                        $"<td style='FONT-SIZE:18px;FONT-FAMILY:Trebuchet MS;FONT-WEIGHT:bold;COLOR:#212121;LINE-HEIGHT:20px;PADDING-TOP:30px;PADDING-BOTTOM:10px;text-align:left;'>" +
                                                                            $"Название товара" +
                                                                        $"</td>" +
                                                                        $"<td style='FONT-SIZE:18px;FONT-FAMILY:Trebuchet MS;FONT-WEIGHT:bold;COLOR:#212121;LINE-HEIGHT:20px;PADDING-TOP:30px;PADDING-BOTTOM:10px;text-align:left;'>" +
                                                                            $"Кол-во" +
                                                                        $"</td>" +
                                                                        $"<td style='FONT-SIZE:18px;FONT-FAMILY:Trebuchet MS;FONT-WEIGHT:bold;COLOR:#212121;LINE-HEIGHT:20px;PADDING-TOP:30px;PADDING-BOTTOM:10px;text-align:left;'>" +
                                                                            $"Цена за шт." +
                                                                        $"</td>" +
                                                                        $"<td style='FONT-SIZE:18px;FONT-FAMILY:Trebuchet MS;FONT-WEIGHT:bold;COLOR:#212121;LINE-HEIGHT:20px;PADDING-TOP:30px;PADDING-BOTTOM:10px;text-align:right;'>" +
                                                                            $"Сумма" +
                                                                        $"</td>" +
                                                                    $"</tr>";
                                                                    foreach (var op in orderItems)
                                                                    {
                                                                        message += $"<tr>" +
                                                                                       $"<td style='FONT-SIZE:18px;FONT-FAMILY:Trebuchet MS;COLOR:#212121;LINE-HEIGHT:20px;PADDING-TOP:10px;PADDING-BOTTOM:10px;text-align:left;'>" +
                                                                                           productNames[orderItems.IndexOf(op)] +
                                                                                       $"</td>" +
                                                                                       $"<td style='FONT-SIZE:18px;FONT-FAMILY:Trebuchet MS;COLOR:#212121;LINE-HEIGHT:20px;PADDING-TOP:10px;PADDING-BOTTOM:10px;text-align:left;'>";
                                                                                       //double sumPrice = 0;
                                                                                       //for (var i = 0; i < cart.Count; i++)
                                                                                       //{
                                                                                       //    if (op.ArticleId == cart[i].Article.Id)
                                                                                       //    {
                                                                                       //        message += cart[i].Quantity;
                                                                                       //        sumPrice = op.Article.Price * cart[i].Quantity;
                                                                                       //    }
                                                                                       //}
                                                                                       message += op.Quantity;
                                                                            message += $"</td>" +
                                                                                       $"<td style='FONT-SIZE:18px;FONT-FAMILY:Trebuchet MS;COLOR:#212121;LINE-HEIGHT:20px;PADDING-TOP:10px;PADDING-BOTTOM:10px;text-align:left;'>" +
                                                                                           op.UnitPrice + $" руб." +
                                                                                       $"</td>" +
                                                                                       $"<td style='FONT-SIZE:18px;FONT-FAMILY:Trebuchet MS;COLOR:#212121;LINE-HEIGHT:20px;PADDING-TOP:10px;PADDING-BOTTOM:10px;text-align:right;'>" +
                                                                                           op.TotalPrice + $" руб." +
                                                                                       $"</td>" +
                                                                                   $"</tr>";
                                                                        //sumWODelivery += sumPrice;
                                                                    }
                                                     message += $"</tbody>" +
                                                            $"</table>" +
                                                            $"<table cellspacing='0' cellpadding='0' width='704' align='center' border='0'>" +
                                                                $"<tbody>" +
                                                                    $"<tr>" +
                                                                        $"<td style='FONT-SIZE:18px;FONT-FAMILY:Trebuchet MS;FONT-WEIGHT:bold;COLOR:#212121;LINE-HEIGHT:20px;PADDING-TOP:30px;text-align:left;'>" +
                                                                            $"Итого:" +
                                                                        $"</td>" +
                                                                        $"<td style='FONT-SIZE:18px;FONT-FAMILY:Trebuchet MS;COLOR:#212121;LINE-HEIGHT:20px;PADDING-TOP:30px;text-align:right;'>" +
                                                                            orderTotalPrice + $" руб." +
                                                                        $"</td>" +
                                                                    $"</tr>" +
                                                                $"</tbody>" +
                                                            $"</table>" +
                                                            $"<table cellspacing='0' cellpadding='0' width='704' align='center' border='0'>" +
                                                                $"<tbody>" +
                                                                    $"<tr>" +
                                                                        $"<td style='FONT-SIZE:18px;FONT-FAMILY:Trebuchet MS;FONT-WEIGHT:bold;COLOR:#212121;LINE-HEIGHT:20px;PADDING-TOP:30px;text-align:left;'>" +
                                                                            $"Форма оплаты:" +
                                                                        $"</td>" +
                                                                        $"<td style='FONT-SIZE:18px;FONT-FAMILY:Trebuchet MS;COLOR:#212121;LINE-HEIGHT:20px;PADDING-TOP:30px;text-align:right;'>" +
                                                                            pm.Name +
                                                                        $"</td>" +
                                                                    $"</tr>" +
                                                                $"</tbody>" +
                                                            $"</table>" +
                                                            $"<table cellspacing='0' cellpadding='0' width='704' align='center' border='0'>" +
                                                                $"<tbody>" +
                                                                    $"<tr>" +
                                                                        $"<td style='FONT-SIZE:18px;FONT-FAMILY:Trebuchet MS;FONT-WEIGHT:bold;COLOR:#212121;LINE-HEIGHT:20px;PADDING-TOP:30px;text-align:left;'>" +
                                                                            $"Доставка:" +
                                                                        $"</td>" +
                                                                        $"<td style='FONT-SIZE:18px;FONT-FAMILY:Trebuchet MS;COLOR:#212121;LINE-HEIGHT:20px;PADDING-TOP:30px;text-align:right;'>" +
                                                                            order.DeliveryMethod +
                                                                        $"</td>" +
                                                                    $"</tr>" +
                                                                $"</tbody>" +
                                                            $"</table>" +
                                                            $"<table cellspacing='0' cellpadding='0' width='704' align='center' border='0'>" +
                                                                $"<tbody>" +
                                                                    $"<tr>" +
                                                                        $"<td style='FONT-SIZE:18px;FONT-FAMILY:Trebuchet MS;FONT-WEIGHT:bold;COLOR:#212121;LINE-HEIGHT:20px;PADDING-TOP:30px;PADDING-BOTTOM:28px;text-align:left;'>" +
                                                                            $"Общая сумма заказа:" +
                                                                        $"</td>" +
                                                                        $"<td style='FONT-SIZE:18px;FONT-FAMILY:Trebuchet MS;COLOR:#212121;LINE-HEIGHT:20px;PADDING-TOP:30px;text-align:right;'>" +
                                                                            totalOrderPrice + $" руб." +
                                                                        $"</td>" +
                                                                    $"</tr>" +
                                                                $"</tbody>" +
                                                            $"</table>" +
                                                        $"</td>" +
                                                    $"</tr>";
            
                EmailService emailService = new EmailService();
                await emailService.SendEmailAsync(user.Email, "Ваш заказ №" + order.OrderNumber, message, "Набитый комод");//$"Подтвердите ваш Email <a href=''>перейдя по этой ссылке</a>");
                cart.Clear();
                SessionHelper.SetObjectAsJson(HttpContext.Session, "cart", cart);

                //Письмо для жести
                var newMessage = $"<H1>Появился новый заказ #" + order.OrderNumber + $" пользователя" + user.UserName + $"!</H1>" + message;
                await emailService.SendEmailAsync("Nabitiy.Komod.tlt@yandex.ru", "Появился новый заказ заказ №" + order.OrderNumber, newMessage, "Набитый комод");

                //if (order.Id > 0)
                //{
                return RedirectToAction("Confirm");
                //}
            }
            return StatusCode(500);
        }

        [HttpGet]
        public ActionResult Confirm()
        {
            return View();
        }

        [HttpGet]
        public ActionResult OrderInfo(long orderId)
        {
            Order order = orderService.GetOrder(orderId);
            //DeliveryMethod dm = deliveryMethodService.GetDeliveryMethod(
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
                ClientFIO = order.ClientFIO,
                DeliveryPrice = order.DeliveryPrice,
                DiscountPercent = order.DiscountPercent,
                DiscountType = order.DiscountType
            };

            List<OrderItem> orderItems = orderItemService.GetOrderItems().Where(oi => oi.OrderId == ovm.Id).ToList();
            List<OrderItemViewModel> loivm = new List<OrderItemViewModel>();
            List<PropertyValue>  propertyValues = propertyValueService.GetPropertyValues().ToList();
            List<PropertyValCatArt> pvca = propertyValCatArtValueService.GetAll().ToList();
            
            foreach (var oi in orderItems)
            {
                OrderItemViewModel oivm = new OrderItemViewModel
                {
                    Id = oi.Id,
                    Quantity = oi.Quantity,
                    UnitPrice = oi.UnitPrice,
                    TotalPrice = oi.TotalPrice,
                };
                Article article = articleService.GetArticle(oi.ArticleId);
                Product product = productService.GetProduct(article.ProductId);
                oivm.ArticleName = article.Name;
                oivm.ProductId = product.Id;
                oivm.ProductName = product.Name;
                List<PropertyValCatArt> articlePVCA = pvca.Where(p=> p.ArticleId == article.Id).ToList();
                List<long> pvcaIds = new List<long>();
                foreach(var p in articlePVCA)
                {
                    pvcaIds.Add(p.PropertyValueId);
                }
                List<PropertyValue> articlePV = propertyValues.Where(pv=> pvcaIds.Contains(pv.Id)).ToList();
                List<PropertyValueViewModel> lpvvm = new List<PropertyValueViewModel>();
                foreach(var pv in articlePV)
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

        //Персональный ли код
        [HttpGet]
        public async Task<bool> IsUserUsePromo(string promoName)
        {
            ClaimsPrincipal currentUser = this.User;
            User user = await _userManager.GetUserAsync(currentUser);

            List<Order> orders = orderService.GetOrders().Where(o => o.UserName == user.UserName).ToList();

            bool taskAnswer = false;
            if (orders.Select(o => o.PromoName).Contains(promoName)) taskAnswer = true;
            return taskAnswer;
        }

        //Сколько осталось
        [HttpGet]
        public async Task<bool> IsCountIsEnoughPromo(string promoName)
        {
            ClaimsPrincipal currentUser = this.User;
            User user = await _userManager.GetUserAsync(currentUser);

            Promocode promo = promocodeService.GetPromocodes().SingleOrDefault(p => p.Name == promoName);

            bool taskAnswer = false;
            if (promo.Count < 0) return taskAnswer;
            if (promo.Count > 0) taskAnswer = true;
            return taskAnswer;
        }

        //Товары в комплекте

    }
}