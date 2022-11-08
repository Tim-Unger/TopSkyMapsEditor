using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using TopskyMapsEditor;
using static Functions.AddFunctions;

namespace Renderer
{
    internal class RenderColorOptions : MainWindow
    {
        internal static void RenderColor(object item)
        {
            #pragma warning disable CS8600, CS8602, CS8604
            //null warnings are disabled as this is only executed if a color is selected and the list is not empty
            Color color = Vars.GeneralSettings.Colors.Where(color => color.Name == item.ToString()).FirstOrDefault();

            Main.SingleItemView.Children.Clear();
            Main.SingleItemView.Children.Add(Render(color));
        }

        private static Grid Render(Color color)
        {
            Grid grid = new();

            return grid;
        }
    }
}
