namespace Finance.Bot.Business.Exceptions
{
    public class MissingStateKeyException : Exception
    {
        public string Key { get; }

        public MissingStateKeyException(string key) : base($"The key \"{key}\" is missing is the state.")
        {
            Key = key;
        }
    }
}
