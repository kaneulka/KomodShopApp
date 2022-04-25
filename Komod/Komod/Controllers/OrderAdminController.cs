using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Komod.Data;
using Komod.Ser.ArticleSer;
using Komod.Ser.OrderSer;
using Komod.Ser.OrderStatusSer;
using Komod.Ser.PaymentMethodSer;
using Komod.Ser.PropertyValueSer;
using Komod.Ser.PropertyValCatArtSer;
using Komod.Ser.ProductSer;
using Komod.Web.Models;
using Komod.Web.Models.OrderItemModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Komod.Web.Models.PropertyModels;
using System.IO;
using Komod.Ser.StockStatusSer;
using Microsoft.AspNetCore.Hosting;
using Komod.Ser;
using System.Security.Claims;
using Komod.Ser.DeliveryMethodSer;

namespace Komod.Web.Controllers
{
    [Authorize(Roles = "admin")]
    public class OrderAdminController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IArticleService articleService;
        private readonly IDeliveryMethodService deliveryMethodService;
        private readonly IProductService productService;
        private readonly IPropertyValueService propertyValueService;
        private readonly IPropertyValCatArtService propertyValCatArtValueService;
        private readonly IOrderService orderService;
        private readonly IOrderItemService orderItemService;
        private readonly IOrderStatusService orderStatusService;
        private readonly IPaymentMethodService paymentMethodService;
        private readonly IStockStatusService stockStatusService;
        IWebHostEnvironment _appEnvironment;

        public OrderAdminController(UserManager<User> userManager, 
            SignInManager<User> signInManager, 
            IArticleService articleService, 
            IProductService productService, 
            IPropertyValueService propertyValueService,
            IPropertyValCatArtService propertyValCatArtValueService,
            IOrderService orderService, 
            IPaymentMethodService paymentMethodService, 
            IOrderStatusService orderStatusService, 
            IOrderItemService orderItemService,
            IStockStatusService stockStatusService,
            IWebHostEnvironment appEnvironment,
            IDeliveryMethodService deliveryMethodService
        )
        {
            _userManager = userManager;
            _signInManager = signInManager;
            this.articleService = articleService;
            this.productService = productService;
            this.orderService = orderService;
            this.orderItemService = orderItemService;
            this.orderStatusService = orderStatusService;
            this.paymentMethodService = paymentMethodService;
            this.propertyValueService = propertyValueService;
            this.propertyValCatArtValueService = propertyValCatArtValueService;
            this.stockStatusService = stockStatusService;
            _appEnvironment = appEnvironment;
            this.deliveryMethodService = deliveryMethodService;

        }


        [HttpGet]
        public IActionResult Orders(int page = 1, int sortType = 0, string searchString = null)
        {
            IEnumerable<Order> orders;
            if (searchString == null)
            {
                orders = orderService.GetOrders();
            }
            else
            {
                searchString = searchString.ToUpper();
                orders = orderService.GetOrders().Where(s => s.OrderNumber.ToUpper().Contains(searchString)
                    || s.AddedDate.ToString().ToUpper().Contains(searchString)
                    || s.OrderStatus.Name.ToUpper().Contains(searchString)
                );
            }

            switch (sortType)
            {
                case 0:
                    orders = orders.OrderByDescending(b => b.AddedDate);
                    break;
                case 1:
                    orders = orders.OrderBy(b => b.AddedDate);
                    break;
                //case 2:
                //    orders = orders.OrderByDescending(b => b.ModifiedDate);
                //    break;
                //case 3:
                //    orders = orders.OrderBy(b => b.ModifiedDate);
                //    break;
                case 4:
                    orders = orders.OrderBy(b => b.OrderNumber);
                    break;
                case 5:
                    orders = orders.OrderByDescending(b => b.OrderNumber);
                    break;
            }
            ViewBag.SortType = sortType;

            int pageSize = 20;

            List<OrderViewModel> listOrders = new List<OrderViewModel>();

            var count = orders.Count();
            var items = orders.Skip((page - 1) * pageSize).Take(pageSize).ToList();
            PageViewModel pageViewModel = new PageViewModel(count, page, pageSize);
            items.ForEach(u =>
            {
                OrderViewModel order = new OrderViewModel
                {
                    Id = u.Id,
                    OrderNumber = u.OrderNumber,
                    AddedDate = u.AddedDate,
                    UserName = u.UserName,
                    OrderStatusName = orderStatusService.GetOrderStatus(u.OrderStatusId).Name,
                    DeliveryMethodName = u.DeliveryMethod.Name
                };
                listOrders.Add(order);
            });

            OrdersViewModel viewModel = new OrdersViewModel
            {
                PageViewModel = pageViewModel,
                Orders = listOrders
            };

            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> CloseOrder(long orderId)
        {
            OrderStatus orderStatus = orderStatusService.GetOrderStatuses().SingleOrDefault(os => os.Name == "Выполнен");
            Order order = orderService.GetOrder(orderId);
            order.OrderStatusId = orderStatus.Id;
            orderService.UpdateOrder(order);

            var message = $"<tr>" +
                                                        $"<td>" +
                                                            $"<table cellspacing='0' cellpadding='0' width='820' align='center' border='0'>" +
                                                                $"<tbody>" +
                                                                    $"<tr>" +
                                                                        $"<td style='FONT-SIZE:18px;FONT-FAMILY:Trebuchet MS;FONT-WEIGHT:bold;COLOR:#212121;LINE-HEIGHT:20px;PADDING-TOP:28px;'>" +
                                                                            order.ClientFIO + $", Ваш заказ <span style='FONT-SIZE:18px;FONT-WEIGHT:bold;COLOR:#D80000;'>№ " + order.OrderNumber + "</span> был выполнен!" +
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
            await emailService.SendEmailAsync(order.ClientEmail, "Ваш заказ №" + order.OrderNumber + " был выполнен!", message);//$"Подтвердите ваш Email <a href=''>перейдя по этой ссылке</a>");

            return RedirectToAction("Orders", "OrderAdmin");
        }

        [HttpPost]
        public async Task<IActionResult> CancelOrder(long orderId)
        {
            OrderStatus orderStatus = orderStatusService.GetOrderStatuses().SingleOrDefault(os => os.Name == "Отменен");
            Order order = orderService.GetOrder(orderId);
            order.OrderStatusId = orderStatus.Id;
            orderService.UpdateOrder(order);
            List<OrderItem> orderItems = orderItemService.GetOrderItems().Where(oi=> oi.OrderId == orderId).ToList();
            List<Article> articles = articleService.GetArticles().Where(a=> orderItems.Select(oi=> oi.ArticleId).ToList().Contains(a.Id ) ).ToList();
            List<Article> articlesToUpdate = new List<Article>();
            List<long> productIds = new List<long>();
            List<StockStatus> ss = stockStatusService.GetStockStatuses().ToList();
            foreach (var a in articles)
            {
                a.Quantity = orderItems.SingleOrDefault(oi=> oi.ArticleId == a.Id).Quantity + a.Quantity;
                if (a.StockStatusId == ss.SingleOrDefault(ss => ss.Name == "Нет в наличии").Id) a.StockStatusId = ss.SingleOrDefault(ss => ss.Name == "В наличии").Id;
                articlesToUpdate.Add(a);
                if (!productIds.Contains(a.ProductId)) productIds.Add(a.ProductId);
            }
            articleService.UpdateArticles(articlesToUpdate);
            foreach(var pi in productIds)
            {
                UpdateProduct(pi);
            }


            var message = $"<tr>" +
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
            await emailService.SendEmailAsync(order.ClientEmail, "Ваш заказ №" + order.OrderNumber + " Отменен", message);


            return RedirectToAction("Orders", "OrderAdmin");
        }

        [HttpPost]
        public async Task<IActionResult> ConfirmOrder(long orderId)
        {
            OrderStatus orderStatus = orderStatusService.GetOrderStatuses().SingleOrDefault(os => os.Name == "Подтвержден");
            Order order = orderService.GetOrder(orderId);
            order.OrderStatusId = orderStatus.Id;
            orderService.UpdateOrder(order);

            var message = $"<tr>" +
                                                        $"<td>" +
                                                            $"<table cellspacing='0' cellpadding='0' width='820' align='center' border='0'>" +
                                                                $"<tbody>" +
                                                                    $"<tr>" +
                                                                        $"<td style='FONT-SIZE:18px;FONT-FAMILY:Trebuchet MS;FONT-WEIGHT:bold;COLOR:#212121;LINE-HEIGHT:20px;PADDING-TOP:28px;'>" +
                                                                            order.ClientFIO + $", Ваш заказ <span style='FONT-SIZE:18px;FONT-WEIGHT:bold;COLOR:#D80000;'>№ " + order.OrderNumber + "</span> был подтвержден!" +
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
            await emailService.SendEmailAsync(order.ClientEmail, "Ваш заказ №" + order.OrderNumber + " Подтвержден", message);

            return RedirectToAction("Orders", "OrderAdmin");
        }

        [HttpGet]
        public IActionResult OrderInfo(long orderId)
        {
            Order order = orderService.GetOrder(orderId);
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
                ClientEmail = order.ClientEmail,
                ClientAddress = order.ClientAddress,
                ClientPhone = order.ClientPhone,
                ClientOtherPhone = order.ClientOtherPhone,
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
                    TotalPrice = oi.TotalPrice
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

        [HttpGet]
        public IActionResult ChangeDeliveryPrice(long orderId)
        {
            Order order = orderService.GetOrder(orderId);
            Order model = new Order
            {
                Id = order.Id,
                DeliveryPrice = order.DeliveryPrice
            };
            return PartialView("_ChangeDeliveryPrice", model);
        }

        [HttpPost]
        public async Task<IActionResult> ChangeDeliveryPrice(Order model)
        {
            Order order = orderService.GetOrder(model.Id);
            order.DeliveryPrice = model.DeliveryPrice;
            orderService.UpdateOrder(order);

            var message = $"<tr>" +
                                                        $"<td>" +
                                                            $"<table cellspacing='0' cellpadding='0' width='820' align='center' border='0'>" +
                                                                $"<tbody>" +
                                                                    $"<tr>" +
                                                                        $"<td style='FONT-SIZE:18px;FONT-FAMILY:Trebuchet MS;FONT-WEIGHT:bold;COLOR:#212121;LINE-HEIGHT:20px;PADDING-TOP:28px;'>" +
                                                                            order.ClientFIO + $", У заказа <span style='FONT-SIZE:18px;FONT-WEIGHT:bold;COLOR:#D80000;'>№ " + order.OrderNumber + "</span> была изменена цена доставки!" +
                                                                        $"</td>" +
                                                                    $"</tr>" +
                                                                    $"<tr>" +
                                                                        $"<td style='FONT-SIZE:18px;FONT-FAMILY:Trebuchet MS;COLOR:#212121;LINE-HEIGHT:20px;PADDING-TOP:12px;'>" +
                                                                            $"Новая цена доставки - " + order.DeliveryPrice +
                                                                        $"</td>" +
                                                                    $"</tr>" +
                                                                $"</tbody>" +
                                                            $"</table>";
            //Письмо об отмене зкаказа
            EmailService emailService = new EmailService();
            await emailService.SendEmailAsync(order.ClientEmail, "У вашего заказа №" + order.OrderNumber + " изменена стоимость доставки", message);

            return RedirectToAction("Orders", "OrderAdmin");
        }
    }
}