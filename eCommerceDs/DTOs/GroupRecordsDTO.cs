namespace eCommerceDs.DTOs
{
    /// <summary>
    /// DTO that represents a group along with its associated records
    /// </summary>
    public class GroupRecordsDTO
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
        /// Total number of records associated with this group
        /// </summary>
        public int TotalRecords { get; set; }

        /// <summary>
        /// List of records associated with this group
        /// </summary>
        public List<RecordItemDTO> Records { get; set; } = new List<RecordItemDTO>();
    }
}
