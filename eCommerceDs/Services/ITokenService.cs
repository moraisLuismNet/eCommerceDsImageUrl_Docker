using eCommerceDs.DTOs;

namespace eCommerceDs.Services
{
    public interface ITokenService
    {
        LoginResponseDTO GenerateTokenService(UserLoginDTO user);
    }
}
