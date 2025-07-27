using System.Net.NetworkInformation;
using Microsoft.CodeAnalysis.Options;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Logging.Console;
using NZWalks.Data;
using NZWalks.Mappings;
using NZWalks.Models.Domain;
using NZWalks.Models.Repositories;
using Serilog;

internal class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.



        builder.Services.AddControllers();
        builder.Services.AddHttpContextAccessor();
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        builder.Services.AddDbContext<NZWalksDbContext>(options =>
        options.UseSqlServer(builder.Configuration.GetConnectionString("NZWalksConnectionString"))
        );

        builder.Services.AddScoped<IRegionRepository, SQLRegionRepository>();
        builder.Services.AddScoped<IWalkRepository, SQLWalkRepository>();
        builder.Services.AddScoped<IImageRepository, LocalImageRepository>();

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

        app.UseStaticFiles(new StaticFileOptions
        {
            FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "Images")),
            RequestPath = "/Images"
        });

        app.MapControllers();

        app.Run();
    }
}