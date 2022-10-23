using System;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Media;
using System.Xml.Linq;
using TopskyMapsEditor;
using Button = System.Windows.Controls.Button;
using StackPanel = System.Windows.Controls.StackPanel;

namespace TopskyMapsEditor.Renderer
{
    internal class RenderScrollviewer : MainWindow
    {
        private static System.Windows.Controls.TextBox searchBox = new System.Windows.Controls.TextBox
        {
            Padding = new Thickness(0, 5, 0, 5),
            Margin = new Thickness(5, 0, 5, 0),
            VerticalAlignment = VerticalAlignment.Center,
            Background = new SolidColorBrush(System.Windows.Media.Color.FromRgb(53, 91, 135)),
            FontWeight = FontWeights.Bold,
            Foreground = new SolidColorBrush(Colors.White),
            Text = "Search"
        }; 

        private static ScrollViewer scrollViewer = Main.ListViewScrollViewer;
        private static  StackPanel stackPanel = Main.ListStackPanel;
        private static  StackPanel saveStackPanel = Main.ListStackPanel;
        private static List<Button> buttonList = new List<Button>();

        public static void RenderListView(List<TopskyMap> listIn)
        {
            Thickness margin = new Thickness(5, 0, 5, 0);
            Thickness padding = new Thickness(0, 5, 0, 5);
            System.Windows.Media.Color darkBlue = System.Windows.Media.Color.FromRgb(28, 40, 54);
            System.Windows.Media.Color lightBlue = System.Windows.Media.Color.FromRgb(53, 91, 135);


            foreach (var map in listIn)
            {
                TextBlock textBlock = new();

                Button button = new();

                button.Foreground = new SolidColorBrush(Colors.White);
                button.Content = map.Name.TrimEnd('\r','\n');
                button.Padding = new Thickness(0);
                button.Background = new SolidColorBrush(darkBlue);
                button.Margin = margin;
                button.Padding = padding;
                button.BorderThickness = new Thickness(0);
                button.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
                button.Click += Button_Click;

                stackPanel.Children.Add(button);

                buttonList.Add(button);
            }

            searchBox.GotFocus += SearchBox_GotFocus;
            searchBox.TextChanged += SearchBox_TextChanged;

            Grid grid = Main.ButtonGrid;
            
            grid.Children.Add(searchBox);
        }

        private static void SearchBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            System.Windows.Controls.TextBox textBox = searchBox;
            string searchText = textBox.Text;

            List<Button> searchResults = new List<Button>();

            //Check if search is whitespace
            if (searchText == " " || searchText == null || searchText == "" || searchText == string.Empty)
            {
                stackPanel.Children.RemoveRange(0, stackPanel.Children.Count);
                //foreach (Button button in saveStackPanel.Children.OfType<Button>())
                //{
                //    stackPanel.Children.Add(button);
                //}
                foreach(Button button in buttonList)
                {
                    stackPanel.Children.Add(button);
                }
            }
            else
            {
                //for(int i = 0; i < saveStackPanel.Children.Count; i++)
                //{
                    
                //}
                foreach (Button children in buttonList/*stackPanel.Children.OfType<Button>()*/)
                {
                    var element = children;

                    //stackPanel.Children.RemoveRange(8, stackPanel.Children.Count);
                    //bool elementContainsSearch = element.Name.IndexOf(content, StringComparison.OrdinalIgnoreCase) >= 0;
                    string buttonContent = children.Content.ToString();
                    bool elementContainsSearch = buttonContent.Contains(searchText, StringComparison.OrdinalIgnoreCase);
                    if (elementContainsSearch)
                    {
                        searchResults.Add(element);
                    }
                }

                stackPanel.Children.RemoveRange(0, stackPanel.Children.Count);

                foreach(var result in searchResults)
                {
                    stackPanel.Children.Add(result);
                }
            }
            //throw new NotImplementedException();
        }

        private static void SearchBox_GotFocus(object sender, RoutedEventArgs e)
        {
            if(searchBox.Text == "Search")
            {
                searchBox.Text = String.Empty;
            }
            //throw new NotImplementedException();
        }

        private static void Button_Click(object sender, RoutedEventArgs e)
        {
            TopskyMap topskyMap = new TopskyMap();

            StackPanel stackPanel = Main.ListStackPanel;
            //int index = stackPanel.Children.IndexOf(sender as UIElement);
            var buttonText = (sender as Button).Content;


            foreach(var map in Vars.TopskyMaps)
            {
                if(map.Name == buttonText)
                {
                    topskyMap = map;
                    break;
                }
            }

            RenderMapOverview.RenderMap(topskyMap);
        }
    }
}
