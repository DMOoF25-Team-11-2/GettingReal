using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GettingReal.Model;

namespace GettingReal.Handler
{
    public class ReportGenerator //generates reports in Xaml-format
    {
         public static string ReportMaterialsInBox(IEnumerable<Box> data)
         {
            StringBuilder xamlBuilder = new StringBuilder();
            MaterialRepository materialRepository = new();

            xamlBuilder.AppendLine("<StackPanel xmlns='http://schemas.microsoft.com/winfx/2006/xaml/presentation'>");

            foreach (var item in data)
            {
                var guids = item.MaterialGuids;
                if (guids != null)
                {
                    foreach (var guid in guids)

                    {
                        var material = materialRepository.Get(guid);

                        if (material != null)

                        {

                            xamlBuilder.AppendLine($"   <TextBlock Text='{material.Name}' FontSize='16' />");
                            xamlBuilder.AppendLine($"   <TextBlock Text='{material.Description}' FontSize='12' Foreground='Gray' />");

                        }
                    }

                }
            }  
            xamlBuilder.AppendLine("</StackPanel>");

            return xamlBuilder.ToString();

         }

        public static string ReportWorkshop2Dto3D(Workshop workshop)
        {
            StringBuilder xamlBuilder = new();
            BoxRepository boxRepository = new();

            var boxes = boxRepository.GetBoxesForWorkshop(workshop);

            xamlBuilder.AppendLine("<StackPanel xmlns='http://schemas.microsoft.com/winfx/2006/xaml/presentation'>");

            xamlBuilder.AppendLine($"   <TextBlock Text='2D to 3D Workshop: {workshop.Name}' FontSize='18' FontWeight='Bold' />");
            xamlBuilder.AppendLine($"   <TextBlock Text='Description: {workshop.Description}' FontSize='12' />");
            xamlBuilder.AppendLine("   <TextBlock Text='Boxes to bring:' FontSize='14' FontWeight='SemiBold' Margin='0,10,0,5' />");

            if (boxes != null && boxes.Any())
            {
                foreach (var box in boxes)
                {
                    xamlBuilder.AppendLine($"   <TextBlock Text='• {box.Name}' FontSize='12' />");
                }
            }
            else
            {
                xamlBuilder.AppendLine("   <TextBlock Text='No boxes assigned.' FontSize='12' FontStyle='Italic' />");
            }

            xamlBuilder.AppendLine("</StackPanel>");

            return xamlBuilder.ToString();
        }

        public static string ReportWorkshop3DPrint(Workshop workshop)
        {
            StringBuilder xamlBuilder = new();
            BoxRepository boxRepository = new();

            var boxes = boxRepository.GetBoxesForWorkshop(workshop);

            xamlBuilder.AppendLine("<StackPanel xmlns='http://schemas.microsoft.com/winfx/2006/xaml/presentation'>");

            xamlBuilder.AppendLine($"   <TextBlock Text='3D-print Workshop: {workshop.Name}' FontSize='18' FontWeight='Bold' />");
            xamlBuilder.AppendLine($"   <TextBlock Text='Description: {workshop.Description}' FontSize='12' />");
            xamlBuilder.AppendLine("   <TextBlock Text='Boxes to bring:' FontSize='14' FontWeight='SemiBold' Margin='0,10,0,5' />");

            if (boxes != null && boxes.Any())
            {
                foreach (var box in boxes)
                {
                    xamlBuilder.AppendLine($"   <TextBlock Text='• {box.Name}' FontSize='12' />");
                }
            }
            else
            {
                xamlBuilder.AppendLine("   <TextBlock Text='No boxes assigned.' FontSize='12' FontStyle='Italic' />");
            }

            xamlBuilder.AppendLine("</StackPanel>");

            return xamlBuilder.ToString();
        }
    }

}
