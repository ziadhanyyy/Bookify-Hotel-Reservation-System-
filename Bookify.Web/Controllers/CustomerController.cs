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
        public async Task<IActionResult> AvailableRooms()
        {
            var rooms = await _customerService.GetAvailableRoomsAsync();
            return View(rooms);
        }

        [HttpGet]
        public async Task<IActionResult> SearchRooms(string query)
        {
            var rooms = await _customerService.SearchRoomsAsync(query);
            return View("AvailableRooms", rooms);
        }

        [HttpGet]
        public async Task<IActionResult> RoomDetails(int roomId)
        {
            var room = await _customerService.GetRoomDetailsAsync(roomId);
            if (room == null) return NotFound();
            return View(room);
        }

        // --- Wishlist ---
        [HttpPost]
        [HttpPost]
        public async Task<IActionResult> AddToWishlist(int roomId)
        {
            var userId = _userManager.GetUserId(User);
            if (userId == null) return Unauthorized();

            Console.WriteLine($"DEBUG: UserId={userId}, RoomId={roomId}"); // تحقق من القيمة

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
            // Assign logged-in user
            if (dto.UserId == null) return Unauthorized();

            await _customerService.ConfirmBookingAsync(dto);

            // Redirect to PaymentController Checkout action
            return RedirectToAction("Checkout", "Payment", new { bookingId = dto.Id });
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
        public async Task<IActionResult> AddReview(ReviewDto dto)
        {
            dto.UserId =_userManager.GetUserId(User);

            if (dto.UserId == null) return Unauthorized();

            await _customerService.AddReviewAsync(dto);
            return RedirectToAction("BookingHistory");
        }

       
    }
}
