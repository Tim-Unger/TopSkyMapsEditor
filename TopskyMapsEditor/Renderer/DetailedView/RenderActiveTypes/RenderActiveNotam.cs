using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using TopskyMapsEditor;
using static Functions.AddFunctions;

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

            TextBox notamAirportText = AddTextBox(activeItem.NotamIcao);
            grid.Children.Add(notamAirportText);
            Grid.SetRow(notamAirportText, 1);

            TextBox notamTextText = AddTextBox(activeItem.NotamText);
            grid.Children.Add(notamTextText);
            Grid.SetRow(notamTextText, 3);

            return grid;
        }

    }
}
