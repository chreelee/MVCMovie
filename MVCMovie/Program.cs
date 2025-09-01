using Microsoft.EntityFrameworkCore;
using MvcMovie.Models;
using MvcMovie.Services;
using MVCMovie.Data;
namespace MVCMovie
{
    public class Program
    {
        public static void Main(string[] args)
        {
            //  wires the context to the connection string, which config will read
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddDbContext<MvcMovieContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("MVCMovieContext") ?? throw new InvalidOperationException("Connection string 'MVCMovieContext' not found.")));
            // Add services to the container.
            builder.Services.AddControllersWithViews();

            // add movies services
            builder.Services.AddScoped<IMovieService, MovieService>();


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
