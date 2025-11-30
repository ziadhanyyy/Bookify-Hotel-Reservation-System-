using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bookify.Core.DTOs
{
    public class RoomDto
    {
        [Required]
        public string RoomNumber { get; set; }
        public string? Name { get; set; }       
        public string? Type { get; set; }       
        public decimal Price { get; set; }     
        public int RoomTypeId { get; set; }
        public int Id { get; set; }            
        public string? Description { get; set; }
        public string? ImageURL { get; set; }
        public List<ReviewDto> Reviews { get; set; } = new();
    }
}
