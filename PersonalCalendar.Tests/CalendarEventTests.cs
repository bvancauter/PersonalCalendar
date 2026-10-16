using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using PersonalCalendar.Data;
using PersonalCalendar.Models;

namespace PersonalCalendar.Tests {
    public class CalendarEventTests {
        [Fact]
        public void AddCalendarEvent_ShouldPersistCalendarEvent() {
            var connection = new SqliteConnection("DataSource=:memory:");
            connection.Open();

            var options = new DbContextOptionsBuilder<CalendarDbContext>()
                .UseSqlite(connection)
                .Options;

            var context = new CalendarDbContext(options);
            context.Database.EnsureCreated();

            var calendarEvent = new CalendarEvent {
                Title = "RDV Médecin",
                Date = new DateOnly(2026, 10, 16),
                StartTime = new TimeOnly(14, 0),
                EndTime = new TimeOnly(15, 0)
            };

            context.Events.Add(calendarEvent);
            context.SaveChanges();
        }
    }
}
