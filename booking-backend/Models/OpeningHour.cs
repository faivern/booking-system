using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace booking_backend.Models
{
    /// <summary>
    /// Represents business opening hours.
    /// </summary>
    [Table("tbl_OpeningHour")]
    public class OpeningHour
    {
        /// <summary>
        /// Gets or sets the unique identifier for the opening hour.
        /// </summary>
        [Key]
        [Column("OpeningHourId")]
        public int OpeningHourId { get; set; }
        
        /// <summary>
        /// Gets or sets the business identifier.
        /// </summary>
        [Column("BusinessId")]
        public int BusinessId { get; set; }
        
        /// <summary>
        /// Gets or sets the day of the week (0-6 representing Monday-Sunday).
        /// </summary>
        [Column("DayOfWeek")]
        public byte DayOfWeek { get; set; }
        
        /// <summary>
        /// Gets or sets the opening time on this day.
        /// </summary>
        [Column("StartTime")]
        public TimeOnly StartTime { get; set; }
        
        /// <summary>
        /// Gets or sets the closing time on this day.
        /// </summary>
        [Column("EndTime")]
        public TimeOnly EndTime { get; set; }

        /// <summary>
        /// Gets or sets the associated business.
        /// </summary>
        [ForeignKey("BusinessId")]
        public Business? Business { get; set; }
    }
}