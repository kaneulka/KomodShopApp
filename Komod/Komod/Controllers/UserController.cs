using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Komod.Data;
using Komod.Ser;
using Komod.Web.Models;
using Komod.Web.Models.UserModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Komod.Web.Controllers
{
    [Authorize(Roles = "admin")]
    public class UserController : Controller
    {
        RoleManager<IdentityRole> _roleManager;
        UserManager<User> _userManager;

        public UserController(RoleManager<IdentityRole> roleManager, UserManager<User> userManager)
        {
            _roleManager = roleManager;
            _userManager = userManager;
        }

        public IActionResult Users(int page = 1, int sortType = 0, string searchString = null)
        {
            List<UserViewModel> model = new List<UserViewModel>();
            _userManager.Users.ToList().ForEach(u =>
            {
                UserViewModel user = new UserViewModel
                {
                    Id = u.Id,
                    Email = u.Email,
                    UserFIO = u.UserFIO
                };
                model.Add(user);
            });

            IEnumerable<User> users;
            if (searchString == null)
            {
                users = _userManager.Users;
            }
            else
            {
                searchString = searchString.ToUpper();
                users = _userManager.Users.Where(s => s.Email.ToUpper().Contains(searchString)
                    || s.UserFIO.ToUpper().Contains(searchString)
                );
            }

            switch (sortType)
            {
                case 0:
                    users = users.OrderBy(b => b.Email);
                    break;
                case 1:
                    users = users.OrderByDescending(b => b.Email);
                    break;
                case 2:
                    users = users.OrderBy(b => b.UserFIO);
                    break;
                case 3:
                    users = users.OrderByDescending(b => b.UserFIO);
                    break;
            }
            ViewBag.SortType = sortType;

            int pageSize = 20;

            List<UserViewModel> listUsers = new List<UserViewModel>();

            var count = users.Count();
            var items = users.Skip((page - 1) * pageSize).Take(pageSize).ToList();
            PageViewModel pageViewModel = new PageViewModel(count, page, pageSize);
            items.ForEach(u =>
            {
                UserViewModel user = new UserViewModel
                {
                    Id = u.Id,
                    Email = u.Email,
                    UserFIO = u.UserFIO,
                    EmailConfirmed = u.EmailConfirmed

                };
                listUsers.Add(user);
            });

            UsersViewModel viewModel = new UsersViewModel
            {
                PageViewModel = pageViewModel,
                Users = listUsers
            };

            return View(viewModel);
        }

        public IActionResult AddUser()
        {
            CreateUserViewModel model = new CreateUserViewModel();

            return PartialView("_AddUser", model);
        }

        [HttpPost]
        public async Task<IActionResult> AddUser(CreateUserViewModel model)
        {
            User userEntity = new User
            {
                Email = model.Email,
                UserName = model.UserName,
                UserFIO = model.UserFIO,
                EmailConfirmed = true
            };
            var result = await _userManager.CreateAsync(userEntity, model.Password);
            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(userEntity, "user");
                return RedirectToAction("Users");
            }
            else
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }
            return PartialView("_AddUser", model);
        }

        public async Task<IActionResult> EditUser(string id)
        {
            User user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            UserViewModel model = new UserViewModel
            {
                Id = user.Id,
                Email = user.Email,
                UserFIO = user.UserFIO
            };
            return PartialView("_EditUser", model);
        }

        [HttpPost]
        public async Task<IActionResult> EditUser(UserViewModel model)
        {
            if (ModelState.IsValid)
            {
                User user = await _userManager.FindByIdAsync(model.Id);
                if (user != null)
                {
                    user.Email = model.Email;
                    user.UserName = model.Email;
                    user.UserFIO = model.UserFIO;

                    var result = await _userManager.UpdateAsync(user);
                    if (result.Succeeded)
                    {
                        return RedirectToAction("Users");
                    }
                    else
                    {
                        foreach (var error in result.Errors)
                        {
                            ModelState.AddModelError(string.Empty, error.Description);
                        }
                    }
                }
            }
            return PartialView("_EditUser", model);
        }

        [HttpGet]
        public async Task<PartialViewResult> DeleteUser(string id)
        {
            UserViewModel model = new UserViewModel();
            if (id != null)
            {
                User user = await _userManager.FindByIdAsync(id);
                model.Email = user.UserName;
            }
            return PartialView("_DeleteUser", model);
        }

        [HttpPost]
        public async Task<ActionResult> DeleteUser(UserViewModel model)
        {
            User user = await _userManager.FindByIdAsync(model.Id);
            if (user != null)
            {
                IdentityResult result = await _userManager.DeleteAsync(user);
            }
            return RedirectToAction("Users");
        }

        public async Task<IActionResult> ChangePassword(string id)
        {
            User user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            ChangePasswordViewModel model = new ChangePasswordViewModel { Id = user.Id, Email = user.Email };
            return PartialView("_ChangePassword", model);
        }

        [HttpPost]
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                User user = await _userManager.FindByIdAsync(model.Id);
                if (user != null)
                {
                    var _passwordValidator =
                        HttpContext.RequestServices.GetService(typeof(IPasswordValidator<User>)) as IPasswordValidator<User>;
                    var _passwordHasher =
                        HttpContext.RequestServices.GetService(typeof(IPasswordHasher<User>)) as IPasswordHasher<User>;

                    IdentityResult result =
                        await _passwordValidator.ValidateAsync(_userManager, user, model.NewPassword);
                    if (result.Succeeded)
                    {
                        user.PasswordHash = _passwordHasher.HashPassword(user, model.NewPassword);
                        await _userManager.UpdateAsync(user);
                        return RedirectToAction("Users");
                    }
                    else
                    {
                        foreach (var error in result.Errors)
                        {
                            ModelState.AddModelError(string.Empty, error.Description);
                        }
                    }
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Пользователь не найден");
                }
            }
            return PartialView("_ChangePassword", model);
        }

        [HttpGet]
        public async Task<IActionResult> EditUserRoles(string userId)
        {
            // получаем пользователя
            User user = await _userManager.FindByIdAsync(userId);
            if (user != null)
            {
                // получем список ролей пользователя
                var userRoles = await _userManager.GetRolesAsync(user);
                var allRoles = _roleManager.Roles.ToList();
                ChangeRoleViewModel model = new ChangeRoleViewModel
                {
                    UserId = user.Id,
                    UserEmail = user.Email,
                    UserRoles = userRoles,
                    AllRoles = allRoles
                };
                return PartialView("_EditUserRoles", model);
            }

            return NotFound();
        }
        [HttpPost]
        public async Task<IActionResult> EditUserRoles(string userId, List<string> roles)
        {
            // получаем пользователя
            User user = await _userManager.FindByIdAsync(userId);
            if (user != null)
            {
                // получем список ролей пользователя
                var userRoles = await _userManager.GetRolesAsync(user);
                // получаем все роли
                var allRoles = _roleManager.Roles.ToList();
                // получаем список ролей, которые были добавлены
                var addedRoles = roles.Except(userRoles);
                // получаем роли, которые были удалены
                var removedRoles = userRoles.Except(roles);

                await _userManager.AddToRolesAsync(user, addedRoles);

                await _userManager.RemoveFromRolesAsync(user, removedRoles);

                return RedirectToAction("Users");
            }

            return NotFound();
        }



        //Проверка почты
        [AcceptVerbs("Get", "Post")]
        public IActionResult CheckEmail(string email)
        {
            List<UserViewModel> users = new List<UserViewModel>();
            _userManager.Users.ToList().ForEach(u =>
            {
                UserViewModel user = new UserViewModel
                {
                    Id = u.Id,
                    Email = u.Email,
                    UserFIO = u.UserFIO
                };
                users.Add(user);
            });

            foreach (var user in users)
            {
                if (email == user.Email)
                    return Json("false");
            }
            return Json("true");
        }
        public async Task SendConfirmationEmail(User user)
        {
            var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            var callbackUrl = Url.Action(
                "ConfirmEmail",
                "Account",
                new { userId = user.Id, code },
                protocol: HttpContext.Request.Scheme
                );
            await _userManager.AddToRoleAsync(user, "user");
            EmailService emailService = new EmailService();
            await emailService.SendEmailAsync(user.Email, "Confirm your account", $"Подтвердите ваш Email <a href='{callbackUrl}'>перейдя по этой ссылке</a>");
        }
    }
}