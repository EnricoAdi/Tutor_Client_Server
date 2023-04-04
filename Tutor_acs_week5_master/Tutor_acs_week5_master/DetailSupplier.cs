using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Tutor_acs_week5_master.Model; //jangan lupa diimport
namespace Tutor_acs_week5_master
{
    public partial class DetailSupplier : Form
    {
        public DetailSupplier(MasterSupplier ms, Supplier s)
        {
            // di sini kita akan meminta 2 parameter yaitu form parentnya (master supplier) dan data supplier yang akan diedit 
            //gunanya, kita bisa ngakses function dari form parentnya dari sini
            //perlu diperhatikan, function yang mau diakses di form parent harus sifatnya public
            parent = ms;
            supplier = s;
            InitializeComponent();
        }
        MasterSupplier parent;
        Supplier supplier;
        private void DetailSupplier_Load(object sender, EventArgs e)
        {
            //mengisi isi form dengan data supplier 
            lblID.Text = supplier.ID.ToString();
            lblKode.Text = supplier.KODE;
            txtNama.Text = supplier.NAMA;
            txtAlamat.Text = supplier.ALAMAT;
            txtEmail.Text = supplier.EMAIL;
            txtNoTelp.Text = supplier.NO_TELP;
        }

        private void DetailSupplier_FormClosed(object sender, FormClosedEventArgs e)
        {
            //function ini akan dipanggil ketika event form close dipanggil, sehingga nanti dia akses form parentnya (master supplier), menjalankan function MasterSupplier_Load untuk refresh datagridview

            parent.MasterSupplier_Load(null, null);

            //parameternya boleh null karena kita cuma akan pakai isi code di dalamnya saja
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {

            string nama = txtNama.Text;
            string notel = txtNoTelp.Text;
            string email = txtEmail.Text;
            string alamat = txtAlamat.Text;
             
            supplier.NAMA = nama;
            supplier.NO_TELP = notel;
            supplier.ALAMAT = alamat;
            supplier.EMAIL = email;
            try
            {
                supplier.update();
                MessageBox.Show("Berhasil update data supplier baru");
                this.Close(); //untuk tutup form ini, otomatis dia akan memanggil function DetailSupplier_FormClosed
            }
            catch (Exception exc)
            {
                MessageBox.Show(exc.Message);
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            DialogResult conf = MessageBox.Show("Apakah anda yakin mau menghapus data supplier?","Konfirmasi",MessageBoxButtons.YesNo);
            //hasil dari tekan button di message box hasilnya berupa dialogresult
            if(conf == DialogResult.Yes)
            {
                try
                {
                    supplier.delete();
                    MessageBox.Show("Berhasil delete data supplier baru");
                    this.Close();
                }
                catch (Exception exc)
                {
                    MessageBox.Show(exc.Message);
                }
            }
        }
    }
}
