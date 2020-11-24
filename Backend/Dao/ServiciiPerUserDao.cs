using Backend.Models;
using Backend.Utils;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace Backend.Dao
{
    public class ServiciiPerUserDao
    {
        public static async Task<List<ServiciiPerUser>> getAllUsersServicesAsync()
        {
            List<ServiciiPerUser> servicii = new List<ServiciiPerUser>();

            const string getQuery = "select ServiciiPerUser.id, ServiciiPerUser.userId, " +
                "ServiciiPerUser.serviciuId, Users.fullName, ServiciiSuplimentare.nume, ServiciiSuplimentare.pret" +
                " from hotelDB.dbo.ServiciiPerUser, hotelDB.dbo.Users, hotelDB.dbo.ServiciiSuplimentare where Users.id" +
                " = ServiciiPerUser.userId and ServiciiSuplimentare.id = ServiciiPerUser.serviciuId;";

            var conn = DBConnection.openConn();
            using (var cmd = new SqlCommand(getQuery, conn))
            {
                var dataReader = await cmd.ExecuteReaderAsync();
                while (await dataReader.ReadAsync())
                {
                    ServiciiPerUser svpu = new ServiciiPerUser();
                    svpu.id = dataReader.GetInt32(0);
                    svpu.userId = dataReader.GetInt32(1);
                    svpu.serviciuId = dataReader.GetInt32(2);
                    svpu.userName = dataReader.GetString(3);
                    svpu.numeServiciu = dataReader.GetString(4);
                    svpu.pretServiciu = dataReader.GetInt32(5);

                    servicii.Add(svpu);

                }

                cmd.Dispose();
                dataReader.Close();
            }
            return servicii;
        }

        public static async Task<ServiciiPerUser> getServiciuPerUserById(int id)
        {
            ServiciiPerUser serviciiPerUser = null;

            const string getQuery = "select ServiciiPerUser.id, ServiciiPerUser.userId, ServiciiPerUser.serviciuId, " +
                "Users.fullName, ServiciiSuplimentare.nume, ServiciiSuplimentare.pret from hotelDB.dbo.ServiciiPerUser, " +
                "hotelDB.dbo.Users, hotelDB.dbo.ServiciiSuplimentare where(Users.id = ServiciiPerUser.userId and " +
                "ServiciiSuplimentare.id = ServiciiPerUser.serviciuId) and ServiciiPerUser.id = @id;";

            var conn = DBConnection.openConn();
            using (var cmd = new SqlCommand(getQuery, conn))
            {
                cmd.Parameters.AddWithValue("@id", id);
                var dataReader = await cmd.ExecuteReaderAsync();
                if (await dataReader.ReadAsync())
                {
                    serviciiPerUser = new ServiciiPerUser();
                    serviciiPerUser.id = dataReader.GetInt32(0);
                    serviciiPerUser.userId = dataReader.GetInt32(1);
                    serviciiPerUser.serviciuId = dataReader.GetInt32(2);
                    serviciiPerUser.userName = dataReader.GetString(3);
                    serviciiPerUser.numeServiciu = dataReader.GetString(4);
                    serviciiPerUser.pretServiciu = dataReader.GetInt32(5);

                }

                cmd.Dispose();
                dataReader.Close();
            }

            return serviciiPerUser;
        }

        public static async Task<List<ServiciiPerUser>> getOneUserAllServicesAsync(int userid)
        {
            List<ServiciiPerUser> servicii = new List<ServiciiPerUser>();

            const string getQuery = "select ServiciiPerUser.id, ServiciiPerUser.userId, ServiciiPerUser.serviciuId, " +
                "Users.fullName, ServiciiSuplimentare.nume, ServiciiSuplimentare.pret from hotelDB.dbo.ServiciiPerUser, " +
                "hotelDB.dbo.Users, hotelDB.dbo.ServiciiSuplimentare where(Users.id = ServiciiPerUser.userId and " +
                "ServiciiSuplimentare.id = ServiciiPerUser.serviciuId) and ServiciiPerUser.userId = @id;";


            var conn = DBConnection.openConn();
            using (var cmd = new SqlCommand(getQuery, conn))
            {
                cmd.Parameters.AddWithValue("@id", userid);
                var dataReader = await cmd.ExecuteReaderAsync();
                while (await dataReader.ReadAsync())
                {
                    ServiciiPerUser svpu = new ServiciiPerUser();
                    svpu.id = dataReader.GetInt32(0);
                    svpu.userId = dataReader.GetInt32(1);
                    svpu.serviciuId = dataReader.GetInt32(2);
                    svpu.userName = dataReader.GetString(3);
                    svpu.numeServiciu = dataReader.GetString(4);
                    svpu.pretServiciu = dataReader.GetInt32(5);

                    servicii.Add(svpu);
                }

                cmd.Dispose();
                dataReader.Close();
            }

            return servicii;
        }

        public static async Task<int> addNewServiceToUser(ServiciiPerUser serviciiPerUser)
        {
            const string queryString = "insert into hotelDB.dbo.ServiciiPerUser(userId, serviciuId) values (@userId, @serviciuId);";

            var conn = DBConnection.openConn();
            using (var cmd = new SqlCommand(queryString, conn))
            using (var dataAdapter = new SqlDataAdapter())
            {
                cmd.Parameters.AddWithValue("@userId", serviciiPerUser.userId);
                cmd.Parameters.AddWithValue("@serviciuId", serviciiPerUser.serviciuId);
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

        public static async Task<int> deleteServicePerUserAsync(int servicePerUserId)
        {
            if (await getServiciuPerUserById(servicePerUserId) == null)
                return 0;

            const string deleteQuery = "delete from hotelDB.dbo.ServiciiPerUser where id = @id;";

            var conn = DBConnection.openConn();
            using (var cmd = new SqlCommand(deleteQuery, conn))
            using (var dataAdapter = new SqlDataAdapter())
            {
                cmd.Parameters.AddWithValue("@id", servicePerUserId);
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