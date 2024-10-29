using Microsoft.Win32;
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
    /// Interaction logic for CreateEvent.xaml
    /// </summary>
    public partial class CreateEvent : Page
    {
        public CreateEvent()
        {
            InitializeComponent();
        }

        string path;
        string fileName;

        private void BtnCreateEventOpenFileDialog_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Image files (*.png;*.jpeg;*.jpg)|*.png;*.jpeg;*.jpg";

            bool? success = openFileDialog.ShowDialog();
            if (success == true)
            {
                path = openFileDialog.FileName;
                fileName = openFileDialog.SafeFileName;

                TbCreateEventFileDialog.Text = fileName;
                ImgEventPreview.ImageSource = new BitmapImage(new Uri(path));
            }
            else
            {
                // No image selected
                // MessageBox.Show("No image selected");
            }
        }
    }
}
