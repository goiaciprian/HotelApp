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
    public class RoomDao
    {
        public static async Task<List<RoomType>> findAllRoomTypesAsync()
        {
            List<RoomType> rooms = new List<RoomType>();
            const string getQuery = "select * from hotelDB.dbo.Rooms;";

            var conn = DBConnection.openConn();
            using (var cmd = new SqlCommand(getQuery, conn))
            using (var reader = await cmd.ExecuteReaderAsync())
            {
                while (await reader.ReadAsync())
                {
                    RoomType room = new RoomType();
                    room.id = reader.GetInt32(0);
                    room.tipCamera = reader.GetString(1);
                    room.camereDisponibile = reader.GetInt32(2);
                    room.pretPeNoapte = reader.GetInt32(3);
                    rooms.Add(room);
                }
                cmd.Dispose();
                reader.Close();
            }

            return rooms;

        }

        public static async Task<RoomType> findRoomTypeByIdAsync(int id)
        {
            RoomType room = null;
            const string getQuery = "select * from hotelDB.dbo.Rooms where id = @id;";

            var conn = DBConnection.openConn();
            using (var cmd = new SqlCommand(getQuery, conn))
            {
                cmd.Parameters.AddWithValue("@id", id);
                var reader = await cmd.ExecuteReaderAsync();
                if (await reader.ReadAsync())
                {
                    room = new RoomType();
                    room.id = reader.GetInt32(0);
                    room.tipCamera = reader.GetString(1);
                    room.camereDisponibile = reader.GetInt32(2);
                    room.pretPeNoapte = reader.GetInt32(3);
                }

                cmd.Dispose();
                reader.Close();
            }
            return room;
        }

        public static async Task<int> addRoomTypeAsync(RoomType rt)
        {
            const string insertQuery = "insert into hotelDB.dbo.Rooms(tipCamera, cameraDisponibile, pretPeNoapte) values " +
                "(@tipCamera, @camereDisponibile, @pretPeNoapte);";

            var conn = DBConnection.openConn();
            using (var cmd = new SqlCommand(insertQuery, conn))
            using (var dataAdapter = new SqlDataAdapter())
            {
                cmd.Parameters.AddWithValue("@tipCamera", rt.tipCamera);
                cmd.Parameters.AddWithValue("@camereDisponibile", rt.camereDisponibile);
                cmd.Parameters.AddWithValue("@pretPeNoapte", rt.pretPeNoapte);
                try
                {
                    dataAdapter.InsertCommand = cmd;
                    await dataAdapter.InsertCommand.ExecuteNonQueryAsync();

                    cmd.Dispose();
                    dataAdapter.Dispose();

                    return 1;
                }
                catch(Exception e)
                {
                    Debug.WriteLine(e);
                    return -1;
                }
            }
        }

        public static async Task<int> updateRoomTypeAsync(int id, RoomType update)
        {
            if (await findRoomTypeByIdAsync(id) == null)
                return 0;

            const string updateQuery = "update hotelDB.dbo.Rooms set " +
                "tipCamera = @tipCamera, cameraDisponibile = @camereDisponibile, pretPeNoapte = @pretPeNoapte where id = @id;";

            var conn = DBConnection.openConn();
            using (var cmd = new SqlCommand(updateQuery, conn))
            using (var dataAdapter = new SqlDataAdapter())
            {
                cmd.Parameters.AddWithValue("@tipCamera", update.tipCamera);
                cmd.Parameters.AddWithValue("@camereDisponibile", update.camereDisponibile);
                cmd.Parameters.AddWithValue("@pretPeNoapte", update.pretPeNoapte);
                cmd.Parameters.AddWithValue("@id", id);
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

        public static async Task<int> deleteRoomTypeAsync(int id)
        {
            if (await findRoomTypeByIdAsync(id) == null)
                return 0;

            const string deleteString = "delete from hotelDB.dbo.Rooms where id = @id;";

            var conn = DBConnection.openConn();
            using (var cmd = new SqlCommand(deleteString, conn))
            using (var dataAdapter = new SqlDataAdapter())
            {
                cmd.Parameters.AddWithValue("@id", id);
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
    }
}