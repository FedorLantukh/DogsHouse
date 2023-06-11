using AppLogic.Services;
using DataAccess.Context;
using DataAccess.Repository;
using DogsHouseWeb.Validators;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using System.Net;

namespace DogsHouseWeb
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddMemoryCache();

            // Add services to the container.

            builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
            builder.Services.AddScoped<IDogService, DogService>();
            builder.Services.AddScoped<IDogValidator, DogValidator>();

            var connectionString = builder.Configuration.GetConnectionString("DogDbConnection");


            builder.Services.AddDbContext<DogDbContext>(options =>
            options.UseSqlServer(connectionString));

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(options => {

                options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, "DogsHouseWeb.xml"));
            });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.Use(async (context, next) =>
            {
                var maxRequestsPerSecond = 10;
                var memoryCache = context.RequestServices.GetRequiredService<IMemoryCache>();

                var requestCount = await memoryCache.GetOrCreateAsync<int>(context.Request.HttpContext.Connection.RemoteIpAddress.ToString(), entry =>
                {
                    entry.SetAbsoluteExpiration(TimeSpan.FromSeconds(1));
                    return Task.FromResult(0);
                });

                if (requestCount > maxRequestsPerSecond)
                {
                    context.Response.StatusCode = (int)HttpStatusCode.TooManyRequests;
                    await context.Response.WriteAsync("429 Too Many Requests");
                    return;
                }

                requestCount++;
                memoryCache.Set(context.Request.HttpContext.Connection.RemoteIpAddress.ToString(), requestCount);

                await next();
            });

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}