using Microsoft.CodeAnalysis.Options;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using NZWalks.Data;
using NZWalks.Mappings;
using NZWalks.Models.Domain;
using NZWalks.Models.Repositories;

internal class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.

        builder.Services.AddControllers();
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        builder.Services.AddDbContext<NZWalksDbContext>(options =>
        options.UseSqlServer(builder.Configuration.GetConnectionString("NZWalksConnectionString"))
        );

        builder.Services.AddScoped<IRegionRepository, SQLRegionRepository>();
        builder.Services.AddScoped<IWalkRepository, SQLWalkRepository>();

        builder.Services.AddAutoMapper(typeof(AutoMapperProfiles));

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();

            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint("/swagger/v1/swagger.json", "NZWalks API v1");
                options.RoutePrefix = string.Empty;
            });
        }


        app.UseHttpsRedirection();

        app.UseAuthorization();

        app.MapControllers();

        app.Run();
    }
}