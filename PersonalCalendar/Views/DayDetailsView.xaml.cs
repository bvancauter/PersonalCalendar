using PersonalCalendar.Models;
using PersonalCalendar.Services;
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
            var service = new EventService();
            var events = service.GetByDate(selectedDate);

            Events.Clear();
            foreach (var calendarEvent in events) {
                Events.Add(calendarEvent);
            }
        }

        private void LoadTasks(DateOnly selectedDate) {
            var service = new TaskService();
            var tasks = service.GetByDate(selectedDate);

            Tasks.Clear();
            foreach (var task in tasks) {
                Tasks.Add(task);
            }
        }
    }
}
