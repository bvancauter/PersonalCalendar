using PersonalCalendar.Commands;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace PersonalCalendar.ViewModels {
    public class CalendarViewModel : INotifyPropertyChanged {
        private DateTime _currentMonth = new(DateTime.Today.Year, DateTime.Today.Month, 1);

        public DateTime CurrentMonth {
            get => _currentMonth;
            set {
                if (_currentMonth != value) {
                    _currentMonth = value;
                    OnPropertyChanged();
                    OnPropertyChanged(nameof(CurrentMonthName));
                }
            }
        }

        public string CurrentMonthName => _currentMonth.ToString("MMMM yyyy");

        public ICommand PreviousMonthCommand { get; }
        public ICommand NextMonthCommand { get; }

        public CalendarViewModel() {
            PreviousMonthCommand = new RelayCommand(_ => CurrentMonth = CurrentMonth.AddMonths(-1));
            NextMonthCommand = new RelayCommand(_ => CurrentMonth = CurrentMonth.AddMonths(1));
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        private void OnPropertyChanged([CallerMemberName] string? propertyName = null) {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
