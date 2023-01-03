namespace Finance.Business.Models
{
    public class OperationExpandedModel : OperationModel
    {
        public AccountModel Account { get; set; } = default!;
        public UserModel Author { get; set; } = default!;
        public OperationCategoryModel Category { get; set; } = default!;
    }
}
