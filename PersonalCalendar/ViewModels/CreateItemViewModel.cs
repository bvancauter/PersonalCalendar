using PersonalCalendar.Commands;
using PersonalCalendar.Models;
using PersonalCalendar.Services;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace PersonalCalendar.ViewModels {
    public class CreateItemViewModel : INotifyPropertyChanged {
        private readonly EventService _eventService = new EventService();
        private readonly TaskService _taskService = new TaskService();

        private bool _isEventSelected;
        private string _title = string.Empty;
        private string? _description;
        private DateOnly _date;
        private bool _isAllDay = true;

        private TimeOnly? _startTime;
        private TimeOnly? _endTime;
        private TimeOnly? _taskTime;
        private string? _errorMessage;

        private string _startTimeText = string.Empty;
        private string _endTimeText = string.Empty;
        private string _taskTimeText = string.Empty;

        public bool IsEventSelected {
            get => _isEventSelected;
            set {
                if (_isEventSelected != value) {
                    _isEventSelected = value;
                    OnPropertyChanged();
                    OnPropertyChanged(nameof(IsEventSelected));
                    OnPropertyChanged(nameof(ShowEventTimes));
                    OnPropertyChanged(nameof(ShowTaskTime));
                }
            }
        }

        public bool IsTaskSelected {
            get => !_isEventSelected;
            set {
                if (_isEventSelected == value) {
                    _isEventSelected = !value;
                    OnPropertyChanged();
                    OnPropertyChanged(nameof(IsTaskSelected));
                }
            }
        }

        public string Title {
            get => _title;
            set {
                if (_title != value) {
                    _title = value;
                    OnPropertyChanged();
                }
            }
        }

        public string? Description {
            get => _description;
            set {
                if (_description != value) {
                    _description = value;
                    OnPropertyChanged();
                }
            }
        }

        public DateOnly Date {
            get => _date;
            set {
                if (_date != value) {
                    _date = value;
                    OnPropertyChanged();
                }
            }
        }

        public bool IsAllDay {
            get => _isAllDay;
            set {
                if (_isAllDay != value) {
                    _isAllDay = value;
                    OnPropertyChanged();
                    OnPropertyChanged(nameof(ShowEventTimes));
                    OnPropertyChanged(nameof(ShowTaskTime));
                }
            }
        }

        public TimeOnly? StartTime {
            get => _startTime;
            set {
                if (_startTime != value) {
                    _startTime = value;
                    OnPropertyChanged();
                }
            }
        }

        public TimeOnly? EndTime {
            get => _endTime;
            set {
                if (_endTime != value) {
                    _endTime = value;
                    OnPropertyChanged();
                }
            }
        }

        public TimeOnly? TaskTime {
            get => _taskTime;
            set {
                if (_taskTime != value) {
                    _taskTime = value;
                    OnPropertyChanged();
                }
            }
        }

        public string? ErrorMessage {
            get => _errorMessage;
            set {
                if (_errorMessage != value) {
                    _errorMessage = value;
                    OnPropertyChanged();
                }
            }
        }

        public string StartTimeText {
            get => _startTimeText;
            set {
                if (_startTimeText != value) {
                    _startTimeText = value;
                    OnPropertyChanged();
                }
            }
        }

        public string EndTimeText {
            get => _endTimeText;
            set {
                if (_endTimeText != value) {
                    _endTimeText = value;
                    OnPropertyChanged();
                }
            }
        }

        public string TaskTimeText {
            get => _taskTimeText;
            set {
                if (_taskTimeText != value) {
                    _taskTimeText = value;
                    OnPropertyChanged();
                }
            }
        }

        public bool ShowEventTimes => IsEventSelected && !IsAllDay;
        public bool ShowTaskTime => IsTaskSelected && !IsAllDay;

        public ICommand CreateItemCommand { get; }

        public CreateItemViewModel(DateOnly selectedDate) {
            Date = selectedDate;
            CreateItemCommand = new RelayCommand(
                _ => CreateItem(),
                _ => !string.IsNullOrWhiteSpace(Title));
        }

        private void CreateItem() {
            ErrorMessage = null;
            if (string.IsNullOrWhiteSpace(Title)) {
                ErrorMessage = "Title is required.";
                return;
            }
            if (IsEventSelected) {
                TimeOnly? startTime = null;
                TimeOnly? endTime = null;
                if (!IsAllDay) {
                    if (!string.IsNullOrWhiteSpace(StartTimeText)) {
                        if (!TimeOnly.TryParse(StartTimeText, out TimeOnly parsedStartTime)) {
                            ErrorMessage = "Invalid start time format. Use HH:mm.";
                            return;
                        }
                        startTime = parsedStartTime;
                    }

                    if (!string.IsNullOrWhiteSpace(EndTimeText)) {
                        if (!TimeOnly.TryParse(EndTimeText, out TimeOnly parsedEndTime)) {
                            ErrorMessage = "Invalid end time format. Use HH:mm.";
                            return;
                        }
                        endTime = parsedEndTime;
                    }

                    if (startTime.HasValue != endTime.HasValue) {
                        ErrorMessage = "Both start and end times must be provided or both must be empty.";
                        return;
                    }

                    if (startTime.HasValue && endTime.HasValue && startTime.Value >= endTime.Value) {
                        ErrorMessage = "Start time cannot be later than end time.";
                        return;
                    }

                    CalendarEvent calendarEvent = new CalendarEvent {
                        Title = Title.Trim(),
                        Description = Description,
                        Date = Date,
                        StartTime = startTime,
                        EndTime = endTime
                    };

                    _eventService.Create(calendarEvent);
                } else {
                    TimeOnly? taskTime = null;
                    if (!IsAllDay && !string.IsNullOrWhiteSpace(TaskTimeText)) {
                        if (!TimeOnly.TryParse(TaskTimeText, out TimeOnly parsedTaskTime)) {
                            ErrorMessage = "Invalid task time format. Use HH:mm.";
                            return;
                        }
                        taskTime = parsedTaskTime;
                    }
                    var task = new CalendarTask {
                        Title = Title.Trim(),
                        Description = Description,
                        Date = Date,
                        Time = taskTime
                    };

                    _taskService.Create(task);
                }
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        private void OnPropertyChanged([CallerMemberName] string? propertyName = null) {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
