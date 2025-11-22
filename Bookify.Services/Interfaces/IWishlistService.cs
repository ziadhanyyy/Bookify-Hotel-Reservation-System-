using Bookify.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bookify.Services.Interfaces
{
    public interface IWishlistService
    {
        Task<IEnumerable<Wishlist>> GetUserWishlistAsync(int userId);
        Task<bool> AddToWishlistAsync(Wishlist item);
        Task<bool> RemoveFromWishlistAsync(int id);
    }
}
