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


namespace Tutor_acs_week7_crystalreport
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //crystal report viewer
            //sebelum mulai, bisa masukkan component crystal report viewer ke form (dari toolbox) -> ini nanti buat nampilin report di formnya
            //secara default dia kekunci dengan ukuran formnya, buat bisa diresize2, tekan yang segitiga atas bagian atas, terus tekan undock in parent container, atau cari properties bagian dock, diisi none


            //membuat file crystal report
            //di solution explorer, add new item > crystal report
            //sisanya bisa dibaca di crystal report guide.docx
            comboBox1.SelectedIndex = 0;
            DB db = new DB("", "");


        }
        reportMembership report;

        CrystalReport1 report1;

        private void buttonCetak_Click(object sender, EventArgs e)
        {
            string metode = comboBox1.SelectedItem.ToString();

            //report = new reportMembership();


            //report.SetParameterValue("paramMetode", metode);
            //report.SetDatabaseLogon("", "");

            //crystalReportViewer1.ReportSource = null;
            //crystalReportViewer1.ReportSource = report;

            report1 = new CrystalReport1();

            report1.SetParameterValue("paramMetode", metode);
            //report1.SetDatabaseLogon("enrico", "enrico");

            crystalReportViewer1.ReportSource = null;
            crystalReportViewer1.ReportSource = report1;


        }
    }
}
