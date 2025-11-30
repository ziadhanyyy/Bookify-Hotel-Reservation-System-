using Bookify.Core.Entities;
using Bookify.Data.UnitOfWork;
using Bookify.Services.Interfaces;
using Microsoft.Extensions.Options;
using Stripe;
using Stripe.Checkout;
using Stripe.Forwarding;

public class PaymentService : IPaymentService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly StripeSettings _stripeSettings;

    public PaymentService(IUnitOfWork unitOfWork, IOptions<StripeSettings> stripeSettings)
    {
        _unitOfWork = unitOfWork;
        _stripeSettings = stripeSettings.Value;


        StripeConfiguration.ApiKey = _stripeSettings.SecretKey;
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
                        UnitAmount = (long)(amount * 100),
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
            SuccessUrl = $"https://localhost:7167/Payment/Success?bookingId={bookingId}",
            CancelUrl = $"https://localhost:7167/Payment/Failed"
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
