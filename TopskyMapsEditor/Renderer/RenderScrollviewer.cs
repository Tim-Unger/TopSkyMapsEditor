using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using TopskyMapsEditor;

namespace TopskyMapsEditor.Renderer
{
    internal class RenderScrollviewer : MainWindow
    {
        public static void RenderListView(List<string> listIn)
        {
            ScrollViewer scrollViewer = Main.ListViewScrollViewer;
            StackPanel stackPanel = Main.ListStackPanel;

            foreach (var map in listIn)
            {
                TextBlock textBlock = new TextBlock();

                textBlock.Foreground = new SolidColorBrush(Color.FromRgb(255, 255, 255));
                textBlock.Text = map;

                stackPanel.Children.Add(textBlock);
            }
        }
    }
}
