using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using PersonalCalendar.Data;
using PersonalCalendar.Models;

namespace PersonalCalendar.Tests {
    public class CalendarTaskTests {
        [Fact]
        public void AddCalendarTask_ShouldPersistCalendarTask() {
            var connection = new SqliteConnection("DataSource=:memory:");
            connection.Open();

            var options = new DbContextOptionsBuilder<CalendarDbContext>()
                .UseSqlite(connection)
                .Options;

            var context = new CalendarDbContext(options);
            context.Database.EnsureCreated();

            var calendarTask = new CalendarTask {
                Title = "Faire les courses",
                Description = "Acheter du pain, du lait et des œufs",
                Date = new DateOnly(2026, 10, 16),
                Time = new TimeOnly(10, 30)
            };

            context.Tasks.Add(calendarTask);
            context.SaveChanges();
        }
    }
}
