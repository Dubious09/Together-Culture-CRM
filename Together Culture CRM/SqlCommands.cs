using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Windows;

namespace Together_Culture_CRM
{
    public class SqlCommands
    {
        public object ExecuteSqlCommand(MySqlConnection conn, string query, List<Tuple<string, object>> parameters, CommandType commandType)
        {
            Console.WriteLine("Params: " + parameters);

            try
            {
                MySqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = query;
                //cmd.CommandType = commandType;

                // Add parameters to the command
                if (parameters != null) // Not every query has parameters
                {
                    foreach (var param in parameters)
                    {
                        cmd.Parameters.AddWithValue(param.Item1, param.Item2);
                    }
                }

                // Execute the command based on the command type
                if (commandType == CommandType.ExecuteReader)
                {
                    return cmd.ExecuteReader(CommandBehavior.CloseConnection);
                }
                else if (commandType == CommandType.ExecuteScalar)
                {
                    return cmd.ExecuteScalar();
                }
                else
                {
                    cmd.ExecuteNonQuery();
                    return null;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred: " + ex.Message);
                Console.WriteLine("Error: " + ex.Message);
                return null;
            }
        }
    }
}
// Enum for different types of SQL commands
public enum CommandType
{
    ExecuteNonQuery,
    ExecuteScalar,
    ExecuteReader
}

