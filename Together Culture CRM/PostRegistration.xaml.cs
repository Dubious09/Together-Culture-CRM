using MySql.Data.MySqlClient;
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

namespace Together_Culture_CRM
{
    /// <summary>
    /// Interaction logic for PostRegistration.xaml
    /// </summary>
    public partial class PostRegistration : Page
    {
        public MainWindow _mainWindow;
        public PostRegistration()
        {
            InitializeComponent();
            _mainWindow = Application.Current.MainWindow as MainWindow;

            Console.WriteLine(_mainWindow.GetUserID());

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
                        WelcomeText.Text = "Welcome " + reader.GetString(0) + " " + reader.GetString(1) + " to the Together Culture CRM!\n" +
                            "You will be redirected to the profile editor to complete your profile";
                    }
                    conn.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("An error occurred: " + ex.Message);
                }
            }
        }

        private void BtnTakeMeThere_Click(object sender, RoutedEventArgs e)
        {
            Frame Primary = _mainWindow.GetPrimaryFrame();
            Primary.Content = new ProfileEditor();
        }
    }
}
