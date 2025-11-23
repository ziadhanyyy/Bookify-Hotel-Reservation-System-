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
                // هنا ممكن تعمل Logging
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

                await _paymentService.RecordPaymentAsync(
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
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // Failed action
        public IActionResult Failed()
        {
            ViewBag.Message = "Payment Failed ❌ Please try again.";
            return View("PaymentResult");
        }
    }
}
