using System.Windows.Controls;

namespace GettingReal.View;

using GettingReal.ViewModel;


/// <summary>
/// Interaction logic for MaterialView.xaml
/// </summary>
public partial class MaterialView : Page
{
    public MaterialView()
    {
        InitializeComponent();
        DataContext = new MaterialViewModel();
    }
    private void ListViewMaterials_PreviewKeyDown(object sender, System.Windows.Input.KeyEventArgs e)
    {
        if (e.Key == System.Windows.Input.Key.Escape)
        {
            ListViewMaterials.SelectedItem = null;
            e.Handled = true; // Prevent further processing of the Escape key
        }
    }
}
