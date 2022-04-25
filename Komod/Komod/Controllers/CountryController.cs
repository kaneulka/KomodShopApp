using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Komod.Data;
using Komod.Ser.CountrySer;
using Komod.Web.Models;
using Komod.Web.Models.CountryModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Komod.Web.Controllers
{
    [Authorize(Roles = "admin")]
    public class CountryController : Controller
    {
        private readonly ICountryService countryService;

        public CountryController(ICountryService countryService)
        {
            this.countryService = countryService;
        }

        [HttpGet]
        public IActionResult Countries(int page = 1, int sortType = 0, string searchString = null)
        {
            IEnumerable<Country> countrys;
            if (searchString == null)
            {
                countrys = countryService.GetCountries();
            }
            else
            {
                searchString = searchString.ToUpper();
                countrys = countryService.GetCountries().Where(s => s.Name.ToUpper().Contains(searchString)
                    || s.AddedDate.ToString().ToUpper().Contains(searchString)
                    || s.ModifiedDate.ToString().ToUpper().Contains(searchString)
                );
            }

            switch (sortType)
            {
                case 0:
                    countrys = countrys.OrderByDescending(b => b.AddedDate);
                    break;
                case 1:
                    countrys = countrys.OrderBy(b => b.AddedDate);
                    break;
                case 2:
                    countrys = countrys.OrderByDescending(b => b.ModifiedDate);
                    break;
                case 3:
                    countrys = countrys.OrderBy(b => b.ModifiedDate);
                    break;
                case 4:
                    countrys = countrys.OrderBy(b => b.Name);
                    break;
                case 5:
                    countrys = countrys.OrderByDescending(b => b.Name);
                    break;
            }
            ViewBag.SortType = sortType;

            int pageSize = 20;

            List<CountryViewModel> listCountries = new List<CountryViewModel>();

            var count = countrys.Count();
            var items = countrys.Skip((page - 1) * pageSize).Take(pageSize).ToList();
            PageViewModel pageViewModel = new PageViewModel(count, page, pageSize);
            items.ForEach(u =>
            {
                CountryViewModel country = new CountryViewModel
                {
                    Id = u.Id,
                    CountryName = u.Name,
                    AddedDate = u.AddedDate,
                    ModifiedDate = u.ModifiedDate

                };
                listCountries.Add(country);
            });

            CountriesViewModel viewModel = new CountriesViewModel
            {
                PageViewModel = pageViewModel,
                Countries = listCountries
            };

            return View(viewModel);
        }

        [HttpGet]
        public ActionResult AddCountry()
        {
            CountryViewModel model = new CountryViewModel();

            return PartialView("_AddCountry", model);
        }

        [HttpPost]
        public ActionResult AddCountry(CountryViewModel model)
        {
            Country countryEntity = new Country
            {
                Name = model.CountryName,
                AddedDate = DateTime.Now,
                ModifiedDate = DateTime.Now
            };
            if (ModelState.IsValid)
            {
                countryService.InsertCountry(countryEntity);
                if (countryEntity.Id > 0)
                {
                    return RedirectToAction("Countries");
                }
            }
            return PartialView("_AddCountry", model);
        }

        public ActionResult EditCountry(int? id)
        {
            CountryViewModel model = new CountryViewModel();
            if (id.HasValue && id != 0)
            {
                Country countryEntity = countryService.GetCountry(id.Value);
                model.CountryName = countryEntity.Name;
            }
            return PartialView("_EditCountry", model);
        }

        [HttpPost]
        public ActionResult EditCountry(CountryViewModel model)
        {
            Country countryEntity = countryService.GetCountry(model.Id);
            countryEntity.Name = model.CountryName;
            countryEntity.ModifiedDate = DateTime.Now;
            if (ModelState.IsValid)
            {
                countryService.UpdateCountry(countryEntity);
                if (countryEntity.Id > 0)
                {
                    return RedirectToAction("Countries");
                }
            }
            return PartialView("_EditCountry", model);
        }

        [HttpGet]
        public PartialViewResult DeleteCountry(long? id)
        {
            CountryViewModel model = new CountryViewModel();
            if (id.HasValue && id != 0)
            {
                Country countryEntity = countryService.GetCountry(id.Value);
                model.CountryName = countryEntity.Name;
            }
            return PartialView("_DeleteCountry", model);
        }

        [HttpPost]
        public ActionResult DeleteCountry(long id)
        {
            countryService.DeleteCountry(id);
            return RedirectToAction("Countries");
        }
    }
}