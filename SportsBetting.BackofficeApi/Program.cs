using Microsoft.EntityFrameworkCore;
using SportsBetting.Data.Models;
using SportsBetting.Data.Repositories.Interfaces;
using SportsBetting.Data.Repositories;
using SportsBetting.Services.Services.Interfaces;
using SportsBetting.Services.Services;
using StackExchange.Redis;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddControllers();
        builder.Services.AddDbContext<SportsBettingContext>(options => options.UseSqlServer(
            builder.Configuration.GetConnectionString("Database")
        ));

        builder.Services.AddScoped<IEventRepository, EventRepository>();
        builder.Services.AddScoped<ITeamRepository, TeamRepository>();
        builder.Services.AddScoped<IBetOfferRepository, BetOfferRepository>();
        builder.Services.AddScoped<IResultService, ResultService>();
        builder.Services.AddScoped<IResultRepository, ResultRepository>();

        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        builder.Services.AddSingleton<IConnectionMultiplexer>(x => ConnectionMultiplexer.Connect(builder.Configuration["Redis:ConnectionString"]));

        var app = builder.Build();

        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        app.UseAuthorization();

        app.MapControllers();

        app.Run();
    }
}