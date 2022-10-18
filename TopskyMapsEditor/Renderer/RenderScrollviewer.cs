using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using TopskyMapsEditor;

namespace TopskyMapsEditor.Renderer
{
    internal class RenderScrollviewer : MainWindow
    {
        public static void RenderListView(List<TopskyMap> listIn)
        {
            ScrollViewer scrollViewer = Main.ListViewScrollViewer;
            StackPanel stackPanel = Main.ListStackPanel;

            foreach (var map in listIn)
            {
                TextBlock textBlock = new();

                Button button = new(); 

                button.Foreground = new SolidColorBrush(System.Windows.Media.Color.FromRgb(255,255,255));
                button.Content = map.Name.TrimEnd('\r','\n');
                button.Padding = new Thickness(0);
                button.Background = new SolidColorBrush(System.Windows.Media.Color.FromRgb(6, 6, 6));
                button.HorizontalAlignment = HorizontalAlignment.Left;
                button.Click += Button_Click;

                stackPanel.Children.Add(button);
            }
        }

        private static void Button_Click(object sender, RoutedEventArgs e)
        {
            TopskyMap topskyMap = new TopskyMap();

            StackPanel stackPanel = Main.ListStackPanel;
            int index = stackPanel.Children.IndexOf(sender as UIElement);

            topskyMap = Vars.TopskyMaps[index-1];
        }
    }
}
