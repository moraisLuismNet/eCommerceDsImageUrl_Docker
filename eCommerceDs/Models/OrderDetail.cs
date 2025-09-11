namespace eCommerceDs.Models
{
    public class OrderDetail
    {
        public int IdOrderDetail { get; set; }

        // Relationship with Order
        public int OrderId { get; set; }
        public Order Order { get; set; } = null!;

        // Relationship with Record
        public int RecordId { get; set; }
        public Record Record { get; set; } = null!;
        public int Amount { get; set; } 
        public decimal Price { get; set; } 
        public decimal Total => Amount * Price; 
    }
}
