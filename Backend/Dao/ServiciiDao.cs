using Backend.Models;
using Backend.Utils;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace Backend.Dao
{
    public class ServiciiDao
    {
        public static async Task<List<Serviciu>> findAllServiciiAsync()
        {
            List<Serviciu> servicii = new List<Serviciu>();
            const string getAllQuery = "select * from hotelDB.dbo.ServiciiSuplimentare;";

            var conn = DBConnection.openConn();
            using (var cmd = new SqlCommand(getAllQuery, conn))
            using (var reader = await cmd.ExecuteReaderAsync())
            {
                while (await reader.ReadAsync())
                {
                    Serviciu curr = new Serviciu();
                    curr.id = reader.GetInt32(0);
                    curr.numeServiciu = reader.GetString(1);
                    curr.pret = reader.GetInt32(2);

                    servicii.Add(curr);
                }

                cmd.Dispose();
                reader.Close();
            }

            return servicii;
        }

        public static async Task<Serviciu> findServiciuByIdAsync(int id)
        {
            Serviciu serviciu = null;
            const string getAllQuery = "select * from hotelDB.dbo.ServiciiSuplimentare where id = @id;";

            var conn = DBConnection.openConn();
            using (var cmd = new SqlCommand(getAllQuery, conn))
            {
                cmd.Parameters.AddWithValue("@id", id);
                var reader = await cmd.ExecuteReaderAsync();

                if (await reader.ReadAsync())
                {
                    serviciu = new Serviciu();
                    serviciu.id = reader.GetInt32(0);
                    serviciu.numeServiciu = reader.GetString(1);
                    serviciu.pret = reader.GetInt32(2);

                }

                cmd.Dispose();
                reader.Close();
            }

            return serviciu;
        }

        public static async Task<int> addServiciuAsync(Serviciu newServiciu)
        {
            const string insertQuery = "insert into hotelDB.dbo.ServiciiSuplimentare(nume, pret) values (@numeServiciu, @pretServiciu);";

            var conn = DBConnection.openConn();
            using (var cmd = new SqlCommand(insertQuery, conn))
            using (var dataAdapter = new SqlDataAdapter())
            {
                cmd.Parameters.AddWithValue("@numeServiciu", newServiciu.numeServiciu);
                cmd.Parameters.AddWithValue("@pretServiciu", newServiciu.pret);
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

        public static async Task<int> updateServiciuAsync(int id, Serviciu updated)
        {
            if (await findServiciuByIdAsync(id) == null)
                return 0;

            const string updateQuery = "update hotelDB.dbo.ServiciiSuplimentare set nume = @numeServiciu, pret = @pretServiciu where id = @id;";

            var conn = DBConnection.openConn();
            using (var cmd = new SqlCommand(updateQuery, conn))
            using (var dataAdapter = new SqlDataAdapter())
            {
                cmd.Parameters.AddWithValue("@numeServiciu", updated.numeServiciu);
                cmd.Parameters.AddWithValue("@pretServiciu", updated.pret);
                cmd.Parameters.AddWithValue("@id", id);
                try
                {
                    dataAdapter.UpdateCommand = cmd;
                    await dataAdapter.UpdateCommand.ExecuteNonQueryAsync();

                    cmd.Dispose();
                    dataAdapter.Dispose();
                    return 1;
                }
                catch (Exception e)
                {
                    Debug.WriteLine(e);
                    return -1;
                }
            }
        }

        public static async Task<int> deleteServiciuAsync(int id)
        {
            if (await findServiciuByIdAsync(id) == null)
                return 0;
            const string deleteQuery = "delete from hotelDB.dbo.ServiciiSuplimentare where id = @id;";

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