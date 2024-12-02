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
                                                  "(Password, UserType, CreateAt, IsActive, FirstName, LastName, MailingList)" +
                                                  "VALUES (@Password, 'NonMember', NOW(), 1, @FirstName, @LastName, 1)";

        internal static string InsertNewEmail = "INSERT INTO email" +
                                                "(User_ID, Name, Email)" +
                                                "VALUES (LAST_INSERT_ID(), 'Account', @Email)";

        internal static string CheckIfUniqueEmail = "SELECT COUNT(1) FROM email WHERE Email = @Email";

        internal static string GetLastID = "SELECT LAST_INSERT_ID()";

        internal static string GetUsersNames = "SELECT FirstName, LastName FROM users WHERE User_ID = @ID";

        internal static string CheckLoginDetials = "SELECT User_ID FROM email " +
                                                   "WHERE email.Email = @Email AND User_ID IN " +
                                                   "(SELECT User_ID FROM users WHERE Password = @Password)";

        internal static string GetUserID = "SELECT FirstName, LastName FROM Users WHERE User_ID = @ID";
    }
}
