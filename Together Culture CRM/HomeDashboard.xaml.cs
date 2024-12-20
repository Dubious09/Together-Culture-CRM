﻿using System;
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
    public partial class HomeDashboard : Page
    {
        public MainWindow _mainWindow;

        public HomeDashboard()
        {
            InitializeComponent();
            _mainWindow = Application.Current.MainWindow as MainWindow;

            if (_mainWindow == null)
            {
                // Handle the case where MainWindow is not available
                MessageBox.Show("MainWindow is not available.");
            }
        }

        private void BtnLoginRegister_Click(object sender, RoutedEventArgs e)
        {
            Frame Primary = _mainWindow.GetPrimaryFrame(); // Get the primary frame
            Primary.Content = new LoginRegister(); // Set the primary frame to the login/register page
        }
    }
}
