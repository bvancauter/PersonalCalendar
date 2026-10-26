using PersonalCalendar.ViewModels;
using System.Windows;

namespace PersonalCalendar.Views {
    /// <summary>
    /// Interaction logic for DayDetailsView.xaml
    /// </summary>
    public partial class DayDetailsView : Window {
        public DayDetailsView(DateOnly selectedDate) {
            InitializeComponent();
            DataContext = new DayDetailsViewModel(selectedDate);
        }
    }
}
