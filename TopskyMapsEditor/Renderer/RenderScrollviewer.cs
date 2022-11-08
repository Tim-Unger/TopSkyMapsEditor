using System;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.Linq;
using System.Net.Http.Headers;
using System.Security.Cryptography;
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
using TextBox = System.Windows.Controls.TextBox;

namespace TopskyMapsEditor.Renderer
{
    internal class RenderScrollviewer : MainWindow
    {
        private static TextBox searchBox =
            new()
            {
                Padding = new(0, 5, 0, 5),
                Margin = new(5, 0, 5, 0),
                VerticalAlignment = VerticalAlignment.Center,
                Background = new SolidColorBrush(System.Windows.Media.Color.FromRgb(35, 50, 68)),
                FontWeight = FontWeights.Bold,
                Foreground = new SolidColorBrush(Colors.White),
                Text = "Search"
            };

        //private static ScrollViewer scrollViewer = Main.ListViewScrollViewer;
        private static StackPanel listStackPanel = Main.ListStackPanel;
        private static StackPanel folderStackPanel = Main.FolderStackPanel;
        //private static StackPanel saveStackPanel = Main.ListStackPanel;
        private static List<Button> buttonList = new();

        public static void RenderListView(List<TopskyMap> topskyMaps)
        {
            Thickness margin = new (5, 0, 5, 0);
            Thickness padding = new(0, 5, 0, 5);
            System.Windows.Media.Color darkBlue = System.Windows.Media.Color.FromRgb(28, 40, 54);
            System.Windows.Media.Color lightBlue = System.Windows.Media.Color.FromRgb(35, 50, 68);

            foreach (var map in topskyMaps)
            {
                TextBlock textBlock = new();

                Button button =
                    new()
                    {
                        Foreground = new SolidColorBrush(Colors.White),
                        Content = map.Name.TrimEnd('\r', '\n'),
                        Padding = padding,
                        Background = new SolidColorBrush(darkBlue),
                        Margin = margin,
                        BorderThickness = new Thickness(0),
                        HorizontalAlignment = System.Windows.HorizontalAlignment.Left,
                    };
                button.Click += Button_Click;
                listStackPanel.Children.Add(button);

                buttonList.Add(button);
            }

            searchBox.GotFocus += SearchBox_GotFocus;
            searchBox.TextChanged += SearchBox_TextChanged;

            Grid grid = Main.ButtonGrid;

            //TODO exception if a new map is selected
            grid.Children.Add(searchBox);
        }

        public static void RenderFolderView(List <TopskyMap> topskyMaps)
        {
            Thickness margin = new(5, 0, 5, 0);
            Thickness padding = new(0, 5, 0, 5);
            System.Windows.Media.Color darkBlue = System.Windows.Media.Color.FromRgb(28, 40, 54);
            System.Windows.Media.Color lightBlue = System.Windows.Media.Color.FromRgb(35, 50, 68);

            List<string> folders = new();

            //var foldersTemp = topskyMaps.Where(x => !folders.Any()).FirstOrDefault();
            foreach (var map in topskyMaps)
            {
                if (!folders.Contains(map.Folder))
                {
                    folders.Add(map.Folder);
                }
            }

            foreach(var folder in folders)
            {
                //TODO expand/collapse button
                TextBlock folderTitleText = new()
                {
                    Text = folder,
                    FontWeight = FontWeights.Bold,
                    Foreground = new SolidColorBrush(Colors.White)
                };

                StackPanel subFolderStackPanel = new()
                {
                    Margin = new Thickness(30,0,0,0)
                };

                subFolderStackPanel.Children.Add(folderTitleText);

                List<TopskyMap> folderMaps = topskyMaps.Where(map => map.Folder == folder).ToList();

                foreach(var folderItem in folderMaps)
                {
                    TextBlock textBlock = new();

                    Button button =
                        new()
                        {
                            Foreground = new SolidColorBrush(Colors.White),
                            Content = folderItem.Name.TrimEnd('\r', '\n'),
                            Padding = padding,
                            Background = new SolidColorBrush(darkBlue),
                            Margin = margin,
                            BorderThickness = new Thickness(0),
                            HorizontalAlignment = System.Windows.HorizontalAlignment.Left,
                        };
                    button.Click += Button_Click;
                    subFolderStackPanel.Children.Add(button);

                    buttonList.Add(button);
                }

                folderStackPanel.Children.Add(subFolderStackPanel);
            }

            Main.FolderViewScrollViewer.Visibility = Visibility.Hidden;
        }

        private static void SearchBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox textBox = searchBox;
            string searchText = textBox.Text;

            List<Button> searchResults = new();

            //Check if search is whitespace
            if (
                searchText == " "
                || searchText == null
                || searchText == ""
                || searchText == string.Empty
            )
            {
                listStackPanel.Children.RemoveRange(0, listStackPanel.Children.Count);
                //foreach (Button button in saveStackPanel.Children.OfType<Button>())
                //{
                //    stackPanel.Children.Add(button);
                //}
                foreach (Button button in buttonList)
                {
                    listStackPanel.Children.Add(button);
                }
            }
            else
            {
                //for(int i = 0; i < saveStackPanel.Children.Count; i++)
                //{

                //}
                foreach (
                    Button children in buttonList /*stackPanel.Children.OfType<Button>()*/
                )
                {
                    var element = children;

                    //stackPanel.Children.RemoveRange(8, stackPanel.Children.Count);
                    //bool elementContainsSearch = element.Name.IndexOf(content, StringComparison.OrdinalIgnoreCase) >= 0;
                    string buttonContent = children.Content.ToString();
                    bool elementContainsSearch = buttonContent.Contains(
                        searchText,
                        StringComparison.OrdinalIgnoreCase
                    );
                    if (elementContainsSearch)
                    {
                        searchResults.Add(element);
                    }
                }

                listStackPanel.Children.RemoveRange(0, listStackPanel.Children.Count);

                foreach (var result in searchResults)
                {
                    listStackPanel.Children.Add(result);
                }
            }
            //throw new NotImplementedException();
        }

        private static void SearchBox_GotFocus(object sender, RoutedEventArgs e)
        {
            if (searchBox.Text == "Search")
            {
                searchBox.Text = String.Empty;
            }
            //throw new NotImplementedException();
        }

        private static void Button_Click(object sender, RoutedEventArgs e)
        {
            TopskyMap topskyMap = new();

            //StackPanel stackPanel = Main.ListStackPanel;
            //int index = stackPanel.Children.IndexOf(sender as UIElement);
            var buttonText = (sender as Button).Content;

            foreach (var map in Vars.TopskyMaps)
            {
                if (map.Name == buttonText)
                {
                    topskyMap = map;
                    break;
                }
            }

            RenderMapOverview.RenderMap(topskyMap);
        }

        public static void FilterListByFolder(string selectedMap)
        {
            TextBox textBox = searchBox;

            List<string> filteredResultsList = new();

            foreach (var filterMap in Vars.TopskyMaps.Where(folder => folder.Folder == selectedMap))
            {
                if (!filteredResultsList.Contains(filterMap.Name))
                {
                    filteredResultsList.Add(filterMap.Name);
                }
            }

            listStackPanel.Children.RemoveRange(0, listStackPanel.Children.Count);
            foreach (var folderMap in filteredResultsList)
            {
                listStackPanel.Children.Add(buttonList.Where(button => button.Content.ToString() == folderMap).First());
            }
        }
    }
}
