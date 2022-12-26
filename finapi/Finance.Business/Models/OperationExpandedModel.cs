namespace Finance.Business.Models
{
    public class OperationExpandedModel : OperationModel
    {
        public AccountModel Account { get; set; }
        public UserModel Author { get; set; }
        public OperationCategoryModel Category { get; set; }
    }
}
