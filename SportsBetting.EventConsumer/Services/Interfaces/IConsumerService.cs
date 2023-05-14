namespace SportsBetting.EventConsumer.Services.Interfaces
{
    public interface IConsumerService
    {
        void ConsumeMessagesAsync(CancellationToken cancellationToken);
    }
}