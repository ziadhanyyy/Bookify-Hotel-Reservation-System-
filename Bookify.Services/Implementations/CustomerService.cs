using Bookify.Core.DTOs;
using Bookify.Core.Entities;
using Bookify.Data.UnitOfWork;
using Bookify.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bookify.Services.Implementations
{
    public class CustomerService : ICustomerService
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IUnitOfWork _uow;

        public CustomerService(UserManager<User> userManager, SignInManager<User> signInManager, IUnitOfWork uow)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _uow = uow;
        }
        // --- Rooms ---
        public async Task<IEnumerable<RoomDto>> GetAvailableRoomsAsync()
        {
            var rooms = await _uow.Rooms.GetAllAsync(); // استخدم GetAllAsync من UnitOfWork

            return rooms.Where(r => r.IsAvailable)
                        .Select(r => new RoomDto
                        {
                            Id = r.RoomId,
                            Name = r.roomType?.Name ?? "No Type",
                            RoomNumber = r.RoomNumber,
                            Type = r.roomType?.Name ?? "No Type",
                            Price = r.roomType?.PricePerNight ?? 0,
                            RoomTypeId = r.RoomTypeId,
                            Description = r.roomType?.Description ?? "",
                            ImageURL = r.ImageURL
                        });
        }

        public async Task<IEnumerable<RoomDto>> SearchRoomsAsync(string query)
        {
            if (string.IsNullOrWhiteSpace(query))
                return await GetAvailableRoomsAsync();

            query = query.Trim().ToLowerInvariant();

            var rooms = await _uow.Rooms.GetAllAsync(); 

            var filtered = rooms
                .Where(r => r.IsAvailable) 
                .Where(r =>
                {
                    
                    var roomNumber = (r.RoomNumber ?? "").ToLowerInvariant();
                    var typeName = (r.roomType?.Name ?? "").ToLowerInvariant();
                    var typeDesc = (r.roomType?.Description ?? "").ToLowerInvariant();
                    var roomName = (r.roomType?.Name ?? "").ToLowerInvariant(); 

                    return roomNumber.Contains(query)
                        || typeName.Contains(query)
                        || typeDesc.Contains(query)
                        || roomName.Contains(query);
                });

            return filtered.Select(r => new RoomDto
            {
                Id = r.RoomId,
                Name = r.roomType?.Name ?? "No Type",
                RoomNumber = r.RoomNumber,
                Type = r.roomType?.Name ?? "No Type",
                Price = r.roomType?.PricePerNight ?? 0,
                RoomTypeId = r.RoomTypeId,
                Description = r.roomType?.Description ?? "",
                ImageURL = r.ImageURL
            });
        }
        public async Task<RoomDto> GetRoomDetailsAsync(int roomId)
        {
            var room = await _uow.Rooms.GetByIdAsync(roomId); 
            if (room == null) return null;

            return new RoomDto
            {
                Id = room.RoomId,
                Name = room.roomType?.Name ?? "No Type",
                RoomNumber = room.RoomNumber,
                Type = room.roomType?.Name ?? "No Type",
                Price = room.roomType?.PricePerNight ?? 0,
                RoomTypeId = room.RoomTypeId,
                Description = room.roomType?.Description ?? "",
                ImageURL = room.ImageURL
            };
        }



        // --- Wishlist ---
        
        public async Task<bool> AddToWishlistAsync(string userId, int roomId)
        {
            
            var exists = (await _uow.Wishlists.FindAsync(w => w.UserId == userId && w.RoomId == roomId)).Any();
            if (exists) return true;

            var wishlist = new Wishlist { UserId = userId, RoomId = roomId };
            await _uow.Wishlists.AddAsync(wishlist);
            await _uow.CompleteAsync();
            return true;
        }

        public async Task<IEnumerable<RoomDto>> GetWishlistAsync(string userId)
        {
            var wishlists = await _uow.Wishlists.FindAsync(w => w.UserId == userId);
            var rooms = new List<RoomDto>();

            foreach (var w in wishlists)
            {
                
                var room = await _uow.Rooms.GetByIdAsync(w.RoomId);
                if (room == null) continue;

                rooms.Add(new RoomDto
                {
                    Id = room.RoomId,
                    RoomNumber = room.RoomNumber,
                    RoomTypeId = room.RoomTypeId,
                    ImageURL = room.ImageURL
                });
            }

            return rooms;
        }

        // --- Booking ---
        public async Task<int> ConfirmBookingAsync(BookingDto dto)
        {
            var booking = new Booking
            {
                UserId = dto.UserId,
                RoomId = dto.RoomId,
                BookingDate = dto.BookingDate,
                CheckInDate = dto.CheckInDate,
                CheckOutDate = dto.CheckOutDate,
                Nights = dto.Nights,
                TotalAmount = dto.TotalAmount,
                Status = "Pending"
            };

            await _uow.Bookings.AddAsync(booking);
            await _uow.CompleteAsync();

            return booking.Id;  
        }

        public async Task<IEnumerable<BookingDto>> GetBookingHistoryAsync(string userId)
        {
            var bookings = await _uow.Bookings.GetBookingsByUserIdAsync(userId);
            return bookings.Select(b => new BookingDto
            {
                Id = b.Id,
                RoomId = b.RoomId,                
                RoomNumber = b.Room?.RoomNumber,  
                CheckInDate = b.CheckInDate,
                CheckOutDate = b.CheckOutDate,
                Nights = (b.CheckOutDate - b.CheckInDate).Days,
                TotalAmount = b.TotalAmount,
                Status = b.Status,
                BookingDate = b.BookingDate
            });
        }

        // --- Reviews ---
        public async Task<bool> AddReviewAsync(ReviewDto dto)
        {
            var review = new Review
            {
                UserId = dto.UserId,
                RoomId = dto.RoomId,
                Comment = dto.Comment
            };
            await _uow.Reviews.AddAsync(review);
            await _uow.CompleteAsync();
            return true;
        }
        // --- Reviews ---
        public async Task<List<ReviewDto>> GetRoomReviewsAsync(int roomId)
        {
            var reviews = await _uow.Reviews.GetReviewsByRoomIdAsync(roomId);

            return reviews.Select(r => new ReviewDto
            {
               
                UserId = r.UserId,
                Comment = r.Comment,
             
                CreatedAt = r.CreatedAt,
              
                UserName = r.User != null ? r.User.UserName : "Anonymous" 
            }).ToList();
        }

    }
}
