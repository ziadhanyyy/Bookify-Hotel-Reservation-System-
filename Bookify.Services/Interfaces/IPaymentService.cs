using Bookify.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace Bookify.Services.Interfaces { 
    public interface IPaymentService { 
        Task<string> CreateCheckoutSessionAsync(int bookingId, decimal amount);
        Task<Payment> RecordPaymentAsync(int bookingId, string status, decimal amount, string transactionId);
    }
}