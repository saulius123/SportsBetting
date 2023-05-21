using System.ComponentModel.DataAnnotations;
using Confluent.Kafka;
using SportsBetting.Services.Services.Interfaces;
using Newtonsoft.Json;
using SportsBetting.Services.DTOs;
using Microsoft.Extensions.Logging;

namespace SportsBetting.Services.Services
{
    public class ConsumerService
    {
        private readonly ConsumerConfig _config;
        private readonly IConsumer<Ignore, string> _consumer;
        private readonly IKafkaEventService _eventService;
        private readonly ILogger<ConsumerService> _logger;

        public ConsumerService(ConsumerConfig config, IKafkaEventService eventService, ILogger<ConsumerService> logger)
        {
            _config = config;
            _eventService = eventService;
            _consumer = new ConsumerBuilder<Ignore, string>(_config).Build();
            _logger = logger;
        }

        public async Task ConsumeMessagesAsync(CancellationToken cancellationToken)
        {
            _consumer.Subscribe("test-topic");

            while (!cancellationToken.IsCancellationRequested)
            {
                try
                {
                    _logger.LogInformation($"starts");
                    var cr = _consumer.Consume(cancellationToken);
                    _logger.LogInformation($"Consumed message '{cr.Message.Value}' at: '{cr.TopicPartitionOffset}'.");

                    var eventDto = JsonConvert.DeserializeObject<KafkaEventDto>(cr.Message.Value);
                    
                    if (ValidateEventDTO(eventDto, out var validationResults))
                    {
                        await _eventService.CreateIfNotExistsAsync(eventDto);
                    }
                    else
                    {
                        _logger.LogInformation("Invalid message structure:");
                        foreach (var validationResult in validationResults)
                        {
                            _logger.LogInformation($"- {validationResult.ErrorMessage}");
                        }
                    }
                }
                catch (ConsumeException e)
                {
                    _logger.LogInformation($"Error occurred: {e.Error.Reason}");
                }
                catch (JsonSerializationException e)
                {
                    _logger.LogInformation($"Error deserializing JSON message: {e.Message}");
                }
            }

            _consumer.Close();
        }

        private bool ValidateEventDTO(KafkaEventDto eventDto, out ICollection<ValidationResult> validationResults)
        {
            validationResults = new List<ValidationResult>();
            var validationContext = new ValidationContext(eventDto);
            return Validator.TryValidateObject(eventDto, validationContext, validationResults, true);
        }
    }
}