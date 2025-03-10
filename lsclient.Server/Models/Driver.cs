using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace lsclient.Server.Models
{
    public class Driver
    {
        [Key]
        [Column("DRIVER_ID")]
        public string DriverID { get; set; }

        [Required]
        [Column("FULL_NAME")]
        public string FullName { get; set; }

        [Required]
        [EmailAddress]
        [Column("EMAIL")]
        public string Email { get; set; }

        [Required]
        [Phone]
        [Column("PHONE")]
        public string Phone { get; set; }

        [Required]
        [Column("LICENSE_NUMBER")]
        public string LicenseNumber { get; set; }

        [Column("STATUS")]
        public string? Status { get; set; }

        [Column("REGISTRATION_COMMENT")]
        public string? RegstrationComment { get; set; }
        
        [Column("LICENSE_CLASSES")]
        public string? LicenseClasses { get; set; }

        [Column("LICENSE_EXPIRE_DATE")]
        public DateOnly? LicenseExpireDate { get; set; }
        [Column("IS_AVILABLE_FOR_BOOKING")]
        public bool? isAvilableForBooking { get; set; }
        [Column("IMAGE_URL")]
        public string? ImageUrl { get; set; }

        // Navigation properties
        public virtual Company Company { get; set; }
        [ForeignKey("CompanyID")]
        public string CompanyID { get; set; }
        public virtual List<TruckType>? TruckTypes { get; set; } // Relationship to TruckType

    }

}
