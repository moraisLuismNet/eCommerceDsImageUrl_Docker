using eCommerceDs.DTOs;

namespace eCommerceDs.Services
{
    public interface IOrderService
    {
        Task<IEnumerable<OrderDTO>> GetOrdersOrderService();
        Task<IEnumerable<OrderDTO>> GetOrdersByUserEmailOrderService(string userEmail);
        Task<OrderDTO> CreateOrderFromCartOrderService(string userEmail, string paymentMethod);
        Task<bool> ValidateUserAndCartOrderService(string userEmail);
    }
}