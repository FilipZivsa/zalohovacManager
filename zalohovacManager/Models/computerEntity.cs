using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace zalohovacManager.Models
{
    public class computerEntity
    {
        [Key]
        [Column("uuid")]

        public string UUID { get; set; }


        [Column("name")]
        public string Name  { get; set; }

        [Column("enabled")]
        public int Enabled { get; set; }


    }
}
