using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bookify.Core.Entities
{
    public class Review
    {
        [Key]
        public int ReviewId { get; set; }
        public string UserId { get; set; }
        public int RoomId { get; set; }

        public int Rating { get; set; }
        public string Comment { get; set; }
        public string Sentiment { get; set; }    // "Positive", "Negative", "Neutral"
        public DateTime CreatedAt { get; set; }

        public User User { get; set; }
        public Room Room { get; set; }
    }
}
