using Backend.Models;
using Backend.Utils;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace Backend.Dao
{
    public class RezervariDao
    {
        public static async Task<List<Rezervare>> getAllRezervationsAsync()
        {
            List<Rezervare> rezervari = new List<Rezervare>();
            const string getQuery = "select Rezervari.id, Rezervari.nopti, Rezervari.persoane, Rezervari.nrCamere," +
                " Rezervari.userID, Users.fullName, Rezervari.cameraId, Rooms.tipCamera, Rooms.pretPeNoapte," +
                " Rezervari.RezervatPe, Rezervari.RezervatPana, Rezervari.pretFaraServicii, Rezervari.totalPretServicii" +
                " from Rezervari,Users,Rooms where (Rezervari.userID = Users.id and Rezervari.cameraId = Rooms.id);";
            
            var conn = DBConnection.openConn();
            using (var cmd = new SqlCommand(getQuery, conn))
            {
                var dataReader = await cmd.ExecuteReaderAsync();
                while (await dataReader.ReadAsync())
                {
                    Rezervare rez = new Rezervare();
                    rez.id = dataReader.GetInt32(0);
                    rez.nrNopti = dataReader.GetInt32(1);
                    rez.nrPersoane = dataReader.GetInt32(2);
                    rez.nrCamere = dataReader.GetInt32(3);
                    rez.userId = dataReader.GetInt32(4);
                    rez.userName = dataReader.GetString(5);
                    rez.cameraId = dataReader.GetInt32(6);
                    rez.tipCamera = dataReader.GetString(7);
                    rez.pretPeNoapte = dataReader.GetInt32(8);
                    rez.rezervatPe = dataReader.GetDateTime(9);
                    rez.rezervatPana = dataReader.GetDateTime(10);
                    rez.pretFaraServicii = dataReader.GetInt32(11);
                    rez.totalPretServicii = dataReader.GetInt32(12);

                    rezervari.Add(rez);
                }

                cmd.Dispose();
                dataReader.Close();
            }
            return rezervari;
        }

        public static async Task<Rezervare> findReservationById(int id)
        {
            Rezervare rez = null;
            const string getQuery = "select Rezervari.id, Rezervari.nopti, Rezervari.persoane, Rezervari.nrCamere," +
                " Rezervari.userID, Users.fullName, Rezervari.cameraId, Rooms.tipCamera, Rooms.pretPeNoapte," +
                " Rezervari.RezervatPe, Rezervari.RezervatPana, Rezervari.pretFaraServicii, Rezervari.totalPretServicii" +
                " from Rezervari,Users,Rooms where (Rezervari.userID = Users.id and Rezervari.cameraId = Rooms.id) and Rezervari.id = @id;";

            var conn = DBConnection.openConn();
            using (var cmd = new SqlCommand(getQuery, conn))
            {
                cmd.Parameters.AddWithValue("@id", id);
                var dataReader = await cmd.ExecuteReaderAsync();
                if (await dataReader.ReadAsync())
                {
                    rez = new Rezervare();
                    rez.id = dataReader.GetInt32(0);
                    rez.nrNopti = dataReader.GetInt32(1);
                    rez.nrPersoane = dataReader.GetInt32(2);
                    rez.nrCamere = dataReader.GetInt32(3);
                    rez.userId = dataReader.GetInt32(4);
                    rez.userName = dataReader.GetString(5);
                    rez.cameraId = dataReader.GetInt32(6);
                    rez.tipCamera = dataReader.GetString(7);
                    rez.pretPeNoapte = dataReader.GetInt32(8);
                    rez.rezervatPe = dataReader.GetDateTime(9);
                    rez.rezervatPana = dataReader.GetDateTime(10);
                    rez.pretFaraServicii = dataReader.GetInt32(11);
                    rez.totalPretServicii = dataReader.GetInt32(12);

                }
                cmd.Dispose();
                dataReader.Close();
            }

            return rez;
        }

        public static async Task<List<Rezervare>> findAllReservationsOfOneUser(int userId)
        {
            List<Rezervare> rezervari = new List<Rezervare>();
            const string getQuery = "select Rezervari.id, Rezervari.nopti, Rezervari.persoane, Rezervari.nrCamere," +
                " Rezervari.userID, Users.fullName, Rezervari.cameraId, Rooms.tipCamera, Rooms.pretPeNoapte," +
                " Rezervari.RezervatPe, Rezervari.RezervatPana, Rezervari.pretFaraServicii, Rezervari.totalPretServicii" +
                " from Rezervari,Users,Rooms where (Rezervari.userID = Users.id and Rezervari.cameraId = Rooms.id) and Rezervari.userID = @userId;";

            var conn = DBConnection.openConn();
            using (var cmd = new SqlCommand(getQuery, conn))
            {
                cmd.Parameters.AddWithValue("@userId", userId);
                var dataReader = await cmd.ExecuteReaderAsync();
                while (await dataReader.ReadAsync())
                {
                    Rezervare rez = new Rezervare();
                    rez.id = dataReader.GetInt32(0);
                    rez.nrNopti = dataReader.GetInt32(1);
                    rez.nrPersoane = dataReader.GetInt32(2);
                    rez.nrCamere = dataReader.GetInt32(3);
                    rez.userId = dataReader.GetInt32(4);
                    rez.userName = dataReader.GetString(5);
                    rez.cameraId = dataReader.GetInt32(6);
                    rez.tipCamera = dataReader.GetString(7);
                    rez.pretPeNoapte = dataReader.GetInt32(8);
                    rez.rezervatPe = dataReader.GetDateTime(9);
                    rez.rezervatPana = dataReader.GetDateTime(10);
                    rez.pretFaraServicii = dataReader.GetInt32(11);
                    rez.totalPretServicii = dataReader.GetInt32(12);

                    rezervari.Add(rez);
                }
                cmd.Dispose();
                dataReader.Close();
            }

            return rezervari;
        }

        public static async Task<int> addNewRezervationAsync(Rezervare newRezervare)
        {
            const string insertQuery = "insert into Rezervari(userID, nopti, persoane, cameraId, nrCamere, RezervatPe, RezervatPana)" +
                " values (@userID, @nrNopti, @nrPersoane, @cameraId, @nrCamere, @rezervatPe, @rezervatPana);";

            var conn = DBConnection.openConn();
            using (var cmd = new SqlCommand(insertQuery, conn))
            using (var dataAdapter = new SqlDataAdapter())
            {
                try
                {

                    cmd.Parameters.AddWithValue("@userId", newRezervare.userId);
                    cmd.Parameters.AddWithValue("@nrNopti", newRezervare.nrNopti);
                    cmd.Parameters.AddWithValue("@nrPersoane", newRezervare.nrPersoane);
                    cmd.Parameters.AddWithValue("@cameraId", newRezervare.cameraId);
                    cmd.Parameters.AddWithValue("@nrCamere", newRezervare.nrCamere);
                    cmd.Parameters.AddWithValue("@rezervatPe", newRezervare.rezervatPe);
                    cmd.Parameters.AddWithValue("@rezervatPana", newRezervare.rezervatPana);

                    dataAdapter.InsertCommand = cmd;
                    await dataAdapter.InsertCommand.ExecuteNonQueryAsync();

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

        public static async Task<int> deleteReservationById(int id)
        {
            if (await findReservationById(id) == null)
                return 0;

            const string deleteQuery = "delete from Rezervari where Rezervari.id = @id;";


            var conn = DBConnection.openConn();
            using (var cmd = new SqlCommand(deleteQuery, conn))
            using (var dataAdapter = new SqlDataAdapter())
            {
                try
                {
                    cmd.Parameters.AddWithValue("@id", id);
                    dataAdapter.DeleteCommand = cmd;

                    await dataAdapter.DeleteCommand.ExecuteNonQueryAsync();

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

        public static async Task<int> deleteReservationsByUserId(int userId)
        {

            List<int> ids = new List<int>();
            foreach (Rezervare r in await findAllReservationsOfOneUser(userId))
            {
                ids.Add(r.id);
            }

            if (!ids.Any())
                return 0;

            try
            {
                foreach (int id in ids)
                    await deleteReservationById(id);

                return 1;
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
                return -1;
            }

        }
    }
}