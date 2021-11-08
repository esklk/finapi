using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Finance.Data.Models
{
    public class Operation
    {
        public int Id { get; set; }

        public double Ammount { get; set; }

        public int AuthorId { get; set; }

        [ForeignKey(nameof(AuthorId))]
        public User Author { get; set; }

        public int CategoryId { get; set; }

        [ForeignKey(nameof(CategoryId))]
        public OperationCategory Category { get; set; }

        public DateTime CreatedOn { get; set; }
    }
}
