using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Komod.Data;
using Komod.Ser.ColorSer;
using Komod.Ser.PropertySer;
using Komod.Ser.PropertyValCatArtSer;
using Komod.Ser.PropertyValueSer;
using Komod.Web.Models;
using Komod.Web.Models.ProductModels;
using Komod.Web.Models.PropertyModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Komod.Web.Controllers
{
    [Authorize(Roles = "admin")]
    public class PropertyController : Controller
    {
        private readonly IPropertyService propertyService;
        private readonly IPropertyValueService propertyValueService;
        private readonly IColorService colorService;
        private readonly IPropertyValCatArtService propertyValCatArtService;

        public PropertyController(IPropertyService propertyService, IPropertyValueService propertyValueService, IColorService colorService, IPropertyValCatArtService propertyValCatArtService)
        {
            this.propertyService = propertyService;
            this.propertyValueService = propertyValueService;
            this.colorService = colorService;
            this.propertyValCatArtService = propertyValCatArtService;
        }

        [HttpGet]
        public IActionResult Properties(int page = 1, int sortType = 0, string searchString = null)
        {
            IEnumerable<Property> properties;
            if (searchString == null)
            {
                properties = propertyService.GetProperties();
            }
            else
            {
                searchString = searchString.ToUpper();
                properties = propertyService.GetProperties().Where(s => s.Name.ToUpper().Contains(searchString)
                    || s.AddedDate.ToString().ToUpper().Contains(searchString)
                    || s.ModifiedDate.ToString().ToUpper().Contains(searchString)
                );
            }

            switch (sortType)
            {
                case 0:
                    properties = properties.OrderByDescending(b => b.AddedDate);
                    break;
                case 1:
                    properties = properties.OrderBy(b => b.AddedDate);
                    break;
                case 2:
                    properties = properties.OrderByDescending(b => b.ModifiedDate);
                    break;
                case 3:
                    properties = properties.OrderBy(b => b.ModifiedDate);
                    break;
                case 4:
                    properties = properties.OrderBy(b => b.Name);
                    break;
                case 5:
                    properties = properties.OrderByDescending(b => b.Name);
                    break;
            }
            ViewBag.SortType = sortType;

            int pageSize = 20;

            List<PropertyViewModel> listProperties = new List<PropertyViewModel>();

            var count = properties.Count();
            var items = properties.Skip((page - 1) * pageSize).Take(pageSize).ToList();
            PageViewModel pageViewModel = new PageViewModel(count, page, pageSize);
            items.ForEach(u =>
            {
                PropertyViewModel category = new PropertyViewModel
                {
                    Id = u.Id,
                    Name = u.Name,
                    ValueName = u.ValueName,
                    AddedDate = u.AddedDate,
                    ModifiedDate = u.ModifiedDate,
                    TurnOff = u.TurnOff
                };
                listProperties.Add(category);
            });

            PropertiesViewModel viewModel = new PropertiesViewModel
            {
                PageViewModel = pageViewModel,
                Properties = listProperties
            };

            return View(viewModel);
        }

        [HttpGet]
        public ActionResult AddProperty()
        {
            PropertyViewModel model = new PropertyViewModel();

            return PartialView("_AddProperty", model);
        }

        [HttpPost]
        public ActionResult AddProperty(PropertyViewModel model)
        {
            Property propertyEntity = new Property
            {
                Name = model.Name,
                ValueName = model.ValueName,
                AddedDate = DateTime.Now,
                ModifiedDate = DateTime.Now,
                TurnOff = false
            };
            if (ModelState.IsValid)
            {
                propertyService.InsertProperty(propertyEntity);
                if (propertyEntity.Id > 0)
                {
                    return RedirectToAction("Properties");
                }
            }
            return PartialView("_AddProperty", model);
        }

        [HttpGet]
        public ActionResult EditProperty(long id)
        {
            Property propertyEntity = propertyService.GetProperty(id);
            PropertyViewModel model = new PropertyViewModel
            {
                Name = propertyEntity.Name,
                ValueName = propertyEntity.ValueName
            };

            return PartialView("_EditProperty", model);
        }

        [HttpPost]
        public ActionResult EditProperty(PropertyViewModel model, List<string> categories)
        {
            Property propertyEntity = propertyService.GetProperty(model.Id);

            propertyEntity.Name = model.Name;
            propertyEntity.ValueName = model.ValueName;
            propertyEntity.ModifiedDate = DateTime.Now;

            if (ModelState.IsValid)
            {
                if (propertyEntity.Id > 0)
                {
                    propertyService.UpdateProperty(propertyEntity);
                    return RedirectToAction("Properties");
                }
            }
            return PartialView("_EditProperty", model);
        }

        [HttpGet]
        public PartialViewResult DeleteProperty(long? id)
        {
            PropertyViewModel model = new PropertyViewModel();
            if (id.HasValue && id != 0)
            {
                Property propertyEntity = propertyService.GetProperty(id.Value);
                model.Name = propertyEntity.Name;
            }
            return PartialView("_DeleteProperty", model);
        }

        [HttpPost]
        public ActionResult DeleteProperty(long id)
        {
            propertyService.DeleteProperty(id);
            return RedirectToAction("Properties");
        }












        public ActionResult PropertyValuesList(long propertyId)
        {
            List<PropertyValueViewModel> propertyValues = new List<PropertyValueViewModel>();
            propertyValueService.GetPropertyValues().Where(p => p.PropertyId == propertyId).OrderBy(p => p.Value).ToList().ForEach(p =>
            {
                PropertyValueViewModel propertyValue = new PropertyValueViewModel
                {
                    Id = p.Id,
                    Value = p.Value,
                    PropertyId = p.PropertyId
                };
                propertyValues.Add(propertyValue);
            });
            ViewBag.propertyId = propertyId;
            return View(propertyValues);
        }


        [HttpGet]
        public ActionResult AddPropertyValue(long propertyId)
        {
            PropertyValueViewModel model = new PropertyValueViewModel();
            model.PropertyId = propertyId;

            return PartialView("_AddPropertyValue", model);
        }

        [HttpPost]
        public ActionResult AddPropertyValue(PropertyValueViewModel model)
        {
            PropertyValue propertyValueEntity = new PropertyValue
            {
                Value = model.Value,
                PropertyId = model.PropertyId
            };
            if (ModelState.IsValid)
            {
                propertyValueService.InsertPropertyValue(propertyValueEntity);
                if (propertyValueEntity.Id > 0)
                {
                    return RedirectToAction("PropertyValuesList", new { propertyId = model.PropertyId });
                }
            }
            return PartialView("_AddPropertyValue", model);
        }

        [HttpGet]
        public ActionResult EditPropertyValue(long id, long propertyId)
        {
            PropertyValueViewModel model = new PropertyValueViewModel();
            if (id != 0)
            {
                PropertyValue propertyValueEntity = propertyValueService.GetPropertyValue(id);
                model.Id = propertyValueEntity.Id;
                model.Value = propertyValueEntity.Value;
                model.PropertyId = propertyValueEntity.PropertyId;
            }

            return PartialView("_EditPropertyValue", model);
        }

        [HttpPost]
        public ActionResult EditPropertyValue(PropertyValueViewModel model)
        {
            PropertyValue propertyValueEntity = propertyValueService.GetPropertyValue(model.Id);
            propertyValueEntity.Value = model.Value;
            propertyValueEntity.PropertyId = model.PropertyId;

            if (ModelState.IsValid)
            {
                propertyValueService.UpdatePropertyValue(propertyValueEntity);
                if (propertyValueEntity.Id > 0)
                {
                    return RedirectToAction("PropertyValuesList", new { propertyId = model.PropertyId });
                }
            }
            return PartialView("_EditPropertyValue", model);
        }

        [HttpPost]
        public IActionResult DeletePropertyValue(long id)
        {
            PropertyValue propertyValueEntity = propertyValueService.GetPropertyValue(id);
            List<PropertyValCatArt> lpvca = propertyValCatArtService.GetAll().Where(p=> p.PropertyValueId == id).ToList();
            propertyValCatArtService.DeleteSome(lpvca);
            long productId = propertyValueEntity.PropertyId;
            propertyValueService.DeletePropertyValue(propertyValueEntity.Id);
            return RedirectToAction("PropertyValuesList", new { propertyId = productId });
        }





        [HttpGet]
        public IActionResult Colors(int sortType = 0, string searchString = null)
        {
            IEnumerable<Color> colors;
            if (searchString == null)
            {
                colors = colorService.GetColors();
            }
            else
            {
                searchString = searchString.ToUpper();
                colors = colorService.GetColors().Where(s => s.Name.ToUpper().Contains(searchString)
                    || s.ColorCode.ToString().ToUpper().Contains(searchString)
                    || s.AddedDate.ToString().ToUpper().Contains(searchString)
                    || s.ModifiedDate.ToString().ToUpper().Contains(searchString)
                );
            }

            switch (sortType)
            {
                case 0:
                    colors = colors.OrderByDescending(b => b.AddedDate);
                    break;
                case 1:
                    colors = colors.OrderBy(b => b.AddedDate);
                    break;
                case 2:
                    colors = colors.OrderByDescending(b => b.ModifiedDate);
                    break;
                case 3:
                    colors = colors.OrderBy(b => b.ModifiedDate);
                    break;
                case 4:
                    colors = colors.OrderBy(b => b.Name);
                    break;
                case 5:
                    colors = colors.OrderByDescending(b => b.Name);
                    break;
            }
            ViewBag.SortType = sortType;

            List<ColorViewModel> listColors = new List<ColorViewModel>();
            colors.ToList().ForEach(u =>
            {
                ColorViewModel color = new ColorViewModel
                {
                    Id = u.Id,
                    Name = u.Name,
                    ColorCode = u.ColorCode,
                    AddedDate = u.AddedDate,
                    ModifiedDate = u.ModifiedDate
                };
                listColors.Add(color);
            });

            return View(listColors);
        }

        [HttpGet]
        public ActionResult AddColor()
        {
            ColorViewModel model = new ColorViewModel();

            return PartialView("_AddColor", model);
        }

        [HttpPost]
        public ActionResult AddColor(ColorViewModel model)
        {
            Color colorEntity = new Color
            {
                Name = model.Name,
                ColorCode = model.ColorCode,
                AddedDate = DateTime.Now,
                ModifiedDate = DateTime.Now
            };
            if (ModelState.IsValid)
            {
                colorService.InsertColor(colorEntity);
                if (colorEntity.Id > 0)
                {
                    return RedirectToAction("Colors");
                }
            }
            return PartialView("_AddColor", model);
        }
        [HttpGet]
        public ActionResult EditColor(long id, long colorId)
        {
            ColorViewModel model = new ColorViewModel();
            if (id != 0)
            {
                Color ColorEntity = colorService.GetColor(id);
                model.Id = ColorEntity.Id;
                model.Name = ColorEntity.Name;
                model.ColorCode = ColorEntity.ColorCode;
            }

            return PartialView("_EditColor", model);
        }

        [HttpPost]
        public ActionResult EditColor(ColorViewModel model)
        {
            Color ColorEntity = colorService.GetColor(model.Id);
            ColorEntity.Name = model.Name;
            ColorEntity.ColorCode = model.ColorCode;

            if (ModelState.IsValid)
            {
                colorService.UpdateColor(ColorEntity);
                if (ColorEntity.Id > 0)
                {
                    return RedirectToAction("Colors");
                }
            }
            return PartialView("_EditColor", model);
        }

        [HttpPost]
        public ActionResult DeleteColor(long id)
        {
            colorService.DeleteColor(id);
            return RedirectToAction("Colors");
        }


        //Переключатель свойства
        [HttpPost]
        public ActionResult TurnOnOffProperty(long Id, string returnurl)
        {
            Property property = propertyService.GetProperty(Id);
            property.TurnOff = !property.TurnOff;
            propertyService.UpdateProperty(property);

            return RedirectToAction("Properties", "Property", new { returnurl });
        }


    }
}