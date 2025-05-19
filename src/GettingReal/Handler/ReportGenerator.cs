using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using GettingReal.Model;

namespace GettingReal.Handler;

public class ReportGenerator //generates reports in Xaml-format
{
    public static string ReportMaterialsInBox(Workshop workshop)
    {
        StringBuilder xamlBuilder = new StringBuilder();
        MaterialRepository materials = new();
        BoxRepository boxes = new();

        xamlBuilder.AppendLine("<StackPanel xmlns='http://schemas.microsoft.com/winfx/2006/xaml/presentation'>");

        foreach (Guid guid in workshop.ActivityGuids)
        {
            Activity? activity = new ActivityRepository().Get(guid);

            foreach (Guid materialGuidInActivity in activity.MaterialGuids)
            {
                foreach (Box box in boxes.GetAll())
                {
                    foreach (Guid materialGuidInBox in box.MaterialGuids)
                    {
                        if (materialGuidInBox == materialGuidInActivity)
                        {
                            Material? material = materials.Get(materialGuidInActivity);
                            xamlBuilder.AppendLine($"   <TextBlock Text='{material.Name}' FontSize='16' />");
                            xamlBuilder.AppendLine($"   <TextBlock Text='{material.Description}' FontSize='12' Foreground='Gray' />");

                        }
                    }
                }
            }
            
        }  
        xamlBuilder.AppendLine("</StackPanel>");

        return xamlBuilder.ToString();



    }

}
