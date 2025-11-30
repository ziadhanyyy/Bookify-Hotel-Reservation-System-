using System;
using System.ComponentModel.DataAnnotations;

namespace Bookify.Core.Validation
{
    public class ValidateBookingDatesAttribute : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            var booking = (Bookify.Core.DTOs.BookingDto)validationContext.ObjectInstance;

            // Check-in must be today or after today
            if (booking.CheckInDate.Date < DateTime.Today)
                return new ValidationResult("Check-in date cannot be before today.");

            // Check-out must be AFTER check-in (at least next day)
            if (booking.CheckOutDate.Date <= booking.CheckInDate.Date)
                return new ValidationResult("Check-out date must be at least one day after check-in.");

            return ValidationResult.Success;
        }
    }
}
