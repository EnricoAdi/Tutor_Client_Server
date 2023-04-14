using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;  
using System.Data;
namespace Tutor_acs_week6_transaction
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
        public static DataTable get(string select, string tableName, string[] condition , string orderBy="")
        {
            string cmdString = builderString(select, tableName, condition, orderBy);
            SqlCommand cmd = new SqlCommand(cmdString, DB.conn);
            SqlDataAdapter adapter = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            adapter.Fill(dt);

            return dt;
        }
        public static string getScalar(string select, string tableName, string[] condition, string orderBy = "")
        {
            string cmdString = builderString(select, tableName, condition, orderBy);
            SqlCommand cmd = new SqlCommand(cmdString, DB.conn); 
            DB.openConnection();
            string res = cmd.ExecuteScalar().ToString();
            DB.closeConnection();
            return res; //default aku jadikan string semua

        }
        private static string builderString(string select, string tableName, string[] condition, string orderBy = "")
        {
            DataSet s = new DataSet();
            DataTable a = s.Tables["asd"];
            //string[] arr = { "asd", "a" };
            string whereBuilder = " ";
            string orderBuilder = " ";
            if (condition.Length > 0)
            {
                whereBuilder += "WHERE ";
                for (int i = 0; i < condition.Length; i++)
                {
                    whereBuilder += condition[i];
                    whereBuilder += (i + 1 < condition.Length - 1) ? " and " : "";
                }
            }
            if (orderBy != "")
            {
                orderBuilder += $"order by {orderBy}"; //contoh nanti order by id desc,dll
            }
            select = select == "" ? "*" : select;
            string cmdString = $"SELECT {select} FROM {tableName}{whereBuilder}{orderBuilder}";
            return cmdString;
        }
    }
    
}
