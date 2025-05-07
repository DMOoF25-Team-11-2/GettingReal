using System.Windows.Controls;

namespace GettingReal.View;

using GettingReal.ViewModel;

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
