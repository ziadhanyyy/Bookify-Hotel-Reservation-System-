using Bookify.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bookify.Data.UnitOfWork
{
    public interface IUnitOfWork:IDisposable
    {
        IRoomRepository Rooms { get; }
        IBookingRepository Bookings { get; }
        IPaymentRepository Payments { get; }
        IReviewRepository Reviews { get;  }
        IWishlistRepository Wishlists { get;  }
         IRoomTypeRepository RoomTypes { get; }
        int Complete();
        Task<int> CompleteAsync();
    }
}
