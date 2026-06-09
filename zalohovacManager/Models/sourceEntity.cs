using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace zalohovacManager.Models
{

    [Table("source")]
    public class sourceEntity
    {
        [Key]
        [Column("id")]

        public int ID { get; set; }


        [Column("directory")]
        public string Directory { get; set; }

        [Column("job_id")]
        public int JobID { get; set; }

    }
}
