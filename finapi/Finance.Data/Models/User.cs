using System.Collections.Generic;

namespace Finance.Data.Models
{
    public class User
    {
        public int Id { get; set; }

        public ICollection<Account> Accounts { get; set; }

        public string Name { get; set; }

        public ICollection<Operation> Operations { get; set; }

        public ICollection<UserLogin> UserLogins { get; set; }
    }
}
