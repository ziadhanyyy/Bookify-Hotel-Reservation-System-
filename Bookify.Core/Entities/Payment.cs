using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bookify.Core.Entities
{
    public class Payment
    {
        [Key]
        public int Id { get; set; }
        public string PaymentStatus { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal PaymentAmount { get; set; }
        public DateTime PaymentDate { get; set; }
        // nav
        public Booking booking { get; set; }
        public int BookingId { get; set; }
    }
}
