using PersonalCalendar.ViewModels;
using System.Windows;

namespace PersonalCalendar.Views {
    /// <summary>
    /// Interaction logic for CreateItemView.xaml
    /// </summary>
    public partial class CreateItemView : Window {
        public CreateItemView(DateOnly selectedDate) {
            InitializeComponent();
            DataContext = new CreateItemViewModel(selectedDate);
        }
    }
}
