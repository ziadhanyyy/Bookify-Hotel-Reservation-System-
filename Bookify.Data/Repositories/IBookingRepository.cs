using Bookify.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bookify.Data.Repositories
{
    public interface IBookingRepository  :IRepository<Booking>
    {
       Task< IEnumerable<Booking>> GetBookingsByUserIdAsync(string userId);
       Task<IEnumerable<Booking>> GetAllWithDetailsAsync();
    }
}
