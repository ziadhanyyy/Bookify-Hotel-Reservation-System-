using Bookify.Core.DTOs;
using Bookify.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Bookify.Web.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly IAdminService _adminService;
        public AdminController(IAdminService adminService)
        {
            _adminService = adminService;
        }
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }
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
        public async Task<IActionResult> Rooms()
        {
            var rooms = await _adminService.GetRoomsAsync();
            return View(rooms);
        }
    }
}
