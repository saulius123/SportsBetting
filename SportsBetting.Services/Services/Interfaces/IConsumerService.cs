namespace SportsBetting.Services.Services.Interfaces
{
    public interface IConsumerService
    {
        void ConsumeMessagesAsync(CancellationToken cancellationToken);
    }
}