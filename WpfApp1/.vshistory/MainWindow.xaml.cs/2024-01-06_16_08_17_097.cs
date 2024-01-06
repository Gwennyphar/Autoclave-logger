using System.IO;
using System.Windows;
using Application = System.Windows.Application;
using System.Windows.Controls;
using System.Windows.Media.TextFormatting;
using static System.Net.Mime.MediaTypeNames;
using MessageBox = System.Windows.Forms.MessageBox;
using OpenFileDialog = Microsoft.Win32.OpenFileDialog;

namespace WpfApp1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        public MainWindow()
        {
            InitializeComponent();
            wpfDirectory.Text = DateTime.Now.ToString("MM_yyyy");
            Title = "Autoklav v1.0 ";
            //btnBrowse.IsEnabled = false;
        }


        /**
         */
        private void btnBrowse_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Multiselect = true;
            openFileDialog.Filter = "Text files (*.txt)|*.txt";
            openFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            readFiles(openFileDialog);
        }


        /**
         */
        private void readFiles(OpenFileDialog openFileDialog)
        {

            //Writeout
            if (openFileDialog.ShowDialog() == true)
            {
                string sDir = openFileDialog.InitialDirectory;
                string currDir = '/'+wpfDirectory.Text;
                string[] files = Directory.GetFiles(sDir+currDir, "*.txt", SearchOption.TopDirectoryOnly);
                string[] filetext = files.Select(x => File.ReadAllText(x)).ToArray();

                foreach (string filename in openFileDialog.FileNames)
                    wpfListView.Items.Add(filename);

                foreach (string text in filetext)
                    wpfTextbox.AppendText(text);
            }
        }


        /**
         */
        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            // Set a variable to the Documents path.
            string docPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            string currDir = '/' + wpfDirectory.Text;


            SaveFileDialog save = new SaveFileDialog();
            save.Title = "Save File";
            save.Filter = "Text Files (*txt) | *.txt";
            save.InitialDirectory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), OutputFile("", currDir, wpfDirectory.Text) );


            disabledBtn(docPath, currDir);
            if (File.Exists(OutputFile(docPath, currDir, wpfDirectory.Text)))
            {
                MessageBox.Show(OutputFile(docPath, currDir, wpfDirectory.Text) + " existiert bereits.\n\nKlicke im Menü auf Neu und dann auf Speichern, um eine neue Protokolldatei zu erstellen.", "Achtung");
            }
            else
            {
                createFile(docPath, currDir);
            }
        }

        /**
         */
        private void createFile(string docPath, string currDir)
        {
            // Append text to an existing file named "WriteLines.txt".
            if (Directory.Exists(docPath+currDir) && !File.Exists(OutputFile(docPath, currDir, wpfDirectory.Text)) )
            {
                using (StreamWriter outputFile = new StreamWriter(OutputFile(docPath, currDir, wpfDirectory.Text), false))
                {
                    foreach (string text in wpfListView.Items)
                        outputFile.WriteLine(File.ReadAllText(text));
                    MessageBox.Show("Sterilisationsprotokoll_" + wpfDirectory.Text.Replace('_', '-') + ".txt wurde unter " + docPath + currDir + " erstellt.", "Speichern erfolgreich");
                }
            }
        }


        /**
         */
        private static string OutputFile(string docPath, string currDir, string text)
        {
            return System.IO.Path.Combine(docPath + currDir, "Sterilisationsprotokoll_" + text.Replace('_', '-') + ".txt");
        }


        /**
         */
        private void wpfTextbox_TextChanged(object sender, TextChangedEventArgs e)
        {
            //wpfTextbox.Focus();
        }


        /**
         */
        private void wpfDirectory_TextChanged(object sender, TextChangedEventArgs e)
        {
            string currDir = '/' + wpfDirectory.Text;
            string sDir = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

            //if (Directory.Exists(sDir+currDir))
            //{
            //    string[] files = Directory.GetFiles(sDir + currDir, "*.txt", SearchOption.TopDirectoryOnly);
            //    string[] filetext = files.Select(x => File.ReadAllText(x)).ToArray();
            //    if (filetext.Length > 0)
            //    {
            //        clearWindows();
            //        readDirectory(files, filetext);
            //    }
            //} else
            //{
            //    clearWindows();
            //    MessageBox.Show("Der Ordner " + wpfDirectory.Text + " ist nicht vorhanden.");
            //}
            //disabledBtn(sDir, currDir);
        }


        /**
         */
        private void disabledBtn(string docPath, string currDir)
        {
            if ((Directory.Exists(docPath + currDir) &&  !File.Exists(OutputFile(docPath, currDir, wpfDirectory.Text))) )
            {
                btnSave.IsEnabled = true;
            } else
            {
                btnSave.IsEnabled = false;
            }
        }


        /**
         */
        private void readDirectory(string[] files, string[] filetext)
        {
            if(files.Length > 0)
            {
                foreach (string filename in files)
                    wpfListView.Items.Add(filename);

                foreach (string text in filetext)
                    wpfTextbox.AppendText(text+"\n");

                foreach (string filename in files)
                    wpfTestbox.Text = filename;
            }   
        }


        /**
         */
        private void wpfListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //wpfListView.SelectedItem.ToString();
        }


        /**
         */
        private void btnNew_Click(object sender, RoutedEventArgs e)
        {
            // Set a variable to the Documents path.
            string docPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            string currDir = '/' + wpfDirectory.Text;
            string textfile = OutputFile(docPath, currDir, wpfDirectory.Text);
            File.Delete(textfile);
            clearWindows();
            disabledBtn(docPath, currDir);
        }


        /**
         */
        private void clearWindows()
        {
            wpfListView.Items.Clear();
            wpfTextbox.Clear();
        }


        /**
         */
        private void btnInfo_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Protokolliersoftware\nVersion: 1.0.0\n(c) 2023/"+ DateTime.Now.ToString("yy")+" Nico Anders", "Autoklav " + DateTime.Now.ToString("yy"));
        }

        /**
         */
        private void btnExit_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void wpfTestbox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
    }
}