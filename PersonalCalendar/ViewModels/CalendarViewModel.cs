using PersonalCalendar.Commands;
using PersonalCalendar.Services;
using PersonalCalendar.Views;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;

namespace PersonalCalendar.ViewModels {
    public class CalendarViewModel : INotifyPropertyChanged {
        private readonly EventService _eventService = new();
        private readonly TaskService _taskService = new();

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
        public ICommand ViewCreateItemCommand { get; }

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
            ViewCreateItemCommand = new RelayCommand(_ => {
                var selectedDate = SelectedDay?.Date ?? DateOnly.FromDateTime(DateTime.Today);
                var window = new CreateItemView(selectedDate);
                window.Owner = Application.Current.MainWindow;
                window.ShowDialog();
            });

            GenerateDays();
        }

        private void GenerateDays() {
            Days.Clear();
            SelectedDay = null;

            var firstDayOfMonth = new DateOnly(CurrentMonth.Year, CurrentMonth.Month, 1);

            int offset = ((int) firstDayOfMonth.DayOfWeek + 6) % 7;

            var firstDayToDisplay = firstDayOfMonth.AddDays(-offset);

            var monthEvents = _eventService.GetByMonth(CurrentMonth.Year, CurrentMonth.Month);
            var monthTasks = _taskService.GetByMonth(CurrentMonth.Year, CurrentMonth.Month);

            for (int i = 0; i < 42; i++) {
                var date = firstDayToDisplay.AddDays(i);
                var day = new CalendarDayViewModel(date, CurrentMonth.Month, CurrentMonth.Year) {
                    HasEvents = monthEvents.Any(e => e.Date == date),
                    HasTasks = monthTasks.Any(t => t.Date == date)
                };
                Days.Add(day);
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        private void OnPropertyChanged([CallerMemberName] string? propertyName = null) {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
