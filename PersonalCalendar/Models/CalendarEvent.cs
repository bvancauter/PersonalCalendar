namespace PersonalCalendar.Models {
    public class CalendarEvent {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public DateOnly Date { get; set; }
        public TimeOnly? StartTime { get; set; }
        public TimeOnly? EndTime { get; set; }
        public Guid? CategoryId { get; set; }
        public RecurrenceFrequency Frequency { get; set; } = RecurrenceFrequency.None;
    }
}
