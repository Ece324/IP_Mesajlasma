﻿using SimpleTCP;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Client
{
    public partial class Giris : Form
    {
        public Giris()
        {
            InitializeComponent();
        }
        static string conString = "LAPTOP-6SP56EO4\\SQLEXPRESS";
        //Bu veritabanına bağlanmak için gerekli olan bağlantı cümlemiz.Bu satıra dikkat edelim.
        //Sql Servera bağlanırken kullandığımız bilgileri ve veritabanı ismini yazıyoruz.
        SqlConnection baglanti = new SqlConnection(@"Data Source=LAPTOP-6SP56EO4\SQLEXPRESS;Initial Catalog=IPmesajlasma;Integrated Security=True");
        ////bağlantı cümlemizi kullanarak bir SqlConnection bağlantısı oluşturuyoruz.
        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }
        string sifre = "1234";
        private string kullaniciBilgisayarAdi = "";
        private void button1_Click(object sender, EventArgs e)
        {
            
            if (textBox1.Text == sifre)
            {
                kullaniciBilgisayarAdi = textBox4.Text;

                //Form1 frm1 = new Form1(kullaniciBilgisayarAdi);

                Form1 frm = new Form1();
                //frm.Show();
                //this.Hide();
                string kullaniciAdi = textBox4.Text;

                // IP adresi ve bilgisayar adı ekleme işlemi
                if (baglanti.State == ConnectionState.Closed)
                    baglanti.Open();
                string bilgisayarAdi = Dns.GetHostName();
                string ipAdresi = Dns.GetHostByName(bilgisayarAdi).AddressList[0].ToString();
                string kayit = "INSERT INTO IPmesajlasma(IPAddress, PcAdi) VALUES (@IPAddress, @PcAdi)";
                SqlCommand komut = new SqlCommand(kayit, baglanti);
                komut.Parameters.AddWithValue("@IPAddress", ipAdresi);
                komut.Parameters.AddWithValue("@PcAdi", textBox4.Text);
                komut.ExecuteNonQuery();
                baglanti.Close();

                frm.Show();

                this.Hide();

            }
            else
            {
                MessageBox.Show("Yanlış şifre");
            }
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (textBox3.Text == sifre)
            {
                sifre = textBox2.Text;
                MessageBox.Show("Kayıt edildi");
            }
            else
            {
                MessageBox.Show("Yanlış şifre");
            }
        }
        public string bilgisayarAdi()
        {
            string bilgisayarAdi = textBox4.Text;
            return bilgisayarAdi;
        }

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                button1.PerformClick();
            }
        }

        private void Giris_Load(object sender, EventArgs e)
        {

        }
    }
}
