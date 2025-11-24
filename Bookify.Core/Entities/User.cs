using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bookify.Core.Entities
{
    public class User: IdentityUser
    {
        public string? Country { get; set; }
        public string ?Name {  get; set; }
       //nav property
        public ICollection<Booking> Bookings { get; set; }= new List<Booking>();
        public ICollection<Wishlist> Wishlist { get; set; } = new List<Wishlist>();

        public ICollection<Review> Reviews { get; set; } = new List<Review>();

    }
}
