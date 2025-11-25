using Bookify.Core.DTOs;
using Bookify.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bookify.Services.Interfaces
{
    public interface ICustomerService
    {

        // --- Rooms ---
        Task<IEnumerable<RoomDto>> GetAvailableRoomsAsync();
        Task<IEnumerable<RoomDto>> SearchRoomsAsync(string query);
        Task<RoomDto> GetRoomDetailsAsync(int roomId);

        // --- Wishlist ---
        Task<bool> AddToWishlistAsync(string userId, int roomId);
        Task<IEnumerable<RoomDto>> GetWishlistAsync(string userId);

        // --- Booking ---
        Task<int> ConfirmBookingAsync(BookingDto dto);
        Task<IEnumerable<BookingDto>> GetBookingHistoryAsync(string userId);

        // --- Reviews ---
        Task<bool> AddReviewAsync(ReviewDto dto);

    }
}
