using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Together_Culture_CRM
{
    internal class SqlCommands
    {
        private MySqlCommand AddParameters(MainWindow mainWindow, string query, List<Tuple<string, object>> parameters)
        {
            MySqlCommand cmdOut;

            using (MySqlConnection conn = mainWindow.GetDatabaseConnection())
            {
                try
                {
                    conn.Open();
                    MySqlCommand cmd = conn.CreateCommand();
                    cmd.CommandText = query;

                    // Adding parameters to the SQL command
                    foreach (var param in parameters)
                    {
                        cmd.Parameters.AddWithValue(param.Item1, param.Item2);
                    }

                    cmdOut = cmd;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("An error occurred: " + ex.Message);
                    cmdOut = null;
                }
            }

            return cmdOut;
        }
    }
}
