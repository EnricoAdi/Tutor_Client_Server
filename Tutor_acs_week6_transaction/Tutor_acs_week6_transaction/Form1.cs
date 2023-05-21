using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace Tutor_acs_week6_transaction
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            DB db = new DB("", "");
            dataCart.Columns.Add("ID");
            dataCart.Columns.Add("KODE");
            dataCart.Columns.Add("MERK");
            dataCart.Columns.Add("JUMLAH");
            dataCart.Columns.Add("HARGA");
            dataCart.Columns.Add("SUBTOTAL");
             
        }
        DataTable dataCart = new DataTable();
        DataTable dtSupplier = new DataTable();
        private void Form1_Load(object sender, EventArgs e)
        {
            //di datagridview, rowHeadersVisible dibuat false supaya visual studio tidak menambah kolom di paling kiri

            bindDataSet();
            //DataTable x = DB.get("b.*,j.NAMA_JENIS", "bahan b join JENIS_BAHAN j on b.JENIS_BAHAN=j.ID", new string[0] {});
            //dataGridView1.DataSource = x;

            dtSupplier = DB.get("SELECT * FROM SUPPLIER");
            List<String> dataSupplier = new List<string>();
            for (int i = 0; i < dtSupplier.Rows.Count; i++)
            {
                DataRow dr = dtSupplier.Rows[i];
                dataSupplier.Add(dr.Field<string>("NAMA"));
            }
            comboBox1.DataSource = dataSupplier; //buat ngisi combobox pakai list

        }

        private void button2_Click(object sender, EventArgs e)
        {
            FormCariBahan f = new FormCariBahan(this);
            f.ShowDialog();
            
            }
            public void getAddedItem(int id, string kode, string merk, int maxQty)
            {
                //dataCart.Rows.Add(r);
                txtKode.Text = kode;
                lblNama.Text = merk;
                numericUpDown1.Maximum = maxQty;
                bindDataSet();
            }
        void bindDataSet()
        {
            dataGridView1.DataSource = null;
            dataGridView1.DataSource = dataCart;

            int total = 0;
            for (int i = 0; i < dataCart.Rows.Count; i++)
            {
                total += Int32.Parse(dataCart.Rows[i].Field<string>("SUBTOTAL"));
            }
            lblTotal.Text = total.ToString();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //add to cart
            if(txtKode.Text == "") return;
            DataTable detail = DB.get($"SELECT b.*,j.NAMA_JENIS FROM bahan b join JENIS_BAHAN j on b.JENIS_BAHAN=j.ID WHERE KODE='{txtKode.Text}'");
            DataRow r = detail.Rows[0];
            dataCart.Rows.Add(r.Field<int>("ID"),r.Field<string>("KODE"), r.Field<string>("MERK"), numericUpDown1.Value, r.Field<int>("HARGA"), r.Field<int>("HARGA") * numericUpDown1.Value);

            bindDataSet();
        }

        private void button3_Click(object sender, EventArgs e)
        {

            //clear
            dataCart.Clear();
            bindDataSet();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (dataCart.Rows.Count < 1) return;
            //beli
            //get ID supplier
            int index = comboBox1.SelectedIndex;
            int idSupplier = dtSupplier.Rows[index].Field<int>("ID");

            //get tanggal 
            string dateNow = DateTime.Now.ToString("yyyy-MM-dd");
            string yearNow = DateTime.Now.ToString("yyyy");
            string monthNow = DateTime.Now.ToString("MM");
            string dayNow = DateTime.Now.ToString("dd");
            //Ctrl + klik buat munculin autocomple tostring nya

            int total = Int32.Parse(lblTotal.Text);

            //generate nomor nota
            string newNota = $"BELI{yearNow}{monthNow}{dayNow}";
            //get nomor urut
            string urutan = DB.getScalar($"SELECT COUNT(*)+1 FROM H_BELI_BAHAN WHERE NOMOR_NOTA LIKE '{newNota}%'");

            newNota += urutan.PadLeft(3,'0');

            //BEGIN TRANSACTION  
            DB.openConnection(); //open dulu connectionnya
            SqlTransaction transaction = DB.conn.BeginTransaction(); 
            //buat init transaksi, pakai connection.BeginTransaction
            try
            {
                //insert dulu headernya
                string cmdStringHeader = $"INSERT INTO H_BELI_BAHAN VALUES('{newNota}','{dateNow}',{total},{idSupplier})"; 
                SqlCommand cmdHeader = new SqlCommand(cmdStringHeader, DB.conn,transaction); 
                //butuh 3 parameter, query stringnya, connection db nya, sama variabel transaction

                cmdHeader.ExecuteNonQuery();

                //insert detailnya
                for (int i = 0; i < dataCart.Rows.Count; i++)
                {
                    DataRow dr = dataCart.Rows[i];
                    string cmdDetailString = $"INSERT INTO D_BELI_BAHAN VALUES('{newNota}',{dr.Field<string>("ID")},{dr.Field<string>("JUMLAH")},{dr.Field<string>("HARGA")},{dr.Field<string>("SUBTOTAL")})";

                    SqlCommand cmdDetail = new SqlCommand(cmdDetailString, DB.conn, transaction);
                    cmdDetail.ExecuteNonQuery();

                    //kurangi stok bahan 
                    string cmdReduceString = $"UPDATE BAHAN SET QTY_STOK = QTY_STOK - {dr.Field<string>("JUMLAH")} where ID={dr.Field<string>("ID")}";

                    SqlCommand cmdReduce= new SqlCommand(cmdReduceString, DB.conn, transaction);
                    cmdReduce.ExecuteNonQuery();
                }

                transaction.Commit(); //dicommit kalau berhasil
                dataCart.Clear();
                bindDataSet();
                MessageBox.Show("Berhasil insert data transaction baru");
            }
            catch (Exception exc)
            {
                transaction.Rollback(); //dirollback kalau error
                MessageBox.Show(exc.Message.ToString(),"Gagal insert data transaction"); 
            }



        }
    }
}
