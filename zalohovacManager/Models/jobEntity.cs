using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace zalohovacManager.Models
{
    [Table("job")]
    public class jobEntity
    {
        [Key]
        [Column("id")]

        public int ID { get; set; }


        [Column("timing")]
        public string Timing { get; set; }

        [Column("method")]
        public string Method { get; set; }

        [Column("retention_count")]
        public int RetentionCount { get; set; }

        [Column("retention_size")]
        public int RetentionSize { get; set; }

    }
}
