using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace lsclient.Server.Models
{
     public class GenTranslation
    {
       
 
        [Key]
        [Column("CODE")]

        public string? Code { get; set; }
        [Column("LANGUAGE")]
        public string? language { get; set; }

        [Column("DESCRIPTION")]
        public string? Description { get; set; }
        [Column("LAST_ACTION")]
        public string? LastAction { get; set; }
        [Column("CUSER")]
        public string? Cuser { get; set; }
        [Column("CDATE")]
        public DateTime? Cdate { get; set; }
        [Column("EUSER")]
        public string? Euser { get; set; }
        [Column("EDATE")]
        public DateTime? Edate { get; set; }
    }

  
}
