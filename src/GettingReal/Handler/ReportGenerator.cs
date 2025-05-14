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

    }

}
