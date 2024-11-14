using Microsoft.Win32;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.IO;
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

namespace Together_Culture_CRM
{
    /// <summary>
    /// Interaction logic for ProfileEditor.xaml
    /// </summary>
    public partial class ProfileEditor : Page
    {
        public MainWindow _mainWindow;

        public ProfileEditor()
        {
            InitializeComponent();
            _mainWindow = Application.Current.MainWindow as MainWindow;

            // Retrieve the first and last name of the user from the database
            using (MySqlConnection conn = _mainWindow.GetDatabaseConnection())
            {
                try
                {
                    conn.Open();
                    MySqlCommand cmdGetUsersNames = conn.CreateCommand();
                    cmdGetUsersNames.CommandText = Constants.GetUsersNames;
                    cmdGetUsersNames.Parameters.AddWithValue("@ID", _mainWindow.GetUserID());

                    MySqlDataReader reader = cmdGetUsersNames.ExecuteReader();

                    while (reader.Read())
                    {
                        
                    }
                    conn.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("An error occurred: " + ex.Message);
                }
            }
        }

        string path;
        string fileName;

        private void BtnProfileEditOpenFileDialog_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Image files (*.png;*.jpeg;*.jpg)|*.png;*.jpeg;*.jpg";

            bool? success = openFileDialog.ShowDialog();
            if (success == true)
            {
                path = openFileDialog.FileName;
                fileName = openFileDialog.SafeFileName;

                TbProfileEditFileDialog.Text = fileName;
                ImgProfilePreview.ImageSource = new BitmapImage(new Uri(path));
            }
            else
            {
                // No image selected
                MessageBox.Show("No image was selected");
            }
        }
    }
}
