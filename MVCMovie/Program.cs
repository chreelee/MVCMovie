using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MvcMovie.Models;
using MVCMovie.Data;
using MVCMovie.Models;
namespace MVCMovie
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddDbContext<MvcMovieContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("MVCMovieContext") ?? throw new InvalidOperationException("Connection string 'MVCMovieContext' not found.")));
            // above wires the context to the connection string, which config will read

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            var app = builder.Build();

            // create scope
            using (var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider; // create services object

                SeedData.Initialize(services); // run SeedData Initalize method and pass services
            }


            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseRouting();

            app.UseAuthorization();

            app.MapStaticAssets();
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}")
                .WithStaticAssets();

            app.Run();
        }
    }
}
