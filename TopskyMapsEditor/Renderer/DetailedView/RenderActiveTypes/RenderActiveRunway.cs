using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using TopskyMapsEditor;
using static Functions.AddFunctions;

namespace Renderer
{
    internal class RenderActiveRunway
    {
        internal static Grid ActiveRunway(Active activeItem)
        {
            Grid grid = new();

            grid.RowDefinitions.Add(AddRow(1));
            grid.RowDefinitions.Add(AddRow(1));
            grid.RowDefinitions.Add(AddRow(1));
            grid.RowDefinitions.Add(AddRow(1));

            //TODO Refactor
            
            foreach (var runway in activeItem.ActiveRunway.NotArrivalRunways)
            {
                grid.RowDefinitions.Add(AddRow(1));
            }
            foreach (var runway in activeItem.ActiveRunway.DepartureRunways)
            {
                grid.RowDefinitions.Add(AddRow(1));
            }
            foreach (var runway in activeItem.ActiveRunway.NotDepartureRunways)
            {
                grid.RowDefinitions.Add(AddRow(1));
            }

            if(activeItem.ActiveRunway.ArrivalRunways != null)
            {
                

                TextBlock activeRunwaysText = AddTextBlock("Active Runways");
                grid.Children.Add(activeRunwaysText);
                Grid.SetRow(activeRunwaysText, 0);

                TextBlock activeArrRunwaysText = AddTextBlock("Active for Arrival");
                grid.Children.Add(activeArrRunwaysText);
                Grid.SetRow(activeArrRunwaysText, 1);

                Button addAreaButton =
                    new()
                    {
                        Foreground = new SolidColorBrush(Colors.White),
                        Content = "Add new",
                        Background = new SolidColorBrush(
                            System.Windows.Media.Color.FromRgb(35, 50, 68)
                        ),
                        BorderThickness = new Thickness(0),
                        //TODO event handler
                        //addAreaButton.Click += AddNewButton_Click;
                        Margin = new Thickness(5)
                    };

                foreach (var runway in activeItem.ActiveRunway.ArrivalRunways)
                {
                    grid.RowDefinitions.Add(AddRow(1));

                    //TODO Design
                    //TextBox 
                }

                grid.Children.Add(addAreaButton);
                Grid.SetRow(addAreaButton, 2);
            }

            return grid;
        }

    }
}