using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bookify.Core.Entities
{
    public class Booking
    {
        [Key]
        public int  Id {  get; set; }
        public DateTime BookingDate {  get; set; }
        public DateTime CheckInDate { get; set; }
        public DateTime CheckOutDate { get; set; }
        public int Nights { get; set; }

        // Payment Status
        [Required]
        [StringLength(50)]
        public string Status { get; set; } = "Pending";

        // Total price for booking
        [Required]
        public decimal TotalAmount { get; set; }
        //navigation property
        public string UserId { get; set; }
        public User User { get; set; }
        public int RoomId { get; set; } 
        
        public Payment Payment { get; set; }
        public Room Room { get; set; }

    }
}
