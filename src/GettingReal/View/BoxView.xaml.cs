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
    private void ListBoxes_PreviewKeyDown(object sender, System.Windows.Input.KeyEventArgs e)
    {
        if (e.Key == System.Windows.Input.Key.Escape)
        {
            ListBoxes.SelectedItem = null;
            e.Handled = true; // Prevent further processing of the Escape key
        }
    }
}
