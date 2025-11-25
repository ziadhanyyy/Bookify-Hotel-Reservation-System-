using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bookify.Core.DTOs
{
    public class ReviewDto
    {
        public int ReviewId { get; set; }         
        public string UserId { get; set; }         
        public int RoomId { get; set; }           
        public int Rating { get; set; }            
        public string Comment { get; set; }        
        public string Sentiment { get; set; }   // "Positive", "Negative", "Neutral"    
        public DateTime CreatedAt { get; set; }    
    }
}
