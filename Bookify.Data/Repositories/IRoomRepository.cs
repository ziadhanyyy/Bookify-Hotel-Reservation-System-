using Bookify.Core.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bookify.Data.Repositories
{
    public interface IRoomRepository:IRepository<Room>
    {
        Task<IEnumerable<Room>> GetAvailableRoomsAsync();
       Task<IEnumerable<Room>> GetAllAsync();

        Task<Room?> GetByIdAsync(int id);
    }
}
