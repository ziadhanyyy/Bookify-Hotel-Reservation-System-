using Bookify.Core.Entities;
using Bookify.Data.Context;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Bookify.Data.Repositories
{
    public class PaymentRepository : Repository<Payment>, IPaymentRepository
    {
        public PaymentRepository(BookifyDbContext context) : base(context)
        {
        }

        // Methods specific to Payment (besides generic methods)
        public async Task<Payment> GetByTransactionIdAsync(string transactionId)
        {
            return await _context.Set<Payment>()
                .FirstOrDefaultAsync(p => p.TransactionId == transactionId);
        }
    }
}
