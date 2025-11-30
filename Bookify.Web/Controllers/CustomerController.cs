using Bookify.Core.DTOs;
using Bookify.Core.Entities;
using Bookify.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Bookify.Web.Controllers
{
    [Authorize(Roles = "User")]
    public class CustomerController : Controller
    {
        private readonly UserManager<User> _userManager;

        private readonly ICustomerService _customerService;

        public CustomerController(ICustomerService customerService, UserManager<User> userManager)
        {
            _customerService = customerService;
            _userManager = userManager;
        }

        // --- Rooms ---
        [HttpGet]
        public async Task<IActionResult> AvailableRooms(string query)
        {
            IEnumerable<RoomDto> rooms;

            if (string.IsNullOrWhiteSpace(query))
            {
                rooms = await _customerService.GetAvailableRoomsAsync();
            }
            else
            {
                rooms = await _customerService.SearchRoomsAsync(query);
            }

            return View(rooms);
        }

        
        [HttpGet]
        public async Task<IActionResult> RoomDetails(int id)
        {
            var room = await _customerService.GetRoomDetailsAsync(id);
            if (room == null) return NotFound();

          
            room.Reviews = await _customerService.GetRoomReviewsAsync(id);

            return View(room);
        }


        // --- Wishlist ---
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddToWishlist(int roomId)
        {
            var userId = _userManager.GetUserId(User);
            if (userId == null) return Unauthorized();

            // Debug log:
            Console.WriteLine($"DEBUG: AddToWishlist userId={userId}, roomId={roomId}");

            await _customerService.AddToWishlistAsync(userId, roomId);
            return RedirectToAction("Wishlist");
        }


        [HttpGet]
        public async Task<IActionResult> Wishlist()
        {
            var userId = _userManager.GetUserId(User);

            if (userId == null) return Unauthorized();

            var wishlist = await _customerService.GetWishlistAsync(userId);
            return View(wishlist);
        }

        // --- Booking ---
        [HttpPost]
        public async Task<IActionResult> ConfirmBooking(BookingDto dto)
        {
            dto.UserId = _userManager.GetUserId(User);
            if (dto.UserId == null) return Unauthorized();

            
            dto.Nights = (dto.CheckOutDate - dto.CheckInDate).Days;

            
            var room = await _customerService.GetRoomDetailsAsync(dto.RoomId);

            if (room == null)
                return NotFound("Room not found.");

            
            dto.TotalAmount = dto.Nights * room.Price;

            int bookingId = await _customerService.ConfirmBookingAsync(dto);

            return RedirectToAction("Checkout", "Payment", new { bookingId });
        }

        [HttpGet]
        public async Task<IActionResult> StartBooking(int roomId)
        {
            var room = await _customerService.GetRoomDetailsAsync(roomId);
            if (room == null) return NotFound();

            var booking = new BookingDto
            {
                RoomId = room.Id,
                RoomNumber = room.RoomNumber,
                TotalAmount = room.Price,
                BookingDate = DateTime.Now,
                CheckInDate = DateTime.Now.Date,
                CheckOutDate = DateTime.Now.Date.AddDays(1),
                Nights = 1
            };

            return View("StartBooking", booking);
        }


        [HttpGet]
        public async Task<IActionResult> BookingHistory()
        {
            var userId = _userManager.GetUserId(User);
            if (userId == null) return Unauthorized();

            var bookings = await _customerService.GetBookingHistoryAsync(userId);
            return View(bookings);
        }

        // --- Reviews ---
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddReview(ReviewDto dto)
        {
            dto.UserId = _userManager.GetUserId(User);
            dto.CreatedAt = DateTime.Now;

            if (dto.UserId == null) return Unauthorized();

            // Debug
            Console.WriteLine($"RoomId={dto.RoomId}, BookingId={dto.BookingId}, Comment={dto.Comment}");

            await _customerService.AddReviewAsync(dto);
            return RedirectToAction("BookingHistory");
        }





    }
}
