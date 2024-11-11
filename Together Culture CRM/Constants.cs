using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Together_Culture_CRM
{
    internal class Constants
    {
        // MySQL connection details
        internal static string Username = "root";
        internal static string Password = "password";
        internal static string Database = "db";
        internal static string Server = "127.0.0.1";

        // SQL queries
        internal static string InsertNewProfile = "INSERT INTO users" +
                                                  "(Password, CreatedAt, IsActive, FirstName, LastName, MailingList)" +
                                                  "VALUES (@Password , NOW(), 1, @FirstName, @LastName, 1) SslMode=None";
    }
}
