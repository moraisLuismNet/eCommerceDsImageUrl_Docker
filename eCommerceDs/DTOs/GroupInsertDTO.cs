using System.ComponentModel.DataAnnotations;

namespace eCommerceDs.DTOs
{
    /// <summary>
    /// DTO for creating a new group
    /// </summary>
    public class GroupInsertDTO
    {
        /// <summary>
        /// Group name
        /// </summary>
        [Required(ErrorMessage = "The group name is required")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "The group name must be between 2 and 100 characters")]
        public string NameGroup { get; set; } = null!;

        /// <summary>
        /// Group image URL (optional)
        /// </summary>
        [RegularExpression(@"^https?://(i\.)?imgur\.com/.*\.(jpg|jpeg|png|gif)$", 
            ErrorMessage = "The URL must be from Imgur (e.g.: https://i.imgur.com/example.jpg)")]
        public string? ImageUrl { get; set; }

        /// <summary>
        /// Music genre ID
        /// </summary>
        [Range(1, int.MaxValue, ErrorMessage = "The music genre ID is required")]
        public int MusicGenreId { get; set; }
    }
}
