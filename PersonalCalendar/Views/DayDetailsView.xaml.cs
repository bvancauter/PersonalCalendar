using PersonalCalendar.Models;
using System.Collections.ObjectModel;
using System.Windows;

namespace PersonalCalendar.Views {
    /// <summary>
    /// Interaction logic for DayDetailsView.xaml
    /// </summary>
    public partial class DayDetailsView : Window {
        public ObservableCollection<CalendarEvent> Events { get; } = new();
        public ObservableCollection<CalendarTask> Tasks { get; } = new();

        public DayDetailsView(DateOnly selectedDate) {
            InitializeComponent();
            DataContext = this;

            SelectedDateTextBlock.Text = selectedDate.ToString("dddd, d MMMM yyyy");

            LoadEvents(selectedDate);
            LoadTasks(selectedDate);
        }

        private void LoadEvents(DateOnly selectedDate) {
            using var db = App.CreateDbContext();

            var events = db.Events
                .Where(e => e.Date == selectedDate)
                .OrderBy(e => e.StartTime)
                .ToList();

            Events.Clear();

            foreach (var calendarEvent in events) {
                Events.Add(calendarEvent);
            }
            Events.Add(new CalendarEvent {
                Title = "New Event",
                Date = selectedDate,
                StartTime = TimeOnly.FromDateTime(DateTime.Now),
                EndTime = TimeOnly.FromDateTime(DateTime.Now.AddHours(1)),
            });
        }

        private void LoadTasks(DateOnly selectedDate) {
            using var db = App.CreateDbContext();
            var tasks = db.Tasks
                .Where(t => t.Date == selectedDate)
                .OrderBy(t => t.Time)
                .ToList();

            Tasks.Clear();

            foreach (var task in tasks) {
                Tasks.Add(task);
            }
        }
    }
}
