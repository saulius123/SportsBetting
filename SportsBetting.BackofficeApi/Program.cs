using Microsoft.EntityFrameworkCore;
using SportsBetting.Data.Models;
using SportsBetting.Data.Repositories.Interfaces;
using SportsBetting.Data.Repositories;
using SportsBetting.Services.Services.Interfaces;
using SportsBetting.Services.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Serilog;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        builder.Host.UseSerilog((context, configuration) => configuration.ReadFrom.Configuration(context.Configuration));
        
        builder.Services.AddControllers();
        builder.Services.AddDbContext<SportsBettingContext>(options => options.UseSqlServer(
            builder.Configuration.GetConnectionString("Database"),
            x => x.MigrationsAssembly("SportsBetting.BackofficeApi")
        ));

        builder.Services.AddScoped<IEventRepository, EventRepository>();
        builder.Services.AddScoped<IEventService, EventService>();
        builder.Services.AddScoped<ITeamRepository, TeamRepository>();
        builder.Services.AddScoped<IBetOfferRepository, BetOfferRepository>();
        builder.Services.AddScoped<IBetOfferService, BetOfferService>();
        builder.Services.AddScoped<IResultService, ResultService>();
        builder.Services.AddScoped<IResultRepository, ResultRepository>();
        builder.Services.AddScoped<IIdentityService, IdentityService>();
        builder.Services.AddScoped<IUserRepository, UserRepository>();

        //Add Authentication
        builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration.GetSection("AppSettings:Token").Value)),
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
        });


        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen(c =>
        {
            c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Description = "JWT Authorization header using the Bearer scheme. Example: \"Bearer {token}\"",
                Name = "Authorization",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.ApiKey,
                Scheme = "Bearer"
            });

            c.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        },
                            Scheme = "oauth2",
                            Name = "Bearer",
                            In = ParameterLocation.Header,
                        },
                    new List<string>()
                }
            });
        });

        var app = builder.Build();

        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseSerilogRequestLogging();

        app.UseHttpsRedirection();

        app.UseAuthentication();
        app.UseAuthorization();

        app.MapControllers();

        app.Run();
    }
}