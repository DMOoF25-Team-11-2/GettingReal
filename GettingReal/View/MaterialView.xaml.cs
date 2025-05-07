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
}
