
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace lsclient.Server.Models
{

    public class SecUser
    {
        [Key]
        [Column("USER_ID")]
        public string UserID { get; set; }

        [Required]
        [Column("PASSWORD_HASH")]
        public string PasswordHash { get; set; }

        [Column("ROLE")]
        public string Role { get; set; } // e.g., "Admin", "Driver", "Customer", "Company"

        [Required]
        [Column("EMAIL")]
        public string Email { get; set; }

        [Column("LAST_LOGIN")]
        public DateTime? LastLogin { get; set; }

        [Column("STATUS")]
        public string Status { get; set; } // e.g., "Pending", "Approved"
    }
}
