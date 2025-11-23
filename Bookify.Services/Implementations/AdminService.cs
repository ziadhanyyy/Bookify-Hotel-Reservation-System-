using Bookify.Core.DTOs;
using Bookify.Core.Entities;
using Bookify.Data.UnitOfWork;
using Bookify.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
             _uow.Complete();
            return true;
        }

        public async Task<bool> AddRoomAsync(RoomDto dto)
        {
            var room = new Room
            {
                RoomNumber = dto.RoomNumber,
                RoomTypeId = dto.RoomTypeId,
                IsAvailable = true
            };
            await _uow.Rooms.AddAsync(room);
             _uow.Complete();
            return true;
        }
        public async Task<IEnumerable<RoomTypeDto>> GetRoomTypesAsync()
        {
            var roomTypes = await _uow.RoomTypes.GetAllAsync();
            return roomTypes.Select(rt => new RoomTypeDto
            {
               
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
                RoomNumber = r.RoomNumber,
                RoomTypeId = r.RoomTypeId,
                
            });
        }
    }
}
