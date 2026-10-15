namespace PersonalCalendar.Models {
    public class Task {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public DateOnly Date { get; set; }
        public TimeOnly? Time { get; set; }
        public bool IsCompleted { get; set; } = false;
        public Guid? CategoryId { get; set; }
        public RecurrenceFrequency Frequency { get; set; } = RecurrenceFrequency.None;
    }
}
