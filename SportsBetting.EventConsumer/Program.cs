using Confluent.Kafka;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SportsBetting.Data.Models;
using SportsBetting.Data.Repositories.Interfaces;
using SportsBetting.Data.Repositories;
using SportsBetting.Services.Services;
using SportsBetting.Services.Services.Interfaces;
using Serilog;

namespace SportsBetting.EventConsumer
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                .AddJsonFile("appsettings.Development.json")
                .Build();

            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(configuration)
                .WriteTo.Console()
                .CreateLogger();

            try
            {
                Log.Information("Starting up");
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Application start-up failed");
            }
            finally
            {
                Log.CloseAndFlush();
            }

            var services = new ServiceCollection();
            
            ConfigureServices(services, configuration);
            var serviceProvider = services.BuildServiceProvider();

            var consumerService = serviceProvider.GetRequiredService<ConsumerService>();

            var cts = new CancellationTokenSource();

            await consumerService.ConsumeMessagesAsync(cts.Token);
        }

        private static void ConfigureServices(IServiceCollection services, IConfigurationRoot configuration)
        {
            services.AddLogging(loggingBuilder =>
            loggingBuilder.AddSerilog(dispose: true));

            var kafkaConfig = configuration.GetSection("Kafka").Get<ConsumerConfig>();

            services.AddSingleton(kafkaConfig);

            var connectionString = configuration.GetConnectionString("Database");

            services.AddDbContext<SportsBettingContext>(options =>
                options.UseSqlServer(connectionString));

            services.AddScoped<IEventRepository, EventRepository>();
            services.AddScoped<ITeamRepository, TeamRepository>();
            services.AddScoped<ILeagueRepository, LeagueRepository>();
            services.AddScoped<ISportRepository, SportRepository>();
            services.AddScoped<IEventService, EventService>();
            services.AddScoped<ConsumerService>();
        }
    }
}