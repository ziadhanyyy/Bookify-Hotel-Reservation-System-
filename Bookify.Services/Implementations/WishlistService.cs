using Bookify.Core.Entities;
using Bookify.Data.Repositories;
using Bookify.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bookify.Services.Implementations
{
    public class WishlistService : IWishlistService
    {
        private readonly IWishlistRepository _wishlist;
        public WishlistService(IWishlistRepository wishlist) { 
        _wishlist = wishlist;
        }
        public async Task<bool> AddToWishlistAsync(Wishlist item)
        {
            await _wishlist.AddAsync(item);
           throw new NotImplementedException();

            
           
        }

        public Task<IEnumerable<Wishlist>> GetUserWishlistAsync(int userId)
        {
            throw new NotImplementedException();
        }

        public Task<bool> RemoveFromWishlistAsync(int id)
        {
            throw new NotImplementedException();
        }
    }
}
