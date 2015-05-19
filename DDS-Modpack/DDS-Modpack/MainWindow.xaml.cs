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
using System.Net;
using System.IO;
using System.IO.Compression;

namespace DDS_Modpack {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window {
        FolderBrowserDialog dialog = new FolderBrowserDialog();
        string filename = "vehicles.zip", myStringWebResource = null;
        string remoteURL = "https://github.com/Jenartor/DDS-WoT-ModPack/blob/master/vehicles.zip?raw=true";
        public MainWindow() {
            InitializeComponent();
            btn_folder.Visibility = System.Windows.Visibility.Hidden;
            txtbox_welcome.Text = "Willkommen beim DDS-Modpack Installer. Bitte drücken Sie den Button '" + btn_next.Content + "' um fortzufahren!";
        }

        private void btn_cancel_Click(object sender, RoutedEventArgs e) {
            System.Windows.Application.Current.Shutdown();
        }

        private void btn_next_Click(object sender, RoutedEventArgs e) {
            lbl_download.Content = "Download angefangen";
            using (WebClient client = new WebClient()) {
                myStringWebResource = remoteURL;
                client.DownloadFile(myStringWebResource, filename);
            }
            lbl_download.Content = "Download abgeschlossen";
            txtbox_welcome.Text = "Bitte wählen Sie nun ihren 'World of Tanks' Ordner aus!";
            btn_folder.Visibility = System.Windows.Visibility.Visible;
            btn_next.Visibility = System.Windows.Visibility.Hidden;
        }

        private void btn_folder_Click(object sender, RoutedEventArgs e) {
            dialog.ShowDialog();
            if (dialog.SelectedPath != "" && dialog.SelectedPath.Contains("Tanks")) {
                btn_install.Visibility = System.Windows.Visibility.Visible;
                txtbox_welcome.Text = "Sie haben den Ordner '" + dialog.SelectedPath + "' gewählt. Bitte überprüfen Sie die Korrektheit, bevor sie auf '" + btn_install.Content + "' drücken.";
            }
        }

        private void btn_install_Click(object sender, RoutedEventArgs e) {
            extract();
        }
        private void extract() {
            string mods = "res_mods\\0.9.7\\";
            string extractPath = System.IO.Path.Combine(dialog.SelectedPath, mods);
            using (ZipArchive myziparchive = ZipFile.OpenRead(filename)) {
                foreach (ZipArchiveEntry entry in myziparchive.Entries) {
                    if (!entry.FullName.EndsWith(".dds", StringComparison.OrdinalIgnoreCase)) {
                        System.IO.Directory.CreateDirectory(extractPath + entry.FullName.Substring(0, entry.FullName.Length - 1));
                    } else {
                        entry.ExtractToFile(System.IO.Path.Combine(extractPath + entry.FullName), true);
                    }
                }
            }
            txtbox_welcome.Text = "Fertig. Nun starten Sie World of Tanks und genießen Sie das DDS-Modpack!";
        }
    }
}
