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

        public bool IsEventSelected {
            get => _isEventSelected;
            set {
                if (_isEventSelected != value) {
                    _isEventSelected = value;
                    OnPropertyChanged();
                    OnPropertyChanged(nameof(IsEventSelected));
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
                if (!IsAllDay) {
                    if (StartTime.HasValue != EndTime.HasValue) {
                        ErrorMessage = "Both start and end times must be provided for non-all-day events.";
                        return;
                    }
                    if (StartTime.HasValue && EndTime.HasValue && StartTime.Value >= EndTime.Value) {
                        ErrorMessage = "Start time must be before end time.";
                        return;
                    }
                }

                CalendarEvent calendarEvent = new CalendarEvent {
                    Title = Title.Trim(),
                    Description = Description,
                    Date = Date,
                    StartTime = IsAllDay ? null : StartTime,
                    EndTime = IsAllDay ? null : EndTime
                };

                _eventService.Create(calendarEvent);
            } else {
                CalendarTask calendarTask = new CalendarTask {
                    Title = Title.Trim(),
                    Description = Description,
                    Date = Date,
                    Time = TaskTime
                };
                _taskService.Create(calendarTask);
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        private void OnPropertyChanged([CallerMemberName] string? propertyName = null) {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
