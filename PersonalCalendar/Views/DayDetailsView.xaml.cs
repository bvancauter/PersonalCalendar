using PersonalCalendar.Models;
using System.Collections.ObjectModel;
using System.Windows;

namespace PersonalCalendar.Views {
    /// <summary>
    /// Interaction logic for DayDetailsView.xaml
    /// </summary>
    public partial class DayDetailsView : Window {
        public ObservableCollection<CalendarEvent> Events { get; } = new();

        public DayDetailsView(DateOnly selectedDate) {
            InitializeComponent();
            DataContext = this;

            SelectedDateTextBlock.Text = selectedDate.ToString("dddd, d MMMM yyyy");

            LoadEvents(selectedDate);
        }

        private void LoadEvents(DateOnly selectedDate) {
            var db = App.CreateDbContext();

            var events = db.Events
                .Where(e => e.Date == selectedDate)
                .OrderBy(e => e.StartTime)
                .ToList();

            Events.Clear();

            foreach (var calendarEvent in events) {
                Events.Add(calendarEvent);
            }
        }
    }
}
