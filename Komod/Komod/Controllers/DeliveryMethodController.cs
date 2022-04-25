using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Komod.Data;
using Komod.Ser.DeliveryMethodSer;
using Komod.Web.Models.Methods;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NPOI.SS.Formula.Functions;

namespace Komod.Web.Controllers
{
    [Authorize(Roles = "admin")]
    public class DeliveryMethodController : Controller
    {
        private readonly IDeliveryMethodService deliveryMethodService;

        public DeliveryMethodController(IDeliveryMethodService deliveryMethodService)
        {
            this.deliveryMethodService = deliveryMethodService;
        }


        //DeliveryMethod
        [HttpGet]
        public IActionResult DeliveryMethodes(int page = 1, int sortType = 0, string searchString = null)
        {
            IEnumerable<DeliveryMethod> entities;
            if (searchString == null)
            {
                entities = deliveryMethodService.GetDeliveryMethods();
            }
            else
            {
                searchString = searchString.ToUpper();
                entities = deliveryMethodService.GetDeliveryMethods().Where(s => s.Name.ToUpper().Contains(searchString)
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

            List<DeliveryMethodViewModel> listEntities = new List<DeliveryMethodViewModel>();

            //var count = entities.Count();
            //var items = entities.Skip((page - 1) * pageSize).Take(pageSize).ToList();
            //PageViewModel pageViewModel = new PageViewModel(count, page, pageSize);
            entities.ToList().ForEach(u =>
            {
                DeliveryMethodViewModel entityNew = new DeliveryMethodViewModel
                {
                    Id = u.Id,
                    Name = u.Name,
                    AddedDate = u.AddedDate,
                    ModifiedDate = u.ModifiedDate,
                    FreeDelivery = u.FreeDelivery,
                    District = u.District,
                    DeliveryPrice = u.DeliveryPrice
                };
                listEntities.Add(entityNew);
            });

            //DeliveryMethodsViewModel viewModel = new DeliveryMethodsViewModel
            //{
            //    PageViewModel = pageViewModel,
            //    DeliveryMethods = listEntities
            //};

            return View(listEntities);
        }

        [HttpGet]
        public ActionResult AddDeliveryMethod()
        {
            DeliveryMethodViewModel model = new DeliveryMethodViewModel();

            return PartialView("_AddDeliveryMethod", model);
        }

        [HttpPost]
        public ActionResult AddDeliveryMethod(DeliveryMethodViewModel model)
        {
            DeliveryMethod entity = new DeliveryMethod
            {
                Name = model.Name,
                AddedDate = DateTime.Now,
                ModifiedDate = DateTime.Now,
                DeliveryPrice = model.DeliveryPrice,
                FreeDelivery = model.FreeDelivery,
                District = model.District
            };
            if (ModelState.IsValid)
            {
                deliveryMethodService.InsertDeliveryMethod(entity);
                if (entity.Id > 0)
                {
                    return RedirectToAction("DeliveryMethodes");
                }
            }
            return PartialView("_AddDeliveryMethod", model);
        }

        public ActionResult EditDeliveryMethod(int? id)
        {
            DeliveryMethodViewModel model = new DeliveryMethodViewModel();
            if (id.HasValue && id != 0)
            {
                DeliveryMethod entity = deliveryMethodService.GetDeliveryMethod(id.Value);
                model.Name = entity.Name;
                model.DeliveryPrice = entity.DeliveryPrice;
                model.FreeDelivery = entity.FreeDelivery;
                model.District = entity.District;
            }
            return PartialView("_EditDeliveryMethod", model);
        }

        [HttpPost]
        public ActionResult EditDeliveryMethod(DeliveryMethodViewModel model)
        {
            DeliveryMethod entity = deliveryMethodService.GetDeliveryMethod(model.Id);
            entity.Name = model.Name;
            entity.ModifiedDate = DateTime.Now;
            entity.FreeDelivery = model.FreeDelivery;
            entity.DeliveryPrice = model.DeliveryPrice;
            entity.District = model.District;
            if (ModelState.IsValid)
            {
                deliveryMethodService.UpdateDeliveryMethod(entity);
                if (entity.Id > 0)
                {
                    return RedirectToAction("DeliveryMethodes");
                }
            }
            return PartialView("_EditDeliveryMethod", model);
        }

        [HttpGet]
        public PartialViewResult DeleteDeliveryMethod(long? id)
        {
            DeliveryMethodViewModel model = new DeliveryMethodViewModel();
            if (id.HasValue && id != 0)
            {
                DeliveryMethod entity = deliveryMethodService.GetDeliveryMethod(id.Value);
                model.Name = entity.Name;
            }
            return PartialView("_DeleteDeliveryMethod", model);
        }

        [HttpPost]
        public ActionResult DeleteDeliveryMethod(long id)
        {
            deliveryMethodService.DeleteDeliveryMethod(id);
            return RedirectToAction("DeliveryMethodes");
        }
    }
}