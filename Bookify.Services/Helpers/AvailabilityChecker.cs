using Bookify.Core.Entities;
using Bookify.Data.Repositories;
using Bookify.Data.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bookify.Services.Helpers
{
    public class AvailabilityChecker
    {
        private readonly IUnitOfWork _unitOfWork ;

        AvailabilityChecker(IUnitOfWork unitOfWork) { _unitOfWork = unitOfWork; }
         public async Task<bool>IsRoomAvailableAsync(int roomId, DateTime checkIn, DateTime checkOut)
        {
            var bookings = await _unitOfWork.Bookings.FindAsync(b =>
               b.RoomId == roomId &&
               b.CheckOutDate > checkIn &&
               b.CheckInDate < checkOut
           );

            return !bookings.Any();
        }
    }
}
