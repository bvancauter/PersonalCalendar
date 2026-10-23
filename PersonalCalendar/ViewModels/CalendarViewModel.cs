using PersonalCalendar.Commands;
using PersonalCalendar.Views;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
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

        private CalendarDayViewModel? _selectedDay;
        public CalendarDayViewModel? SelectedDay {
            get => _selectedDay;
            set {
                if (_selectedDay != value) {
                    if (_selectedDay is not null) {
                        _selectedDay.IsSelected = false;
                    }
                    _selectedDay = value;
                    if (_selectedDay is not null) {
                        _selectedDay.IsSelected = true;
                    }
                    OnPropertyChanged();
                    OnPropertyChanged(nameof(SelectedDate));
                    OnPropertyChanged(nameof(HasSelectedDay));
                }
            }
        }

        public bool HasSelectedDay => SelectedDay is not null;

        public DateOnly? SelectedDate => SelectedDay?.Date;

        public string CurrentMonthName => _currentMonth.ToString("MMMM yyyy");

        public ObservableCollection<CalendarDayViewModel> Days { get; } = new();

        public ICommand PreviousMonthCommand { get; }
        public ICommand NextMonthCommand { get; }
        public ICommand SelectDayCommand { get; }
        public ICommand ViewDayCommand { get; }

        public CalendarViewModel() {
            PreviousMonthCommand = new RelayCommand(_ => CurrentMonth = CurrentMonth.AddMonths(-1));
            NextMonthCommand = new RelayCommand(_ => CurrentMonth = CurrentMonth.AddMonths(1));
            SelectDayCommand = new RelayCommand(day => {
                if (day is CalendarDayViewModel selectedDay)
                    SelectedDay = selectedDay;
            });
            ViewDayCommand = new RelayCommand(_ => {
                if (SelectedDay is not null) {
                    var window = new DayDetailsView(SelectedDay.Date);
                    window.Owner = Application.Current.MainWindow;
                    window.ShowDialog();
                }
            }, _ => SelectedDay is not null);
            GenerateDays();
        }

        private void GenerateDays() {
            Days.Clear();
            SelectedDay = null;

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
