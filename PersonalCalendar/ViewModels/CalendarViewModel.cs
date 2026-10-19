using PersonalCalendar.Commands;
using System.Collections.ObjectModel;
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
                    GenerateDays();
                }
            }
        }

        public string CurrentMonthName => _currentMonth.ToString("MMMM yyyy");

        public ObservableCollection<CalendarDayViewModel> Days { get; } = new();

        public ICommand PreviousMonthCommand { get; }
        public ICommand NextMonthCommand { get; }

        public CalendarViewModel() {
            PreviousMonthCommand = new RelayCommand(_ => CurrentMonth = CurrentMonth.AddMonths(-1));
            NextMonthCommand = new RelayCommand(_ => CurrentMonth = CurrentMonth.AddMonths(1));
            GenerateDays();
        }

        private void GenerateDays() {
            Days.Clear();

            var firstDayOfMonth = new DateOnly(CurrentMonth.Year, CurrentMonth.Month, 1);

            int offset = ((int) firstDayOfMonth.DayOfWeek + 6) % 7;

            var firstDayToDisplay = firstDayOfMonth.AddDays(-offset);

            for (int i = 0; i < 42; i++) {
                var date = firstDayToDisplay.AddDays(i);
                Days.Add(new CalendarDayViewModel(date, CurrentMonth.Month, CurrentMonth.Year));
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        private void OnPropertyChanged([CallerMemberName] string? propertyName = null) {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
