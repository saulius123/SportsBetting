using Confluent.Kafka;
using KafkaProducer.Services.Interfaces;

namespace Kafkaproducer.Services
{
    internal class MessageSender : IMessageSender
    {
        private readonly ProducerConfig _config;

        public MessageSender(ProducerConfig config)
        {
            _config = config;
        }
        public async Task SendMessageAsync(string topic, string message)
        {
            using var producer = new ProducerBuilder<Null, string>(_config).Build();
            try
            {
                var dr = await producer.ProduceAsync(topic, new Message<Null, string> { Value = message });
                Console.WriteLine($"Message '{message}' sent to topic {dr.Topic}, partition {dr.Partition}, offset {dr.Offset}");
            }
            catch (ProduceException<Null, string> e)
            {
                Console.WriteLine($"Error producing message: {e.Error.Reason}");
            }
        }
    }
}
