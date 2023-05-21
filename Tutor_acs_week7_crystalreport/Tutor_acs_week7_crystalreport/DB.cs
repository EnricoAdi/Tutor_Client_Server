using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;

namespace Tutor_acs_week7_crystalreport
{
    class DB
    {

        private static string connString = "";
        public static SqlConnection conn = null;
        const string NAMADB = "db_tokoroti";
        public DB(string username, string password)
        {
            //class ini digunakan untuk menyimpan konfigurasi database
            try
            {
                connString = $"Data Source=.;Initial Catalog={NAMADB};Integrated Security=True";
                //connString = $"Data Source=.;Initial Catalog={NAMADB};User ID={username};Password={password}"; 

                conn = new SqlConnection(connString);
            }
            catch (Exception exc)
            {
                throw new Exception("Konfigurasi gagal, " + exc.Message.ToString());
            }
        }
        public static void openConnection()
        {
            if (conn.State == ConnectionState.Open)
            {
                conn.Close();
            }
            conn.Open();
        }
        public static void closeConnection()
        {
            conn.Close();
        }

        public static DataTable get(string query)
        {
            SqlCommand cmd = new SqlCommand(query, DB.conn);
            SqlDataAdapter adapter = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            adapter.Fill(dt);

            return dt;
        }
        public static string getScalar(string query)
        {
            SqlCommand cmd = new SqlCommand(query, DB.conn);
            DB.openConnection();
            string res = cmd.ExecuteScalar().ToString();
            DB.closeConnection();
            return res; //default aku jadikan string semua
        }

        public static List<String> getList(string query, string rowname)
        {
            DataTable d = DB.get(query);
            List<String> data = new List<string>();
            for (int i = 0; i < d.Rows.Count; i++)
            {
                DataRow dr = d.Rows[i];
                data.Add(dr.Field<string>(rowname));
            }
            return data;
        }
    }
}
