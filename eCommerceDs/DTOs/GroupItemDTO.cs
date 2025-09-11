namespace eCommerceDs.DTOs
{
    /// <summary>
    /// DTO that represents a group in list and basic operations
    /// </summary>
    public class GroupItemDTO
    {
        /// <summary>
        /// Group ID
        /// </summary>
        public int IdGroup { get; set; }

        /// <summary>
        /// Group name
        /// </summary>
        public string NameGroup { get; set; } = null!;

        /// <summary>
        /// Group image URL
        /// </summary>
        public string? ImageUrl { get; set; }

        /// <summary>
        /// Music genre ID
        /// </summary>
        public int MusicGenreId { get; set; }
    }
}
