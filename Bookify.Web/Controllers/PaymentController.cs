using Bookify.Data.UnitOfWork;
using Bookify.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Bookify.Web.Controllers
{
    public class PaymentController : Controller
    {
        private readonly IPaymentService _paymentService;
        private readonly IUnitOfWork _unitOfWork;

        public PaymentController(IPaymentService paymentService, IUnitOfWork unitOfWork)
        {
            _paymentService = paymentService;
            _unitOfWork = unitOfWork;
        }

        // Checkout action
        public async Task<IActionResult> Checkout(int bookingId)
        {
            try
            {
                var booking = await _unitOfWork.Bookings.GetByIdAsync(bookingId);
                if (booking == null)
                    return NotFound("Booking not found.");

                if (booking.TotalAmount <= 0)
                    return BadRequest("Invalid booking amount.");

                var url = await _paymentService.CreateCheckoutSessionAsync(bookingId, booking.TotalAmount);
                return Redirect(url);
            }
            catch (Exception ex)
            {
                
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // Success action
        public async Task<IActionResult> Success(int bookingId)
        {
            try
            {
                var booking = await _unitOfWork.Bookings.GetByIdAsync(bookingId);
                if (booking == null)
                    return NotFound("Booking not found.");

                string transactionId = Guid.NewGuid().ToString();

                var payment = await _paymentService.RecordPaymentAsync(
                    bookingId,
                    "Success",
                    booking.TotalAmount,
                    transactionId
                );

                booking.Status = "Confirmed";
                _unitOfWork.Complete();

                ViewBag.Message = "Payment Successful! 🎉";
                ViewBag.BookingId = bookingId;
                ViewBag.Amount = booking.TotalAmount;
                ViewBag.TransactionId = transactionId;

                return View("PaymentResult");
            }
            catch (Exception ex)
            {
                return Content("❌ ERROR IN SUCCESS ACTION: " + ex.Message);
            }
        }


        // Failed action
        public async Task<IActionResult> Failed(int? bookingId)
        {
            ViewBag.Message = "Payment Failed ❌ Please try again.";
            
            // If bookingId is provided, try to get booking details
            if (bookingId.HasValue)
            {
                try
                {
                    var booking = await _unitOfWork.Bookings.GetByIdAsync(bookingId.Value);
                    if (booking != null)
                    {
                        ViewBag.BookingId = bookingId.Value;
                        ViewBag.Amount = booking.TotalAmount;
                        
                        // Optionally record failed payment
                        try
                        {
                            string transactionId = Guid.NewGuid().ToString();
                            await _paymentService.RecordPaymentAsync(
                                bookingId.Value,
                                "Failed",
                                booking.TotalAmount,
                                transactionId
                            );
                            ViewBag.TransactionId = transactionId;
                        }
                        catch
                        {
                            // Log error but don't fail the page
                        }
                    }
                }
                catch
                {
                    // If booking not found, just show generic failure message
                }
            }
            
            return View("PaymentResult");
        }
    }
}
