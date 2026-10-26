using PersonalCalendar.Models;
using PersonalCalendar.Services;
using System.Collections.ObjectModel;

namespace PersonalCalendar.ViewModels {
    public class DayDetailsViewModel {
        private readonly EventService _eventService;
        private readonly TaskService _taskService;

        public ObservableCollection<CalendarEvent> Events { get; } = new();
        public ObservableCollection<CalendarTask> Tasks { get; } = new();
        public DateOnly SelectedDate { get; }
        public string SelectedDateText => SelectedDate.ToString("dddd, d MMMM yyyy");

        public DayDetailsViewModel(DateOnly selectedDate) {
            SelectedDate = selectedDate;

            _eventService = new EventService();
            _taskService = new TaskService();

            LoadEvents();
            LoadTasks();
        }

        private void LoadEvents() {
            var events = _eventService.GetByDate(SelectedDate);
            Events.Clear();
            foreach (var calendarEvent in events) {
                Events.Add(calendarEvent);
            }
        }

        private void LoadTasks() {
            var tasks = _taskService.GetByDate(SelectedDate);
            Tasks.Clear();
            foreach (var calendarTask in tasks) {
                Tasks.Add(calendarTask);
            }
        }
    }
}
