
using DnD.Api.Endpoints;
using DnD.Api.Exceptions;
using DnD.Domain.Services;
using DnD.Infrastructure;
using DnD.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Serilog;
using System.Text.Json.Serialization;

namespace DnD.Api
{
    public class Program
    {

        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Host.UseSerilog((context, config) => config.ReadFrom.Configuration(context.Configuration));

            builder.Services.AddDbContext<AppDbContext>(options =>
            options.UseSqlServer(builder.Configuration.GetConnectionString("Default")));

            builder.Services.AddScoped<ICharacterService, CharacterService>();
            builder.Services.AddScoped<IInventoryService, InventoryService>();
            builder.Services.AddScoped<IMasterService, MasterService>();
            builder.Services.AddScoped<IWikiService, WikiService>();
            builder.Services.AddScoped<IPlayerService, PlayerService>();

            builder.Services.AddTransient<ICharacterCalculatorService, CharacterCalculatorService>();

            builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
            builder.Services.AddProblemDetails();
            builder.Services.AddAuthorization();

            builder.Services.ConfigureHttpJsonOptions(options =>
            {
                options.SerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
            });

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            app.UseExceptionHandler();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapCharacterEndpoints();
            app.MapInventoryEndpoints();
            app.MapWikiEndpoints();
            app.MapPlayerEndpoints();
            app.MapMasterEndpoints();


            app.Run();
        }
    }
}
