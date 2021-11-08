using System;

namespace Finance.Business.Models
{
    public class OperationModel
    {
        public int Id { get; set; }

        public int AccountId { get; set; }

        public double Ammount { get; set; }

        public int AuthorId { get; set; }

        public int CategoryId { get; set; }

        public DateTime CreatedOn { get; set; }
    }
}
