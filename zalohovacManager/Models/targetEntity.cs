using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;


namespace zalohovacManager.Models
{
    [Table("target")]
    public class targetEntity
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
