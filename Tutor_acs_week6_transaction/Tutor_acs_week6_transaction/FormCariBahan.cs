using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Tutor_acs_week6_transaction
{
    public partial class FormCariBahan : Form
    {
        public FormCariBahan(Form1 p)
        {
            InitializeComponent();
            parent = p;
        }
        Form1 parent;
        DataTable detail = new DataTable();
        private void FormCariBahan_Load(object sender, EventArgs e)
        {
            lblWarning.Visible = false;
            groupBox1.Visible = false;
        }
        private void button1_Click(object sender, EventArgs e)
        {
            //buat search
            string kode = txtKode.Text;
            if (kode == "") return;

            //cari kode exist 
            string temu = DB.getScalar($"select count(*) from BAHAN where KODE='{kode}'");
            int jumlah = Int32.Parse(temu);
            if (jumlah < 1)
            {
                lblWarning.Visible = true; 
                groupBox1.Visible = false;
                return;
            }
            lblWarning.Visible = false;
            groupBox1.Visible = true;
            detail = DB.get($"select b.*,j.NAMA_JENIS from bahan b join JENIS_BAHAN j on b.JENIS_BAHAN=j.ID where KODE='{kode}'");
            DataRow r = detail.Rows[0];
            bindTable(r);
        }
        private void bindTable(DataRow r)
        {
            lblKode.Text = r.Field<string>("KODE");
            lblHarga.Text = r.Field<int>("HARGA").ToString();
            lblSatuan.Text = r.Field<string>("SATUAN");
            lblJenis.Text = r.Field<string>("NAMA_JENIS");
            lblMerk.Text = r.Field<string>("MERK");
            lblQty.Text = r.Field<int>("QTY_STOK").ToString();
            lblStatus.Text = r.Field<int>("STATUS").ToString()=="1"? "Aktif" : "Tidak Aktif";

        }
        private void btnAdd_Click(object sender, EventArgs e)
        { 
            if (groupBox1.Visible == false) return;
            DataRow r = detail.Rows[0]; 
            parent.getAddedItem(r.Field<int>("ID"),r.Field<string>("KODE"), r.Field<string>("MERK"), r.Field<int>("QTY_STOK"));
            this.Close();
        }
    }
}
