using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using Microsoft.IdentityModel.Tokens;
using NZWalks.API.Data;
using NZWalks.API.Models.Repositories;
using NZWalks.Data;
using NZWalks.Mappings;
using NZWalks.Models.Repositories;

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
        builder.Services.AddDbContext<NZWalksAuthDbContext>(options =>
        options.UseSqlServer(builder.Configuration.GetConnectionString("NZWalksAuthConnectionString"))
        );

        builder.Services.AddScoped<IRegionRepository, SQLRegionRepository>();
        builder.Services.AddScoped<IWalkRepository, SQLWalkRepository>();
        builder.Services.AddScoped<ITokenRepository, TokenRepository>();
        builder.Services.AddScoped<IImageRepository, LocalImageRepository>();

        builder.Services.AddAutoMapper(typeof(AutoMapperProfiles));

        builder.Services.AddIdentityCore<IdentityUser>()
        .AddRoles<IdentityRole>()
        .AddTokenProvider<DataProtectorTokenProvider<IdentityUser>>("NZWalks")
        .AddEntityFrameworkStores<NZWalksAuthDbContext>()
        .AddDefaultTokenProviders();

        builder.Services.Configure<IdentityOptions>(options =>
        {
            options.Password.RequireDigit = true;
            options.Password.RequireLowercase = true;
            options.Password.RequireNonAlphanumeric = true;
            options.Password.RequireUppercase = true;
            options.Password.RequiredLength = 6;
            options.Password.RequiredUniqueChars = 1;
        });

        // Jwt Authentication
        builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
        .AddJwtBearer(options =>
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
        });

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

        app.UseAuthentication();

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