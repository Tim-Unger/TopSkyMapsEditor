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

namespace TopskyMapsEditor
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    
    public partial class MainWindow : Window
    {
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

            //string InitialDirectory = null;
            //if (
            //    Directory.Exists(
            //        Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"/EuroScope"
            //    )
            //)
            //{
            //    InitialDirectory = Environment.GetFolderPath(
            //    Environment.SpecialFolder.MyDocuments
            //) + @"/EuroScope";
            //}
            //else
            //{
            //    InitialDirectory = Environment.GetFolderPath(
            //    Environment.SpecialFolder.MyDocuments
            //);
            //}

            //fileDialog.InitialDirectory = InitialDirectory;
            fileDialog.InitialDirectory = Environment.GetFolderPath(
                Environment.SpecialFolder.MyDocuments
            );
            fileDialog.Filter = "TopSkyMap.txt (*.txt)|*.txt|All files (*.*)|*.*";

            if (fileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                string Path = fileDialog.FileName;
                SelectSctButton.IsEnabled = true;

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

                //DO stuff

                MapPlaceholder.Visibility = Visibility.Hidden;
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
            }
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
