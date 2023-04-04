using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks; 
using System.Data.SqlClient; //import ini untuk mengakses command sql

namespace Tutor_acs_week5_master
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
                
                connString = $"Data Source=.;Initial Catalog={NAMADB};User ID={username};Password={password}";
                //connection string ini digunakan semacam identitas pengenal buat connect ke database
                //string yang ada dollarnya ($) ini untuk interpolation string, silahkan cari sendiri apa itu :)

                conn = new SqlConnection(connString);
                //ini untuk init connection, supaya kita nantinya bisa pakai connection ini dan gaperlu buat connection baru lagi.
                //dimasukkin try catch karena kalo ada error si sql bakal ngethrow error

                //PASTIKAN DI HALAMAN CONNECTION CLASS DB DI INIT, kalau gak ndabisa di open / close
            }
            catch (Exception exc)
            {
                throw new Exception("Konfigurasi gagal, "+exc.Message.ToString());
            }
        }
        //funcion2 ini dibuat static 
        public static void openConnection()
        {
            if (conn.State == System.Data.ConnectionState.Open)
            {
                conn.Close();
            }
            conn.Open();
            //untuk open
        }
        public static void closeConnection()
        {
            conn.Close();
            //untuk close
        }
    }
}
