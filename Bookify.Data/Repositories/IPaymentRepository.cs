using Bookify.Core.Entities;
using System.Threading.Tasks;

namespace Bookify.Data.Repositories
{
    public interface IPaymentRepository : IRepository<Payment>
    {
        Task<Payment> GetByTransactionIdAsync(string transactionId);
    }
}
