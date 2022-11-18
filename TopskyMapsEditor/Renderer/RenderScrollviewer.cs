using System;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.Linq;
using System.Net.Http.Headers;
using System.Printing;
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
using TreeView = System.Windows.Controls.TreeView;
using static TopskyMapsEditor.Vars;
using System.Windows.Shell;

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

        //private static StackPanel folderStackPanel = Main.FolderStackPanel;
        //private static StackPanel saveStackPanel = Main.ListStackPanel;
        private static List<Button> buttonList = new();
        public static TreeView Tree { get; set; } =
            new TreeView()
            {
                Background = new SolidColorBrush(Colors.Transparent),
                BorderThickness = new Thickness(0),
                Foreground = white
            };

        public static void RenderListView(List<TopskyMap> topskyMaps)
        {
            Thickness margin = new(5, 0, 5, 0);
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

        public static void RenderFolderView(List<TopskyMap> topskyMaps)
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

            foreach (var folder in folders)
            {
                TreeView treeView = Tree;
                //TreeView treeView = Main.FolderTree;
                TreeViewItem treeViewItem =
                    new()
                    {
                        Foreground = new SolidColorBrush(Colors.White),
                        FontWeight = FontWeights.Bold,
                        Header = folder,
                        IsExpanded = true,
                        Padding = padding,
                        Margin = margin,
                    };

                List<TopskyMap> folderMaps = topskyMaps.Where(map => map.Folder == folder).ToList();

                foreach (var map in folderMaps)
                {
                    TreeViewItem mapItem = new();

                    mapItem.Foreground = new SolidColorBrush(Colors.White);
                    mapItem.Header = map.Name;
                    mapItem.IsExpanded = true;
                    mapItem.FontWeight = FontWeights.Regular;
                    mapItem.Margin = margin;
                    mapItem.Padding = padding;

                    mapItem.Selected += MapItem_Selected;

                    treeViewItem.Items.Add(mapItem);
                }

                searchBox.GotFocus += SearchBox_GotFocus;
                searchBox.TextChanged += SearchBox_TextChanged;

                treeView.Items.Add(treeViewItem);
            }

            Main.FolderTreeGrid.Children.Add(Tree);

            if (Vars.SelectedTab == SelectedTab.List)
            {
                Main.FolderTreeGrid.Visibility = Visibility.Hidden;
            }
        }

        private static void MapItem_Selected(object sender, RoutedEventArgs e)
        {
            
#pragma warning disable CS8600, CS8602, CS8604

            TreeViewItem item = sender as TreeViewItem;
            TopskyMap topskyMap = TopskyMaps
                .Where(map => map.Name == item.Header.ToString())
                .FirstOrDefault();

            RenderMapOverview.RenderMap(topskyMap);
        }

        private static void SearchBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox textBox = searchBox;
            string searchText = textBox.Text;

            TreeView tempTree = null;
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

                foreach (Button button in buttonList)
                {
                    listStackPanel.Children.Add(button);
                }

                //tempTree = Tree;

                //Main.FolderTreeGrid.Children.Clear();
                //Main.FolderTreeGrid.Children.Add(tempTree);

                //Main.FolderTreeGrid.Visibility = Visibility.Visible;

                return;
            }

            //tempTree = null;

            ////Single Item List
            foreach (Button children in buttonList)
            {
                var element = children;

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

            ////Folder List
            //tempTree = Tree;

            //foreach(TreeViewItem item in tempTree.Items)
            //{
            //    foreach(TreeViewItem children in item.Items)
            //    {
            //        if(children.Header.ToString().Contains(searchText, StringComparison.CurrentCultureIgnoreCase))
            //        {
            //            //TODO
            //        }
            //    }
            //    if(item.Header.ToString().Contains(searchText, StringComparison.CurrentCultureIgnoreCase))
            //    {
            //        item.IsExpanded = false;
            //    }
            //}

            //Main.FolderTreeGrid.Children.Clear();
            //Main.FolderTreeGrid.Children.Add(tempTree);
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

            foreach (var map in TopskyMaps)
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

            //Filter for Single Item View
            List<string> filteredResultsList = new();

            foreach (var filterMap in TopskyMaps.Where(folder => folder.Folder == selectedMap))
            {
                if (!filteredResultsList.Contains(filterMap.Name))
                {
                    filteredResultsList.Add(filterMap.Name);
                }
            }

            listStackPanel.Children.RemoveRange(0, listStackPanel.Children.Count);
            foreach (var folderMap in filteredResultsList)
            {
                listStackPanel.Children.Add(
                    buttonList.Where(button => button.Content.ToString() == folderMap).First()
                );
            }

            //Filter for Folder View
            TreeView treeView = Main.FolderTreeGrid.Children[0] as TreeView;

            TreeViewItem filteredFolderItem = new();

            foreach (TreeViewItem item in treeView.Items)
            {
                if (item.Header.ToString() == selectedMap.Trim())
                {
                    item.IsExpanded = true;
                    continue;
                }

                item.IsExpanded = false;
            }


            AddRemoveFilterButton();
        }

        private static void SearchFolders(string searchText)
        {
            //List<string> searchResults = new();

            //searchResults =
        }

        private static void AddRemoveFilterButton()
        {
            Button button =
                new()
                {
                    Padding = new Thickness(10, 5, 10, 5),
                    Background = new SolidColorBrush(
                        System.Windows.Media.Color.FromRgb(35, 50, 68)
                    ),
                    BorderThickness = new Thickness(0),
                    Content = "Remove Filters",
                    FontWeight = FontWeights.Bold,
                    Foreground = new SolidColorBrush(Colors.White)
                };

            button.Click += RemoveFiltersButton_Click;

            if (Main.ViewButtonsStackPanel.Children.Count > 2)
            {
                Main.ViewButtonsStackPanel.Children.RemoveAt(2);
            }

            Main.ViewButtonsStackPanel.Children.Add(button);
        }

        private static void RemoveFiltersButton_Click(object sender, RoutedEventArgs e)
        {
            listStackPanel.Children.RemoveRange(0, listStackPanel.Children.Count);

            foreach (Button button in buttonList)
            {
                listStackPanel.Children.Add(button);
            }

            TreeView tree = Main.FolderTreeGrid.Children[0] as TreeView;
            foreach (TreeViewItem item in tree.Items)
            {
                item.IsExpanded = true;
            }
            Main.ViewButtonsStackPanel.Children.Remove(sender as Button);
        }
    }
}
