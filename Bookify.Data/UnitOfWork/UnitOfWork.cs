using Bookify.Data.Context;
using Bookify.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bookify.Data.UnitOfWork
{
    public class UnitOfWork:IUnitOfWork
    {
        private readonly BookifyDbContext _context;

        public UnitOfWork(BookifyDbContext context)
        {
            _context = context;
            Rooms = new RoomRepository(_context);     
            Bookings = new BookingRepository(_context);
            Reviews = new ReviewRepository(_context);
            Wishlists = new WishlistRepository(_context);
            RoomTypes = new RoomTypeRepository(_context);
        }

        public IRoomRepository Rooms { get; private set; }
        public IBookingRepository Bookings { get; private set; }
        public IReviewRepository Reviews { get; private set; }
        public IWishlistRepository Wishlists { get; private set; }
        public IRoomTypeRepository RoomTypes { get; private set; }

        public async Task<int> CompleteAsync() => await _context.SaveChangesAsync();
        public int Complete() => _context.SaveChanges(); 

        public void Dispose() => _context.Dispose();     
    }
}

