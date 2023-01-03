using System.Collections.Generic;

namespace Finance.Data.Models
{
    public class User
    {
        public int Id { get; set; }

        public ICollection<Account> Accounts { get; set; } = default!;

        public string Name { get; set; } = default!;

        public ICollection<Operation> Operations { get; set; } = default!;

        public ICollection<UserLogin> UserLogins { get; set; } = default!;
    }
}
