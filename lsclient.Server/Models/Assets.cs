using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace lsclient.Server.Models
{
    public class Image
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]  
        [Column("IMAGE_ID")]
        public int ImageId { get; set; }
         
        [Column("IMAGE_NAME")]
        public string? ImageName { get; set; }

        [Column("DIRECTORY")]
        public string? Directory { get; set; }


        [Column("IMAGE_URL")]
        public string? ImageUrl { get; set; }

          
    }

}
