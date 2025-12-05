using Bookify.Core.DTOs;
using Bookify.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.IO;

namespace Bookify.Web.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly IAdminService _adminService;
        private readonly ICustomerService _customerService;
        public AdminController(IAdminService adminService, ICustomerService customerService)
        {
            _adminService = adminService;
            _customerService = customerService;
        }
        [HttpGet]
        public  async Task<IActionResult> Index()
        {
            var stats = await _adminService.GetDashboardStatsAsync();
            return View(stats);
        }

        // ------------------- Assign Role -------------------
        [HttpGet]
        public IActionResult AssignRole()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AssignRole(AssignRoleDto assignRoleDto)
        {
            if (!ModelState.IsValid)
                return View(assignRoleDto);

            try
            {
                var result = await _adminService.AssignRoleAsync(assignRoleDto);

                if (result)
                {
                    TempData["Success"] = "Role assigned successfully!";
                    return RedirectToAction("Index");
                }
                else
                {
                    TempData["Error"] = "Failed to assign role. User not found or already has this role.";
                    return View(assignRoleDto);
                }
            }
            catch (System.Exception ex)
            {
                TempData["Error"] = ex.Message;
                return View(assignRoleDto);
            }
        }
        // ------------------- Room Types -------------------
        [HttpGet]
        public IActionResult AddRoomType() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddRoomType(RoomTypeDto dto)
        {
            if (!ModelState.IsValid)
                return View(dto);

            var result = await _adminService.AddRoomTypeAsync(dto);
            if (result)
            {
                TempData["Success"] = "Room type added successfully!";
                return RedirectToAction("RoomTypes");
            }

            TempData["Error"] = "Failed to add room type.";
            return View(dto);
        }

        [HttpGet]
        public async Task<IActionResult> RoomTypes()
        {
            var roomTypes = await _adminService.GetRoomTypesAsync();
            return View(roomTypes);
        }
        [HttpGet]
        public async Task<IActionResult> EditRoom(int id)
        {
            var rooms = await _adminService.GetRoomsAsync();
            var room = rooms.FirstOrDefault(r => r.Id == id);

            if (room == null) return NotFound();

            ViewBag.RoomTypes = await _adminService.GetRoomTypesAsync();
            return View(room);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditRoom(RoomDto dto, IFormFile? roomImage)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.RoomTypes = await _adminService.GetRoomTypesAsync();
                return View(dto);
            }

            var result = await _adminService.UpdateRoomAsync(dto, roomImage);
            if (result)
            {
                TempData["Success"] = "Room updated successfully!";
                return RedirectToAction("Rooms");
            }

            TempData["Error"] = "Failed to update room.";
            return View(dto);
        }


        // ------------------- Rooms -------------------
        [HttpGet]
        public async Task<IActionResult> AddRoom()
        {
            // To load room types for the dropdown

            ViewBag.RoomTypes = await _adminService.GetRoomTypesAsync();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddRoom(RoomDto dto, IFormFile roomImage)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.RoomTypes = await _adminService.GetRoomTypesAsync();
                return View(dto);
            }

            try
            {
                var result = await _adminService.AddRoomAsync(dto, roomImage);
                if (result)
                {
                    TempData["Success"] = "Room added successfully!";
                    return RedirectToAction("Rooms");
                }
                else
                {
                    TempData["Error"] = "Failed to add room.";
                }
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Failed to add room: {ex.Message}";
            }

            ViewBag.RoomTypes = await _adminService.GetRoomTypesAsync();
            return View(dto);
        }


        [HttpGet]
        public async Task<IActionResult> Rooms()
        {
            var rooms = await _adminService.GetRoomsAsync();
            return View(rooms);
        }

        [HttpGet]
        public async Task<IActionResult> RoomDetails(int id)
        {
            var room = await _customerService.GetRoomDetailsAsync(id);
            if (room == null) return NotFound();
            return View(room);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteRoom(int id)
        {
            var result = await _adminService.DeleteRoomAsync(id);
            if (result)
            {
                TempData["Success"] = "Room deleted successfully!";
            }
            else
            {
                TempData["Error"] = "Room not found or could not be deleted.";
            }
            return RedirectToAction("Rooms");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteRoomType(int id)
        {
            var result = await _adminService.DeleteRoomTypeAsync(id);
            if (result)
            {
                TempData["Success"] = "Room type deleted successfully!";
            }
            else
            {
                TempData["Error"] = "Room type not found or it has rooms assigned to it.";
            }
            return RedirectToAction("RoomTypes");
        }
        [HttpGet]
        public async Task<IActionResult> EditRoomType(int id)
        {
            var roomType = await _adminService.GetRoomTypesAsync();
            var dto = roomType.FirstOrDefault(rt => rt.Id == id);
            if (dto == null) return NotFound();

            return View(dto);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditRoomType(RoomTypeDto dto)
        {
            if (!ModelState.IsValid) return View(dto);

            var result = await _adminService.UpdateRoomTypeAsync(dto);
            if (result)
            {
                TempData["Success"] = "Room type updated successfully!";
                return RedirectToAction("RoomTypes");
            }

            TempData["Error"] = "Failed to update room type.";
            return View(dto);
        }



        // ------------------- Bookings -------------------
        [HttpGet]
        public async Task<IActionResult> Bookings()
        {
            var bookings = await _adminService.GetAllBookingsAsync();
            return View(bookings);
        }
    }
}
