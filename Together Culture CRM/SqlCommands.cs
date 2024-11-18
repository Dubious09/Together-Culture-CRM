using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Windows;

namespace Together_Culture_CRM
{
    internal class SqlCommands
    {
        // Execute a SQL command with parameters and return the result        
        public object ExecuteSqlCommand(MySqlConnection conn, string query, List<Tuple<string, object>> parameters, CommandType commandType)
        {
            try
            {
                MySqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = query;

                // Add parameters to the command
                foreach (var param in parameters)
                {
                    cmd.Parameters.AddWithValue(param.Item1, param.Item2);
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
            catch (Exception ex) // Catch any SQL exceptions and display an error message
            {
                MessageBox.Show("An error occurred: " + ex.Message);
                Console.WriteLine("Error: " + ex.Message);
                return null;
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
}