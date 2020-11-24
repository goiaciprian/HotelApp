using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace Backend.Utils
{
    public class DBConnection
    {
        private static string connString = System.Configuration.ConfigurationManager.ConnectionStrings["UserConnectionString"].ConnectionString;
        /*private const string connString = "Server=MYPC\\SQLEXPRESS;Database=hotelDB;Trusted_Connection=True;";*/
        private static SqlConnection conn = null;

        public static SqlConnection openConn()
        {
            try
            {
                if (conn == null || conn.State == ConnectionState.Closed)
                {
                    conn = new SqlConnection(connString);
                    conn.Open();
                }
                return conn;

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return null;
            }
        }

        public static void closeConn()
        {
            if (conn != null || conn.State == ConnectionState.Closed)
                try
                {
                    conn.Close();
                }
                catch { }
        }
    }
}