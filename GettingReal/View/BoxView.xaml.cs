using System.Windows.Controls;

namespace GettingReal.View;
using GettingReal.ViewModel;
/// <summary>
/// Interaction logic for BoxView.xaml
/// </summary>
public partial class BoxView : Page
{
    public BoxView()
    {
        InitializeComponent();
        DataContext = new BoxViewModel();
    }
}
