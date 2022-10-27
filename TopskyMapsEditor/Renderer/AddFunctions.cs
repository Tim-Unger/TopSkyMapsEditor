using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows;

namespace Functions;

internal class AddFunctions
{
    internal static RowDefinition AddRow(int height)
    {
        RowDefinition row = new() { Height = new GridLength(height, GridUnitType.Star) };

        return row;
    }

    internal static TextBlock AddTextBlock(string text /*TODO , FontWeights fontWeight*/)
    {
        TextBlock textBlock =
            new()
            {
                Foreground = new SolidColorBrush(Colors.White),
                Text = text,
                Margin = new Thickness(10, 0, 0, 0),
                FontWeight = FontWeights.Bold
            };

        return textBlock;
    }

    internal static TextBox AddTextBox(string content)
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

    internal static ColumnDefinition AddColumn(int width)
    {
        ColumnDefinition column = new();

        column.Width = new GridLength(width, GridUnitType.Star);

        return column;
    }
}
