using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace Bookify.Core.Entities { 
    [Index(nameof(TransactionId), IsUnique = true)] 
    public class Payment { 
        [Key] 
        public int Id { get; set; } 
        [Required] public int BookingId { get; set; } 
        [Required]
        [StringLength(200)] 
        public string TransactionId { get; set; } 
        [Required]
        [Range(1, double.MaxValue, ErrorMessage = "Amount must be greater than 0.")] 
        public decimal Amount { get; set; }
        [Required] 
        public DateTime PaidAt { get; set; } = DateTime.Now;
        [Required]
        [StringLength(50)] 
        public string Status { get; set; }
        [ForeignKey(nameof(BookingId))] 
        public Booking Booking { get; set; } 
    } 
}