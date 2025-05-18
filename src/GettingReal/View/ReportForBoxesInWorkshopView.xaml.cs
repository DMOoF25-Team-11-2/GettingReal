using System.Windows.Controls;
using GettingReal.ViewModel;

namespace GettingReal.View
{
    /// <summary>
    /// Interaction logic for ReportForBoxesInWorkshopView.xaml
    /// </summary>
    public partial class ReportForBoxesInWorkshopView : Page
    {
        public ReportForBoxesInWorkshopView()
        {
            InitializeComponent();
            DataContext = new ReportForBoxesInWorkshopViewModel();

        }
    }
}
