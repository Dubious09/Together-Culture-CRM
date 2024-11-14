using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
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
    /// Interaction logic for LoginRegister.xaml
    /// </summary>
    public partial class LoginRegister : Page
    {
        // Instance of the MainWindow
        public MainWindow _mainWindow;

        public LoginRegister()
        {
            InitializeComponent();
            _mainWindow = Application.Current.MainWindow as MainWindow;

            if (_mainWindow == null)
            {
                // Handle the case where MainWindow is not available
                MessageBox.Show("MainWindow is not available.");
            }
        }

        // This method is called when the Register button is clicked
        // It will attempt to access to the pre-establish connection to the data base before it tries to register a new user
        // The method will perform checks for the user input and then execute the SQL command to insert the new user into the database
        public void BtnRegister_Click(object sender, RoutedEventArgs e)
        {
            if (_mainWindow != null)
            {
                if (string.IsNullOrEmpty(TbFirstName.Text) ||
                    string.IsNullOrEmpty(TbLastName.Text) ||
                    string.IsNullOrEmpty(PbNewPassword.Password) ||
                    string.IsNullOrEmpty(PbConfirmNewPassword.Password) ||
                    string.IsNullOrEmpty(TbNewEmail.Text) || 
                    string.IsNullOrEmpty(TbConfirmNewEmail.Text))
                {
                    // One or more fields are empty
                    RegisterTopText.Foreground= Brushes.Red;
                    RegisterTopText.Text = "Please fill in all fields";
                    return;
                }

                if (PbNewPassword.Password != PbConfirmNewPassword.Password)
                {
                    // Passwords do not match
                    RegisterTopText.Foreground = Brushes.Red;
                    RegisterTopText.Text = "Passwords do not match";
                    return;
                }

                if (TbNewEmail.Text != TbConfirmNewEmail.Text)
                {
                    // Emails do not match
                    RegisterTopText.Foreground = Brushes.Red;
                    RegisterTopText.Text = "Emails do not match";
                    return;
                }

                // Check if the email is already in use
                using (MySqlConnection conn = _mainWindow.GetDatabaseConnection())
                {
                    try
                    {
                        conn.Open();
                        MySqlCommand cmdUniqueEmail = conn.CreateCommand();
                        cmdUniqueEmail.CommandText = Constants.CheckIfUniqueEmail;
                        cmdUniqueEmail.Parameters.AddWithValue("@Email", TbNewEmail.Text);

                        int count = Convert.ToInt32(cmdUniqueEmail.ExecuteScalar());
                        if (count > 0)
                        {
                            RegisterTopText.Foreground = Brushes.Red;
                            RegisterTopText.Text = "Email already in use";
                            conn.Close();
                            return;
                        }
                        conn.Close();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("An error occurred: " + ex.Message);
                    }
                }

                // Insert the new user into the database
                using (MySqlConnection conn = _mainWindow.GetDatabaseConnection())
                {
                    try
                    {
                        conn.Open();
                        MySqlCommand cmdUserTable = conn.CreateCommand();
                        // Adding parameters to the SQL command
                        cmdUserTable.CommandText = Constants.InsertNewProfile;
                        cmdUserTable.Parameters.AddWithValue("@FirstName", TbFirstName.Text);
                        cmdUserTable.Parameters.AddWithValue("@LastName", TbLastName.Text);
                        cmdUserTable.Parameters.AddWithValue("@Password", PbNewPassword.Password);
                        
                        cmdUserTable.ExecuteNonQuery();

                        MySqlCommand cmdGetID = conn.CreateCommand();
                        cmdGetID.CommandText = Constants.GetLastID;

                        _mainWindow.SetLoggedInUserID(Convert.ToInt32(cmdGetID.ExecuteScalar()));

                        MySqlCommand cmdEmailTable = conn.CreateCommand();
                        cmdEmailTable.CommandText = Constants.InsertNewEmail;
                        cmdEmailTable.Parameters.AddWithValue("@Email", TbNewEmail.Text);

                        cmdEmailTable.ExecuteNonQuery();
                        conn.Close();

                        // Navigate to the Confirmation screen
                        Frame Primary = _mainWindow.GetPrimaryFrame();
                        Primary.Content = new PostRegistration();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("An error occurred: " + ex.Message);
                    }
                }
            }
            else
            {
                MessageBox.Show("MainWindow is not available.");
            }
        }

        // This method is called when the Login button is clicked
        // It will attempt to access to the pre-establish connection to the data base before it tries to login the user
        // The method will perform checks for the user input and then execute the SQL command to check the login details
        private void LoginBtn_Click(object sender, RoutedEventArgs e)
        {
            if (_mainWindow != null)
            {
                if (string.IsNullOrEmpty(TbEmail.Text) || string.IsNullOrEmpty(PwField.Password))
                {
                    // One or more fields are empty
                    LoginTopText.Foreground = Brushes.Red;
                    LoginTopText.Text = "Please fill in all fields";
                    return;
                }

                using (MySqlConnection conn = _mainWindow.GetDatabaseConnection())
                {
                    try
                    {
                        conn.Open();
                        MySqlCommand cmdLogin = conn.CreateCommand();
                        cmdLogin.CommandText = Constants.CheckLoginDetials;
                        cmdLogin.Parameters.AddWithValue("@Email", TbEmail.Text);
                        cmdLogin.Parameters.AddWithValue("@Password", PwField.Password);

                        // Execute the SQL command and check if the user exists/credentials are correct
                        MySqlDataReader reader = cmdLogin.ExecuteReader();
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                _mainWindow.SetLoggedInUserID(reader.GetInt32(0));
                            }
                            conn.Close();

                            // Navigate to the HomeDashboard
                            Frame Primary = _mainWindow.GetPrimaryFrame();
                            Primary.Content = new HomeDashboard();
                        }
                        else
                        {
                            // Invalid email or password
                            LoginTopText.Foreground = Brushes.Red;
                            LoginTopText.Text = "Invalid email or password";
                            PwField.Clear();
                            conn.Close();
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("An error occurred: " + ex.Message);
                        Console.WriteLine(ex.Message);
                    }
                }

            }
            else
            {
                MessageBox.Show("MainWindow is not available.");
            }
        }
    }
}
