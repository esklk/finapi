using System;

namespace Finance.Web.Api.Models
{
    public class OperationDataModel
    {
        public double Ammount { get; set; }

        public int CategoryId { get; set; }

        public DateTime? CreatedAt { get; set; }
    }
}
