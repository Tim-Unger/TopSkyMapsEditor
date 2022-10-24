using System;
using System.Collections.Generic;
using System.Drawing.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using TopskyMapsEditor;

namespace Renderer
{
    internal class RenderActiveArea
    {
        internal static Grid ActiveArea(Active activeItem)
        {
            Grid grid = new();
            StackPanel stackPanel = new();

            grid.RowDefinitions.Add(AddRow(1));
            //Add Area Button
            grid.RowDefinitions.Add(AddRow(2));

            TextBlock activeAreasText = AddTextBlock("Active Areas");
            grid.Children.Add(activeAreasText);
            Grid.SetRow(activeAreasText, 0);

            Button addAreaButton = new()
            {
                Foreground = new SolidColorBrush(Colors.White),
                Content = "Add new",
                Background = new SolidColorBrush(System.Windows.Media.Color.FromRgb(53, 91, 135)),
                BorderThickness = new Thickness(0),
                //TODO event handler
                //addAreaButton.Click += AddNewButton_Click;
                Margin = new Thickness(5)
            };
            grid.Children.Add(addAreaButton);
            Grid.SetRow(addAreaButton, 1);

            int currentRow = 2;
            if(activeItem.ActiveAreasList != null)
            {
                foreach (var line in activeItem.ActiveAreasList)
                {
                    grid.RowDefinitions.Add(AddRow(2));

                    TextBlock areaName = AddTextBlock(line);
                    grid.Children.Add(areaName);
                    Grid.SetRow(areaName, currentRow);

                    currentRow++;
                }
            }

            TextBlock notActiveAreasText = AddTextBlock("Not Active Areas");
            grid.Children.Add(notActiveAreasText);
            Grid.SetRow(notActiveAreasText, currentRow + 1);

            Button addNotAreaButton = new()
            {
                Foreground = new SolidColorBrush(Colors.White),
                Content = "Add new",
                Background = new SolidColorBrush(System.Windows.Media.Color.FromRgb(53, 91, 135)),
                BorderThickness = new Thickness(0),
                //TODO event handler
                //addAreaButton.Click += AddNewButton_Click;
                Margin = new Thickness(5)
            };
            grid.Children.Add(addNotAreaButton);
            Grid.SetRow(addNotAreaButton, currentRow + 2);

            currentRow += 3;
            if (activeItem.NotActiveAreasList != null)
            {
                foreach (var line in activeItem.NotActiveAreasList)
                {
                    grid.RowDefinitions.Add(AddRow(2));

                    TextBlock areaName = AddTextBlock(line);
                    grid.Children.Add(areaName);
                    Grid.SetRow(areaName, currentRow);

                    currentRow++;
                }
            }

            //grid.Children.Add(stackPanel);

            return grid;
        }

        private static RowDefinition AddRow(int height)
        {
            RowDefinition row = new()
            {
                Height = new GridLength(height, GridUnitType.Star)
            };

            return row;
        }

        private static TextBlock AddTextBlock(string text)
        {
            TextBlock textBlock = new()
            {
                Foreground = new SolidColorBrush(Colors.White),
                Text = text,
                Margin = new Thickness(10, 0, 0, 0),
                FontWeight = FontWeights.Bold
            };

            return textBlock;
        }
    }
}
