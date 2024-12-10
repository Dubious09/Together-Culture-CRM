using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xceed.Wpf.AvalonDock.Themes;

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
        internal static string InsertNewProfile = @"INSERT INTO users
                                                    (Password, UserType, CreateAt, IsActive, FirstName, LastName, MailingList)
                                                    VALUES (@Password, 'NonMember', NOW(), 1, @FirstName, @LastName, 1)";

        internal static string InsertNewEmail = @"INSERT INTO emails
                                                  (User_ID, Name, Email)
                                                  VALUES (LAST_INSERT_ID(), 'Account', @Email)";

        internal static string CheckIfUniqueEmail = @"SELECT COUNT(1) FROM emails
                                                      WHERE Email = @Email";

        internal static string GetLastID = @"SELECT LAST_INSERT_ID()";

        internal static string GetUsersNames = @"SELECT FirstName, LastName FROM users
                                                 WHERE User_ID = @ID";

        internal static string CheckLoginDetials = @"SELECT u.User_ID, u.Image
                                                     FROM users u
                                                     JOIN emails e ON u.User_ID = e.User_ID
                                                     WHERE e.Email = @Email AND u.Password = @Password";

        internal static string GetDetails = @"SELECT 
                                                  u.FirstName, u.LastName, u.CreateAt, Image,
                                                  e.Email, 
                                                  a.Name, a.Line_1, a.Line_2, a.County, a.Postcode,
                                                  p.PNumber
                                              FROM Users u
                                              LEFT JOIN emails e ON u.User_ID = e.User_ID
                                              LEFT JOIN addresses a ON u.User_ID = a.User_ID
                                              LEFT JOIN phonenumbers p ON u.User_ID = p.User_ID
                                              WHERE u.User_ID = @UserID";

        internal static string GetAddressDetails = @"SELECT Name, Line_1, Line_2, County, Postcode
                                                     FROM addresses
                                                     WHERE User_ID = @UserID";

        internal static string InsertImage = @"UPDATE Users SET Image = @Image
                                               WHERE User_ID = @UserID";

        internal static string GetImage = "SELECT Image FROM Users WHERE User_ID = @UserID";

        internal static string InsertNullPhoneNumber = @"INSERT INTO phonenumbers (User_ID, PNumber)
                                                         VALUES (@UserID, NULL)";

        internal static string InsertNullAddress = @"INSERT INTO addresses (User_ID, Name, Line_1, Line_2, County, Postcode)
                                                     VALUES (@UserID, NULL, NULL, NULL, NULL, NULL)";

        internal static string updateProfile = @"UPDATE Users u
                                                      JOIN emails e ON u.User_ID = e.User_ID
                                                      JOIN addresses a ON u.User_ID = a.User_ID
                                                      JOIN phonenumbers p ON u.User_ID = p.User_ID
                                                      SET 
                                                          u.FirstName = @FirstName,
                                                          u.LastName = @LastName,
                                                          e.Email = @Email,
                                                          a.Name = @AddressName,
                                                          a.Line_1 = @AddressLineOne,
                                                          a.Line_2 = @AddressLineTwo,
                                                          a.County = @AddressCounty,
                                                          a.Postcode = @AddressPostcode,
                                                          p.PNumber = @PhoneNumber
                                                      WHERE u.User_ID = @UserID";


    }
}
