using System.Windows.Controls;
using GettingReal.ViewModel;

namespace GettingReal.View;
/// <summary>
/// Interaction logic for WorkshopView.xaml
/// </summary>
public partial class WorkshopView : Page
{
    public WorkshopView()
    {
        InitializeComponent();
        DataContext = new WorkshopViewModel();
    }
}
