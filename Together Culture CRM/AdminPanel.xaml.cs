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
    /// Interaction logic for AdminPanel.xaml
    /// </summary>
    public partial class AdminPanel : Page
    {
        public AdminPanel()
        {
            InitializeComponent();
        }

        private void BtnGraphView_Click(object sender, RoutedEventArgs e) => AdminFrame.Content = new AdminGraphs();

        private void BtnTableView_Click(object sender, RoutedEventArgs e)
        {
            AdminFrame.Content = new AdminTable();
        }

        private void BtnNewEvent_Click(object sender, RoutedEventArgs e)
        {
            AdminFrame.Content = new CreateEvent();
        }
    }
}
