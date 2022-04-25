using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Komod.Web.Models.UserModels;
using Komod.Web.Models;
using Komod.Data;

namespace Komod.Web.Controllers
{
    [Authorize(Roles = "admin")]
    public class RoleController : Controller
    {
        RoleManager<IdentityRole> _roleManager;
        UserManager<User> _userManager;
        public RoleController(RoleManager<IdentityRole> roleManager, UserManager<User> userManager)
        {
            _roleManager = roleManager;
            _userManager = userManager;
        }
        public IActionResult Roles(int page = 1, int sortType = 0, string searchString = null)
        {
            IEnumerable<IdentityRole> roles;
            if (searchString == null)
            {
                roles = _roleManager.Roles;
            }
            else
            {
                searchString = searchString.ToUpper();
                roles = _roleManager.Roles.Where(s => s.Name.ToUpper().Contains(searchString));
            }

            switch (sortType)
            {
                case 0:
                    roles = roles.OrderBy(b => b.Name);
                    break;
                case 1:
                    roles = roles.OrderByDescending(b => b.Name);
                    break;
            }
            ViewBag.SortType = sortType;

            int pageSize = 20;

            List<RoleViewModel> listRoles = new List<RoleViewModel>();

            var count = roles.Count();
            var items = roles.Skip((page - 1) * pageSize).Take(pageSize).ToList();
            PageViewModel pageViewModel = new PageViewModel(count, page, pageSize);
            items.ForEach(u =>
            {
                RoleViewModel role = new RoleViewModel
                {
                    Id = u.Id,
                    Name = u.Name

                };
                listRoles.Add(role);
            });

            RolesViewModel viewModel = new RolesViewModel
            {
                PageViewModel = pageViewModel,
                Roles = listRoles
            };

            return View(viewModel);
        }

        public IActionResult AddRole()
        {
            RoleViewModel model = new RoleViewModel();
            return PartialView("_AddRole", model);
        }

        [HttpPost]
        public async Task<IActionResult> AddRole(RoleViewModel model)
        {
            var name = model.Name;
            if (!string.IsNullOrEmpty(name))
            {
                IdentityResult result = await _roleManager.CreateAsync(new IdentityRole(name));
                if (result.Succeeded)
                {
                    return RedirectToAction("Roles");
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
            }
            return PartialView("_AddRole", model);
        }

        public async Task<PartialViewResult> DeleteRoleAsync(string id)
        {
            RoleViewModel model = new RoleViewModel();
            if (id != null)
            {
                IdentityRole role = await _roleManager.FindByIdAsync(id);
                model.Name = role.Name;
            }
            return PartialView("_DeleteRole", model);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteRole(string id)
        {
            IdentityRole role = await _roleManager.FindByIdAsync(id);
            if (role != null)
            {
                IdentityResult result = await _roleManager.DeleteAsync(role);
            }
            return RedirectToAction("Roles");
        }
    }
}