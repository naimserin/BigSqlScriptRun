using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Data.SqlClient;
using System.Text.RegularExpressions;
using System.Threading;


namespace SQLScriptRun
{
    public partial class frm1 : Form
    {
        public frm1()
        {
            InitializeComponent();
        }
        //public SqlConnection baglanti = new SqlConnection("Data Source=31.210.35.26;Initial Catalog=z_KonyaSeker;User ID=konyaseker_user;Password=53l1pTdNLSQeFVI;");
        //SqlConnection baglanti = new SqlConnection(@"Data Source=85.25.196.237\MSSQLSERVER2008;Initial Catalog=z_KonyaSeker;User ID=u_KonyaSeker;Password=p_KonyaSeker;");    
        void VeritabanıKurulumu()
        {
            SqlConnection baglanti = new SqlConnection(@"Data Source=" + txtServer.Text + ";Initial Catalog=" + txtCatalog.Text + ";User ID=" + txtUserID.Text + ";Password=" + txtPassword.Text + ";");
            string dosya = File.ReadAllText(txtDosya.Text);
            string[] komutlar = Regex.Split(dosya, @"^\s*GO\s*$", RegexOptions.Multiline | RegexOptions.IgnoreCase);
            baglanti.Open();
            progressBar1.Maximum = komutlar.Length;
            bool sonuc = true;
            foreach (string komut in komutlar)
            {
                if (komut.Trim() != "")
                {
                    try
                    {
                        new SqlCommand(komut, baglanti).ExecuteNonQuery();
                        progressBar1.Value++;
                    }
                    catch
                    {
                        //sonuc = false;
                    }
                }
            }
            if (sonuc)
                MessageBox.Show("Script Başarıyla Çalıştırıldı.", "Başarılı", MessageBoxButtons.OK, MessageBoxIcon.Information);
            baglanti.Close();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            CheckForIllegalCrossThreadCalls = false;
            progressBar1.Minimum = 0;

        }

        private void btnSec_Click(object sender, EventArgs e)
        {
            OpenFileDialog ac = new OpenFileDialog();
            ac.Filter = "SQL Script (*.sql) |*.sql";
            ac.Multiselect = false;
            if (ac.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                txtDosya.Text = ac.FileName;
            else
                txtDosya.Text = "";

        }

        private void btnAktar_Click(object sender, EventArgs e)
        {
            progressBar1.Value++;
            Thread t = new Thread(new ThreadStart(VeritabanıKurulumu));
            t.Start();

        }
 


    }
}
