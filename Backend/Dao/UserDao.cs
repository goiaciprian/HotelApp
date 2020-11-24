using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Backend.Models;
using Backend.Utils;

namespace Backend.Dao
{
    public class UserDao
    {
        public static async Task<IEnumerable<User>> findAllAsync()
        {
            List<User> users = new List<User>();

            const string getQuery = "select * from hotelDB.dbo.Users;";

            var conn = DBConnection.openConn();
            using (var cmd = new SqlCommand(getQuery, conn))
            using (var datareader = await cmd.ExecuteReaderAsync())
            {
                while (await datareader.ReadAsync())
                {
                    User user = new User();
                    user.id = datareader.GetInt32(0);
                    user.fullName = datareader.GetString(1);
                    user.email = datareader.GetString(2);
                    user.password = datareader.GetString(3);
                    user.isAdmin = datareader.GetInt32(4);
                    users.Add(user);
                }
                cmd.Dispose();
                datareader.Close();
            }
            return users;

        }

        public static async Task<User> findByIdAsync(int id)
        {
            User user = null;
            const string getQuery = "select * from hotelDB.dbo.Users where id = @id;";

            var conn = DBConnection.openConn();
            using (var cmd = new SqlCommand(getQuery, conn))
            {
                cmd.Parameters.AddWithValue("@id", id);
                var reader = await cmd.ExecuteReaderAsync();
                if (await reader.ReadAsync())
                {
                    user = new User();
                    user.id = reader.GetInt32(0);
                    user.fullName = reader.GetString(1);
                    user.email = reader.GetString(2);
                    user.password = reader.GetString(3);
                    user.isAdmin = reader.GetInt32(4);
                }

                cmd.Dispose();
                reader.Close();
                return user;
            }
        }

        public static async Task<User> findByEmailAsync(string email)
        {
            User user = null;
            const string getQuery = "select * from hotelDB.dbo.Users where email = @email;";

            var conn = DBConnection.openConn();
            using (var cmd = new SqlCommand(getQuery, conn))
            {
                cmd.Parameters.AddWithValue("@email", email);
                var reader = await cmd.ExecuteReaderAsync();
                if (await reader.ReadAsync())
                {
                    user = new User();
                    user.id = reader.GetInt32(0);
                    user.fullName = reader.GetString(1);
                    user.email = reader.GetString(2);
                    user.password = reader.GetString(3);
                    user.isAdmin = reader.GetInt32(4);
                }

                cmd.Dispose();
                reader.Close();
                return user;
            }
        }


        public static async Task<int> addUserAsync(User newUser)
        {
            if (await findByEmailAsync(newUser.email) != null)
                return 3;

            const string insertQuery = "insert into hotelDB.dbo.Users(fullName, email, password, isAdmin) values " +
                "(@fullName, @email, @pass, @admin);";

            var conn = DBConnection.openConn();
            using (var cmd = new SqlCommand(insertQuery, conn))
            using (var dataAdapter = new SqlDataAdapter())
            {
                cmd.Parameters.AddWithValue("@fullName", newUser.fullName);
                cmd.Parameters.AddWithValue("@email", newUser.email);
                cmd.Parameters.AddWithValue("@pass", BCrypt.Net.BCrypt.EnhancedHashPassword(newUser.password));
                cmd.Parameters.AddWithValue("@admin", newUser.isAdmin);
                try
                {
                    dataAdapter.InsertCommand = cmd;
                    await dataAdapter.InsertCommand.ExecuteNonQueryAsync();

                    cmd.Dispose();
                    dataAdapter.Dispose();

                    return 1;
                }
                catch
                {
                    return -1;
                }
            }

        }

        public static async Task<int> updateUserAsync(int id, User updatedUser)
        {
            if (await findByIdAsync(id) == null)
                return 0;

            const string updateQuery = "update hotelDB.dbo.Users " +
                "set fullName = @name, email = @email, password = @pass, isAdmin = @admin where id = @id;";

            var conn = DBConnection.openConn();
            using (var cmd = new SqlCommand(updateQuery, conn))
            using (var dataAdapter = new SqlDataAdapter())
            {
                cmd.Parameters.AddWithValue("@name", updatedUser.fullName);
                cmd.Parameters.AddWithValue("@email", updatedUser.email);
                cmd.Parameters.AddWithValue("@pass", updatedUser.password);
                cmd.Parameters.AddWithValue("@admin", updatedUser.isAdmin);
                cmd.Parameters.AddWithValue("@id", id);
                try
                {
                    Console.WriteLine(updateQuery);
                    dataAdapter.InsertCommand = cmd;
                    await dataAdapter.InsertCommand.ExecuteNonQueryAsync();

                    cmd.Dispose();
                    dataAdapter.Dispose();

                    return 1;
                }
                catch
                {
                    return -1;
                }
            }
        }

        public static async Task<int> deleteUserByIdAsync(int id)
        {
            List<Rezervare> hasReservations = await RezervariDao.findAllReservationsOfOneUser(id);
            if(hasReservations.Any())
            {
                return 2;
            }
            if (await findByIdAsync(id) == null)
                return 0;

            const string deleteQuery = "delete from hotelDB.dbo.Users where id = @id;";

            var conn = DBConnection.openConn();
            using (var cmd = new SqlCommand(deleteQuery, conn))
            using (var dataAdapter = new SqlDataAdapter())
            {
                cmd.Parameters.AddWithValue("@id", id);
                try
                {
                    dataAdapter.DeleteCommand = cmd;
                    await dataAdapter.DeleteCommand.ExecuteNonQueryAsync();

                    cmd.Dispose();
                    dataAdapter.Dispose();
                    return 1;
                }
                catch
                {
                    return -1;
                }

            }
        }
    }
}