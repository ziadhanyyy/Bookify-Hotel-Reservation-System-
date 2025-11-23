using Bookify.Core.Entities;
using Bookify.Data.UnitOfWork;
using Bookify.Services.Interfaces;
using Stripe.Checkout;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Bookify.Services.Implementations
{
    public class PaymentService : IPaymentService
    {
        private readonly IUnitOfWork _unitOfWork;

        public PaymentService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<string> CreateCheckoutSessionAsync(int bookingId, decimal amount)
        {
            var options = new SessionCreateOptions
            {
                PaymentMethodTypes = new List<string> { "card" },
                LineItems = new List<SessionLineItemOptions>
                {
                    new SessionLineItemOptions
                    {
                        PriceData = new SessionLineItemPriceDataOptions
                        {
                            UnitAmount = (long)(amount * 100), // Stripe expects amount in cents
                            Currency = "usd",
                            ProductData = new SessionLineItemPriceDataProductDataOptions
                            {
                                Name = "Hotel Room Booking"
                            }
                        },
                        Quantity = 1
                    }
                },
                Mode = "payment",
                SuccessUrl = $"https://localhost:7220/Payment/Success?bookingId={bookingId}",
                CancelUrl = $"https://localhost:7220/Payment/Failed"
            };

            var service = new SessionService();
            var session = await service.CreateAsync(options);

            return session.Url;
        }

        public async Task<Payment> RecordPaymentAsync(int bookingId, string status, decimal amount, string transactionId)
        {
            var payment = new Payment
            {
                BookingId = bookingId,
                Status = status,
                Amount = amount,
                TransactionId = transactionId,
                PaidAt = DateTime.Now
            };

            await _unitOfWork.Payments.AddAsync(payment);
             _unitOfWork.Complete(); 

            return payment;
        }
    }
}
