using PersonalCalendar.ViewModels;
using System.Windows;

namespace PersonalCalendar.Views {
    /// <summary>
    /// Interaction logic for CalendarView.xaml
    /// </summary>
    public partial class CalendarView : Window {
        public CalendarView() {
            InitializeComponent();
            DataContext = new CalendarViewModel();
        }
    }
}
