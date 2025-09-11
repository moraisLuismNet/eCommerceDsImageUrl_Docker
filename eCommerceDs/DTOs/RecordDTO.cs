using System.ComponentModel.DataAnnotations;

namespace eCommerceDs.DTOs
{
    /// <summary>
    /// DTO that represents a record
    /// </summary>
    public class RecordDTO
    {
        /// <summary>
        /// Record ID
        /// </summary>
        public int IdRecord { get; set; }

        /// <summary>
        /// Record title
        /// </summary>
        [Required(ErrorMessage = "The record title is required")]
        [StringLength(100, ErrorMessage = "The record title cannot have more than 100 characters")]
        public string TitleRecord { get; set; } = null!;

        /// <summary>
        /// Record year of publication
        /// </summary>
        [Range(1900, 2100, ErrorMessage = "The year of publication must be between 1900 and 2100")]
        public int YearOfPublication { get; set; }

        /// <summary>
        /// Record image URL
        /// </summary>
        [Url(ErrorMessage = "The image URL is invalid")]
        [RegularExpression(@"^https?://(i\.)?imgur\.com/.*\.(jpg|jpeg|png|gif)$", 
            ErrorMessage = "The URL must be from Imgur (e.g.: https://i.imgur.com/example.jpg)")]
        public string? ImageRecord { get; set; }

        /// <summary>
        /// Record price
        /// </summary>
        [Range(0.01, double.MaxValue, ErrorMessage = "The price must be greater than zero")]
        public decimal Price { get; set; }

        /// <summary>
        /// Record stock
        /// </summary>
        [Range(0, int.MaxValue, ErrorMessage = "The stock cannot be negative")]
        public int Stock { get; set; }

        /// <summary>
        /// Record discontinued
        /// </summary>
        public bool Discontinued { get; set; }

        /// <summary>
        /// Group ID
        /// </summary>
        [Range(1, int.MaxValue, ErrorMessage = "The group ID is required")]
        public int GroupId { get; set; }

        /// <summary>
        /// Group name
        /// </summary>
        public string NameGroup { get; set; } = null!;
    }
}
