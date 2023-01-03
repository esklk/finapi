using System.Collections.Generic;

namespace Finance.Data.Models
{
    public class Account
    {
        public int Id { get; set; }

        public string Name { get; set; } = default!;

        public ICollection<OperationCategory> OperationCategories { get; set; } = default!;

        public ICollection<Operation> Operations { get; set; } = default!;

        public ICollection<User> Users { get; set; } = default!;
    }
}
