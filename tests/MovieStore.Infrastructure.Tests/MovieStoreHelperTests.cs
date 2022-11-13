using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using MoviesStore.Domain.Models;
using MoviesStore.Infrastructure.Context;

namespace MovieStore.Infrastructure.Tests
{
    public class MovieStoreHelperTests
    {
        public static DbContextOptions<MovieStoreDbContext> MovieStoreDbContextOptionsSQLiteInMemory()
        {
            var connectionStringBuilder = new SqliteConnectionStringBuilder { DataSource = ":memory:" };
            var connection = new SqliteConnection(connectionStringBuilder.ToString());

            var options = new DbContextOptionsBuilder<MovieStoreDbContext>()
                .UseSqlite(connection)
                .Options;

            return options;
        }

        public static async void CreateDataBaseSQLiteInMemory(DbContextOptions<MovieStoreDbContext> options)
        {
            await using (var context = new MovieStoreDbContext(options))
            {
                await context.Database.OpenConnectionAsync();
                await context.Database.EnsureCreatedAsync();
                CreateData(context);
            }
        }

        #region Test Data

        private static void CreateData(MovieStoreDbContext movieStoreDbContext)
        {
            movieStoreDbContext.Categories.Add(new Category { Id = 1, Name = "Category Test 1" });
            movieStoreDbContext.Categories.Add(new Category { Id = 2, Name = "Category Test 2" });
            movieStoreDbContext.Categories.Add(new Category { Id = 3, Name = "Category Test 3" });
            movieStoreDbContext.Movies.Add(new Movie()
            {
                Id = 1,
                Title = "Movie Test 1",
                Author = "Author Test 1",
                Description = "Description Test 1",
                CategoryId = 1,
                ReleaseDate = new DateTime(2020, 1, 1, 0, 0, 0, 0)
            });
            movieStoreDbContext.Movies.Add(new Movie()
            {
                Id = 2,
                Title = "Movie Test 2",
                Author = "Author Test 2",
                Description = "Description Test 2",
                CategoryId = 1,
                ReleaseDate = new DateTime(2020, 2, 2, 0, 0, 0, 0)
            });
            movieStoreDbContext.Movies.Add(new Movie()
            {
                Id = 3,
                Title = "Movie Test 3",
                Author = "Author Test 3",
                Description = "Description Test 3",
                CategoryId = 3,
                ReleaseDate = new DateTime(2020, 3, 3, 0, 0, 0, 0)
            });

            movieStoreDbContext.SaveChangesAsync();
        }

        public static async Task CleanDataBase(DbContextOptions<MovieStoreDbContext> options)
        {
            await using (var context = new MovieStoreDbContext(options))
            {
                foreach (var movie in context.Movies)
                {
                    context.Movies.Remove(movie);
                }

                await context.SaveChangesAsync();
            }

            await using (var context = new MovieStoreDbContext(options))
            {
                foreach (var category in context.Categories)
                {
                    context.Categories.Remove(category);
                }

                await context.SaveChangesAsync();
            }
        }

        #endregion

    }
}
