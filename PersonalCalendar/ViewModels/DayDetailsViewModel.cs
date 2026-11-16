using PersonalCalendar.Commands;
using PersonalCalendar.Models;
using PersonalCalendar.Services;
using PersonalCalendar.Views;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;

namespace PersonalCalendar.ViewModels {
    public class DayDetailsViewModel {
        private readonly EventService _eventService;
        private readonly TaskService _taskService;

        public ObservableCollection<CalendarEvent> Events { get; } = new();
        public ObservableCollection<CalendarTask> Tasks { get; } = new();
        public DateOnly SelectedDate { get; }
        public string SelectedDateText => SelectedDate.ToString("dddd, d MMMM yyyy");

        public ICommand EditEventCommand { get; }
        public ICommand EditTaskCommand { get; }

        public DayDetailsViewModel(DateOnly selectedDate) {
            SelectedDate = selectedDate;

            _eventService = new EventService();
            _taskService = new TaskService();

            EditEventCommand = new RelayCommand(
                item => EditEvent(item as CalendarEvent),
                item => item is CalendarEvent);

            EditTaskCommand = new RelayCommand(
                item => EditTask(item as CalendarTask),
                item => item is CalendarTask);

            LoadEvents();
            LoadTasks();
        }

        private void EditEvent(CalendarEvent? calendarEvent) {
            if (calendarEvent is null) {
                return;
            }

            var window = new CreateItemView(calendarEvent) { Owner = Application.Current.MainWindow };
            window.ShowDialog();

            LoadEvents();
            LoadTasks();
        }

        private void EditTask(CalendarTask? calendarTask) {
            if (calendarTask is null) {
                return;
            }

            var window = new CreateItemView(calendarTask) { Owner = Application.Current.MainWindow };
            window.ShowDialog();

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
