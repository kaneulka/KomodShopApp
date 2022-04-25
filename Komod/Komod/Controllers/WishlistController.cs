using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Komod.Data;
using Komod.Ser.WishlistSer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Komod.Web.Controllers
{
    public class WishlistController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly IWishlistService wishlistService;
        private readonly IWishlistItemService wishlistItemService;

        public WishlistController(UserManager<User> userManager, IWishlistService wishlistService, IWishlistItemService wishlistItemService)
        {
            _userManager = userManager;
            this.wishlistService = wishlistService;
            this.wishlistItemService = wishlistItemService;
        }
        public async Task<IActionResult> AddProductToWishlist(long productId)
        {
            if (User.Identity.IsAuthenticated)
            {
                ClaimsPrincipal currentUser = this.User;
                User user = await _userManager.GetUserAsync(currentUser);
                long wishlistId = wishlistService.GetWishlistByUser(user.UserName).Id;
                WishlistItem wishlistItem = new WishlistItem()
                {
                    WishlistId = wishlistId,
                    ProductId = productId
                };
                wishlistItemService.InsertWishlistItem(wishlistItem);
                return StatusCode(201);
            }
            else
            {
                return StatusCode(401);
            }
        }

        public async Task<IActionResult> RemoveProductFromWishlost(long productId)
        {
            if (User.Identity.IsAuthenticated)
            {
                ClaimsPrincipal currentUser = this.User;
                User user = await _userManager.GetUserAsync(currentUser);
                long wishlistId = wishlistService.GetWishlistByUser(user.UserName).Id;
                WishlistItem wishlistItem = wishlistItemService.GetWishlistItems().SingleOrDefault(wi => wi.ProductId == productId && wi.WishlistId == wishlistId);
                wishlistItemService.DeleteWishlistItem(wishlistItem);
                return StatusCode(201);
            }
            else
            {
                return StatusCode(401);
            }
        }
    }
}