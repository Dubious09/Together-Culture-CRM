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
using System.Windows.Threading;
using MySql.Data;
using MySql.Data.MySqlClient;

namespace Together_Culture_CRM
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        MySqlConnection connection;
        int loggedInUserID = 0;

        public MainWindow()
        {
            InitializeComponent();
            mysqlConnection();
        }

        public void SetLoggedInUserID(int id) => loggedInUserID = id;

        public int GetUserID() => loggedInUserID;

        public void mysqlConnection()
        {
            string connStr = $"server={Constants.Server};user={Constants.Username};database={Constants.Database};password={Constants.Password}";
            MySqlConnection conn = new MySqlConnection(connStr);
            try
            {
                Console.WriteLine("Connecting to MySQL...");
                //conn.Open();
                connection = conn;
                // Perform database operations
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            //conn.Close();
            Console.WriteLine("Done.");
        }

        public MySqlConnection GetDatabaseConnection()
        {
            return new MySqlConnection(connection.ConnectionString);
        }

        public Frame GetPrimaryFrame() => Primary;

        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e) => DragMove();

        private void BtnClose_Click(object sender, RoutedEventArgs e) => Application.Current.Shutdown();

        private void BtnMaximise_Click(object sender, RoutedEventArgs e)
        {
            if (WindowState == WindowState.Maximized)
                WindowState = WindowState.Normal;
            else WindowState = WindowState.Maximized;
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
            btnEvents.IsChecked = false;
            btnCalander.IsChecked = false;
            btnMe.IsChecked = false;
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
    }
}
