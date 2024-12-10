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
    public partial class ProfileEditor : Page
    {
        public MainWindow _mainWindow;
        int userID;

        public ProfileEditor()
        {
            InitializeComponent();
            _mainWindow = Application.Current.MainWindow as MainWindow;

            // Retrieve the user ID from the MainWindow
            userID = _mainWindow.GetUserID();

            if (_mainWindow != null)
            {
                LoadUserProfile();
            }
            else
            {
                MessageBox.Show("MainWindow is not available.");
            }
        }

        string path;
        string fileName;
        BitmapImage profilePicture;
        bool imageUpdated = false;

        // This method will open a file dialog to select an image
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
                imageUpdated = true;
            }
            else
            {
                // No image selected
                MessageBox.Show("No image was selected");
            }
        }

        private void BtnDeleteAccount_Click(object sender, RoutedEventArgs e)
        {
            var sqlCommands = new SqlCommands();
            MySqlConnection conn = _mainWindow.GetDatabaseConnection();

            try
            {
                conn.Open();

                // Delete the user's data from all related tables
                string deleteEmailsQuery = "DELETE FROM emails WHERE User_ID = @UserID";
                string deleteAddressesQuery = "DELETE FROM addresses WHERE User_ID = @UserID";
                string deletePhoneNumbersQuery = "DELETE FROM phonenumbers WHERE User_ID = @UserID";
                string deleteUserQuery = "DELETE FROM users WHERE User_ID = @UserID";

                var parameters = new List<Tuple<string, object>>
                {
                    new Tuple<string, object>("@UserID", userID)
                };

                // Execute the delete commands
                sqlCommands.ExecuteSqlCommand(conn, deleteEmailsQuery, parameters, CommandType.ExecuteNonQuery);
                sqlCommands.ExecuteSqlCommand(conn, deleteAddressesQuery, parameters, CommandType.ExecuteNonQuery);
                sqlCommands.ExecuteSqlCommand(conn, deletePhoneNumbersQuery, parameters, CommandType.ExecuteNonQuery);
                sqlCommands.ExecuteSqlCommand(conn, deleteUserQuery, parameters, CommandType.ExecuteNonQuery);

                MessageBox.Show("User profile deleted successfully.");

                // Reset the logged in user ID
                _mainWindow.SetLoggedInUserID(0);

                // Navigate to the login or home screen
                Frame Primary = _mainWindow.GetPrimaryFrame();
                Primary.Content = new HomeDashboard();
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

        private void BtnDeleteData_Click(object sender, RoutedEventArgs e)
        {

        }

        private void BtnSaveChanges_Click(object sender, RoutedEventArgs e)
        {
            if (TbAddressPhoneNumber.Text != TbAddressPhoneNumberOne.Text)
            {
                PhoneNumberLabel.Foreground = Brushes.Red;
                ConfirmPhoneNumberLabel.Foreground = Brushes.Red;
                return;
            }
            else
            {
                Brush brush = (Brush)(new BrushConverter().ConvertFrom("#FDF6EC"));
                PhoneNumberLabel.Foreground = brush;
                ConfirmPhoneNumberLabel.Foreground = brush;
            }

            if (TbAddressPasswordOne.Password != TbAddressPasswordTwo.Password)
            {
                PasswordLabel.Foreground = Brushes.Red;
                ConfirmPasswordLabel.Foreground = Brushes.Red;
                return;
            }
            else
            {
                Brush brush = (Brush)(new BrushConverter().ConvertFrom("#FDF6EC"));
                PasswordLabel.Foreground = brush;
                ConfirmPasswordLabel.Foreground = brush;
            }

            if (imageUpdated)
            {
                InsertImage();
                _mainWindow.updateProfilePicture(profilePicture);
            }

            var sqlCommands = new SqlCommands();
            MySqlConnection conn = _mainWindow.GetDatabaseConnection();

            try
            {
                conn.Open();

                string updateProfileQuery = Constants.updateProfile;

                var parameters = new List<Tuple<string, object>>
                {
                    new Tuple<string, object>("@FirstName", TbFirstName.Text),
                    new Tuple<string, object>("@LastName", TbLastName.Text),
                    new Tuple<string, object>("@Email", TbAddressEmail.Text),
                    new Tuple<string, object>("@AddressName", TbAddressName.Text),
                    new Tuple<string, object>("@AddressLineOne", TbAddressLineOne.Text),
                    new Tuple<string, object>("@AddressLineTwo", TbAddressLineTwo.Text),
                    new Tuple<string, object>("@AddressCounty", TbAddressCounty.Text),
                    new Tuple<string, object>("@AddressPostcode", TbAddressPostcode.Text),
                    new Tuple<string, object>("@PhoneNumber", TbAddressPhoneNumber.Text),
                    new Tuple<string, object>("@UserID", userID)
                };

                sqlCommands.ExecuteSqlCommand(conn, updateProfileQuery, parameters, CommandType.ExecuteNonQuery);

                // Navigate to the HomeDashboard
                Frame Primary = _mainWindow.GetPrimaryFrame();
                Primary.Content = new HomeDashboard();
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

        // This method will insert the image into the database
        private void InsertImage()
        {
            if (string.IsNullOrEmpty(path))
            {
                MessageBox.Show("No image selected.");
                return;
            }

            // Convert the image to a byte array
            byte[] image = null;
            using (FileStream stream = new FileStream(path, FileMode.Open, FileAccess.Read))
            {
                using (BinaryReader reader = new BinaryReader(stream))
                {
                    image = reader.ReadBytes((int)stream.Length);
                }
            }

            profilePicture = _mainWindow.ConvertToBitmapImage(image);

            var sqlCommands = new SqlCommands();
            MySqlConnection conn = _mainWindow.GetDatabaseConnection();

            try
            {
                conn.Open();

                string insertImageQuery = Constants.InsertImage;

                var parameters = new List<Tuple<string, object>>
                {
                    new Tuple<string, object>("@Image", image),
                    new Tuple<string, object>("@UserID", userID)
                };

                sqlCommands.ExecuteSqlCommand(conn, insertImageQuery, parameters, CommandType.ExecuteNonQuery);
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

        // This method will load the user profile details
        private void LoadUserProfile()
        {
            var sqlCommands = new SqlCommands();
            MySqlConnection conn = _mainWindow.GetDatabaseConnection();

            try
            {
                conn.Open();

                // Get the user details from multiple tables at once using a JOIN query
                string getDetailsQuery = Constants.GetDetails;

                var parameters = new List<Tuple<string, object>>
                {
                    new Tuple<string, object>("@UserID", userID)
                };

                using (MySqlDataReader reader = (MySqlDataReader)sqlCommands.ExecuteSqlCommand(
                    conn, getDetailsQuery, parameters, CommandType.ExecuteReader))
                {
                    if (reader != null && reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            // Set the textboxes to the user's details
                            TbFirstName.Text = reader["FirstName"].ToString();
                            TbLastName.Text = reader["LastName"].ToString();
                            TbProfileName.Text = reader["FirstName"].ToString() + " " + reader["LastName"].ToString();
                            TbMemberSince.Text = "Profile created: " + reader["CreateAt"].ToString();
                            TbAddressEmail.Text = reader["Email"].ToString();
                            TbAddressName.Text = reader["Name"].ToString();
                            TbAddressLineOne.Text = reader["Line_1"].ToString();
                            TbAddressLineTwo.Text = reader["Line_2"].ToString();
                            TbAddressCounty.Text = reader["County"].ToString();
                            TbAddressPostcode.Text = reader["Postcode"].ToString();
                            TbAddressPhoneNumber.Text = reader["PNumber"].ToString();

                            // Retrieve and display the image
                            if (reader["Image"] != DBNull.Value)
                            {
                                byte[] imageBytes = (byte[])reader["Image"];
                                using (MemoryStream ms = new MemoryStream(imageBytes))
                                {
                                    BitmapImage bitmap = new BitmapImage();
                                    bitmap.BeginInit();
                                    bitmap.StreamSource = ms;
                                    bitmap.CacheOption = BitmapCacheOption.OnLoad;
                                    bitmap.EndInit();
                                    ImgProfilePreview.ImageSource = bitmap;
                                }
                            }
                        }
                    }
                    else
                    {
                        MessageBox.Show("User not found.");
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
    }
}
