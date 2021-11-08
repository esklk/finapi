using System.Collections.Generic;

namespace Finance.Data.Models
{
    public class Account
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public ICollection<OperationCategory> OperationCategories { get; set; }

        public ICollection<Operation> Operations { get; set; }

        public ICollection<User> Users { get; set; }
    }
}
