using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bookify.Core.DTOs
{
    public class DashboardStatsDto
    {
        public int TotalRooms { get; set; }
        public int TotalBookings { get; set; }
        public int RoomTypes { get; set; }
        
    }
}
