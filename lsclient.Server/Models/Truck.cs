using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace lsclient.Server.Models
{
    public class Truck
    {
        [Key]
        [Column("TRUCK_ID")]
        public string TruckID { get; set; }

        [Required]
        [Column("TRUCK_NUMBER")]
        public string TruckNumber { get; set; }

        [Required]
        [Column("MODEL")]
        public string Model { get; set; }

        [Required]
        [Column("COMPANY_ID")]
        public string CompanyID { get; set; }
       
        [Column("DriverID")]
        public string? DriverID { get; set; }

        [Column("IS_ACTIVE")]
        public bool IsActive { get; set; }

        [Column("IS_TRUCK_AVILABLE_FOR_BOOKING")]
        public bool IsTruckAvilableForBooking { get; set; }

        [Required]
        [Column("TRUCK_TYPE_ID")]
        public string TruckTypeID { get; set; }

        [Column("CHASIS_NO")]
        public string? chasisNo { get; set; }
        [Column("BRAND")]
        public string? Brand { get; set; }

        [Column("ENGINE_CAPACITY")]
        public string? EngineCapacity { get; set; }

        [Column("FUEL_TYPE")]
        public string? FuelType { get; set; }

        [Column("CABIN_TYPE")]
        public string? CabinType { get; set; }

        [Column("CATEGORY")]
        public string? Category { get; set; }
        
        [Column("DRIVE")]
        public string? Drive { get; set; }

        // Navigation properties
        [ForeignKey("CompanyID")]
        //[JsonIgnore]
        public virtual Company Company { get; set; }

        [ForeignKey("TruckTypeID")]
        public virtual TruckType TruckType { get; set; }

        [ForeignKey("DriverID")]
        //[JsonIgnore]
        public virtual Driver Driver { get; set; }
        [JsonIgnore]
        public virtual ICollection<JobRequest> JobRequests { get; set; }

        public virtual ICollection<Location> Locations { get; set; }
    }
}
