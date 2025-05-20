using System.Windows.Controls;

namespace GettingReal.View
{
    /// <summary>
    /// Interaction logic for ReportForBoxInventoryView.xaml
    /// </summary>
    public partial class ReportForBoxInventoryView : Page
    {
        public ReportForBoxInventoryView()
        {
            InitializeComponent();
            DataContext = new ViewModel.ReportForBoxInventoryViewModel();
        }
    }
}
