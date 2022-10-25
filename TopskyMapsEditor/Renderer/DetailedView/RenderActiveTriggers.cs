using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using TopskyMapsEditor;

namespace Renderer
{
    internal class RenderActiveTriggers : MainWindow
    {
        public static void RenderTriggers(string mapName)
        {
            var map = Vars.TopskyMaps.Where(name => name.Name == mapName).FirstOrDefault();

            if(map.ActiveList.Count > 0)
            {
                RenderTriggers(map.ActiveList);
            }
            else
            {
                NoTriggers();
            }
        }

        private static void NoTriggers()
        {
            Grid grid = new();

            grid.RowDefinitions.Add(AddRow(2));
            grid.RowDefinitions.Add(AddRow(1));
            grid.RowDefinitions.Add(AddRow(1));
            grid.RowDefinitions.Add(AddRow(13));
            grid.Margin = new Thickness(10, 0, 0, 0);

            TextBlock text = new();
            text.Foreground = new SolidColorBrush(Colors.White);
            text.Text = "No Active-Triggers found";
            text.FontWeight = FontWeights.Bold;
            grid.Children.Add(text);
            Grid.SetRow(text, 1);

            Button addNewButton = new();

            addNewButton.Foreground = new SolidColorBrush(Colors.White);
            addNewButton.Content = "Add new";
            addNewButton.Background = new SolidColorBrush(System.Windows.Media.Color.FromRgb(35, 50, 68));
            addNewButton.Background = new SolidColorBrush(System.Windows.Media.Color.FromRgb(35, 50, 68));
            addNewButton.BorderThickness = new Thickness(0);
            addNewButton.Click += AddNewButton_Click;
            addNewButton.Margin = new Thickness(5);
            grid.Children.Add(addNewButton);
            Grid.SetRow(addNewButton, 2);

            Main.SingleItemView.Children.Add(grid);
        }

        private static void AddNewButton_Click(object sender, RoutedEventArgs e)
        {
            //TODO add Button Click
            //throw new NotImplementedException();
        }

        private static void RenderTriggers(List<Active> activeList)
        {
            Grid grid = new();

            grid.RowDefinitions.Add(AddRow(2));
            grid.RowDefinitions.Add(AddRow(15));
            ScrollViewer scrollViewer = new();
            scrollViewer.VerticalScrollBarVisibility = ScrollBarVisibility.Auto;

            StackPanel stackPanel = new();

            foreach(var list in activeList)
            {
                Grid singleActiveTriggerGrid = new();
                switch (list.ActiveType)
                {
                    case ActiveType.Time:
                        singleActiveTriggerGrid.Children.Add(RenderActiveTime.ActiveTime(list));
                        break;
                    case ActiveType.Notam:
                        singleActiveTriggerGrid.Children.Add(RenderActiveNotam.ActiveNotam(list));
                        break;
                    case ActiveType.Area:
                        singleActiveTriggerGrid.Children.Add(RenderActiveArea.ActiveArea(list));
                        break;
                    case ActiveType.Runway:
                        break;
                    case ActiveType.Id:
                        break;
                }

                stackPanel.Children.Add(singleActiveTriggerGrid);
            }

            grid.Children.Add(scrollViewer);
            scrollViewer.Content = stackPanel;
            Grid.SetRow(scrollViewer, 1);
            Main.SingleItemView.Children.Add(grid);
        }

        private static RowDefinition AddRow (int height)
        {
            RowDefinition row = new();

            row.Height = new GridLength(height, GridUnitType.Star);

            return row;
        }
    }
}