namespace eCommerceDs.DTOs
{
    public class CartDetailDTO
    {
        public int IdCartDetail { get; set; }
        public int CartId { get; set; }
        public string UserEmail { get; set; } = null!;
        public string? GroupName { get; set; }
        public int RecordId { get; set; }
        public int Amount { get; set; } 
        public decimal Price { get; set; } 
        public decimal Total => Amount * Price;
        public RecordSumaryDTO Record { get; set; }
    }

    public class RecordSumaryDTO
    {
        public int IdRecord { get; set; }
        public string? ImageRecord { get; set; }
        public string TitleRecord { get; set; }
        public decimal Price { get; set; }
        public string? GroupName { get; set; }
    }
}
