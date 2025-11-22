using Bookify.Core.Entities;
using Bookify.Data.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bookify.Data.Repositories
{
    public class WishlistRepository:Repository<Wishlist>,IWishlistRepository

    {
         public WishlistRepository(BookifyDbContext c) : base(c) { }

        public async Task<IEnumerable<Wishlist>> GetWishlistByUserIDAsync(string userID)
        {
            return await _context.Wishlists
                 .Where(w => w.UserId == userID).ToListAsync();
        }
    }
}
