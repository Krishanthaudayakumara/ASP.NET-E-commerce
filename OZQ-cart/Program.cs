using OZQ_cart.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using OZQ_cart.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using AutoMapper;
using OZQ_cart.AutoMapper;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.

        builder.Services.AddControllers();
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        // Add the Identity services
        builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

        // Add the DbContext service
        builder.Services.AddDbContext<ApplicationDbContext>(x => x.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

        builder.Services.AddAuthentication()
                    .AddCookie(options =>
                    {
                        options.SlidingExpiration = true;
                        options.ExpireTimeSpan = TimeSpan.FromMinutes(30);
                    });

        builder.Services.AddCors(options =>
        {
            options.AddPolicy("AllowSpecificOrigin",
                builder => builder.WithOrigins("*").AllowAnyMethod().AllowAnyHeader());
        });

        builder.Services.AddControllers().AddJsonOptions(options => options.JsonSerializerOptions.IgnoreReadOnlyProperties = true);

        var config = new MapperConfiguration(cfg => {
            cfg.AddProfile(new MyAutoMapperProfile());
        });
        var mapper = config.CreateMapper();
        builder.Services.AddSingleton(mapper);



        var app = builder.Build();

        app.UseDefaultFiles();
        app.UseStaticFiles();


        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        app.UseAuthentication();
        app.UseAuthorization();

        app.MapControllers();
        app.UseCors(options => options.WithOrigins("*").AllowAnyMethod().AllowAnyHeader());
        app.UseCors();


        app.Run();
    }
}
