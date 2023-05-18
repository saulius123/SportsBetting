namespace SportsBetting.Scrapper.Services.Interfaces
{
    public interface IMessageSender
    {
        Task SendMessageAsync(string topic, string message);
    }
}