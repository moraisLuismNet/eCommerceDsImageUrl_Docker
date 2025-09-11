using eCommerceDs.DTOs;
using eCommerceDs.Models;
using eCommerceDs.Repository;

namespace eCommerceDs.Services
{
    public class CartDetailService : ICartDetailService
    {
        private readonly ICartDetailRepository _cartDetailRepository;
        private readonly IRecordRepository<Record> _recordRepository;
        private readonly eCommerceDsContext _context;

        public CartDetailService(
            ICartDetailRepository cartDetailRepository,
            IRecordRepository<Record> recordRepository,
            eCommerceDsContext context)
        {
            _cartDetailRepository = cartDetailRepository;
            _recordRepository = recordRepository;
            _context = context;
        }


        public async Task<IEnumerable<CartDetailDTO>> GetCartDetailsByCartIdCartDetailService(int cartId)
        {
            var cartDetails = await _cartDetailRepository.GetCartDetailsByCartIdCartDetailRepository(cartId);
            if (cartDetails == null || !cartDetails.Any())
            {
                // Return empty collection 
                return new List<CartDetailDTO>();
            }

            return (IEnumerable<CartDetailDTO>)cartDetails;
        }


        public async Task<CartDetail> GetCartDetailByCartIdAndRecordIdCartDetailService(int cartId, int recordId)
        {
            return await _cartDetailRepository.GetCartDetailByCartIdAndRecordIdCartDetailRepository(cartId, recordId);
        }


        public async Task AddCartDetailCartDetailService(CartDetail cartDetail)
        {
            var record = await _recordRepository.GetByIdAsyncRecordRepository(cartDetail.RecordId);
            if (record == null)
            {
                throw new InvalidOperationException("Record not found");
            }

            if (record.Stock < cartDetail.Amount)
            {
                throw new InvalidOperationException($"Not enough stock for record {record.IdRecord}. Available: {record.Stock}, Requested: {cartDetail.Amount}");
            }

            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                _context.CartDetails.Add(cartDetail);
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }


        public async Task UpdateCartDetailCartDetailService(CartDetail cartDetail)
        {
            await _cartDetailRepository.UpdateCartDetailCartDetailRepository(cartDetail);
        }


        public async Task RemoveFromCartDetailCartDetailService(CartDetail cartDetail)
        {
            _cartDetailRepository.RemoveFromCartDetailCartDetailRepository(cartDetail);
            await _cartDetailRepository.SaveCartDetailRepository();
        }

    }
}
