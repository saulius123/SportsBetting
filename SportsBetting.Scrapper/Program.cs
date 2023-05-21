using Confluent.Kafka;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using SportsBetting.Scrapper.DTOs;
using SportsBetting.Scrapper.Services;
using SportsBetting.Scrapper.Services.Interfaces;
using StackExchange.Redis;

namespace SportsBetting.Scrapper
{
    class Program
    {
        static async Task Main(string[] args)
        {
            IConfigurationBuilder builder = new ConfigurationBuilder().AddJsonFile("appsettings.Development.json", false, true);
            IConfigurationRoot root = builder.Build();

            var kafkaProducerConfig = new ProducerConfig
            {
                BootstrapServers = root["Kafka:BootstrapServers"]
            };

            IMessageSender messageSender = new MessageSender(kafkaProducerConfig);

            IEventScrapper basketNewsScrapper = new BasketNewsScrapper();

            var events = await basketNewsScrapper.GetEvents();

            var topic = "test-topic";

            var redis = ConnectionMultiplexer.Connect(root["Redis:ConnectionString"]);
            var db = redis.GetDatabase();

            var shouldContinue = true;

            try
            {
                while (shouldContinue)
                {
                    shouldContinue = await ProcessAndSendEvents(events, messageSender, topic, db);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error producing message: {e.Message}");
            }
        }

        private static async Task<bool> ProcessAndSendEvents(IEnumerable<KafkaEventDto> events, IMessageSender messageSender, string topic, IDatabase db)
        {
            Console.WriteLine("Press Enter key to run the BasketNews scrapper");
            while (Console.ReadKey().Key != ConsoleKey.Enter) { }

            if (Console.KeyAvailable && Console.ReadKey(true).Key == ConsoleKey.Escape)
            {
                return false;
            }

            
            int eventCount = 0;
            foreach (KafkaEventDto eventItem in events)
            {
                string eventKey = $"scrappedEvent:{eventItem.Team1}:{eventItem.League}:{eventItem.StartDateTime}";

                // Check if the event already exists in Redis
                if (!db.KeyExists(eventKey))
                {
                    // Send the message
                    string message = JsonConvert.SerializeObject(eventItem);
                    await messageSender.SendMessageAsync(topic, message);
                    eventCount++;

                    // Store the event in Redis
                    db.StringSet(eventKey, "");
                }
            }

            return true;
        }
    }
}