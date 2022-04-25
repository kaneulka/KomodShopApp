using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Komod.Data;
using Komod.Ser.OrderStatusSer;
using Komod.Ser.StockStatusSer;
using Komod.Web.Models;
using Komod.Web.Models.StatusModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Komod.Web.Controllers
{
    [Authorize(Roles = "admin")]
    public class StatusController : Controller
    {
        private readonly IStockStatusService stockStatusService;
        private readonly IOrderStatusService orderStatusService;

        public StatusController(IStockStatusService stockStatusService, IOrderStatusService orderStatusService)
        {
            this.stockStatusService = stockStatusService;
            this.orderStatusService = orderStatusService;
        }


        //StockStatus
        [HttpGet]
        public IActionResult StockStatuses(int page = 1, int sortType = 0, string searchString = null)
        {
            IEnumerable<StockStatus> entities;
            if (searchString == null)
            {
                entities = stockStatusService.GetStockStatuses();
            }
            else
            {
                searchString = searchString.ToUpper();
                entities = stockStatusService.GetStockStatuses().Where(s => s.Name.ToUpper().Contains(searchString)
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

            List<StockStatusViewModel> listEntities = new List<StockStatusViewModel>();

            var count = entities.Count();
            var items = entities.Skip((page - 1) * pageSize).Take(pageSize).ToList();
            PageViewModel pageViewModel = new PageViewModel(count, page, pageSize);
            items.ForEach(u =>
            {
                StockStatusViewModel entityNew = new StockStatusViewModel
                {
                    Id = u.Id,
                    Name = u.Name,
                    AddedDate = u.AddedDate,
                    ModifiedDate = u.ModifiedDate

                };
                listEntities.Add(entityNew);
            });

            StockStatusesViewModel viewModel = new StockStatusesViewModel
            {
                PageViewModel = pageViewModel,
                StockStatuses = listEntities
            };

            return View(viewModel);
        }

        [HttpGet]
        public ActionResult AddStockStatus()
        {
            StockStatusViewModel model = new StockStatusViewModel();

            return PartialView("_AddStockStatus", model);
        }

        [HttpPost]
        public ActionResult AddStockStatus(StockStatusViewModel model)
        {
            StockStatus entity = new StockStatus
            {
                Name = model.Name,
                AddedDate = DateTime.Now,
                ModifiedDate = DateTime.Now
            };
            if (ModelState.IsValid)
            {
                stockStatusService.InsertStockStatus(entity);
                if (entity.Id > 0)
                {
                    return RedirectToAction("StockStatuses");
                }
            }
            return PartialView("_AddStockStatus", model);
        }

        public ActionResult EditStockStatus(int? id)
        {
            StockStatusViewModel model = new StockStatusViewModel();
            if (id.HasValue && id != 0)
            {
                StockStatus entity = stockStatusService.GetStockStatus(id.Value);
                model.Name = entity.Name;
            }
            return PartialView("_EditBrandStatus", model);
        }

        [HttpPost]
        public ActionResult EditStockStatus(StockStatusViewModel model)
        {
            StockStatus entity = stockStatusService.GetStockStatus(model.Id);
            entity.Name = model.Name;
            entity.ModifiedDate = DateTime.Now;
            if (ModelState.IsValid)
            {
                stockStatusService.UpdateStockStatus(entity);
                if (entity.Id > 0)
                {
                    return RedirectToAction("StockStatuses");
                }
            }
            return PartialView("_EditStockStatus", model);
        }

        [HttpGet]
        public PartialViewResult DeleteStockStatus(long? id)
        {
            StockStatusViewModel model = new StockStatusViewModel();
            if (id.HasValue && id != 0)
            {
                StockStatus entity = stockStatusService.GetStockStatus(id.Value);
                model.Name = entity.Name;
            }
            return PartialView("_DeleteStockStatus", model);
        }

        [HttpPost]
        public ActionResult DeleteStockStatus(long id)
        {
            stockStatusService.DeleteStockStatus(id);
            return RedirectToAction("StockStatuses");
        }


        //OrderStatus
        [HttpGet]
        public IActionResult OrderStatuses(int page = 1, int sortType = 0, string searchString = null)
        {
            IEnumerable<OrderStatus> entities;
            if (searchString == null)
            {
                entities = orderStatusService.GetOrderStatuses();
            }
            else
            {
                searchString = searchString.ToUpper();
                entities = orderStatusService.GetOrderStatuses().Where(s => s.Name.ToUpper().Contains(searchString)
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

            List<OrderStatusViewModel> listEntities = new List<OrderStatusViewModel>();

            var count = entities.Count();
            var items = entities.Skip((page - 1) * pageSize).Take(pageSize).ToList();
            PageViewModel pageViewModel = new PageViewModel(count, page, pageSize);
            items.ForEach(u =>
            {
                OrderStatusViewModel entityNew = new OrderStatusViewModel
                {
                    Id = u.Id,
                    Name = u.Name,
                    AddedDate = u.AddedDate,
                    ModifiedDate = u.ModifiedDate

                };
                listEntities.Add(entityNew);
            });

            OrderStatusesViewModel viewModel = new OrderStatusesViewModel
            {
                PageViewModel = pageViewModel,
                OrderStatuses = listEntities
            };

            return View(viewModel);
        }

        [HttpGet]
        public ActionResult AddOrderStatus()
        {
            OrderStatusViewModel model = new OrderStatusViewModel();

            return PartialView("_AddOrderStatus", model);
        }

        [HttpPost]
        public ActionResult AddOrderStatus(OrderStatusViewModel model)
        {
            OrderStatus entity = new OrderStatus
            {
                Name = model.Name,
                AddedDate = DateTime.Now,
                ModifiedDate = DateTime.Now
            };
            if (ModelState.IsValid)
            {
                orderStatusService.InsertOrderStatus(entity);
                if (entity.Id > 0)
                {
                    return RedirectToAction("OrderStatuses");
                }
            }
            return PartialView("_AddOrderStatus", model);
        }

        public ActionResult EditOrderStatus(int? id)
        {
            OrderStatusViewModel model = new OrderStatusViewModel();
            if (id.HasValue && id != 0)
            {
                OrderStatus entity = orderStatusService.GetOrderStatus(id.Value);
                model.Name = entity.Name;
            }
            return PartialView("_EditOrderStatus", model);
        }

        [HttpPost]
        public ActionResult EditOrderStatus(OrderStatusViewModel model)
        {
            OrderStatus entity = orderStatusService.GetOrderStatus(model.Id);
            entity.Name = model.Name;
            entity.ModifiedDate = DateTime.Now;
            if (ModelState.IsValid)
            {
                orderStatusService.UpdateOrderStatus(entity);
                if (entity.Id > 0)
                {
                    return RedirectToAction("StockStatuses");
                }
            }
            return PartialView("_EditStockStatus", model);
        }

        [HttpGet]
        public PartialViewResult DeleteOrderStatus(long? id)
        {
            OrderStatusViewModel model = new OrderStatusViewModel();
            if (id.HasValue && id != 0)
            {
                OrderStatus entity = orderStatusService.GetOrderStatus(id.Value);
                model.Name = entity.Name;
            }
            return PartialView("_DeleteOrderStatus", model);
        }

        [HttpPost]
        public ActionResult DeleteOrderStatus(long id)
        {
            orderStatusService.DeleteOrderStatus(id);
            return RedirectToAction("OrderStatuses");
        }
    }
}