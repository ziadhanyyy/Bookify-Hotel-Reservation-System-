using Bookify.Core.Entities;
using Bookify.Data.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bookify.Data.Repositories
{
    public class RoomTypeRepository :Repository<RoomType>, IRoomTypeRepository
    {
       public RoomTypeRepository(BookifyDbContext context) : base(context) { }
    }
}
