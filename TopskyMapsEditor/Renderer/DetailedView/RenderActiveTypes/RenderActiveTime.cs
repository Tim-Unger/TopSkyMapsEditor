using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Media;
using TopskyMapsEditor;
using TextBox = System.Windows.Controls.TextBox;
using static Functions.AddFunctions;

namespace Renderer
{
    internal class RenderActiveTime
    {
        internal static Grid ActiveTime(Active activeItem)
        {
            Grid grid = new();

            grid.RowDefinitions.Add(AddRow(1));
            grid.RowDefinitions.Add(AddRow(2));
            grid.RowDefinitions.Add(AddRow(1));
            grid.RowDefinitions.Add(AddRow(2));

            TextBlock startDateText = AddTextBlock("Start Date");
            grid.Children.Add(startDateText);
            Grid.SetRow(startDateText, 0);

            TextBlock endDateText = AddTextBlock("End Date");
            grid.Children.Add(endDateText);
            Grid.SetRow(endDateText, 2);

            DateTime startTimeConverted = activeItem.StartTime ?? default(DateTime);
            DateTime endTimeConverted = activeItem.EndTime ?? default(DateTime);

            Grid startDateGrid = AddDateSelector(startTimeConverted);
            grid.Children.Add(startDateGrid);
            Grid.SetRow(startDateGrid, 1);

            Grid endDateGrid = AddDateSelector(endTimeConverted);
            grid.Children.Add(endDateGrid);
            Grid.SetRow(endDateGrid, 3);

            return grid;
        }

        private static RowDefinition AddRow(int height)
        {
            RowDefinition row = new();

            row.Height = new GridLength(height, GridUnitType.Star);

            return row;
        }

        private static ColumnDefinition AddColumn(int width)
        {
            ColumnDefinition column = new();

            column.Width = new GridLength(width, GridUnitType.Star);

            return column;
        }

        private static TextBlock AddTextBlock(string text)
        {
            TextBlock textBlock = new();
            textBlock.Foreground = new SolidColorBrush(Colors.White);
            textBlock.Text = text;
            textBlock.Margin = new Thickness(10, 0, 0, 0);
            textBlock.FontWeight = FontWeights.Bold;

            return textBlock;
        }

        private static Grid AddDateSelector(DateTime date)
        {
            Grid grid = new();

            grid.Margin = new Thickness(10, 0, 0, 20);

            //Year
            grid.ColumnDefinitions.Add(AddColumn(2));
            //Month
            grid.ColumnDefinitions.Add(AddColumn(1));
            //Day
            grid.ColumnDefinitions.Add(AddColumn(1));

            grid.ColumnDefinitions.Add(AddColumn(1));
            //Hour
            grid.ColumnDefinitions.Add(AddColumn(1));
            //Minute
            grid.ColumnDefinitions.Add(AddColumn(1));

            TextBox year = AddTextBox(date.Year.ToString());
            grid.Children.Add(year);
            Grid.SetColumn(year, 0);

            TextBox month = AddTextBox(date.Month.ToString());
            grid.Children.Add(month);
            Grid.SetColumn(month, 1);

            TextBox day = AddTextBox(date.Day.ToString());
            grid.Children.Add(day);
            Grid.SetColumn(day, 2);

            TextBox hour = AddTextBox(date.Hour.ToString());
            grid.Children.Add(hour);
            Grid.SetColumn(hour, 4);

            TextBox minute = AddTextBox(date.Minute.ToString());
            grid.Children.Add(minute);
            Grid.SetColumn(minute, 5);

            return grid;
        }

        private static void textBoxGotFocus(Object sender, RoutedEventArgs e, TextBox textBox)
        {
            string[] contentToClear = new string[] { "YYYY", "MM*", "DD*", "HH", "MM" };
            if (contentToClear.Contains(textBox.Text)) 
            {
                textBox.Text = String.Empty;
            }
        }
    }
}
