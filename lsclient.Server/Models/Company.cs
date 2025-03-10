
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace lsclient.Server.Models
{
  
    public class Company
    {
        [Key]
        [Column("COMPANY_ID")]
        public string CompanyID { get; set; }

        [Required]
        [Column("COMPANY_TIN_NUMBER")]
        public string CompanyTinNumber { get; set; }

        [Required]
        [Column("COMPANY_NAME")]
        public string CompanyName { get; set; }

        [Required]
        [Column("ADMIN_FULL_NAME")]
        public string AdminFullName { get; set; }

        [Required]
        [EmailAddress]
        [Column("ADMIN_EMAIL")]
        public string AdminEmail { get; set; }

        [Required]
        [Phone]
        [Column("ADMIN_CONTACT")]
        public string AdminContact { get; set; }

        [Column("COMPANY_ADDRESS")]
        public string? CompanyAddress { get; set; }

        [Column("COMPANY_DESCRIPTION")]
        public string? CompanyDescription { get; set; }

        [Column("COMPANY_LATITUDE")]
        public double? CompanyLatitude { get; set; }

        [Column("COMPANY_LONGITUDE")]
        public double? CompanyLongitude { get; set; }

        // Navigation properties
       // [JsonIgnore]
        //public virtual ICollection<Truck> Trucks { get; set; }
       // public virtual ICollection<Driver> Drivers { get; set; }
    }
}
