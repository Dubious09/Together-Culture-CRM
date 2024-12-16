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
using System.Windows.Threading;
using MySql.Data;
using MySql.Data.MySqlClient;

namespace Together_Culture_CRM
{
    public partial class MainWindow : Window
    {
        MySqlConnection connection;
        int loggedInUserID = 0;

        public MainWindow()
        {
            InitializeComponent(); // Load the main window
            mysqlConnection(); // Connect to the database
        }

        // Connect to the database
        public void mysqlConnection()
        {
            string connStr = $"server={Constants.Server};user={Constants.Username};database={Constants.Database};password={Constants.Password}"; // Set the connection string
            MySqlConnection conn = new MySqlConnection(connStr);
            try
            {
                connection = conn; // Set the connection
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            Console.WriteLine("Done.");
        }

        public void SetLoggedInUserID(int id) => loggedInUserID = id; // Set the logged in user ID

        public int GetUserID() => loggedInUserID;


        public MySqlConnection GetDatabaseConnection()
        {
            return new MySqlConnection(connection.ConnectionString); // Return the database connection
        }

        public Frame GetPrimaryFrame() => Primary;

        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e) => DragMove(); // Allow the window to be dragged

        private void BtnClose_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown(); // Close the application
        }

        private void BtnMaximise_Click(object sender, RoutedEventArgs e)
        {
            if (WindowState == WindowState.Maximized) // Maximise the window
                WindowState = WindowState.Normal; // If the window is already maximised, normalise it
            else WindowState = WindowState.Maximized; // Otherwise, maximise it
        }

        private void BtnMinimise_Click(object sender, RoutedEventArgs e)
        {
            //change the WindowStyle to single border just before minimising it
            WindowStyle = WindowStyle.SingleBorderWindow;
            WindowState = WindowState.Minimized;
        }

        private void MainWindow_OnActivated(object sender, EventArgs e)
        {
            // Change the WindowStyle back to None, but only after the Window has been activated
            Dispatcher.BeginInvoke(DispatcherPriority.ApplicationIdle, new Action(() => WindowStyle = WindowStyle.None));
        }

        private void BtnEvents_Click(object sender, RoutedEventArgs e) => Primary.Content = new EventFeed();

        private void BtnCalander_Click(object sender, RoutedEventArgs e) => Primary.Content = new Callander();

        private void BtnDashboard_Click(object sender, RoutedEventArgs e)
        {
            Primary.Content = new HomeDashboard();
            btnEvents.IsChecked = false; // Uncheck the events button
            btnCalander.IsChecked = false; // Uncheck the calendar button
            btnMe.IsChecked = false; // Uncheck the me button
        }

        private void btnMe_Click(object sender, RoutedEventArgs e)
        {
            if (loggedInUserID == 0)
            {
                Primary.Content = new LoginRegister();
            }
            else
            {
                Primary.Content = new ProfileEditor();
            }
        }

        // Update the profile picture
        public void updateProfilePicture(BitmapImage img)
        {
            // Update the profile picture
            ProfileIcon.ImageSource = img;
        }

        
        public BitmapImage ConvertToBitmapImage(byte[] imageBytes) // Convert an image to a bitmap image
        {
            using (MemoryStream ms = new MemoryStream(imageBytes))
            {
                BitmapImage bitmap = new BitmapImage();
                bitmap.BeginInit();
                bitmap.StreamSource = ms;
                bitmap.CacheOption = BitmapCacheOption.OnLoad;
                bitmap.EndInit();
                return bitmap;
            }
        }
    }
}
