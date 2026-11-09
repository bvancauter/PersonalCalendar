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

        private bool _isEventSelected = true;
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
                    OnPropertyChanged(nameof(IsTaskSelected));
                    OnPropertyChanged(nameof(ShowEventTimes));
                    OnPropertyChanged(nameof(ShowTaskTime));
                }
            }
        }

        public bool IsTaskSelected {
            get => !_isEventSelected;
            set {
                if (value) {
                    IsEventSelected = false;
                }
            }
        }

        public string Title {
            get => _title;
            set {
                if (_title != value) {
                    _title = value;
                    OnPropertyChanged();
                    CommandManager.InvalidateRequerySuggested();
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
                ErrorMessage = "Le titre est obligatoire.";
                return;
            }

            if (IsEventSelected) {
                TimeOnly? startTime = null;
                TimeOnly? endTime = null;

                if (!IsAllDay) {
                    if (!string.IsNullOrWhiteSpace(StartTimeText)) {
                        if (!TimeOnly.TryParse(StartTimeText, out var parsedStartTime)) {
                            ErrorMessage = "Format de l'heure de début invalide. Utilise HH:mm.";
                            return;
                        }

                        startTime = parsedStartTime;
                    }

                    if (!string.IsNullOrWhiteSpace(EndTimeText)) {
                        if (!TimeOnly.TryParse(EndTimeText, out var parsedEndTime)) {
                            ErrorMessage = "Format de l'heure de fin invalide. Utilise HH:mm.";
                            return;
                        }

                        endTime = parsedEndTime;
                    }

                    if (startTime.HasValue != endTime.HasValue) {
                        ErrorMessage = "Renseigne les deux horaires ou laisse-les vides.";
                        return;
                    }

                    if (startTime.HasValue && endTime <= startTime) {
                        ErrorMessage = "L'heure de fin doit être après l'heure de début.";
                        return;
                    }
                }

                var calendarEvent = new CalendarEvent {
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
                    if (!TimeOnly.TryParse(TaskTimeText, out var parsedTaskTime)) {
                        ErrorMessage = "Format de l'heure de la tâche invalide. Utilise HH:mm.";
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

        public event PropertyChangedEventHandler? PropertyChanged;

        private void OnPropertyChanged([CallerMemberName] string? propertyName = null) {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
