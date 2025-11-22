using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bookify.Core.Entities
{
    public class Room
    {
     [Key]
    public int RoomId { get; set; }
    public string RoomNumber { get; set; }
    public int RoomTypeId { get; set; }
    public bool IsAvailable { get; set; }
    public string ImageURL { get; set; }

    public RoomType roomType { get; set; }
   
}
}
