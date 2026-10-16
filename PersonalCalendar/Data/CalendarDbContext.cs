using Microsoft.EntityFrameworkCore;
using PersonalCalendar.Models;

namespace PersonalCalendar.Data {
    public class CalendarDbContext : DbContext {
        public CalendarDbContext(DbContextOptions<CalendarDbContext> options) : base(options) {

        }

        public DbSet<CalendarEvent> Events => Set<CalendarEvent>();
        public DbSet<CalendarTask> Tasks => Set<CalendarTask>();
        public DbSet<Category> Categories => Set<Category>();
    }
}
