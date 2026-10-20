namespace PersonalCalendar.ViewModels {
    public class CalendarDayViewModel {
        public DateOnly Date { get; }
        public bool IsCurrentMonth { get; }
        public bool IsToday => Date == DateOnly.FromDateTime(DateTime.Now);
        public int DayNumber => Date.Day;

        public CalendarDayViewModel(DateOnly date, int displayedMonth, int displayedYear) {
            Date = date;
            IsCurrentMonth = date.Month == displayedMonth && date.Year == displayedYear;
        }
    }
}
