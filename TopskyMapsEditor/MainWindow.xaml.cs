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
    }

    public partial class MainWindow : Window
    {
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
            OpenFileDialog fileDialog = new OpenFileDialog();

            fileDialog.InitialDirectory = Environment.GetFolderPath(
                Environment.SpecialFolder.MyDocuments
            );
            fileDialog.Filter = "TopSkyMap.txt (*.txt)|*.txt|All files (*.*)|*.*";

            if (fileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                string Path = fileDialog.FileName;

                if (System.IO.Path.GetFileNameWithoutExtension(Path) != "TopSkyMaps")
                {
                    DialogResult result = System.Windows.Forms.MessageBox.Show(
                        "The File you selected is not named TopSkyMaps, do you want to continue anyways?",
                        "Wrong File Name",
                        MessageBoxButtons.YesNo
                    );

                    if (result == System.Windows.Forms.DialogResult.No)
                    {
                        //TODO Rerun Dialog
                    }
                }

                StreamReader streamReader = new StreamReader(Path);
                string RawText = streamReader.ReadToEnd();
                streamReader.Close();

                //TODO
                TopskyMapTitles = TopskyMapClass.GetTopskyMapNames(RawText);
                TopskyMaps = TopskyMapClass.GetTopskyMaps(RawText);
                Vars.GeneralSettings = TopskyMapClass.GetGeneralSettings(RawText);

                IsTopskyFolderSelected = true;
                SelectMapButton.Content = Path;
                if (IsTopskyFolderSelected && IsSctFolderSelected)
                {
                    PlaceholderMainGrid.Visibility = Visibility.Hidden;
                    MainGrid.Visibility = Visibility.Visible;

                    RenderScrollviewer.RenderListView(TopskyMaps);
                }
                //MapPlaceholder.Visibility = Visibility.Hidden;
            }
        }

        private void SelectSctButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog fileDialog = new OpenFileDialog();

            fileDialog.InitialDirectory = Environment.GetFolderPath(
                Environment.SpecialFolder.MyDocuments
            );
            fileDialog.Filter = "SCT file (*.sct)|*.sct|All files (*.*)|*.*";

            if (fileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                var Path = fileDialog.FileName;

                StreamReader streamReader = new StreamReader(Path);
                string RawText = streamReader.ReadToEnd();

                SctClass.ReadSctFile(RawText);

                string EseSameFolderString = ReadEseClass.CheckIfEseExists(Path);

                if (EseSameFolderString != null)
                {
                    DialogResult dialogResult = MessageBox.Show("ESE found in the same folder as the SCT.\nDo you want to use this ESE?", "Use ESE", MessageBoxButtons.YesNo);

                    if (dialogResult == System.Windows.Forms.DialogResult.Yes)
                    {
                        ReadEseClass.ReadEse(Path);
                    }
                }

                IsSctFolderSelected = true;
                SelectSctButton.Content = Path;
                if (IsTopskyFolderSelected && IsSctFolderSelected)
                {
                    PlaceholderMainGrid.Visibility = Visibility.Hidden;
                    MainGrid.Visibility = Visibility.Visible;

                    //TODO
                    RenderScrollviewer.RenderListView(TopskyMaps);
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

        private void FolderViewButton_Click(object sender, RoutedEventArgs e) { }

        private void ListViewButton_Click(object sender, RoutedEventArgs e) { }

        private void RawViewButton_Click(object sender, RoutedEventArgs e) { }

        private void DetailedViewButton_Click(object sender, RoutedEventArgs e) { }

        private void PreviewButton_Click(object sender, RoutedEventArgs e) { }

        private void EditButton_Click(object sender, RoutedEventArgs e) { }

        private void SelectEse_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog fileDialog = new OpenFileDialog();

            fileDialog.InitialDirectory = Environment.GetFolderPath(
                Environment.SpecialFolder.MyDocuments
            );
            fileDialog.Filter = "ESE file (*.ese)|*.ese|All files (*.*)|*.*";

            if (fileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                var Path = fileDialog.FileName;

                ReadEseClass.ReadEse(Path);
            }
        }

        private void Window_ContentRendered(object sender, EventArgs e)
        {
            Main = this;
            PlaceholderMainGrid.Visibility = Visibility.Visible;
            MainGrid.Visibility = Visibility.Hidden;
        }

        private void ListViewSearch_GotFocus(object sender, RoutedEventArgs e)
        {
            //TODO Key input event handler
            //string input += e.KeyChar.ToString();
        }

        private void ListViewSearch_LostFocus(object sender, RoutedEventArgs e)
        {

        }

        private void AddTopskyMapButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ListViewSearch_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
    }
}
