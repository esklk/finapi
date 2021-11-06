using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Finance.Data.Models
{
    public class OperationCategory
    {
        public int Id { get; set; }

        public bool IsIncome { get; set; }

        public string Name { get; set; }

        public int AccountId { get; set; }

        [ForeignKey(nameof(AccountId))]
        public Account Account { get; set; }

        public ICollection<Operation> Operations { get; set; }
    }
}
