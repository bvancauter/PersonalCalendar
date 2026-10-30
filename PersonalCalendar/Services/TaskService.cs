using PersonalCalendar.Models;

namespace PersonalCalendar.Services {
    public class TaskService {
        public List<CalendarTask> GetByDate(DateOnly date) {
            using var db = App.CreateDbContext();

            return db.Tasks
                .Where(t => t.Date == date)
                .OrderBy(t => t.Time)
                .ToList();
        }

        public void Create(CalendarTask calendarTask) {
            using var db = App.CreateDbContext();

            db.Tasks.Add(calendarTask);
            db.SaveChanges();
        }
    }
}
