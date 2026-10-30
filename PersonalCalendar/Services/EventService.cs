using PersonalCalendar.Models;

namespace PersonalCalendar.Services {
    public class EventService {
        public List<CalendarEvent> GetByDate(DateOnly date) {
            using var db = App.CreateDbContext();

            return db.Events
                .Where(e => e.Date == date)
                .OrderBy(e => e.StartTime)
                .ToList();
        }

        public void Create(CalendarEvent calendarEvent) {
            using var db = App.CreateDbContext();

            db.Events.Add(calendarEvent);
            db.SaveChanges();
        }
    }
}
