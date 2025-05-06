using System.Windows;

namespace GettingReal;
using GettingReal.View;
/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
    }

    #region Menu bar
    // File Menu
    private void Exit_Click(object sender, RoutedEventArgs e)
    {
        Application.Current.Shutdown();
    }

    // Vis Menu
    private void Show_Box_Click(object sender, RoutedEventArgs e)
    {
        MainFrame.Navigate(new BoxView());
    }

    private void Show_Material_Click(object sender, RoutedEventArgs e)
    {
        MessageBox.Show("Viser Materiale");
    }

    private void Show_Activity_Click(object sender, RoutedEventArgs e)
    {
        MessageBox.Show("Viser Aktivitet");
    }

    private void Show_Workshop_Click(object sender, RoutedEventArgs e)
    {
        MessageBox.Show("Viser Workshop");
    }

    #endregion
}