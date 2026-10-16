using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using System.IO;

namespace PersonalCalendar.Data {
    public class CalendarDbContextFactory
    : IDesignTimeDbContextFactory<CalendarDbContext> {
        public CalendarDbContext CreateDbContext(string[] args) {
            var databasePath = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                "PersonalCalendar",
                "personalcalendar.db");

            Directory.CreateDirectory(Path.GetDirectoryName(databasePath)!);

            var options = new DbContextOptionsBuilder<CalendarDbContext>()
                .UseSqlite($"Data Source={databasePath}")
                .Options;

            return new CalendarDbContext(options);
        }
    }
}
