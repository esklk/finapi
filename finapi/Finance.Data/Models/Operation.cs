using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Finance.Data.Models
{
    public class Operation
    {
        public int Id { get; set; }

        public int AccountId { get; set; }

        [ForeignKey(nameof(AccountId))]
        public Account Account { get; set; } = default!;

        public double Ammount { get; set; }

        public int AuthorId { get; set; }

        [ForeignKey(nameof(AuthorId))]
        public User Author { get; set; } = default!;

        public int CategoryId { get; set; } = default!;

        [ForeignKey(nameof(CategoryId))]
        public OperationCategory Category { get; set; } = default!;

        public DateTime CreatedOn { get; set; }
    }
}
