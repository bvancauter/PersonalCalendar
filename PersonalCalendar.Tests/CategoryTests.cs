using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using PersonalCalendar.Data;
using PersonalCalendar.Models;

namespace PersonalCalendar.Tests {
    public class CategoryTests {
        [Fact]
        public void AddCategory_ShouldPersistCategory() {
            var connection = new SqliteConnection("DataSource=:memory:");
            connection.Open();

            var options = new DbContextOptionsBuilder<CalendarDbContext>()
                .UseSqlite(connection)
                .Options;

            var context = new CalendarDbContext(options);
            context.Database.EnsureCreated();

            var category = new Category {
                Name = "Work",
                Color = "#FF0000"
            };

            context.Categories.Add(category);
            context.SaveChanges();
        }
    }
}
