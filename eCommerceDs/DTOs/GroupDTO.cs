namespace eCommerceDs.DTOs
{
    /// <summary>
    /// DTO that represents a group in the system
    /// </summary>
    public class GroupDTO
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

        /// <summary>
        /// Music genre name
        /// </summary>
        public string? NameMusicGenre { get; set; } = null!;

        /// <summary>
        /// Total number of records associated with this group
        /// </summary>
        public int? TotalRecords { get; set; }
    }
}
