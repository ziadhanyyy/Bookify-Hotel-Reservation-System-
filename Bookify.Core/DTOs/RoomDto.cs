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
        public int RoomTypeId { get; set; }
        
    }
}
