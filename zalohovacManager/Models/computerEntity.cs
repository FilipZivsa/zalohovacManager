using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace zalohovacManager.Models
{
    [Table("computer")]
    public class computerEntity
    {
        [Key]
        [Column("uuid")]

        public Guid UUID { get; set; }


        [Column("name")]
        public string Name  { get; set; }

        [Column("enabled")]
        public int Enabled { get; set; }


    }
}
