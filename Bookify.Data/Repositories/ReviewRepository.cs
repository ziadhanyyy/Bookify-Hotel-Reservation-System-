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
    public class ReviewRepository : Repository<Review>, IReviewRepository
    {
        public ReviewRepository(BookifyDbContext context) : base(context)
        {
        }

       

        public async Task<IEnumerable<Review>> GetReviewsByRoomIdAsync(int id)
        {
            return  await _context.Reviews.Where(x => x.RoomId == id).ToListAsync();
            
        }
    }
}
