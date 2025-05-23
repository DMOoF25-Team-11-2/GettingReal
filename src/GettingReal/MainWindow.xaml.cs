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
        MainFrame.Navigate(new MaterialView());
    }

    private void Show_Activity_Click(object sender, RoutedEventArgs e)
    {
        MainFrame.Navigate(new ActivityView());
    }

    private void Show_Workshop_Click(object sender, RoutedEventArgs e)
    {
        MainFrame.Navigate(new WorkshopView());
    }

    // Rapport Menu
    private void RapportBoxesForWorkshop_Click(object sender, RoutedEventArgs e)
    {
        MainFrame.Navigate(new ReportForBoxesInWorkshopView());
    }

    private void RapportBoxInventory_Click(object sender, RoutedEventArgs e)
    {
        MainFrame.Navigate(new ReportForBoxInventoryView());
    }
    #endregion

    private void About_Click(object sender, RoutedEventArgs e)
    {
        MessageBox.Show("451 Grader\nVersion 1.0\n\nDeveloped by\nJens Tirsvad Nielsen - [jenstirsvad@gmail.com]\n" +
            "Benjamin Jon Leonhardt - [bjle71347@edu.ucl.dk]\n" +
            "Cecillie Skoven Møller - csmo71359@edu.ucl.dk\n" +
            "Michael Kragh - mikr71394@edu.ucl.dk"
            , "About Getting Real", MessageBoxButton.OK, MessageBoxImage.Information);
    }
}