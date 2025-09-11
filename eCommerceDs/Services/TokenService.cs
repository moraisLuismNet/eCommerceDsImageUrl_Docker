using eCommerceDs.DTOs;
using eCommerceDs.Models;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace eCommerceDs.Services
{
    public class TokenService : ITokenService
    {
        private readonly IConfiguration _configuration;
        private readonly eCommerceDsContext _context;

        public TokenService(IConfiguration configuration, eCommerceDsContext context)
        {
            _configuration = configuration;
            _context = context;
        }


        public LoginResponseDTO GenerateTokenService(UserLoginDTO user)
        {
            var userDB = _context.Users.FirstOrDefault(x => x.Email == user.Email);
            if (userDB == null)
            {
                throw new UnauthorizedAccessException("User not found");
            }

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, userDB.Role)
            };

            var key = _configuration["JWTKey"];
            var keyKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
            var signinCredentials = new SigningCredentials(keyKey, SecurityAlgorithms.HmacSha256);

            var securityToken = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddDays(30),
                signingCredentials: signinCredentials
            );

            var tokenString = new JwtSecurityTokenHandler().WriteToken(securityToken);

            return new LoginResponseDTO
            {
                Token = tokenString,
                Email = user.Email
            };
        }
    }
}
