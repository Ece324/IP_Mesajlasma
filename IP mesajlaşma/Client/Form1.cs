using SimpleTCP;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Client
{
    public partial class Form1 : Form
    {
        SimpleTcpClient client;

        /*Kütüphaneye ait bir sınıf.
        TCP/IP tabanlı iletişimi basitleştiren, kolaylaştıran işlevlere sahip, ağ üzerinde veri iletişimini sağlayan ve internetin temel protokollerinden biri.
        sunucuyla iletişimi yönetmek ve mesaj gönderme/alma işlemlerini gerçekleştirmek için kullanılacaktır.*/
        [Obsolete]
        public Form1()
        {
            InitializeComponent();
        }

        static string conString = "LAPTOP-6SP56EO4\\SQLEXPRESS";
        //Bu veritabanına bağlanmak için gerekli olan bağlantı cümlemiz.Bu satıra dikkat edelim.
        //Sql Servera bağlanırken kullandığımız bilgileri ve veritabanı ismini yazıyoruz.
        SqlConnection baglanti = new SqlConnection(@"Data Source=LAPTOP-6SP56EO4\SQLEXPRESS;Initial Catalog=IPmesajlasma;Integrated Security=True");
        ////bağlantı cümlemizi kullanarak bir SqlConnection bağlantısı oluşturuyoruz.

        private void button1_Click(object sender, EventArgs e)
        {
            // Bağlantı işlemini gerçekleştiren kodlar
            client.Connect(label3.Text, Convert.ToInt32(textBox2.Text));
            button1.Enabled = false;
            button2.Enabled = true;

            //// IP adresi ve bilgisayar adı ekleme işlemi
            //if (baglanti.State == ConnectionState.Closed)
            //    baglanti.Open();

            //string kayit = "INSERT INTO IPmesajlasma(IPAddress, PcAdi) VALUES (@IPAddress, @PcAdi)";
            //SqlCommand komut = new SqlCommand(kayit, baglanti);
            //string bilgisayarAdi = Dns.GetHostName();
            //komut.Parameters.AddWithValue("@IPAddress", label3.Text);
            //komut.Parameters.AddWithValue("@PcAdi", bilgisayarAdi);
            //komut.ExecuteNonQuery();
            //baglanti.Close();

            //Verileri DataGridView'e getirme işlemi
            if (baglanti.State == ConnectionState.Closed)
                baglanti.Open();

            string kayit_buton = "SELECT PcAdi FROM IPmesajlasma";
            SqlCommand komut_buton = new SqlCommand(kayit_buton, baglanti);
            SqlDataAdapter da = new SqlDataAdapter(komut_buton);
            DataTable dt = new DataTable();
            da.Fill(dt);
            dataGridView1.DataSource = dt;

            baglanti.Close();


        }

        [Obsolete]
        private void Form1_Load(object sender, EventArgs e)
        {
            client = new SimpleTcpClient();
            //nesne oluşturur ve sunucuyla iletişim için kullanılır
            client.StringEncoder = Encoding.UTF8;
            //gönderilen ve alınan metin verilerinin UTF-8 karakter kodlaması kullanılarak işlenmesini sağlar.
            //UTF-8, geniş bir karakter kümesini destekleyen bir metin kodlama biçimidir

            string bilgisayarAdi = Dns.GetHostName();
            string ipAdresi = Dns.GetHostByName(bilgisayarAdi).AddressList[0].ToString();
            label3.Text = ipAdresi;
            //bilgisayarın ıp adresini bulup yazar
            button2.Enabled = false;


        }

        private void button2_Click(object sender, EventArgs e)
        {
            //yazılan mesajı gönderme butonu
            if (client.TcpClient.Connected)
            //istemcinin sunucuya bağlı olup olmadığını kontrol eder
            {
                string message = textBox3.Text;
                //mesajı değişkene atar
                var reply = client.WriteLineAndGetReply(message, TimeSpan.FromSeconds(3));
                //mesajı belirtilen sürede gönderir
                string bilgisayarAdi = Dns.GetHostName();
                textBox4.Text += bilgisayarAdi + ": " + message + "\r\n";
            }
            else
            {
                MessageBox.Show("Sunucuya bağlanılmadı. Lütfen bağlantıyı sağlayın.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            textBox3.Text = "";
        }

        private void button3_Click(object sender, EventArgs e)
        {
            // Silinecek kayıt için IP adresini alın
            string IPsil = label3.Text;

            // Veritabanı bağlantısını açın ve kaydı silin

            if (baglanti.State == ConnectionState.Closed)
                baglanti.Open();

            string deleteCommand = "DELETE FROM IPmesajlasma WHERE IPAddress = @IPAddress";
            SqlCommand deleteKomut = new SqlCommand(deleteCommand, baglanti);
            deleteKomut.Parameters.AddWithValue("@IPAddress", IPsil);
            baglanti.Close();
            // Formu kapatın
            this.Close();
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
