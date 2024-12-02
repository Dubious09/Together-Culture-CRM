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
                    RegisterTopText.Foreground = Brushes.Red;
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

                var sqlCommands = new SqlCommands();
                MySqlConnection conn = _mainWindow.GetDatabaseConnection();

                try
                {
                    conn.Open();

                    // Check if the email is already in use
                    string checkEmailQuery = Constants.CheckIfUniqueEmail;
                    var checkEmailParams = new List<Tuple<string, object>>
                    {
                        new Tuple<string, object>("@Email", TbNewEmail.Text)
                    };

                    int count = Convert.ToInt32(sqlCommands.ExecuteSqlCommand(conn, checkEmailQuery, checkEmailParams, CommandType.ExecuteScalar));
                    if (count > 0)
                    {
                        RegisterTopText.Foreground = Brushes.Red;
                        RegisterTopText.Text = "Email already in use";
                        return;
                    }

                    // Insert the new user into the database
                    string insertUserQuery = Constants.InsertNewProfile;
                    var insertUserParams = new List<Tuple<string, object>>
                    {
                        new Tuple<string, object>("@FirstName", TbFirstName.Text),
                        new Tuple<string, object>("@LastName", TbLastName.Text),
                        new Tuple<string, object>("@Password", PbNewPassword.Password)
                    };

                    sqlCommands.ExecuteSqlCommand(conn, insertUserQuery, insertUserParams, CommandType.ExecuteNonQuery);

                    // Get the last inserted ID
                    string getLastIdQuery = Constants.GetLastID;
                    int userId = Convert.ToInt32(sqlCommands.ExecuteSqlCommand(conn, getLastIdQuery, null, CommandType.ExecuteScalar));
                    _mainWindow.SetLoggedInUserID(userId);

                    // Insert the new email into the database
                    string insertEmailQuery = Constants.InsertNewEmail;
                    var insertEmailParams = new List<Tuple<string, object>>
                    {
                        new Tuple<string, object>("@Email", TbNewEmail.Text)
                    };

                    sqlCommands.ExecuteSqlCommand(conn, insertEmailQuery, insertEmailParams, CommandType.ExecuteNonQuery);

                    // Navigate to the Confirmation screen
                    Frame Primary = _mainWindow.GetPrimaryFrame();
                    Primary.Content = new PostRegistration();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("An error occurred: " + ex.Message);
                    Console.WriteLine("Error: " + ex.Message);
                }
                finally
                {
                    conn.Close();
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

                var sqlCommands = new SqlCommands();
                string loginQuery = Constants.CheckLoginDetials;
                var loginParams = new List<Tuple<string, object>>
                {
                    new Tuple<string, object>("@Email", TbEmail.Text),
                    new Tuple<string, object>("@Password", PwField.Password)
                };

                MySqlConnection conn = _mainWindow.GetDatabaseConnection();
                try
                {
                    conn.Open();
                    using (MySqlDataReader reader = (MySqlDataReader)sqlCommands.ExecuteSqlCommand(
                        conn, loginQuery, loginParams, CommandType.ExecuteReader))
                    {
                        if (reader != null && reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                Console.WriteLine("Returned ID: " + reader.GetInt32(0));
                                _mainWindow.SetLoggedInUserID(reader.GetInt32(0));
                            }

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
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("An error occurred: " + ex.Message);
                    Console.WriteLine("Error: " + ex.Message);
                }
                finally
                {
                    conn.Close();
                }
            }
            else
            {
                MessageBox.Show("MainWindow is not available.");
            }
        }
    }
}
