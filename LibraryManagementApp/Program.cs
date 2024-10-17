using LibraryManagementApp.Entities;
using LibraryManagementApp.Interfaces;
using LibraryManagementApp.Mapper;
using LibraryManagementApp.Repositories;
using LibraryManagementApp.Services;
using LibraryManagementApp.UnitOfWorkPattern;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using NLog;
using NLog.Web;
using System.Net;

namespace LibraryManagementApp
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var logger = NLog.LogManager.Setup().LoadConfigurationFromFile("NLog.config").GetCurrentClassLogger();

            var builder = WebApplication.CreateBuilder(args);

            LoggerConfiguration(logger, builder);

            builder.Services.AddAutoMapper(typeof(AutoMapperProfile));
            // Add services to the container.
            builder.Services.AddDbContext<LibraryDbContext>();
            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
            builder.Services.AddScoped<IAuthorService, AuthorService>();
            builder.Services.AddScoped<IBookService, BookService>();
            builder.Services.AddScoped<IBookAllocationService, BookAllocationService>();
            builder.Services.AddScoped<IUserService, UserService>();
            builder.Services.AddScoped<IRepository<Author>, Repository<Author>>();
            builder.Services.AddScoped<IRepository<Book>, Repository<Book>>();
            builder.Services.AddScoped<IRepository<User>, Repository<User>>();

            builder.Services.AddScoped<IBookAllocationRepository, BookAllocationRepository>();
            builder.Services.AddScoped<IAuthorRepository, AuthorRepository>();
            builder.Services.AddScoped<IBookRepository, BookRepository>();
            builder.Services.AddScoped<IUserRepository, UserRepository>();
            builder.Services.AddStackExchangeRedisCache(options =>
            {
                options.Configuration = builder.Configuration.GetConnectionString("RedisCache");
                options.InstanceName = "MyAppCache_";
            });
            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo { Title = "Simple Report APIs", Version = "v1" });
            }); ;

            var app = builder.Build();

            // Migrate the database on startup
            using (var scope = app.Services.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<LibraryDbContext>();
                // This will apply any pending migrations automatically
                // with this command you dont need to run command "Update-Database" but still needs to run command
                // "Add-Migration MigrationName" prior running the application otherwise runtime exeption will be thrown.
                dbContext.Database.Migrate();
            }

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Simple Report API V1");
                });
            }

            app.UseExceptionHandler(options =>
               {
                   options.Run(async context =>
                   {
                       context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                       var ex = context.Features.Get<IExceptionHandlerFeature>();
                       if (ex != null)
                       {
                           logger.Error(ex.Error, ex.Error.StackTrace);

                           var result = JsonConvert.SerializeObject(new { StatusCode = context.Response.StatusCode, Message = ex.Error.Message });
                           context.Response.ContentType = "application/json";
                           await context.Response.WriteAsync(result);
                       }
                   });
               });

            app.UseHttpsRedirection();

            //  app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }

        private static void LoggerConfiguration(Logger logger, WebApplicationBuilder builder)
        {
            // Remove default logging providers
            builder.Logging.ClearProviders();

            // Add NLog as a logging provider
            builder.Logging.SetMinimumLevel(Microsoft.Extensions.Logging.LogLevel.Trace);
            builder.Host.UseNLog();

            logger.Info("logger is configured successfully");
        }
    }
}
