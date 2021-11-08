namespace Finance.Web.Api.Services
{
    interface INestingCheckerFactory
    {
        public INestingChecker Create(string nestedResourceIdentifier);
    }
}
