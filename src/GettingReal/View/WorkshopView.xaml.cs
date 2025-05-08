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

    private void ListViewWorkshops_PreviewKeyDown(object sender, System.Windows.Input.KeyEventArgs e)
    {
        if (e.Key == System.Windows.Input.Key.Escape)
        {
            ListViewWorkshops.SelectedItem = null;
            e.Handled = true; // Prevent further processing of the Escape key
        }
    }

}
