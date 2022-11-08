using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;

namespace TopskyMapsEditor.Renderer
{
    public class RenderMapOverview : MainWindow
    {
        public static SolidColorBrush White = new SolidColorBrush(Colors.White);
        public static SolidColorBrush LightBlue = new SolidColorBrush(System.Windows.Media.Color.FromRgb(35, 50, 68));
        public static SolidColorBrush DarkBlue = new SolidColorBrush(System.Windows.Media.Color.FromRgb(28, 40, 54));
        public static void RenderMap(TopskyMap map)
        {
            Main.NameTextBox.Text = map.Name;
            AddFoldersToDropdown(map);

            if(map.Layer != null)
            {
                Main.LayerTextBox.Text = map.Layer.ToString();
            }
            if(map.Zoom != null)
            {
                Main.ZoomTextBox.Text = map.Zoom.ToString();
            }

            //TODO ASRdata
            //if(map.a)
            if(map.Color != null)
            {
                Main.ColorBox.Text = map.Color.ToString();
            }

            ResetButtons();
            if(map.Style != null)
            {
                switch (map.Style.StyleType)
                {
                    default:
                        break;
                    case StyleType.Solid:
                        Main.SolidStyleButton.Foreground = DarkBlue;
                        Main.SolidStyleButton.Background = White;
                        break;
                    case StyleType.Dash:
                        Main.DashedStyleButton.Foreground = DarkBlue;
                        Main.DashedStyleButton.Background = White;
                        break;
                    case StyleType.Dot:
                        Main.DottedStyleButton.Foreground = DarkBlue;
                        Main.DottedStyleButton.Background = White;
                        break;
                    case StyleType.DashDot:
                        Main.DashedDottedStyleButton.Foreground = DarkBlue;
                        Main.DashedDottedStyleButton.Background = White;
                        break;
                    case StyleType.DashDotDot:
                        Main.DashedDottedDottedStyleButton.Foreground = DarkBlue;
                        Main.DashedDottedDottedStyleButton.Background = White;
                        break;
                }
            }

        }

        public static void AddFoldersToDropdown(TopskyMap selectedMap)
        {
            List<string> folderList = new();

            //TODO rewrite with LINQ?
            foreach (var map in Vars.TopskyMaps)
            {
                if (!folderList.Contains(map.Folder))
                {
                    folderList.Add(map.Folder);
                }
            }

            //TODO Clears to remove duplicate items for now
            Main.FolderDropdown.Items.Clear();
            foreach (var folder in folderList)
            {
                Main.FolderDropdown.Items.Add(folder);

                //TODO doesn't work for some reason
                Main.FolderDropdown.Text = selectedMap.Folder ?? "None";
            }
        }


        public static void FilterFolders()
        {
            string mapName = Main.NameTextBox.Text;
            if (mapName != null)
            {
                TopskyMap? map = Vars.TopskyMaps.Where(map => map.Name == mapName).FirstOrDefault();

                if(map != null)
                {
                    RenderScrollviewer.FilterListByFolder(Main.FolderDropdown.Text);
                }
            }
        }

        private static void ResetButtons()
        {
            Main.SolidStyleButton.Foreground = White;
            Main.SolidStyleButton.Background = LightBlue;

            Main.DashedStyleButton.Foreground = White;
            Main.DashedStyleButton.Background = LightBlue;

            Main.DottedStyleButton.Foreground = White;
            Main.DottedStyleButton.Background = LightBlue;

            Main.DashedDottedStyleButton.Foreground = White;
            Main.DashedDottedStyleButton.Background = LightBlue;

            Main.DashedDottedDottedStyleButton.Foreground = White;
            Main.DashedDottedDottedStyleButton.Background = LightBlue;

        }
    }
}
