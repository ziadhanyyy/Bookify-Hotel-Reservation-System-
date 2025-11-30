public class ReviewDto
{
    public int BookingId { get; set; }      // جديد
    public int RoomId { get; set; }
    public string Comment { get; set; }     // بدل Content
    public string UserId { get; set; } public string UserName { get; set; }
   
    public DateTime CreatedAt { get; set; } // optional
}
