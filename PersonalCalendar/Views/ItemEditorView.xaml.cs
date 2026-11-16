using PersonalCalendar.Models;
using PersonalCalendar.ViewModels;
using System.Windows;

namespace PersonalCalendar.Views {
    /// <summary>
    /// Interaction logic for CreateItemView.xaml
    /// </summary>
    public partial class CreateItemView : Window {
        public CreateItemView(DateOnly selectedDate) {
            InitializeComponent();
            var viewModel = new ItemEditorViewModel(selectedDate);
            viewModel.RequestClose += () => Close();
            DataContext = viewModel;
        }

        public CreateItemView(CalendarEvent calendarEvent) {
            InitializeComponent();
            var viewModel = new ItemEditorViewModel(calendarEvent);
            viewModel.RequestClose += () => Close();
            DataContext = viewModel;
        }

        public CreateItemView(CalendarTask calendarTask) {
            InitializeComponent();
            var viewModel = new ItemEditorViewModel(calendarTask);
            viewModel.RequestClose += () => Close();
            DataContext = viewModel;
        }
    }
}
