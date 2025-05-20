using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using GettingReal.Model;


namespace GettingReal.Handler;
using System.IO;

using System.Windows;
using System.Windows.Markup;
using GettingReal.Model;
public class ReportGenerator //generates reports in Xaml-format
{
    public static string ReportBoxesNeededForWorkshop(Workshop workshop)
    {
        int col1 = 200;
        int col2 = 200;

        StringBuilder xamlBuilder = new StringBuilder();

        xamlBuilder.AppendLine("<StackPanel Orientation=\"Vertical\" xmlns='http://schemas.microsoft.com/winfx/2006/xaml/presentation' Margin=\"20\" >");
        xamlBuilder.AppendLine("<StackPanel Orientation=\"Horizontal\">");
        xamlBuilder.AppendLine($"   <TextBlock Text='{"Kassenavn"}' FontSize='16' FontWeight='Bold' Width=\"{col1}\" />");
        xamlBuilder.AppendLine($"   <TextBlock Text='{"Beskrivelse"}' FontSize='16' FontWeight='Bold' Width=\"{col2}\" />");
        xamlBuilder.AppendLine("</StackPanel>");

        IEnumerable<Box> boxes = workshop.GetBoxesForWorkshop();
        foreach (Box box in boxes)

        {
            xamlBuilder.AppendLine("<StackPanel Orientation=\"Horizontal\">");
            xamlBuilder.AppendLine($"   <TextBlock Text='{box.Name}' FontSize='16' Width=\"{col1}\" />");
            xamlBuilder.AppendLine($"   <TextBlock Text='{box.Description}' FontSize='16' Foreground='Gray' Width=\"{col2}\" />");
            xamlBuilder.AppendLine("</StackPanel>");
        }


        xamlBuilder.AppendLine("</StackPanel>");

        return xamlBuilder.ToString();

    }
 
    public static string ReportBoxInventory(Box box)
    {
        int col1 = 200;
        int col2 = 200;
        StringBuilder xamlBuilder = new StringBuilder();
        xamlBuilder.AppendLine("<StackPanel Orientation=\"Vertical\" xmlns='http://schemas.microsoft.com/winfx/2006/xaml/presentation' Margin=\"20\" >");
        xamlBuilder.AppendLine("<StackPanel Orientation=\"Horizontal\">");
        xamlBuilder.AppendLine($"   <TextBlock Text='{"Kassenavn"}' FontSize='16' FontWeight='Bold' Width=\"{col1}\" />");
        xamlBuilder.AppendLine($"   <TextBlock Text='{"Beskrivelse"}' FontSize='16' FontWeight='Bold' Width=\"{col2}\" />");
        xamlBuilder.AppendLine("</StackPanel>");

        IEnumerable<Material> materials = new MaterialRepository().GetAll();
        foreach (Guid guid in box.MaterialGuids)
        {
            var material = new MaterialRepository().Get(guid);
            if (material != null)
            {
                xamlBuilder.AppendLine("<StackPanel Orientation=\"Horizontal\">");
                xamlBuilder.AppendLine($"   <TextBlock Text='{material.Name}' FontSize='16' Width=\"{col1}\" />");
                xamlBuilder.AppendLine($"   <TextBlock Text='{material.Description}' FontSize='16' Foreground='Gray' Width=\"{col2}\" />");
                xamlBuilder.AppendLine("</StackPanel>");
            }
        }

        xamlBuilder.AppendLine("</StackPanel>");
        return xamlBuilder.ToString();
    }

    public static void Print(string xamlString)
    {
        Grid xamlContainer = new Grid();

        StringReader stringReader = new StringReader(xamlString);
        System.Xml.XmlReader xmlReader = System.Xml.XmlReader.Create(stringReader);
        UIElement element = (UIElement)XamlReader.Load(xmlReader);

        // Assuming you have a container like a Grid or StackPanel
        xamlContainer.Children.Add(element);

        PrintDialog printDialog = new PrintDialog();
        if (printDialog.ShowDialog() == true)
        {
            printDialog.PrintVisual(xamlContainer, "Printing XAML Content");
        }

    }
  }