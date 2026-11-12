using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace PersonalCalendar.ViewModels {
    public class CalendarDayViewModel : INotifyPropertyChanged {
        public DateOnly Date { get; }
        public bool IsCurrentMonth { get; }
        public bool IsToday => Date == DateOnly.FromDateTime(DateTime.Now);
        public int DayNumber => Date.Day;
        private bool _isSelected;
        public bool IsSelected {
            get => _isSelected;
            set {
                if (_isSelected != value) {
                    _isSelected = value;
                    OnPropertyChanged();
                }
            }
        }

        private bool _hasEvents;
        public bool HasEvents {
            get => _hasEvents;
            set {
                if (_hasEvents != value) {
                    _hasEvents = value;
                    OnPropertyChanged();
                }
            }
        }

        private bool _hasTasks;
        public bool HasTasks {
            get => _hasTasks;
            set {
                if (_hasTasks != value) {
                    _hasTasks = value;
                    OnPropertyChanged();
                }
            }
        }

        public CalendarDayViewModel(DateOnly date, int displayedMonth, int displayedYear) {
            Date = date;
            IsCurrentMonth = date.Month == displayedMonth && date.Year == displayedYear;
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string? propertyName = null) {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
