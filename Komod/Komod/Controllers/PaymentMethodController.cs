using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Komod.Data;
using Komod.Ser.PaymentMethodSer;
using Komod.Web.Models;
using Komod.Web.Models.PaymentMethodModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Komod.Web.Controllers
{
    [Authorize(Roles = "admin")]
    public class PaymentMethodController : Controller
    {
        private readonly IPaymentMethodService paymentMethodService;

        public PaymentMethodController(IPaymentMethodService paymentMethodService)
        {
            this.paymentMethodService = paymentMethodService;
        }


        //PaymentMethod
        [HttpGet]
        public IActionResult PaymentMethodes(int page = 1, int sortType = 0, string searchString = null)
        {
            IEnumerable<PaymentMethod> entities;
            if (searchString == null)
            {
                entities = paymentMethodService.GetPaymentMethods();
            }
            else
            {
                searchString = searchString.ToUpper();
                entities = paymentMethodService.GetPaymentMethods().Where(s => s.Name.ToUpper().Contains(searchString)
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

            List<PaymentMethodViewModel> listEntities = new List<PaymentMethodViewModel>();

            var count = entities.Count();
            var items = entities.Skip((page - 1) * pageSize).Take(pageSize).ToList();
            PageViewModel pageViewModel = new PageViewModel(count, page, pageSize);
            items.ForEach(u =>
            {
                PaymentMethodViewModel entityNew = new PaymentMethodViewModel
                {
                    Id = u.Id,
                    Name = u.Name,
                    AddedDate = u.AddedDate,
                    ModifiedDate = u.ModifiedDate

                };
                listEntities.Add(entityNew);
            });

            PaymentMethodsViewModel viewModel = new PaymentMethodsViewModel
            {
                PageViewModel = pageViewModel,
                PaymentMethods = listEntities
            };

            return View(viewModel);
        }

        [HttpGet]
        public ActionResult AddPaymentMethod()
        {
            PaymentMethodViewModel model = new PaymentMethodViewModel();

            return PartialView("_AddPaymentMethod", model);
        }

        [HttpPost]
        public ActionResult AddPaymentMethod(PaymentMethodViewModel model)
        {
            PaymentMethod entity = new PaymentMethod
            {
                Name = model.Name,
                AddedDate = DateTime.Now,
                ModifiedDate = DateTime.Now
            };
            if (ModelState.IsValid)
            {
                paymentMethodService.InsertPaymentMethod(entity);
                if (entity.Id > 0)
                {
                    return RedirectToAction("PaymentMethodes");
                }
            }
            return PartialView("_AddPaymentMethod", model);
        }

        public ActionResult EditPaymentMethod(int? id)
        {
            PaymentMethodViewModel model = new PaymentMethodViewModel();
            if (id.HasValue && id != 0)
            {
                PaymentMethod entity = paymentMethodService.GetPaymentMethod(id.Value);
                model.Name = entity.Name;
            }
            return PartialView("_EditPaymentMethod", model);
        }

        [HttpPost]
        public ActionResult EditPaymentMethod(PaymentMethodViewModel model)
        {
            PaymentMethod entity = paymentMethodService.GetPaymentMethod(model.Id);
            entity.Name = model.Name;
            entity.ModifiedDate = DateTime.Now;
            if (ModelState.IsValid)
            {
                paymentMethodService.UpdatePaymentMethod(entity);
                if (entity.Id > 0)
                {
                    return RedirectToAction("PaymentMethodes");
                }
            }
            return PartialView("_EditPaymentMethod", model);
        }

        [HttpGet]
        public PartialViewResult DeletePaymentMethod(long? id)
        {
            PaymentMethodViewModel model = new PaymentMethodViewModel();
            if (id.HasValue && id != 0)
            {
                PaymentMethod entity = paymentMethodService.GetPaymentMethod(id.Value);
                model.Name = entity.Name;
            }
            return PartialView("_DeletePaymentMethod", model);
        }

        [HttpPost]
        public ActionResult DeletePaymentMethod(long id)
        {
            paymentMethodService.DeletePaymentMethod(id);
            return RedirectToAction("PaymentMethodes");
        }
    }
}