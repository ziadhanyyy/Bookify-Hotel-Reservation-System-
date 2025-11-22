using Bookify.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bookify.Data.Repositories
{
    public interface IReviewRepository:IRepository<Review>
    {
        Task<IEnumerable<Review>> GetReviewsByRoomIdAsync(int id);
    }
}
