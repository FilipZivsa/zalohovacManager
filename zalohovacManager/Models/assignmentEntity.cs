using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace zalohovacManager.Models
{
    [Table("assignment")]
    public class assignmentEntity
    {
        [Key]
        [Column("id")]

        public int ID { get; set; }


        [Column("computer_uuid")]
        public Guid ComputerUUID { get; set; }

        [Column("job_id")]
        public int JobID { get; set; }

        [Column("assign_at")]
        public DateTime AssignAt { get; set; }

  
    }
}
