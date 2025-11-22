using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bookify.Core.Entities
{
    public class Wishlist
    {
        [Key]
        public int WishsistId {  get; set; }
        public string UserId { get; set; }
        public int RoomId { get; set; }
        public User user { get; set; }
        public Room room { get; set; }

    }
}
