using Komod.Data;
using Komod.Repo.WishlistRepo;
using System;
using System.Collections.Generic;
using System.Text;

namespace Komod.Ser.WishlistSer
{
    public class WishlistItemService : IWishlistItemService
    {
        private IWishlistItemRepository wishlistItemRepository;

        public WishlistItemService(IWishlistItemRepository wishlistItemRepository)
        {
            this.wishlistItemRepository = wishlistItemRepository;
        }

        public IEnumerable<WishlistItem> GetWishlistItems()
        {
            return wishlistItemRepository.GetAll();
        }

        public WishlistItem GetWishlistItem(WishlistItem wishlistItem)
        {
            return wishlistItemRepository.Get(wishlistItem);
        }

        public void InsertWishlistItem(WishlistItem wishlistItem)
        {
            wishlistItemRepository.Insert(wishlistItem);
        }
        public void UpdateWishlistItem(WishlistItem wishlistItem)
        {
            wishlistItemRepository.Update(wishlistItem);
        }

        public void DeleteWishlistItem(WishlistItem wishlistItem)
        {
            wishlistItemRepository.Delete(wishlistItem);
        }
    }
}
