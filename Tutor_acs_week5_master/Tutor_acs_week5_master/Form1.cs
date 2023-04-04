using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Tutor_acs_week5_master
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            //hello gais 
            //jadi di sini ada 
            //1. Form connection (ini)
            //2. Form Master Supplier
            //3. Form detail master supplier
            //4. Class DB
            //5. Class yang ada di Folder Model 
            //silahkan di buka masing2 :)
        }

        private void button1_Click(object sender, EventArgs e)
        {

            string username = txtUsername.Text;
            string password = txtPassword.Text;

            try
            {
                DB db = new DB(username, password); //ini kita init dulu class DB nya
                DB.openConnection();
                DB.closeConnection();
                MessageBox.Show("Connection success"); //ini gunanya untuk mencoba buka connection apa bisa / tidak, kalau tidak bisa brti ada salah di konfigurasi

                MasterSupplier m = new MasterSupplier();
                this.Hide();
                m.Show();
            }
            catch (Exception exc)
            {

                MessageBox.Show(exc.Message);
            }
        }
    }
}
