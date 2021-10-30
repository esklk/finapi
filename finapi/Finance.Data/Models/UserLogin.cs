using System.ComponentModel.DataAnnotations.Schema;

namespace Finance.Data.Models
{
    public class UserLogin
    {
        public string Identifier { get; set; }

        public string Provider { get; set; }

        public int UserId { get; set; }

        [ForeignKey(nameof(UserId))]
        public User User { get; set; }
    }
}
