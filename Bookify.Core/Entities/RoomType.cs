using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bookify.Core.Entities
{
    public class RoomType
    {
        [Key]
        public int RoomTypeId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Price per night must be greater than 0.")]
        public decimal PricePerNight { get; set; }
        public int Capacity { get; set; }

        public ICollection<Room> Rooms { get; set; }
    }
}
