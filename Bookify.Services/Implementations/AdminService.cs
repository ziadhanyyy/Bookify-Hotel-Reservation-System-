using Bookify.Core.DTOs;
using Bookify.Core.Entities;
using Bookify.Data.UnitOfWork;
using Bookify.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;


namespace Bookify.Services.Implementations
{
    public class AdminService : IAdminService

    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IUnitOfWork _uow;
         public AdminService(UserManager<User> userManager, RoleManager<IdentityRole> roleManager,IUnitOfWork uow)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _uow = uow;
        }

        public async Task<bool> AssignRoleAsync(AssignRoleDto assignRoleDto)
        {
            var user =  await _userManager.FindByEmailAsync(assignRoleDto.Email);
            if (user == null)
                return false;

            var result = await _userManager.AddToRoleAsync(user, assignRoleDto.Role);
            return result.Succeeded;
        }
        public async Task<bool> AddRoomTypeAsync(RoomTypeDto dto)
        {
            var roomType = new RoomType
            {
                
                Name = dto.Name,
                Description = dto.Description,
                PricePerNight = dto.PricePerNight,
                 Capacity = dto.Capacity

    };
            await _uow.RoomTypes.AddAsync(roomType);
             await _uow.CompleteAsync();
            return true;
        }

        public async Task<bool> AddRoomAsync(RoomDto dto, IFormFile roomImage)
        {
            string imageUrl = null;

            // Handle image upload
            if (roomImage != null && roomImage.Length > 0)
            {
                var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif", ".webp" };
                var fileExtension = Path.GetExtension(roomImage.FileName).ToLowerInvariant();

                if (!allowedExtensions.Contains(fileExtension))
                    throw new Exception("Invalid file type. Please upload an image (JPG, PNG, GIF, or WEBP).");

                if (roomImage.Length > 5 * 1024 * 1024)
                    throw new Exception("File size exceeds 5MB limit.");

                var roomsImagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", "rooms");
                if (!Directory.Exists(roomsImagePath))
                    Directory.CreateDirectory(roomsImagePath);

                var uniqueFileName = $"{Guid.NewGuid()}{fileExtension}";
                var filePath = Path.Combine(roomsImagePath, uniqueFileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await roomImage.CopyToAsync(stream);
                }

                imageUrl = $"/images/rooms/{uniqueFileName}";
            }

            // Create Room entity
            var room = new Room
            {
                RoomNumber = dto.RoomNumber,
                RoomTypeId = dto.RoomTypeId,
                IsAvailable = true,
                ImageURL = imageUrl
            };

            await _uow.Rooms.AddAsync(room);
            await _uow.CompleteAsync();

            return true;
        }
        public async Task<bool> UpdateRoomTypeAsync(RoomTypeDto dto)
        {
            var roomType = await _uow.RoomTypes.GetByIdAsync(dto.Id);
            if (roomType == null) return false;

            roomType.Name = dto.Name;
            roomType.Description = dto.Description;
            roomType.PricePerNight = dto.PricePerNight;
            roomType.Capacity = dto.Capacity;

            _uow.RoomTypes.Update(roomType);
            await _uow.CompleteAsync();
            return true;
        }


        public async Task<IEnumerable<RoomTypeDto>> GetRoomTypesAsync()
        {
            var roomTypes = await _uow.RoomTypes.GetAllAsync();
            return roomTypes.Select(rt => new RoomTypeDto
            {
                Id = rt.RoomTypeId,
                Name = rt.Name,
                Description = rt.Description,
                PricePerNight = rt.PricePerNight
            });
        }
        public async Task<IEnumerable<RoomDto>> GetRoomsAsync()
        {
            var rooms = await _uow.Rooms.GetAllAsync();
            return rooms.Select(r => new RoomDto
            {
                Id = r.RoomId,
                RoomNumber = r.RoomNumber,
                RoomTypeId = r.RoomTypeId,
                ImageURL = r.ImageURL
            });
        }
        public async Task<bool> UpdateRoomAsync(RoomDto dto, IFormFile? roomImage)
        {
            var room = await _uow.Rooms.GetByIdAsync(dto.Id);
            if (room == null) return false;

            room.RoomNumber = dto.RoomNumber;
            room.RoomTypeId = dto.RoomTypeId;

            
            if (roomImage != null && roomImage.Length > 0)
            {
                var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif", ".webp" };
                var fileExtension = Path.GetExtension(roomImage.FileName).ToLowerInvariant();

                if (!allowedExtensions.Contains(fileExtension))
                    throw new Exception("Invalid file type.");

                var roomsImagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", "rooms");
                var uniqueFileName = $"{Guid.NewGuid()}{fileExtension}";
                var filePath = Path.Combine(roomsImagePath, uniqueFileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await roomImage.CopyToAsync(stream);
                }

                room.ImageURL = $"/images/rooms/{uniqueFileName}";
            }

            _uow.Rooms.Update(room);
            await _uow.CompleteAsync();
            return true;
        }

        public async Task<IEnumerable<BookingDto>> GetAllBookingsAsync()
        {
            var bookings = await _uow.Bookings.GetAllWithDetailsAsync();

            return bookings.Select(b => new BookingDto
            {
                Id = b.Id,
                UserEmail = b.User?.Email ?? "",
                RoomNumber = b.Room?.RoomNumber ?? "",
                BookingDate = b.BookingDate,
                CheckInDate = b.CheckInDate,
                CheckOutDate = b.CheckOutDate,
                Nights = b.Nights,
                TotalAmount = b.TotalAmount,
                Status = b.Status
            });
        }
        public async Task<bool> DeleteRoomAsync(int roomId)
        {
            var room = await _uow.Rooms.GetByIdAsync(roomId);
            if (room == null)
                return false;

            _uow.Rooms.Delete(room);
            await _uow.CompleteAsync();
            return true;
        }
        public async Task<bool> DeleteRoomTypeAsync(int roomTypeId)
        {
            var roomType = await _uow.RoomTypes.GetByIdAsync(roomTypeId);
            if (roomType == null)
                return false;

            
            var roomsWithType = (await _uow.Rooms.FindAsync(r => r.RoomTypeId == roomTypeId)).ToList();
            if (roomsWithType.Any())
                return false; 

            _uow.RoomTypes.Delete(roomType);
            await _uow.CompleteAsync();
            return true;
        }
        public async Task<DashboardStatsDto> GetDashboardStatsAsync()
        {
           

            var totalRooms = (await _uow.Rooms.GetAllAsync()).Count();
            var totalBookings = (await _uow.Bookings.GetAllAsync()).Count();
            var totalRoomTypes = (await _uow.RoomTypes.GetAllAsync()).Count();


            return new DashboardStatsDto
            {
                TotalRooms = totalRooms,
                TotalBookings = totalBookings,
                RoomTypes = totalRoomTypes,
                
            };
        }

    }
}
