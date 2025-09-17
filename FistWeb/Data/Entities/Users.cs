using System.ComponentModel.DataAnnotations.Schema;

namespace FistWeb.Data.Entities
{
    [Table("users", Schema = "clothings")]
    public class Users
    {
        [Column("userid")]
        public long UserId { get; set; }

        [Column("fullname")]
        public string Username { get; set; }

        [Column("facebookphone")]
        public string Phone { get; set; }

        [Column("CrateDate")]
        public DateTime CreateDate { get; set; }
    }
}