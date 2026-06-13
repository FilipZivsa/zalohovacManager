
using Microsoft.EntityFrameworkCore;
using zalohovacManager.Database;
using zalohovacManager.Services;

namespace zalohovacManager
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);





            builder.Services.AddDbContext<DatabaseContext>(options =>
            {
                options.UseLazyLoadingProxies();
                options.UseMySQL(builder.Configuration.GetConnectionString("MojeSkolniDatabaze"));
            });

            builder.Services.AddScoped<JobService>();






            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
