namespace Finance.Bot.Business.Services
{
    public interface IMessageProcessor
    {
        Task ProcessAsync(string? text);
    }
}
