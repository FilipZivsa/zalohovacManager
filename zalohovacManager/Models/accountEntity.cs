using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace zalohovacManager.Models
{
    [Table("account")]
    public class accountEntity
    {
        [Key]
        [Column("id")]
        public int ID { get; set; }

        [Column("username")]
        public string Username { get; set; }

        [Column("password")]
        public string Password { get; set; }
    }
}