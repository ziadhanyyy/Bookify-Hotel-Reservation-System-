using Bookify.Core.Entities;
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
            Payments = new PaymentRepository(_context);
        }

        public IRoomRepository Rooms { get; private set; }
        public IBookingRepository Bookings { get; private set; }
        public IPaymentRepository Payments { get; private set; }



        public int Complete() => _context.SaveChanges(); 

        public void Dispose() => _context.Dispose();     
    }
}

