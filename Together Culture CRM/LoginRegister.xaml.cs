using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
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
                        cmdUserTable.Parameters.AddWithValue("@LastName", TbLastName.Text);
                        cmdUserTable.Parameters.AddWithValue("@Password", PbNewPassword.Password);
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

        private void LoginBtn_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
