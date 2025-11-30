using Bookify.Core.DTOs;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bookify.Services.Interfaces
{
    public interface IAdminService
    {
        Task<bool> AssignRoleAsync(AssignRoleDto assignRoleDto);
        Task<bool> AddRoomTypeAsync(RoomTypeDto dto);
        Task<IEnumerable<RoomTypeDto>> GetRoomTypesAsync();
        Task<bool> AddRoomAsync(RoomDto dto, IFormFile roomImage);
        Task<IEnumerable<RoomDto>> GetRoomsAsync();
        Task<IEnumerable<BookingDto>> GetAllBookingsAsync();
        Task<bool> DeleteRoomAsync(int roomId);
        Task<bool> DeleteRoomTypeAsync(int roomTypeId);
        Task<DashboardStatsDto> GetDashboardStatsAsync();


    }
}
