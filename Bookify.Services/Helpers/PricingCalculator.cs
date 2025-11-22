using Bookify.Core.Entities;
using Bookify.Data.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bookify.Services.Helpers
{
    public class PricingCalculator
    {
        private readonly IUnitOfWork _unitOfWork;

        public PricingCalculator(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public decimal CalculatePrice(int roomId, DateTime checkIn, DateTime checkOut)
        {
            var room = _unitOfWork.Rooms.GetByIdAsync(roomId).Result;
            var nights = (checkOut - checkIn).Days;

            if (room == null)
            {
                
                throw new InvalidOperationException("Cannot calculate price: Booking is missing Room details.");
            }
   
            return nights * room.roomType.PricePerNight;
        }
    }
}

