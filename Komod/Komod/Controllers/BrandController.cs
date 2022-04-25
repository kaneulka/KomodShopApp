using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Komod.Data;
using Komod.Ser.BrandSer;
using Komod.Web.Models;
using Komod.Web.Models.BrandModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Komod.Web.Controllers
{
    [Authorize(Roles = "admin")]
    public class BrandController : Controller
    {
        private readonly IBrandService brandService;

        public BrandController(IBrandService brandService)
        {
            this.brandService = brandService;
        }

        [HttpGet]
        public IActionResult Brands(int page = 1, int sortType = 0, string searchString = null)
        {
            IEnumerable<Brand> brands;
            if (searchString == null)
            {
                brands = brandService.GetBrands();
            }
            else
            {
                searchString = searchString.ToUpper();
                brands = brandService.GetBrands().Where(s => s.Name.ToUpper().Contains(searchString)
                    || s.AddedDate.ToString().ToUpper().Contains(searchString)
                    || s.ModifiedDate.ToString().ToUpper().Contains(searchString)
                );
            }

            switch (sortType)
            {
                case 0:
                    brands = brands.OrderByDescending(b => b.AddedDate);
                    break;
                case 1:
                    brands = brands.OrderBy(b => b.AddedDate);
                    break;
                case 2:
                    brands = brands.OrderByDescending(b => b.ModifiedDate);
                    break;
                case 3:
                    brands = brands.OrderBy(b => b.ModifiedDate);
                    break;
                case 4:
                    brands = brands.OrderBy(b => b.Name);
                    break;
                case 5:
                    brands = brands.OrderByDescending(b => b.Name);
                    break;
            }
            ViewBag.SortType = sortType;

            int pageSize = 20;

            List<BrandViewModel> listBrands = new List<BrandViewModel>();

            var count = brands.Count();
            var items = brands.Skip((page - 1) * pageSize).Take(pageSize).ToList();
            PageViewModel pageViewModel = new PageViewModel(count, page, pageSize);
            items.ForEach(u =>
            {
                BrandViewModel brand = new BrandViewModel
                {
                    Id = u.Id,
                    BrandName = u.Name,
                    AddedDate = u.AddedDate,
                    ModifiedDate = u.ModifiedDate

                };
                listBrands.Add(brand);
            });

            BrandsViewModel viewModel = new BrandsViewModel
            {
                PageViewModel = pageViewModel,
                Brands = listBrands
            };

            return View(viewModel);
        }

        [HttpGet]
        public ActionResult AddBrand()
        {
            BrandViewModel model = new BrandViewModel();

            return PartialView("_AddBrand", model);
        }

        [HttpPost]
        public ActionResult AddBrand(BrandViewModel model)
        {
            Brand brandEntity = new Brand
            {
                Name = model.BrandName,
                AddedDate = DateTime.Now,
                ModifiedDate = DateTime.Now
            };
            if (ModelState.IsValid)
            {
                brandService.InsertBrand(brandEntity);
                if (brandEntity.Id > 0)
                {
                    return RedirectToAction("Brands");
                }
            }
            return PartialView("_AddBrand", model);
        }

        public ActionResult EditBrand(int? id)
        {
            BrandViewModel model = new BrandViewModel();
            if (id.HasValue && id != 0)
            {
                Brand brandEntity = brandService.GetBrand(id.Value);
                model.BrandName = brandEntity.Name;
            }
            return PartialView("_EditBrand", model);
        }

        [HttpPost]
        public ActionResult EditBrand(BrandViewModel model)
        {
            Brand brandEntity = brandService.GetBrand(model.Id);
            brandEntity.Name = model.BrandName;
            brandEntity.ModifiedDate = DateTime.Now;
            if (ModelState.IsValid)
            {
                brandService.UpdateBrand(brandEntity);
                if (brandEntity.Id > 0)
                {
                    return RedirectToAction("Brands");
                }
            }
            return PartialView("_EditBrand", model);
        }

        [HttpGet]
        public PartialViewResult DeleteBrand(long? id)
        {
            BrandViewModel model = new BrandViewModel();
            if (id.HasValue && id != 0)
            {
                Brand brandEntity = brandService.GetBrand(id.Value);
                model.BrandName = brandEntity.Name;
            }
            return PartialView("_DeleteBrand", model);
        }

        [HttpPost]
        public ActionResult DeleteBrand(long id)
        {
            brandService.DeleteBrand(id);
            return RedirectToAction("Brands");
        }
    }
}