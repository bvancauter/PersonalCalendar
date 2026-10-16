using Microsoft.EntityFrameworkCore;
using PersonalCalendar.Data;
using System.IO;
using System.Windows;

namespace PersonalCalendar {
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application {
        public static string DatabasePath { get; } = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
            "PersonalCalendar",
            "personalcalendar.db");

        protected override void OnStartup(StartupEventArgs e) {
            base.OnStartup(e);

            Directory.CreateDirectory(
                Path.GetDirectoryName(DatabasePath)!);
        }

        public static CalendarDbContext CreateDbContext() {
            var options = new DbContextOptionsBuilder<CalendarDbContext>()
                .UseSqlite($"Data Source={DatabasePath}")
                .Options;

            return new CalendarDbContext(options);
        }
    }
}
