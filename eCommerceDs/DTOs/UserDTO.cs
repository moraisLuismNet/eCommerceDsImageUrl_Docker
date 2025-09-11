namespace eCommerceDs.DTOs
{
    public class UserDTO
    {
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
        public string Role { get; set; } = null!;
        public int CartId { get; set; }
        public byte[]? Salt { get; set; }
    }
}
