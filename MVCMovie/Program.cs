using Microsoft.EntityFrameworkCore;
using MvcMovie.Features.Movies.Services;
using MvcMovie.Models;
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
            // Add services to the container
            // instructs razor to search the Features folder first
            builder.Services.AddControllersWithViews()
                .AddRazorOptions(options => options.ViewLocationExpanders.Add(new MvcMovie.Infrastructure.FeatureViewLocationExpander()));

            // add movies services
            builder.Services.AddScoped<MvcMovie.Features.Movies.Services.IMovieService, MvcMovie.Features.Movies.Services.MovieService>();


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
            app.UseStaticFiles(); // included here to allow us to reference layout files
            app.UseRouting();

            app.UseAuthorization();
            app.MapControllers();
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}"
                );
            app.Run();
        }
    }
}
