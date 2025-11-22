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
    public class RoomRepository : Repository<Room>, IRoomRepository

    {
        public RoomRepository(BookifyDbContext context) : base(context) { }
        public async Task<IEnumerable<Room>> GetAvailableRoomsAsync()
        {
            return  await _context.Rooms.Where(x=>x.IsAvailable==true).ToListAsync();
        }

       
    }
}
