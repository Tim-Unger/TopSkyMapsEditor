using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace TopskyMapsEditor.Renderer
{
    public class RenderMapOverview : MainWindow
    {
        public static void RenderMap(TopskyMap map)
        {
            Main.NameTextBox.Text = map.Name;
            Main.FolderTextBox.Text = map.Folder;
        }
    }
}
