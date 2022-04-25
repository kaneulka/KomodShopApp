using Komod.Data;
using Komod.Repo.WishlistRepo;
using System;
using System.Collections.Generic;
using System.Text;

namespace Komod.Ser.WishlistSer
{
    public class WishlistService : IWishlistService
    {
        private IWishlistRepository wishlistRepository;

        public WishlistService(IWishlistRepository wishlistRepository)
        {
            this.wishlistRepository = wishlistRepository;
        }

        public Wishlist GetWishlist(long id)
        {
            return wishlistRepository.Get(id);
        }
        public Wishlist GetWishlistByUser(string name)
        {
            return wishlistRepository.GetByUser(name);
        }

        public void InsertWishlist(Wishlist wishlist)
        {
            wishlistRepository.Insert(wishlist);
        }

        public void DeleteWishlist(long id)
        {
            Wishlist wishlist = GetWishlist(id);
            wishlistRepository.Remove(wishlist);
            wishlistRepository.SaveChanges();
        }
    }
}
