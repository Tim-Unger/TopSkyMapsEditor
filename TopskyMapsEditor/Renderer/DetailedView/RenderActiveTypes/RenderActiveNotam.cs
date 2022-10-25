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
    internal class RenderActiveNotam
    {
        internal static Grid ActiveNotam(Active activeItem)
        {
            Grid grid = new();

            grid.RowDefinitions.Add(AddRow(1));
            grid.RowDefinitions.Add(AddRow(2));
            grid.RowDefinitions.Add(AddRow(1));
            grid.RowDefinitions.Add(AddRow(7));

            TextBlock notamAirport = AddTextBlock("NOTAM Airport/FIR");
            grid.Children.Add(notamAirport);
            Grid.SetRow(notamAirport, 0);

            TextBlock notamText = AddTextBlock("NOTAM Text");
            grid.Children.Add(notamText);
            Grid.SetRow(notamText, 2);

            TextBox notamAirportText = addTextBox(activeItem.NotamIcao);
            grid.Children.Add(notamAirportText);
            Grid.SetRow(notamAirportText, 1);

            TextBox notamTextText = addTextBox(activeItem.NotamText);
            grid.Children.Add(notamTextText);
            Grid.SetRow(notamTextText, 3);

            return grid;
        }

        private static ColumnDefinition AddColumn(int width)
        {
            ColumnDefinition column = new();

            column.Width = new GridLength(width, GridUnitType.Star);

            return column;
        }

        private static RowDefinition AddRow(int height)
        {
            RowDefinition row = new();

            row.Height = new GridLength(height, GridUnitType.Star);

            return row;
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
        private static TextBox addTextBox(string content)
        {
            TextBox textBox = new();

            textBox.Foreground = new SolidColorBrush(Colors.White);
            textBox.Background = new SolidColorBrush(
                System.Windows.Media.Color.FromRgb(35, 50, 68)
            );
            textBox.Text = content;
            textBox.BorderThickness = new Thickness(0);
            textBox.Margin = new Thickness(5, 0, 5, 0);

            return textBox;
        }
    }
}
