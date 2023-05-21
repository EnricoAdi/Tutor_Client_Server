

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tutor_acs_week5_master;
using System.Data.SqlClient; //Jangan lupa import ini
using System.Data; 
namespace Tutor_acs_week5_master.Model
{
    public class Supplier
    {

        public int ID;
        public string KODE;
        public string NAMA;
        public string ALAMAT;
        public string EMAIL;
        public string NO_TELP;

        public static DataTable fetch()
        { 
            //nantinya, dari function fetch ini akan ngembaliin sebuah datatable, yang bisa langsung dipasang jadi datasource untuk datagridview di form. Ini biar codenya jadi lebih clean dan terstruktur :)

            SqlCommand cmd = new SqlCommand("SELECT * FROM SUPPLIER order by ID desc", DB.conn);
            SqlDataAdapter adapter = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            adapter.Fill(dt);

            return dt;
        }

        public static Tuple<int, string> getNewIDandKode(string nama)
        {
            string newKode = "";
            int newId = 0;
            SqlCommand cmd1 = new SqlCommand($"SELECT PATINDEX('{nama}',' ') ", DB.conn);
            DB.openConnection();
            int posisiSpasi = Int32.Parse(cmd1.ExecuteScalar().ToString());
            int jmlHuruf = posisiSpasi == 0 ? 1 : 2; //ini shorthand if untuk mengisi variabel jmlHuruf, yang nantinya diisi 1 kalau posisi spasi 0, dan diisi 2 kalau posisi spasi lebih dari 0
            //maksudnya, ini aku mau cari tau berapa banyak kata dari posisi spasi

            //atau
            //if (posisiSpasi == 0)
            //{
            //    jmlHuruf = 1;
            //}
            //else
            //{
            //    jmlHuruf = 2;
            //}
            //DB.closeConnection();

            if (jmlHuruf == 1)
            {
                newKode = nama.Substring(0, 4).ToUpper();
            }
            else
            {
                //lebih dari 1
                newKode = nama.Substring(0, 2).ToUpper() + nama.Substring(posisiSpasi, 2).ToUpper();
            }

            cmd1 = new SqlCommand($"SELECT count(*) + 1 from supplier where KODE LIKE '{newKode}%'", DB.conn);
            DB.openConnection();
            int x = Int32.Parse(cmd1.ExecuteScalar().ToString());
            DB.closeConnection();
            newKode = newKode + x.ToString().PadLeft(5, '0');

            DB.openConnection();
            SqlCommand cmd = new SqlCommand("SELECT count(*)+1 from supplier", DB.conn);
            newId = Int32.Parse(cmd.ExecuteScalar().ToString());
            DB.closeConnection();


            return new Tuple<int, string>(newId, newKode);
        }
        public void insert()
        {
            Tuple<int, string> newData = getNewIDandKode(this.NAMA);
            SqlCommand cmd = new SqlCommand("INSERT INTO SUPPLIER VALUES(@id,@kode,@nama,@alamat,@email,@notelp)", DB.conn);
            cmd.Parameters.AddWithValue("@id", newData.Item1);
            cmd.Parameters.AddWithValue("@kode", newData.Item2);
            cmd.Parameters.AddWithValue("@nama", this.NAMA);
            cmd.Parameters.AddWithValue("@alamat", this.ALAMAT);
            cmd.Parameters.AddWithValue("@email", this.EMAIL);
            cmd.Parameters.AddWithValue("@notelp", this.NO_TELP);

            //untuk hindari SQL INJECTION, kita bisa pakai parameter sql 
            

            DB.openConnection();
            try
            {
                cmd.ExecuteNonQuery();
            }
            catch (Exception exc)
            {
                throw new Exception(exc.Message);
            }
            DB.closeConnection();
        }
        public static void insertWithParams(string nama, string alamat, string email, string notelp)
        {

            Tuple<int, string> newData = getNewIDandKode(nama);
            SqlCommand cmd = new SqlCommand("INSERT INTO SUPPLIER VALUES(@id,@kode,@nama,@alamat,@email,@notelp)", DB.conn);
            cmd.Parameters.AddWithValue("@id", newData.Item1);
            cmd.Parameters.AddWithValue("@kode", newData.Item2);
            cmd.Parameters.AddWithValue("@nama", nama);
            cmd.Parameters.AddWithValue("@alamat", alamat);
            cmd.Parameters.AddWithValue("@email", email);
            cmd.Parameters.AddWithValue("@notelp", notelp);

            //untuk hindari SQL INJECTION, kita bisa pakai parameter sql 


            DB.openConnection();
            try
            {
                cmd.ExecuteNonQuery();
            }
            catch (Exception exc)
            {
                throw new Exception(exc.Message);
            }
            DB.closeConnection();
        }
        public void update()
        { 
            SqlCommand cmd = new SqlCommand("UPDATE SUPPLIER SET NAMA = @nama, ALAMAT = @alamat, EMAIL = @email, NO_TELP = @notelp WHERE ID=@id", DB.conn);
            cmd.Parameters.AddWithValue("@id", this.ID); 
            cmd.Parameters.AddWithValue("@nama", this.NAMA);
            cmd.Parameters.AddWithValue("@alamat", this.ALAMAT);
            cmd.Parameters.AddWithValue("@email", this.EMAIL);
            cmd.Parameters.AddWithValue("@notelp", this.NO_TELP);

            DB.openConnection();
            try
            {
                cmd.ExecuteNonQuery();
            }
            catch (Exception exc)
            {
                throw new Exception(exc.Message);
            }
            DB.closeConnection();
        }
        public void delete()
        { 
            SqlCommand cmd = new SqlCommand("DELETE FROM SUPPLIER WHERE ID=@id", DB.conn);
            cmd.Parameters.AddWithValue("@id", this.ID); 

            DB.openConnection();
            try
            {
                cmd.ExecuteNonQuery();
            }
            catch (Exception exc)
            {
                throw new Exception(exc.Message);
            }
            DB.closeConnection();
        }
    }
}
