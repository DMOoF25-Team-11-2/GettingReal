using System.Text;
using System.Windows.Controls;

namespace GettingReal.Handler;
using System.IO;

using System.Windows;
using System.Windows.Markup;
using GettingReal.Model;

/// <summary>
/// Generates reports in XAML format.
/// Print XAML content to the default printer.
/// </summary>
public class ReportGenerator //generates reports in Xaml-format
{
    private static List<int> _col = []; ///> List of column widths
    private static int _colCount = 0; ///> Current column index

    /// <summary>
    /// Generates a report of the boxes needed for a workshop in XAML format.
    /// </summary>
    /// <param name="workshop"></param>
    /// <returns>string containing xaml</returns>
    /// <remarks>
    /// Change return to a XAML container containing the XAML content you want to print.
    /// </remarks>
    public static string ReportBoxesNeededForWorkshop(Workshop workshop)
    {
        // Create a list of column widths
        _col.Add(30);
        _col.Add(200);
        _col.Add(200);

        // Initialize a StringBuilder to build the XAML string
        StringBuilder xamlBuilder = new StringBuilder();

        _colCount = 0;

        // Header for report
        xamlBuilder.AppendLine("<StackPanel Orientation=\"Vertical\" xmlns='http://schemas.microsoft.com/winfx/2006/xaml/presentation' Margin=\"20\" >");
        xamlBuilder.AppendLine("<StackPanel Orientation=\"Horizontal\">");
        xamlBuilder.AppendLine($"   <TextBlock Text='' FontSize='16' FontWeight='Bold' Width=\"{_col[_colCount++]}\" />");
        xamlBuilder.AppendLine($"   <TextBlock Text='Kassenavn' FontSize='16' FontWeight='Bold' Width=\"{_col[_colCount++]}\" />");
        xamlBuilder.AppendLine($"   <TextBlock Text='Beskrivelse' FontSize='16' FontWeight='Bold' Width=\"{_col[_colCount++]}\" />");
        xamlBuilder.AppendLine("</StackPanel>");

        // Content for report
        // Iterate through the boxes needed for the workshop
        IEnumerable<Box> boxes = workshop.GetBoxesForWorkshop();
        foreach (Box box in boxes)
        {
            _colCount = 0;
            xamlBuilder.AppendLine("<StackPanel Orientation='Horizontal'>");
            xamlBuilder.AppendLine($"   <StackPanel Margin='0,0,0,0' Width='{_col[_colCount++]}'>");
            xamlBuilder.AppendLine($"       <Border BorderBrush='Black' BorderThickness='1' Margin='0,0,0,0' Width='20' Height='20' />");
            xamlBuilder.AppendLine("    </StackPanel>");
            xamlBuilder.AppendLine($"   <TextBlock Text='{box.Name}' FontSize='16' Width='{_col[_colCount++]}' />");
            xamlBuilder.AppendLine($"   <TextBlock Text='{box.Description}' FontSize='16' Foreground='Gray' Width='{_col[_colCount++]}'/>");
            xamlBuilder.AppendLine("</StackPanel>");
        }

        xamlBuilder.AppendLine("</StackPanel>");

        return xamlBuilder.ToString();
    }

    /// <summary>
    /// Generates a report of the box inventory in XAML format.
    /// </summary>
    /// <param name="box"></param>
    /// <returns>string containing xaml</returns>
    /// <remarks>
    /// Change return to a XAML container containing the XAML content you want to print.
    /// </remarks>
    public static string ReportBoxInventory(Box box)
    {
        // Create a list of column widths
        _col.Add(200);
        _col.Add(200);

        _colCount = 0;

        // Header for report
        StringBuilder xamlBuilder = new StringBuilder();
        xamlBuilder.AppendLine("<StackPanel Orientation=\"Vertical\" xmlns='http://schemas.microsoft.com/winfx/2006/xaml/presentation' Margin=\"20\" >");
        xamlBuilder.AppendLine("<StackPanel Orientation=\"Horizontal\">");
        xamlBuilder.AppendLine($"   <TextBlock Text='{"Kassenavn"}' FontSize='16' FontWeight='Bold' Width=\"{_col[_colCount++]}\" />");
        xamlBuilder.AppendLine($"   <TextBlock Text='{"Beskrivelse"}' FontSize='16' FontWeight='Bold' Width=\"{_col[_colCount++]}\" />");
        xamlBuilder.AppendLine("</StackPanel>");

        // Content for report
        // Iterate through the materials in the box
        IEnumerable<Material> materials = new MaterialRepository().GetAll();
        foreach (Guid guid in box.MaterialGuids)
        {
            _colCount = 0;
            var material = new MaterialRepository().Get(guid);
            if (material != null)
            {
                xamlBuilder.AppendLine("<StackPanel Orientation=\"Horizontal\">");
                xamlBuilder.AppendLine($"   <TextBlock Text='{material.Name}' FontSize='16' Width=\"{_col[_colCount++]}\" />");
                xamlBuilder.AppendLine($"   <TextBlock Text='{material.Description}' FontSize='16' Foreground='Gray' Width=\"{_col[_colCount++]}\" />");
                xamlBuilder.AppendLine("</StackPanel>");
            }
        }

        xamlBuilder.AppendLine("</StackPanel>");
        return xamlBuilder.ToString();
    }

    /// <summary>
    /// Prints the XAML string to the default printer.
    /// </summary>
    /// <param name="xamlString"></param>
    /// <remarks>
    /// Change the parameter to a XAML container containing the XAML content you want to print.
    /// </remarks>
    public static void Print(string xamlString)
    {
        Grid xamlContainer = new();

        StringReader stringReader = new(xamlString);
        System.Xml.XmlReader xmlReader = System.Xml.XmlReader.Create(stringReader);
        UIElement element = (UIElement)XamlReader.Load(xmlReader);

        // Assuming you have a container like a Grid or StackPanel
        xamlContainer.Children.Add(element);

        PrintDialog printDialog = new();
        if (printDialog.ShowDialog() == true)
        {
            printDialog.PrintVisual(xamlContainer, "Printing XAML Content");
        }
    }
}