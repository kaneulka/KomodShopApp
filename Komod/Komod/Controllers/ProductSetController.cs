using Komod.Data;
using Komod.Ser.ProductSer;
using Komod.Ser.ProductSetSer;
using Komod.Web.Models;
using Komod.Web.Models.ProductModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Komod.Web.Controllers
{
    [Authorize(Roles = "admin")]
    public class ProductSetController : Controller
    {
        private readonly IProductSetService productSetService;
        private readonly IProductService productService;

        public ProductSetController(IProductSetService productSetService, IProductService productService)
        {
            this.productSetService = productSetService;
            this.productService = productService;
        }

        [HttpGet]
        public IActionResult ProductSets(int page = 1, int sortType = 0, string searchString = null)
        {
            IEnumerable<ProductSet> ProductSets;
            if (searchString == null)
            {
                ProductSets = productSetService.GetProductSets();
            }
            else
            {
                searchString = searchString.ToUpper();
                ProductSets = productSetService.GetProductSets().Where(s => s.ProductSetName.ToUpper().Contains(searchString)
                );
            }

            switch (sortType)
            {
                case 4:
                    ProductSets = ProductSets.OrderBy(b => b.ProductSetName);
                    break;
                case 5:
                    ProductSets = ProductSets.OrderByDescending(b => b.ProductSetName);
                    break;
            }
            ViewBag.SortType = sortType;

            int pageSize = 20;

            List<ProductSetViewModel> listProductSets = new List<ProductSetViewModel>();

            var count = ProductSets.Count();
            var items = ProductSets.Skip((page - 1) * pageSize).Take(pageSize).ToList();
            PageViewModel pageViewModel = new PageViewModel(count, page, pageSize);
            items.ForEach(u =>
            {
                ProductSetViewModel ProductSet = new ProductSetViewModel
                {
                    Id = u.Id,
                    SetName = u.SetName,
                    ProductSetName = u.ProductSetName.Replace("||", ", "),
                    DiscounPercent = u.DiscounPercent,
                    ActiveSet = u.ActiveSet

                };
                listProductSets.Add(ProductSet);
            });

            ProductSetsViewModel viewModel = new ProductSetsViewModel
            {
                PageViewModel = pageViewModel,
                ProductSets = listProductSets
            };

            return View(viewModel);
        }

        [HttpGet]
        public ActionResult AddProductSet()
        {
            ProductSetViewModel model = new ProductSetViewModel();
            model.ProductsInAdmin = productService.GetProducts().ToList();

            return PartialView("_AddProductSet", model);
        }

        [HttpPost]
        public ActionResult AddProductSet(ProductSetViewModel model, List<string> productNames)
        {
            ProductSet productSetEntity = new ProductSet
            {
                SetName = model.SetName,
                DiscounPercent = model.DiscounPercent,
                ActiveSet = model.ActiveSet
            };
            foreach(var p in productNames)
            {
                productSetEntity.ProductSetName += p + "||";
            }

            if (ModelState.IsValid)
            {
                productSetService.InsertProductSet(productSetEntity);
                if (productSetEntity.Id > 0)
                {
                    return RedirectToAction("ProductSets");
                }
            }
            return PartialView("_AddProductSet", model);
        }

        public ActionResult EditProductSet(int? id)
        {
            ProductSetViewModel model = new ProductSetViewModel();
            if (id.HasValue && id != 0)
            {
                ProductSet productSetEntity = productSetService.GetProductSet(id.Value);
                model.ProductSetName = productSetEntity.ProductSetName;
                model.DiscounPercent = productSetEntity.DiscounPercent;
                model.ActiveSet = productSetEntity.ActiveSet;
                model.SetName = productSetEntity.SetName;
                model.ProductsInAdmin = productService.GetProducts().ToList();
            }
            return PartialView("_EditProductSet", model);
        }

        [HttpPost]
        public ActionResult EditProductSet(ProductSetViewModel model, List<string> productNames)
        {
            ProductSet productSetEntity = productSetService.GetProductSet(model.Id);
            productSetEntity.DiscounPercent = model.DiscounPercent;
            productSetEntity.ActiveSet = model.ActiveSet;
            productSetEntity.ProductSetName = "";
            productSetEntity.ProductSetName = model.SetName;
            foreach (var p in productNames)
            {
                productSetEntity.ProductSetName += p + "||";
            }
            if (ModelState.IsValid)
            {
                productSetService.UpdateProductSet(productSetEntity);
                if (productSetEntity.Id > 0)
                {
                    return RedirectToAction("ProductSets");
                }
            }
            return PartialView("_EditProductSet", model);
        }

        [HttpGet]
        public PartialViewResult DeleteProductSet(long? id)
        {
            ProductSetViewModel model = new ProductSetViewModel();
            if (id.HasValue && id != 0)
            {
                ProductSet productSetEntity = productSetService.GetProductSet(id.Value);
                model.SetName = productSetEntity.SetName;
            }
            return PartialView("_DeleteProductSet", model);
        }

        [HttpPost]
        public ActionResult DeleteProductSet(long id)
        {
            productSetService.DeleteProductSet(id);
            return RedirectToAction("ProductSets");
        }
    }
}
