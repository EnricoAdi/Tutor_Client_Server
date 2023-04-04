using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using Tutor_acs_week5_master.Model; //ini gunanya untuk ngeimport semua module yang ada di folder "model" supaya nanti class2nya bisa kita pakai di sini
namespace Tutor_acs_week5_master
{
    public partial class MasterSupplier : Form
    {
        public MasterSupplier()
        {
            InitializeComponent();
        }

        public void MasterSupplier_Load(object sender, EventArgs e)
        {
            //ini function yang digunakan untuk ngambil data dari supplier
            //konsepnya, sql command akan ditaruh semua di class supplier, jadi di sini codenya bersih hanya untuk mengontrol action di dalam form saja


            DataTable dtSupplier = Supplier.fetch(); //ini nanti si method fetch untuk ngembaliin datatable sebagai source buat datagridviewnya.

            dataGridView1.DataSource = null; //untuk menghindari error, kita bisa nge-null kan dulu datasourcenya
            dataGridView1.DataSource = dtSupplier; //baru diset dengan datatable yang tadi

            dataGridView1.Columns["ALAMAT"].Visible = false;

            buttonClear_Click(null, null); //ini buat ngeclear field2 aja
            
            //bisa dilakukan dengan datareader, tapi silahkan cari sendiri kalau mau pakai :)
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string nama = txtNama.Text ;
            string notel = txtNoTelp.Text ;
            string email = txtEmail.Text ;
            string alamat = txtAlamat.Text ;
            Supplier s = new Supplier();
            s.NAMA = nama;
            s.NO_TELP = notel;
            s.ALAMAT = alamat;
            s.EMAIL = email;
            try
            { 
                s.insert();
                MessageBox.Show("Berhasil insert data supplier baru"); 
                MasterSupplier_Load(sender, e);
            }
            catch (Exception exc)
            {
                MessageBox.Show(exc.Message);
            }
        }

        private void buttonClear_Click(object sender, EventArgs e)
        {
            txtNama.Text = "";
            txtNoTelp.Text = "";
            txtEmail.Text = "";
            txtAlamat.Text = "";
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
        }

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            int index = e.RowIndex;
            Supplier s = new Supplier();
            //ini kalau mau pakai constructor buat ngisi propertiesnya bisa, tapi di sini kondisinya aku langsung ngisi propertiesnya :)
            s.ID = Int32.Parse(dataGridView1.Rows[index].Cells["ID"].Value.ToString());
            s.KODE = dataGridView1.Rows[index].Cells["KODE"].Value.ToString();
            s.NAMA = dataGridView1.Rows[index].Cells["NAMA"].Value.ToString();
            s.ALAMAT = dataGridView1.Rows[index].Cells["ALAMAT"].Value.ToString();
            s.EMAIL = dataGridView1.Rows[index].Cells["EMAIL"].Value.ToString();
            s.NO_TELP = dataGridView1.Rows[index].Cells["NO_TELP"].Value.ToString(); 
            DetailSupplier ds = new DetailSupplier(this, s);
            ds.ShowDialog();
        }
    }
}
