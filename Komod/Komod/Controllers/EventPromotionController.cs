using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Komod.Data;
using Komod.Repo;
using Komod.Ser.CategorySer;
using Komod.Ser.EventProductSer;
using Komod.Ser.EventPromotionSer;
using Komod.Ser.ProductSer;
using Komod.Web.Models;
using Komod.Web.Models.EventPromotionModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NPOI.SS.Formula.Functions;

namespace Komod.Web.Controllers
{
    [Authorize(Roles = "admin")]
    public class EventPromotionController : Controller
    {
        private readonly IEventPromotionService eventPromotionService;
        private readonly IEventProductService eventProductService;
        private readonly ICategoryService categoryService;
        private readonly IProductService productService;
        ApplicationContext _context;
        IWebHostEnvironment _appEnvironment;

        public EventPromotionController(IEventPromotionService eventPromotionService,
            IEventProductService eventProductService,
            ICategoryService categoryService, 
            IProductService productService, 
            ApplicationContext context, 
            IWebHostEnvironment appEnvironment)
        {
            this.eventPromotionService = eventPromotionService;
            this.eventProductService = eventProductService;
            this.categoryService = categoryService;
            this.productService = productService;
            _context = context;
            _appEnvironment = appEnvironment;
        }

        [HttpGet]
        public IActionResult EventPromotions(int page = 1, int sortType = 0, string searchString = null)
        {
            IEnumerable<EventPromotion> entities;
            if (searchString == null)
            {
                entities = eventPromotionService.GetEventPromotions();
            }
            else
            {
                searchString = searchString.ToUpper();
                entities = eventPromotionService.GetEventPromotions().Where(s => s.Name.ToUpper().Contains(searchString)
                    || s.AddedDate.ToString().ToUpper().Contains(searchString)
                    || s.ModifiedDate.ToString().ToUpper().Contains(searchString)
                );
            }

            switch (sortType)
            {
                case 0:
                    entities = entities.OrderByDescending(b => b.AddedDate);
                    break;
                case 1:
                    entities = entities.OrderBy(b => b.AddedDate);
                    break;
                case 2:
                    entities = entities.OrderByDescending(b => b.ModifiedDate);
                    break;
                case 3:
                    entities = entities.OrderBy(b => b.ModifiedDate);
                    break;
                case 4:
                    entities = entities.OrderBy(b => b.Name);
                    break;
                case 5:
                    entities = entities.OrderByDescending(b => b.Name);
                    break;
            }
            ViewBag.SortType = sortType;

            int pageSize = 20;

            List<EventPromotionViewModel> listEventPromotions = new List<EventPromotionViewModel>();

            var count = entities.Count();
            var items = entities.Skip((page - 1) * pageSize).Take(pageSize).ToList();
            PageViewModel pageViewModel = new PageViewModel(count, page, pageSize);
            items.ForEach(u =>
            {
                EventPromotionViewModel eventPromotion = new EventPromotionViewModel
                {
                    Id = u.Id,
                    Name = u.Name,
                    AddedDate = u.AddedDate,
                    ModifiedDate = u.ModifiedDate,
                    ImgPath = u.ImgPath,
                    ActiveEvent = u.ActiveEvent,
                    DiscountPercent = u.DiscountPercent

                };
                listEventPromotions.Add(eventPromotion);
            });

            EventPromotionsViewModel viewModel = new EventPromotionsViewModel
            {
                PageViewModel = pageViewModel,
                EventPromotions = listEventPromotions
            };

            return View(viewModel);
        }

        //Сделать акцию активной
        [HttpPost]
        public ActionResult DoEventPromotionActive(long Id, bool ActiveEvent, string returnurl)
        {
            EventPromotion epEntity = eventPromotionService.GetEventPromotion(Id);
            epEntity.ActiveEvent = ActiveEvent;
            eventPromotionService.UpdateEventPromotion(epEntity);

            return RedirectToAction("EventPromotions", "EventPromotion", new { returnurl });
        }

        [HttpGet]
        public IActionResult AddEventPromotion()
        {
            EventPromotionViewModel model = new EventPromotionViewModel();

            model.Categories = categoryService.GetCategories().ToList();
            model.Products = productService.GetProducts().ToList();
            

            return PartialView("_AddEventPromotion", model);
        }

        [HttpPost]
        public async Task<IActionResult> AddEventPromotion(EventPromotionViewModel model, bool productEventPromotion, bool categoryEventPromotion, List<long> productIds, List<long> categoryIds, IFormFile uploadedFile)
        {
            EventPromotion entity = new EventPromotion
            {
                Name = model.Name,
                AddedDate = DateTime.Now,
                ModifiedDate = DateTime.Now,
                Description = model.Description,
                ActiveEvent = model.ActiveEvent,
                StartEvent = model.StartEvent,
                EndEvent = model.EndEvent,
                DiscountPercent = model.DiscountPercent
            };
            if (ModelState.IsValid)
            {
                if (uploadedFile != null)
                {
                    // путь к папке Files
                    string path = "/Files/EventPromotionsImages/" + uploadedFile.FileName;
                    // сохраняем файл в папку Files в каталоге wwwroot
                    using (var fileStream = new FileStream(_appEnvironment.WebRootPath + path, FileMode.Create))
                    {
                        await uploadedFile.CopyToAsync(fileStream);
                    }
                    entity.ImgPath = path;
                }
                eventPromotionService.InsertEventPromotion(entity);

                if (productEventPromotion)
                {
                    if (categoryEventPromotion)
                    {
                        List<Product> eventProducts = productService.GetProducts().Where(ep => categoryIds.Contains(ep.CategoryId)).ToList();
                        foreach (var ep in eventProducts) eventProductService.InsertEventProduct(new EventProduct() { EventPromotionId = entity.Id, ProductId = ep.Id });
                    }
                    else
                    {
                        List<Product> eventProducts = productService.GetProducts().Where(ep => categoryIds.Contains(ep.CategoryId)).ToList();
                        foreach (var ep in eventProducts) eventProductService.InsertEventProduct(new EventProduct() { EventPromotionId = entity.Id, ProductId = ep.Id });
                    }
                }

                if (entity.Id > 0)
                {
                    return RedirectToAction("EventPromotions");
                }
            }
            return PartialView("_AddEventPromotion", model);
        }

        public IActionResult EditEventPromotion(int? id)
        {
            EventPromotionViewModel model = new EventPromotionViewModel();
            if (id.HasValue && id != 0)
            {
                EventPromotion entity = eventPromotionService.GetEventPromotion(id.Value);
                model.Id = entity.Id;
                model.Name = entity.Name;
                model.Description = entity.Description;
                model.ActiveEvent = entity.ActiveEvent;
                model.StartEvent = entity.StartEvent;
                model.EndEvent = entity.EndEvent;
                List<EventProduct> eventProducts = eventProductService.GetEventProductes().Where(ep => ep.EventPromotionId == id).ToList();
                model.Products = new List<Product>();
                model.DiscountPercent = entity.DiscountPercent;
                foreach (var ep in eventProducts) model.Products.Add(productService.GetProduct(ep.ProductId));
            }
            return PartialView("_EditEventPromotion", model);
        }

        [HttpPost]
        public async Task<IActionResult> EditEventPromotion(EventPromotionViewModel model, bool changeEventProducts, bool productEventPromotion, bool categoryEventPromotion, List<long> productIds, List<long> categoryIds, IFormFile uploadedFile)
        {
            EventPromotion entity = eventPromotionService.GetEventPromotion(model.Id);
            entity.Name = model.Name;
            entity.ModifiedDate = DateTime.Now;
            entity.Description = model.Description;
            entity.ActiveEvent = model.ActiveEvent;
            entity.StartEvent = model.StartEvent;
            entity.EndEvent = model.EndEvent;
            entity.DiscountPercent = model.DiscountPercent;

            if (ModelState.IsValid)
            {
                if (uploadedFile != null)
                {
                    if (entity.ImgPath != null)
                    {
                        FileInfo fi = new FileInfo(_appEnvironment.WebRootPath + "/" + entity.ImgPath);
                        fi.Delete();
                    }
                    // путь к папке Files
                    string path = "/Files/EventPromotionsImages/" + uploadedFile.FileName;
                    // сохраняем файл в папку Files в каталоге wwwroot
                    using (var fileStream = new FileStream(_appEnvironment.WebRootPath + path, FileMode.Create))
                    {
                        await uploadedFile.CopyToAsync(fileStream);
                    }
                    entity.ImgPath = path;
                }
                eventPromotionService.UpdateEventPromotion(entity);


                if (productEventPromotion && changeEventProducts)
                {
                    List<EventProduct> oldEventProducts = eventProductService.GetEventProductes().Where(ep => ep.EventPromotionId == entity.Id).ToList();
                    eventProductService.DeleteEventProducts(oldEventProducts);//МБ НАЙТИ ЛУЧШЕЕ РЕШЕНИЕ, НО ПОКА В ПАДЛУ
                    if (categoryEventPromotion)
                    {
                        List<Product> eventProducts = productService.GetProducts().Where(ep => categoryIds.Contains(ep.CategoryId)).ToList();
                        foreach (var ep in eventProducts) eventProductService.InsertEventProduct(new EventProduct() { EventPromotionId = entity.Id, ProductId = ep.Id });
                    }
                    else
                    {
                        List<Product> eventProducts = productService.GetProducts().Where(ep => categoryIds.Contains(ep.CategoryId)).ToList();
                        foreach (var ep in eventProducts) eventProductService.InsertEventProduct(new EventProduct() { EventPromotionId = entity.Id, ProductId = ep.Id });
                    }
                }

                if (entity.Id > 0)
                {
                    return RedirectToAction("EventPromotions");
                }
            }
            return PartialView("_EditEventPromotion", model);
        }

        [HttpGet]
        public PartialViewResult DeleteEventPromotion(long? id)
        {
            EventPromotionViewModel model = new EventPromotionViewModel();
            if (id.HasValue && id != 0)
            {
                EventPromotion entity = eventPromotionService.GetEventPromotion(id.Value);
                model.Name = entity.Name;
            }
            return PartialView("_DeleteEventPromotion", model);
        }

        [HttpPost]
        public IActionResult DeleteEventPromotion(long id)
        {
            eventPromotionService.DeleteEventPromotion(id);
            List<EventProduct> eventProducts = eventProductService.GetEventProductes().Where(ep => ep.EventPromotionId == id).ToList();
            foreach(var ep in eventProducts)
            {
                eventProductService.DeleteEventProduct(ep);
            }
            return RedirectToAction("EventPromotions");
        }

        //У акции
        [HttpGet]
        public JsonResult GetProductToEvent(long eventId)
        {
            List<EventProduct> lep = eventProductService.GetEventProductes().Where(ep=> ep.EventPromotionId == eventId).ToList();
            List<Product> lp = productService.GetProducts().Where(p=> lep.Select(ep=> ep.ProductId).Contains(p.Id)).ToList();
           
            return Json(lp);
        }
        [HttpGet]
        public JsonResult GetProductToAddEvent(string productName)
        {
            List<Product> lp = productService.GetProducts().Where(p => p.Name.Contains(productName)).ToList();
            return Json(lp);
        }

        //Удаление
        [HttpPost]
        public void RemoveEventProduct(long productId, long eventId)
        {
            EventProduct ep = eventProductService.GetEventProductes().SingleOrDefault(ep => ep.EventPromotionId == eventId && ep.ProductId == productId);
            eventProductService.DeleteEventProduct(ep);
        }
        //Добавление
        [HttpPost]
        public void AddEventProduct(long productId, long eventId)
        {
            EventProduct ep = new EventProduct()
            {
                EventPromotionId = eventId,
                ProductId = productId
            };
            eventProductService.InsertEventProduct(ep);
        }

        //У продукта
        [HttpGet]
        public JsonResult GetEventToProduct(long productId)
        {
            List<EventProduct> lep = eventProductService.GetEventProductes().Where(ep => ep.ProductId == productId).ToList();
            List<EventPromotion> lp = eventPromotionService.GetEventPromotions().Where(p => lep.Select(ep => ep.EventPromotionId).Contains(p.Id)).ToList();

            return Json(lp);
        }
        [HttpGet]
        public JsonResult GetEventToAddProduct(string eventname)
        {
            List<EventPromotion> lp = eventPromotionService.GetEventPromotions().Where(p => p.Name.Contains(eventname)).ToList();
            return Json(lp);
        }
    }
}