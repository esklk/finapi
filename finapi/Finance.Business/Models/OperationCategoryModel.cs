namespace Finance.Business.Models
{
    public class OperationCategoryModel
    {
        public int AccountId { get; set; }

        public int Id { get; set; }

        public bool IsIncome { get; set; }

        public string Name { get; set; } = default!;
    }
}
