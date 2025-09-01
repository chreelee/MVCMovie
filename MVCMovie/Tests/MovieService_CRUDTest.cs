using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MVCMovie.Data;
using MVCMovie.Models;
using MvcMovie.Services;
using MvcMovie.Models;
using Xunit;
using NuGet.Protocol;


namespace Tests;

public class MovieService_CRUDTest
{

    // constructors
    private static MovieService CreateService()
    {
        // will not be relying on database
        var dbCtxBuilder = new DbContextOptionsBuilder<MvcMovieContext>();
        var inMemDb = dbCtxBuilder.UseInMemoryDatabase(Guid.NewGuid().ToString());
        var options = inMemDb.Options;
        var context = new MvcMovieContext(options);

        return new MovieService(context);
    }

    [Fact]
    public async Task MovieService_CanPerformCRUDOp()
    {
        var svc = CreateService();

        // Create
        var movie = new Movie
        {
            Title = "Inception",
            Genre = "Sci-Fi",
            Price = 18.99M,
            Rating = "PG-13",
            ReleaseDate = DateTime.Parse("2010-05-01")
        };

        // add movie to database
        await svc.AddAsync(movie);

        // Read
        var read = await svc.GetByIdAsync(movie.Id);
        // add assertions
        Assert.NotNull(read);
        Assert.Equal(movie.Title, read.Title);

        // Update
        read.Title = "Inception (20th year anniversary edition)";
        await svc.UpdateAsync(read);
        var updated = await svc.GetByIdAsync(movie.Id);
        Assert.NotNull(updated);
        Assert.Equal(read.Title, updated.Title);


        // Delete
        await svc.DeleteAsync(movie.Id);
        var deleted = await svc.GetByIdAsync(movie.Id);
        Assert.Null(deleted);



    }
}
