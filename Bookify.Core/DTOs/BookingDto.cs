using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bookify.Core.DTOs
{
    public class BookingDto
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public string UserEmail { get; set; }
        public int RoomId { get; set; }
        public string RoomNumber { get; set; }
        public DateTime BookingDate { get; set; }
        [Required(ErrorMessage = "Check-in date is required")]
        public DateTime CheckInDate { get; set; }
        [Required(ErrorMessage = "Check-out date is required")]
        public DateTime CheckOutDate { get; set; }
        public int Nights { get; set; }
        public decimal TotalAmount { get; set; }
        public string Status { get; set; }
    }

}
