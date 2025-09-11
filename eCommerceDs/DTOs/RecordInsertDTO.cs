using System.ComponentModel.DataAnnotations;

namespace eCommerceDs.DTOs
{
    public class RecordInsertDTO
    {
        [Required(ErrorMessage = "The record title is required")]
        public string TitleRecord { get; set; } = null!;
        
        [Range(1900, 2100, ErrorMessage = "The year of publication must be between 1900 and 2100")]
        public int YearOfPublication { get; set; }

        [Required(ErrorMessage = "The image URL is required")]
        [Url(ErrorMessage = "The image URL is invalid")]
        [RegularExpression(@"^https?://(i\.)?imgur\.com/.*\.(jpg|jpeg|png|gif)$", 
            ErrorMessage = "The URL must be from Imgur (e.g.: https://i.imgur.com/example.jpg)")]
        public string ImageUrl { get; set; } = null!;

        [Range(0.01, double.MaxValue, ErrorMessage = "The price must be greater than zero")]
        public decimal Price { get; set; }
        
        [Range(0, int.MaxValue, ErrorMessage = "The stock cannot be negative")]
        public int Stock { get; set; }
        
        public bool Discontinued { get; set; }
        
        [Range(1, int.MaxValue, ErrorMessage = "The group ID is required")]
        public int GroupId { get; set; }
    }
}
