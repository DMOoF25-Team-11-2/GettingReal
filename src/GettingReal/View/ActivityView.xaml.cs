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
}
