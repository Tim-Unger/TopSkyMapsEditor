using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Printing;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Automation.Text;
using System.Windows.Controls;
using System.Windows.Interop;
using System.Windows.Media;
using TopskyMapsEditor;
using static Functions.AddFunctions;

namespace Renderer
{
    internal class RenderFontOptions : MainWindow
    {
        enum RowType
        {
            Size,
            Weight,
            Style,
            Alignment
        }

        public static SolidColorBrush LightBlue = new SolidColorBrush(
            System.Windows.Media.Color.FromRgb(35, 50, 68)
        );
        public static SolidColorBrush White = new SolidColorBrush(Colors.White);

        internal static void RenderFont(string mapName)
        {
#pragma warning disable CS8600, CS8602
            TopskyMap map = Vars.TopskyMaps.Where(name => name.Name == mapName).FirstOrDefault();

            RenderOptions(map.TextProperties);
        }

        private static void RenderOptions(TextProperties textProperties)
        {
            Grid grid = new();

            //Draw the Grid
            grid.RowDefinitions.Add(AddRow(2));
            grid.RowDefinitions.Add(AddRow(1));
            grid.RowDefinitions.Add(AddRow(1));
            grid.RowDefinitions.Add(AddRow(1));
            grid.RowDefinitions.Add(AddRow(3));
            grid.RowDefinitions.Add(AddRow(8));

            Grid fontSizeGrid = FontSize(textProperties.FontSize);
            grid.Children.Add(fontSizeGrid);
            Grid.SetRow(fontSizeGrid, 1);

            Grid fontWeightGrid = FontWeight(textProperties.FontStyle.FontWeigth);
            grid.Children.Add(fontWeightGrid);
            Grid.SetRow(fontWeightGrid, 2);

            Grid fontStyleGrid = FontStyle(textProperties.FontStyle);
            grid.Children.Add(fontStyleGrid);
            Grid.SetRow(fontStyleGrid, 3);

            Grid fontAlignmentGrid = FontAlignment(textProperties.TextAlignment);
            grid.Children.Add(fontAlignmentGrid);
            Grid.SetRow(fontAlignmentGrid, 4);

            Main.SingleItemView.Children.Clear();
            Main.SingleItemView.Children.Add(grid);
        }

        private static new Grid FontSize(FontSize fontSize)
        {
            Grid grid = new();

            grid.ColumnDefinitions.Add(AddColumn(4));
            grid.ColumnDefinitions.Add(AddColumn(1));
            grid.ColumnDefinitions.Add(AddColumn(1));
            grid.ColumnDefinitions.Add(AddColumn(1));
            grid.ColumnDefinitions.Add(AddColumn(1));
            grid.ColumnDefinitions.Add(AddColumn(2));

            TextBlock fontSizeText = AddTextBlock("Font Size:");
            fontSizeText.VerticalAlignment = VerticalAlignment.Center;
            grid.Children.Add(fontSizeText);
            Grid.SetColumn(fontSizeText, 0);

            Button equalsButton =
                new()
                {
                    Foreground = new SolidColorBrush(Colors.White),
                    Content = '=',
                    Background = new SolidColorBrush(
                        System.Windows.Media.Color.FromRgb(35, 50, 68)
                    ),
                    BorderThickness = new Thickness(0),
                    //TODO event handler
                    //addAreaButton.Click += AddNewButton_Click;
                    VerticalAlignment = VerticalAlignment.Center,
                    HorizontalAlignment = HorizontalAlignment.Center,
                    Padding = new Thickness(5),
                };
            grid.Children.Add(equalsButton);
            Grid.SetColumn(equalsButton, 1);

            Button decreaseButton =
                new()
                {
                    Foreground = new SolidColorBrush(Colors.White),
                    Content = '-',
                    Background = new SolidColorBrush(
                        System.Windows.Media.Color.FromRgb(35, 50, 68)
                    ),
                    BorderThickness = new Thickness(0),
                    //TODO event handler
                    //addAreaButton.Click += AddNewButton_Click;
                    VerticalAlignment = VerticalAlignment.Center,
                    HorizontalAlignment = HorizontalAlignment.Center,
                    Padding = new Thickness(5),
                };
            grid.Children.Add(decreaseButton);
            Grid.SetColumn(decreaseButton, 2);

            Button increaseButton =
                new()
                {
                    Foreground = new SolidColorBrush(Colors.White),
                    Content = '+',
                    Background = new SolidColorBrush(
                        System.Windows.Media.Color.FromRgb(35, 50, 68)
                    ),
                    BorderThickness = new Thickness(0),
                    //TODO event handler
                    //addAreaButton.Click += AddNewButton_Click;
                    VerticalAlignment = VerticalAlignment.Center,
                    HorizontalAlignment = HorizontalAlignment.Center,
                    Padding = new Thickness(5),
                };
            grid.Children.Add(increaseButton);
            Grid.SetColumn(increaseButton, 3);

            Button multiplyButton =
                new()
                {
                    Foreground = new SolidColorBrush(Colors.White),
                    Content = '*',
                    Background = new SolidColorBrush(
                        System.Windows.Media.Color.FromRgb(35, 50, 68)
                    ),
                    BorderThickness = new Thickness(0),
                    //TODO event handler
                    //addAreaButton.Click += AddNewButton_Click;
                    VerticalAlignment = VerticalAlignment.Center,
                    HorizontalAlignment = HorizontalAlignment.Center,
                    Padding = new Thickness(5),
                };
            grid.Children.Add(multiplyButton);
            Grid.SetColumn(multiplyButton, 4);

            string value = fontSize.Size.ToString() != null ? fontSize.Size.ToString() : "1-99";
            TextBox amountBox = AddTextBox(value);
            amountBox.VerticalAlignment = VerticalAlignment.Center;
            amountBox.Margin = new Thickness(5);
            grid.Children.Add(amountBox);
            Grid.SetColumn(amountBox, 5);

            //TODO reset button colors?
            switch (fontSize.Type)
            {
                case FontSizeType.New:
                    equalsButton.Foreground = LightBlue;
                    equalsButton.Background = White;
                    break;
                case FontSizeType.Reduce:
                    decreaseButton.Foreground = LightBlue;
                    decreaseButton.Background = White;
                    break;
                case FontSizeType.Increase:
                    increaseButton.Foreground = LightBlue;
                    increaseButton.Background = White;
                    break;
                case FontSizeType.Multiply:
                    multiplyButton.Foreground = LightBlue;
                    multiplyButton.Background = White;
                    break;
            }

            return grid;
        }

        private static new Grid FontWeight(int fontWeigth)
        {
            Grid grid = new();

            grid.ColumnDefinitions.Add(AddColumn(6));
            grid.ColumnDefinitions.Add(AddColumn(4));

            TextBlock fontWeigthText = AddTextBlock("Font Weight:");
            fontWeigthText.VerticalAlignment = VerticalAlignment.Center;
            grid.Children.Add(fontWeigthText);
            Grid.SetColumn(fontWeigthText, 0);

            TextBox fontWeigthBox = AddTextBox(fontWeigth.ToString());
            fontWeigthBox.VerticalAlignment = VerticalAlignment.Center;
            fontWeigthBox.Margin = new Thickness(5);
            grid.Children.Add(fontWeigthBox);
            Grid.SetColumn(fontWeigthBox, 1);

            return grid;
        }

        private static new Grid FontStyle(TopskyMapsEditor.FontStyle fontStyle)
        {
            Grid grid = new();

            grid.ColumnDefinitions.Add(AddColumn(7));
            grid.ColumnDefinitions.Add(AddColumn(1));
            grid.ColumnDefinitions.Add(AddColumn(1));
            grid.ColumnDefinitions.Add(AddColumn(1));

            TextBlock fontStyleText = AddTextBlock("Font Style:");
            fontStyleText.VerticalAlignment = VerticalAlignment.Center;
            grid.Children.Add(fontStyleText);
            Grid.SetColumn(fontStyleText, 0);

            //TODO FontExtras
            Button italicButton =
                new()
                {
                    Foreground = new SolidColorBrush(Colors.White),
                    Content = 'I',
                    Background = new SolidColorBrush(
                        System.Windows.Media.Color.FromRgb(35, 50, 68)
                    ),
                    BorderThickness = new Thickness(0),
                    //TODO event handler
                    //addAreaButton.Click += AddNewButton_Click;
                    VerticalAlignment = VerticalAlignment.Center,
                    HorizontalAlignment = HorizontalAlignment.Center,
                    Padding = new Thickness(5),
                    FontStyle = FontStyles.Italic
                };
            grid.Children.Add(italicButton);
            Grid.SetColumn(italicButton, 1);

            Button underlinedButton =
                new()
                {
                    Foreground = new SolidColorBrush(Colors.White),
                    Content = "U",
                    Background = new SolidColorBrush(
                        System.Windows.Media.Color.FromRgb(35, 50, 68)
                    ),
                    BorderThickness = new Thickness(0),
                    //TODO event handler
                    //addAreaButton.Click += AddNewButton_Click;
                    VerticalAlignment = VerticalAlignment.Center,
                    HorizontalAlignment = HorizontalAlignment.Center,
                    Padding = new Thickness(5),
                };

            grid.Children.Add(underlinedButton);
            Grid.SetColumn(underlinedButton, 2);

            Button strikethroughButton =
                new()
                {
                    Foreground = new SolidColorBrush(Colors.White),
                    Content = "S",
                    Background = new SolidColorBrush(
                        System.Windows.Media.Color.FromRgb(35, 50, 68)
                    ),
                    BorderThickness = new Thickness(0),
                    //TODO event handler
                    //addAreaButton.Click += AddNewButton_Click;
                    Padding = new Thickness(5),
                    VerticalAlignment = VerticalAlignment.Center,
                    HorizontalAlignment = HorizontalAlignment.Center,
                };
            grid.Children.Add(strikethroughButton);
            Grid.SetColumn(strikethroughButton, 3);

            if (fontStyle.IsItalic == true)
            {
                italicButton.Foreground = LightBlue;
                italicButton.Background = White;
            }
            if (fontStyle.IsUnderlined == true)
            {
                underlinedButton.Foreground = LightBlue;
                underlinedButton.Background = White;
            }
            if (fontStyle.IsStrikethrough == true)
            {
                strikethroughButton.Foreground = LightBlue;
                strikethroughButton.Background = White;
            }

            return grid;
        }

        private static new Grid FontAlignment(Alignment? fontAlignment)
        {
            Grid grid = new();

            grid.RowDefinitions.Add(AddRow(1));
            grid.RowDefinitions.Add(AddRow(1));
            grid.RowDefinitions.Add(AddRow(1));

            grid.ColumnDefinitions.Add(AddColumn(7));
            grid.ColumnDefinitions.Add(AddColumn(1));
            grid.ColumnDefinitions.Add(AddColumn(1));
            grid.ColumnDefinitions.Add(AddColumn(1));

            TextBlock textAlignmentText = AddTextBlock("Text Alignment:");
            grid.Children.Add(textAlignmentText);
            Grid.SetColumn(textAlignmentText, 0);
            Grid.SetRow(textAlignmentText, 1);

            //Does not work
            Button emptyAlignmentButton =
                new()
                {
                    Background = new SolidColorBrush(
                        System.Windows.Media.Color.FromRgb(35, 50, 68)
                    ),
                    BorderThickness = new Thickness(0),
                    //TODO event handler
                    //addAreaButton.Click += AddNewButton_Click;
                    Margin = new Thickness(2),
                };

            //TODO refactor
            Button topLeftButton =
                new()
                {
                    Background = new SolidColorBrush(
                        System.Windows.Media.Color.FromRgb(35, 50, 68)
                    ),
                    BorderThickness = new Thickness(0),
                    //TODO event handler
                    //addAreaButton.Click += AddNewButton_Click;
                    Margin = new Thickness(2),
                };
            grid.Children.Add(topLeftButton);
            Grid.SetRow(topLeftButton, 0);
            Grid.SetColumn(topLeftButton, 1);

            Button topCenterButton =
                new()
                {
                    Background = new SolidColorBrush(
                        System.Windows.Media.Color.FromRgb(35, 50, 68)
                    ),
                    BorderThickness = new Thickness(0),
                    //TODO event handler
                    //addAreaButton.Click += AddNewButton_Click;
                    Margin = new Thickness(2),
                };
            grid.Children.Add(topCenterButton);
            Grid.SetRow(topCenterButton, 0);
            Grid.SetColumn(topCenterButton, 2);

            Button topRightButton =
                new()
                {
                    Background = new SolidColorBrush(
                        System.Windows.Media.Color.FromRgb(35, 50, 68)
                    ),
                    BorderThickness = new Thickness(0),
                    //TODO event handler
                    //addAreaButton.Click += AddNewButton_Click;
                    Margin = new Thickness(2),
                };
            grid.Children.Add(topRightButton);
            Grid.SetRow(topRightButton, 0);
            Grid.SetColumn(topRightButton, 3);

            Button centerLeftButton =
                new()
                {
                    Background = new SolidColorBrush(
                        System.Windows.Media.Color.FromRgb(35, 50, 68)
                    ),
                    BorderThickness = new Thickness(0),
                    Margin = new Thickness(2)
                };
            grid.Children.Add(centerLeftButton);
            Grid.SetRow(centerLeftButton, 1);
            Grid.SetColumn(centerLeftButton, 1);

            Button centerCenterButton =
                new()
                {
                    Background = new SolidColorBrush(
                        System.Windows.Media.Color.FromRgb(35, 50, 68)
                    ),
                    BorderThickness = new Thickness(0),
                    Margin = new Thickness(2)
                };
            grid.Children.Add(centerCenterButton);
            Grid.SetRow(centerCenterButton, 1);
            Grid.SetColumn(centerCenterButton, 2);

            Button centerRightButton =
                new()
                {
                    Background = new SolidColorBrush(
                        System.Windows.Media.Color.FromRgb(35, 50, 68)
                    ),
                    BorderThickness = new Thickness(0),
                    Margin = new Thickness(2)
                };
            grid.Children.Add(centerRightButton);
            Grid.SetRow(centerRightButton, 1);
            Grid.SetColumn(centerRightButton, 3);

            Button bottomLeftButton =
                new()
                {
                    Background = new SolidColorBrush(
                        System.Windows.Media.Color.FromRgb(35, 50, 68)
                    ),
                    BorderThickness = new Thickness(0),
                    Margin = new Thickness(2)
                };
            grid.Children.Add(bottomLeftButton);
            Grid.SetRow(bottomLeftButton, 2);
            Grid.SetColumn(bottomLeftButton, 1);

            Button bottomCenterButton =
                new()
                {
                    Background = new SolidColorBrush(
                        System.Windows.Media.Color.FromRgb(35, 50, 68)
                    ),
                    BorderThickness = new Thickness(0),
                    Margin = new Thickness(2)
                };
            grid.Children.Add(bottomCenterButton);
            Grid.SetRow(bottomCenterButton, 2);
            Grid.SetColumn(bottomCenterButton, 2);

            Button bottomRightButton =
                new()
                {
                    Background = new SolidColorBrush(
                        System.Windows.Media.Color.FromRgb(35, 50, 68)
                    ),
                    BorderThickness = new Thickness(0),
                    Margin = new Thickness(2)
                };
            grid.Children.Add(bottomRightButton);
            Grid.SetRow(bottomRightButton, 2);
            Grid.SetColumn(bottomRightButton, 3);

            switch (fontAlignment)
            {
                case null:
                    break;
                case Alignment.TopLeft:
                    topLeftButton.Background = White;
                    break;
                case Alignment.TopCenter:
                    topCenterButton.Background = White;
                    break;
                case Alignment.TopRight:
                    topRightButton.Background = White;
                    break;
                case Alignment.CenterLeft:
                    centerLeftButton.Background = White;
                    break;
                case Alignment.CenterCenter:
                    centerCenterButton.Background = White;
                    break;
                case Alignment.CenterRight:
                    centerRightButton.Foreground = White;
                    break;
                case Alignment.BottomLeft:
                    bottomLeftButton.Foreground = White;
                    break;
                case Alignment.BottomCenter:
                    bottomCenterButton.Foreground = White;
                    break;
                case Alignment.BottomRight:
                    bottomRightButton.Foreground = White;
                    break;
            }

            return grid;
        }

        //Not needed for now
        //private static void SetContent() { }
    }
}
