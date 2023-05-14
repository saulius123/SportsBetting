﻿using System;
using System.ComponentModel.DataAnnotations;
using System.Threading;
using Confluent.Kafka;
using KafkaConsumer.Services.Interfaces;
using Newtonsoft.Json;

namespace KafkaConsumer.Services
{
    public class ConsumerService
    {
        private readonly ConsumerConfig _config;
        private readonly IConsumer<Ignore, string> _consumer;
        private readonly IEventService _eventService;

        public ConsumerService(ConsumerConfig config, IEventService eventService)
        {
            _config = config;
            _eventService = eventService;
            _consumer = new ConsumerBuilder<Ignore, string>(_config).Build();
        }

        public async Task ConsumeMessagesAsync(CancellationToken cancellationToken)
        {
            _consumer.Subscribe("test-topic");

            while (!cancellationToken.IsCancellationRequested)
            {
                try
                {
                    var cr = _consumer.Consume(cancellationToken);
                    Console.WriteLine($"Consumed message '{cr.Message.Value}' at: '{cr.TopicPartitionOffset}'.");

                    var eventDto = JsonConvert.DeserializeObject<EventDTO>(cr.Message.Value);
                    Console.WriteLine($"g");
                    if (ValidateEventDTO(eventDto, out var validationResults))
                    {
                        await _eventService.CreateIfNotExistsAsync(eventDto);
                    }
                    else
                    {
                        Console.WriteLine("Invalid message structure:");
                        foreach (var validationResult in validationResults)
                        {
                            Console.WriteLine($"- {validationResult.ErrorMessage}");
                        }
                    }
                }
                catch (ConsumeException e)
                {
                    Console.WriteLine($"Error occurred: {e.Error.Reason}");
                }
                catch (JsonSerializationException e)
                {
                    Console.WriteLine($"Error deserializing JSON message: {e.Message}");
                }
            }

            _consumer.Close();
        }

        private bool ValidateEventDTO(EventDTO eventDto, out ICollection<ValidationResult> validationResults)
        {
            validationResults = new List<ValidationResult>();
            var validationContext = new ValidationContext(eventDto);
            return Validator.TryValidateObject(eventDto, validationContext, validationResults, true);
        }
    }
}