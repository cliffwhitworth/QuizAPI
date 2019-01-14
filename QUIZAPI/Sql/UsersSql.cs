using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using QuizAPI.Models;
using MySql.Data;
using System.Security.Cryptography;
using System.Text;
using System.Configuration;

namespace QuizAPI.Sql
{
    public class UsersSql
    {
        private MySql.Data.MySqlClient.MySqlConnection conn;

        public UsersSql()
        {
            string myConnectionString = ConfigurationManager.ConnectionStrings["dbConnectionString"].ConnectionString;
            try
            {
                conn = new MySql.Data.MySqlClient.MySqlConnection();
                conn.ConnectionString = myConnectionString;
                conn.Open();
            }
            catch (MySql.Data.MySqlClient.MySqlException ex)
            {

            }
        }

        public ArrayList getUsers()
        {
            ArrayList userArray = new ArrayList();
            MySql.Data.MySqlClient.MySqlDataReader mySqlReader = null;
            string sql = "Select * from quiz_taker_info";
            MySql.Data.MySqlClient.MySqlCommand cmd = new MySql.Data.MySqlClient.MySqlCommand(sql, conn);
            mySqlReader = cmd.ExecuteReader();
            while (mySqlReader.Read())
            {
                User user = new User();
                user.Id = mySqlReader.GetInt32(0);
                user.Firstname = mySqlReader.IsDBNull(1) ? null : mySqlReader.GetString(1);
                user.Middlename = mySqlReader.IsDBNull(2) ? null : mySqlReader.GetString(2);
                user.Lastname = mySqlReader.IsDBNull(3) ? null : mySqlReader.GetString(3);
                user.Username = mySqlReader.IsDBNull(4) ? null : mySqlReader.GetString(4);
                userArray.Add(user);
            };
            return userArray;
        }

        public User getUser(int id)
        {
            User user = new User();
            MySql.Data.MySqlClient.MySqlDataReader mySqlReader = null;
            string sql = "Select * from quiz_taker_info where id = " + id.ToString();
            MySql.Data.MySqlClient.MySqlCommand cmd = new MySql.Data.MySqlClient.MySqlCommand(sql, conn);
            mySqlReader = cmd.ExecuteReader();
            if (mySqlReader.Read())
            {
                user.Id = mySqlReader.GetInt32(0);
                user.Firstname = mySqlReader.IsDBNull(1) ? null : mySqlReader.GetString(1);
                user.Middlename = mySqlReader.IsDBNull(2) ? null : mySqlReader.GetString(2);
                user.Lastname = mySqlReader.IsDBNull(3) ? null : mySqlReader.GetString(3);
                user.Username = mySqlReader.IsDBNull(4) ? null : mySqlReader.GetString(4);
                return user;
            }
            else
            {
                return null;
            };
        }

        public bool deleteUser(int id)
        {
            User user = new User();
            MySql.Data.MySqlClient.MySqlDataReader mySqlReader = null;
            string sql = "Select * from quiz_taker_info where id = " + id.ToString();
            MySql.Data.MySqlClient.MySqlCommand cmd = new MySql.Data.MySqlClient.MySqlCommand(sql, conn);
            mySqlReader = cmd.ExecuteReader();
            if (mySqlReader.Read())
            {
                mySqlReader.Close();
                sql = "Delete from quiz_taker_info where id = " + id.ToString();
                cmd = new MySql.Data.MySqlClient.MySqlCommand(sql, conn);
                cmd.ExecuteNonQuery();
                return true;
            }
            else
            {
                return false;
            };
        }

        public bool putUser(int id, User user)
        {
            MySql.Data.MySqlClient.MySqlDataReader mySqlReader = null;
            string sql = "Select * from quiz_taker_info where id = " + id.ToString();
            MySql.Data.MySqlClient.MySqlCommand cmd = new MySql.Data.MySqlClient.MySqlCommand(sql, conn);
            mySqlReader = cmd.ExecuteReader();
            if (mySqlReader.Read())
            {
                mySqlReader.Close();
                sql = "Update quiz_taker_info set first_name = '" + user.Firstname + "', " +
                    "middle_name = '" + user.Middlename + "', " +
                    "last_name = '" + user.Lastname + "', " +
                    "email = '" + user.Username + "', " +
                    "group_id = '" + user.GroupId + "', " +
                    "password = '" + user.Password + "' " +
                    "salt = '" + user.Salt + "' where id = " + id.ToString();
                cmd = new MySql.Data.MySqlClient.MySqlCommand(sql, conn);
                cmd.ExecuteNonQuery();
                return true;
            }
            else
            {
                return false;
            };
        }

        public long postUser(User user)
        {
            string sql = "Insert into quiz_taker_info(first_name, middle_name, last_name, email, group_id, password, salt) values('" + user.Firstname + "', '" + user.Middlename + "', '" + user.Lastname + "', '" + user.Username + "', '" + user.GroupId + "', '" + user.Password + "', '" + user.Salt + "')";
            MySql.Data.MySqlClient.MySqlCommand cmd = new MySql.Data.MySqlClient.MySqlCommand(sql, conn);
            cmd.ExecuteNonQuery();
            long id = cmd.LastInsertedId;
            return id;
        }

        private string getUserName(string userName)
        {
            MySql.Data.MySqlClient.MySqlDataReader mySqlReader = null;
            string sql = "Select salt from quiz_taker_info where email = '" + userName.ToString() + "'";
            MySql.Data.MySqlClient.MySqlCommand cmd = new MySql.Data.MySqlClient.MySqlCommand(sql, conn);
            mySqlReader = cmd.ExecuteReader();
            string salt;
            if (mySqlReader.Read())
            {
                salt = mySqlReader.GetString(0);
            }
            else
            {
                salt = null;
            }

            mySqlReader.Close();
            return salt;
        }

        public bool checkLogin(User user)
        {
            MySql.Data.MySqlClient.MySqlDataReader mySqlReader = null;
            var salty = GetHash(user.Password + getUserName(user.Username.ToString()));
            string sql = "Select * from quiz_taker_info where email = '" + user.Username.ToString() + "' and password = '" + salty + "'";
            MySql.Data.MySqlClient.MySqlCommand cmd = new MySql.Data.MySqlClient.MySqlCommand(sql, conn);
            mySqlReader = cmd.ExecuteReader();
            if (mySqlReader.Read())
            {
                return true;
            }
            else
            {
                return false;
            };
        }

        static string ComputeSha256Hash(string rawData)
        {
            // Create a SHA256   
            using (SHA256 sha256Hash = SHA256.Create())
            {
                // ComputeHash - returns byte array  
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(rawData));

                // Convert byte array to a string   
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                return builder.ToString();
            }
        }

        private static string GetHash(string text)
        {
            // SHA512 is disposable by inheritance.  
            using (var sha256 = SHA256.Create())
            {
                // Send a sample text to hash.  
                var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(text));
                // Get the hashed string.  
                return BitConverter.ToString(hashedBytes).Replace("-", "").ToLower();
            }
        }

        private static string GetSalt()
        {
            byte[] bytes = new byte[128 / 8];
            using (var keyGenerator = RandomNumberGenerator.Create())
            {
                keyGenerator.GetBytes(bytes);
                return BitConverter.ToString(bytes).Replace("-", "").ToLower();
            }
        }
    }
}