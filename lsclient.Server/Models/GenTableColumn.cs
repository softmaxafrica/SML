using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace lsclient.Server.Models
{
     public class GenTableColumn
    {
        [Key]
        [Column("CODE")]
        public string? Code { get; set; }
        [Column("TABLE_CODE")]
        public long? TableCode { get; set; }
        [Column("FIELD")]
        public string? Field { get; set; }
        [Column("HEADER")]
        public string? Header { get; set; }
        [Column("WIDTH")]
        public long? Width { get; set; }
        [Column("TYPE")]
        public string? Type { get; set; }
        [Column("COLUMN_DISPLAY")]
        public string? ColumnDisplay { get; set; }
        [Column("COLUMN_INCLUDED")]
        public string? ColumnIncluded { get; set; }
        [Column("COLUMN_INDEX")]
        public long? ColumnIndex { get; set; }
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
