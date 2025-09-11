using AutoMapper;
using eCommerceDs.DTOs;
using eCommerceDs.Models;
using Microsoft.EntityFrameworkCore;

namespace eCommerceDs.Repository
{
    public class CartDetailRepository : ICartDetailRepository
    {
        private readonly eCommerceDsContext _context;
        private readonly IMapper _mapper;

        public CartDetailRepository(eCommerceDsContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IEnumerable<CartDetailDTO>> GetCartDetailsByCartIdCartDetailRepository(int cartId)
        {
            var cartDetails = await _context.CartDetails
                .Where(cd => cd.CartId == cartId)
                .Include(cd => cd.Record)
                .ThenInclude(r => r.Group)
                .Include(cd => cd.Cart)
                .ToListAsync();

            if (cartDetails == null || !cartDetails.Any())
            {
                // Return empty list 
                return new List<CartDetailDTO>();
            }

            var cartDetailDTOs = cartDetails.Select(cd => new CartDetailDTO
            {
                IdCartDetail = cd.IdCartDetail,
                CartId = cd.CartId,
                RecordId = cd.RecordId,
                Amount = cd.Amount,
                Price = cd.Price,
                UserEmail = cd.Cart != null ? cd.Cart.UserEmail : null,
                GroupName = cd.Record?.Group?.NameGroup,
                Record = cd.Record != null ? new RecordSumaryDTO
                {
                    IdRecord = cd.Record.IdRecord,
                    ImageRecord = cd.Record.ImageRecord,
                    TitleRecord = cd.Record.TitleRecord,
                    Price = cd.Record.Price,
                    GroupName = cd.Record.Group?.NameGroup
                } : null
            }).ToList();

            return cartDetailDTOs;
        }


        public async Task<IEnumerable<CartDetail>> GetCartDetailByCartIdCartDetailRepository(int cartId)
        {
            return await _context.CartDetails
                .Include(cd => cd.Record)
                .Where(cd => cd.CartId == cartId)
                .ToListAsync();
        }


        public async Task<List<CartDetailInfoDTO>> GetCartDetailsInfoCartDetailRepository(int cartId)
        {
            return await _context.CartDetails
                .Where(cd => cd.CartId == cartId)
                .Select(cd => new CartDetailInfoDTO
                {
                    RecordId = cd.RecordId,
                    Amount = cd.Amount
                })
                .ToListAsync();
        }


        public async Task<CartDetail> GetCartDetailByCartIdAndRecordIdCartDetailRepository(int cartId, int recordId)
        {
            return await _context.CartDetails
                .FirstOrDefaultAsync(cd => cd.CartId == cartId && cd.RecordId == recordId);
        }


        public async Task UpdateCartDetailCartDetailRepository(CartDetail cartDetail)
        {
            var existingCartDetail = await _context.CartDetails
                .FirstOrDefaultAsync(cd => cd.IdCartDetail == cartDetail.IdCartDetail);

            if (existingCartDetail != null)
            {
                _context.Entry(existingCartDetail).CurrentValues.SetValues(cartDetail);
                await _context.SaveChangesAsync();
            }
            else
            {
                throw new InvalidOperationException("CartDetail not found");
            }
        }


        public async Task RemoveFromCartDetailCartDetailRepository(CartDetail cartDetail)
        {
            _context.CartDetails.Remove(cartDetail);
        }


        public async Task SaveCartDetailRepository()
        {
            await _context.SaveChangesAsync();
        }


        public async Task RemoveAllCartDetailsCartDetailRepository(int cartId)
        {
            var details = await _context.CartDetails
                .Where(cd => cd.CartId == cartId)
                .ToListAsync();

            _context.CartDetails.RemoveRange(details);
            await _context.SaveChangesAsync();
        }


        public async Task RemoveAllDetailsFromCartCartDetailRepository(int cartId)
        {
            var details = await _context.CartDetails
                .Where(cd => cd.CartId == cartId)
                .ToListAsync();

            _context.CartDetails.RemoveRange(details);
            await _context.SaveChangesAsync();
        }
    }
}
