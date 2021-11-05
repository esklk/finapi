using System.Collections.Generic;

namespace Finance.Data.Models
{
    public class OperationCategory
    {
        public int Id { get; set; }

        public bool IsIncome { get; set; }

        public string Name { get; set; }

        public ICollection<Account> Accounts { get; set; }

        public ICollection<Operation> Operations { get; set; }
        
        public ICollection<User> Users { get; set; }
    }
}
