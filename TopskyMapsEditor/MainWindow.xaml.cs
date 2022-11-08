using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Forms;
using System.IO;
using System.Windows.Media.Animation;
using Parser;
using System.Drawing.Text;
using static TopskyMapsEditor.Vars;
using MessageBox = System.Windows.Forms.MessageBox;
using TopskyMapsEditor.Renderer;
using Parser;
using Renderer;

namespace TopskyMapsEditor
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public class Vars
    {
        public static bool IsTopskyFolderSelected { get; set; }
        public static bool IsSctFolderSelected { get; set; }

        public static List<string> TopskyMapTitles { get; set; }
        public static List<TopskyMap> TopskyMaps { get; set; }
        public static GeneralSettings? GeneralSettings { get; set; }

        public static List<string>? TopskyAreas { get; set; }
    }

    public partial class MainWindow : Window
    {
        //TODO?
        public static MainWindow Main;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void DragMoveRect_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            Environment.Exit(0);
        }

        private void SelectMapButton_Click(object sender, RoutedEventArgs e)
        {
            //TODO fix crash when a map is already selected and the button is clicked
            selectPath:
            OpenFileDialog fileDialog =
                new()
                {
                    InitialDirectory = Environment.GetFolderPath(
                        Environment.SpecialFolder.MyDocuments
                    ),
                    Filter = "TopSkyMap.txt (*.txt)|*.txt|All files (*.*)|*.*"
                };

            if (fileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                string path = fileDialog.FileName;

                if (System.IO.Path.GetFileNameWithoutExtension(path) != "TopSkyMaps")
                {
                    DialogResult result = MessageBox.Show(
                        "The File you selected is not named TopSkyMaps, do you want to continue anyways?",
                        "Wrong File Name",
                        MessageBoxButtons.YesNo
                    );

                    if (result == System.Windows.Forms.DialogResult.No)
                    {
                        goto selectPath;
                    }
                    else
                    {
                        goto readFile;
                    }
                }

                readFile:
                StreamReader streamReader = new(path);
                string RawText = streamReader.ReadToEnd();
                streamReader.Close();

                //TODO
                TopskyMapTitles = TopskyMapClass.GetTopskyMapNames(RawText);
                TopskyMaps = TopskyMapClass.GetTopskyMaps(RawText);
                Vars.GeneralSettings = TopskyMapClass.GetGeneralSettings(RawText);
                Vars.TopskyAreas = ReadAreas.GetAreas(path);

                if (Vars.GeneralSettings.Colors != null)
                {
                    ReadColors.AddColorsToDropdown();
                }

                IsTopskyFolderSelected = true;
                SelectMapButton.Content = path;
                if (IsTopskyFolderSelected && IsSctFolderSelected)
                {
                    PlaceholderMainGrid.Visibility = Visibility.Hidden;
                    MainGrid.Visibility = Visibility.Visible;

                    RenderScrollviewer.RenderListView(TopskyMaps);
                    RenderScrollviewer.RenderFolderView(TopskyMaps);
                }
                //MapPlaceholder.Visibility = Visibility.Hidden;
            }
        }

        private void SelectSctButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog fileDialog =
                new()
                {
                    InitialDirectory = Environment.GetFolderPath(
                        Environment.SpecialFolder.MyDocuments
                    ),
                    Filter = "SCT file (*.sct)|*.sct|All files (*.*)|*.*"
                };

            if (fileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                var path = fileDialog.FileName;

                StreamReader streamReader = new(path);
                string RawText = streamReader.ReadToEnd();
                streamReader.Close();

                SctClass.ReadSctFile(RawText);

                string? eseSameFolderString = ReadEseClass.CheckIfEseExists(path);

                if (eseSameFolderString != null)
                {
                    ReadEseClass.ReadEse(path);
                    //DialogResult dialogResult = MessageBox.Show("ESE found in the same folder as the SCT.\nDo you want to use this ESE?", "Use ESE", MessageBoxButtons.YesNo);

                    //if (dialogResult == System.Windows.Forms.DialogResult.Yes)
                    //{
                    //    ReadEseClass.ReadEse(Path);
                    //}
                }

                IsSctFolderSelected = true;
                SelectSctButton.Content = path;
                if (IsTopskyFolderSelected && IsSctFolderSelected)
                {
                    PlaceholderMainGrid.Visibility = Visibility.Hidden;
                    MainGrid.Visibility = Visibility.Visible;

                    //TODO
                    RenderScrollviewer.RenderListView(TopskyMaps);
                    RenderScrollviewer.RenderFolderView(TopskyMaps);
                }
            }
        }

        private void Save_Click(object sender, RoutedEventArgs e) { }

        private void EditMapsButton_Click(object sender, RoutedEventArgs e)
        {
            BrowseSct.Visibility = Visibility.Hidden;
            EditMaps.Visibility = Visibility.Visible;

            //EditMapsButton.Background = new SolidColorBrush(System.Windows.Media.Color.FromRgb(23, 23, 23));
            //BrowseSctButton.Background = new SolidColorBrush(System.Windows.Media.Color.FromRgb(10, 10, 10));
        }

        private void BrowseSctButton_Click(object sender, RoutedEventArgs e)
        {
            BrowseSct.Visibility = Visibility.Visible;
            EditMaps.Visibility = Visibility.Hidden;

            //EditMapsButton.Background = new SolidColorBrush(System.Windows.Media.Color.FromRgb(10, 10, 10));
            //BrowseSctButton.Background = new SolidColorBrush(System.Windows.Media.Color.FromRgb(23, 23, 23));
        }

        private void FolderViewButton_Click(object sender, RoutedEventArgs e) 
        {
            ListViewScrollViewer.Visibility = Visibility.Hidden;
            FolderViewScrollViewer.Visibility = Visibility.Visible;
        }

        private void ListViewButton_Click(object sender, RoutedEventArgs e) 
        {
            ListViewScrollViewer.Visibility = Visibility.Visible;
            FolderViewScrollViewer.Visibility = Visibility.Hidden;
        }

        private void RawViewButton_Click(object sender, RoutedEventArgs e) { }

        private void DetailedViewButton_Click(object sender, RoutedEventArgs e) { }

        private void PreviewButton_Click(object sender, RoutedEventArgs e) { }

        private void EditButton_Click(object sender, RoutedEventArgs e) { }

        private void SelectEse_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog fileDialog =
                new()
                {
                    InitialDirectory = Environment.GetFolderPath(
                        Environment.SpecialFolder.MyDocuments
                    ),
                    Filter = "ESE file (*.ese)|*.ese|All files (*.*)|*.*"
                };

            if (fileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                var Path = fileDialog.FileName;

                ReadEseClass.ReadEse(Path);
            }
        }

        private void Window_ContentRendered(object sender, EventArgs e)
        {
            Main = this as MainWindow;
            PlaceholderMainGrid.Visibility = Visibility.Visible;
            MainGrid.Visibility = Visibility.Hidden;
        }

        //private void ListViewSearch_GotFocus(object sender, RoutedEventArgs e)
        //{
        //    //TODO Key input event handler
        //    //string input += e.KeyChar.ToString();
        //}

        //private void ListViewSearch_LostFocus(object sender, RoutedEventArgs e) { }

        private void AddTopskyMapButton_Click(object sender, RoutedEventArgs e) { }

        //private void ListViewSearch_TextChanged(object sender, TextChangedEventArgs e) { }

        private void ActiveButton_Click(object sender, RoutedEventArgs e) 
        {
            RenderActiveTriggers.RenderTriggers(NameTextBox.Text);
        }

        private void LinesButton_Click(object sender, RoutedEventArgs e) { }

        private void SymbolsButton_Click(object sender, RoutedEventArgs e) { }

        private void TextsButton_Click(object sender, RoutedEventArgs e) { }

        private void FontOptionsButton_Click(object sender, RoutedEventArgs e)
        {
            RenderFontOptions.RenderFont(NameTextBox.Text);
        }

        private void EditColorButton_Click(object sender, RoutedEventArgs e)
        {
            if(ColorBox.SelectedItem != null)
            {
                RenderColorOptions.RenderColor(ColorBox.SelectedItem);

            }
            else
            {
                MessageBox.Show("No Color selected");
            }
        }

        private void FilterFolderButton_Click(object sender, RoutedEventArgs e)
        {
            RenderMapOverview.FilterFolders();
        }

        private void AddFolderButton_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
