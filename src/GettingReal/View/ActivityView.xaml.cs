using System.Windows.Controls;

namespace GettingReal.View;

using GettingReal.ViewModel;


/// <summary>
/// Interaction logic for ActivityView.xaml
/// </summary>
public partial class ActivityView : Page
{
    public ActivityView()
    {
        InitializeComponent();
        DataContext = new ActivityViewModel();
    }
    private void ListViewActivities_PreviewKeyDown(object sender, System.Windows.Input.KeyEventArgs e)
    {
        if (e.Key == System.Windows.Input.Key.Escape)
        {
            ListViewActivities.SelectedItem = null;
            e.Handled = true; // Prevent further processing of the Escape key
        }
    }
}
