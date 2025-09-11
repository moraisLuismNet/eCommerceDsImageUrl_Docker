namespace eCommerceDs.Models
{
    public class User
    {
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
        public string Role { get; set; } = "User"; 
        public byte[]? Salt { get; set; }

        // One-to-one relationship with Cart
        public int CartId { get; set; }
        public Cart Cart { get; set; } = null!;

        // One-to-many relationship with Orders
        public ICollection<Order> Orders { get; set; } = new List<Order>();
    }
}
