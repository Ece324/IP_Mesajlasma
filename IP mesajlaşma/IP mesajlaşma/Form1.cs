using SimpleTCP;
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
using static System.Windows.Forms.VisualStyles.VisualStyleElement;


namespace IP_mesajlaşma
{
    public partial class Form1 : Form
    {
        [Obsolete]
        public Form1()
        {
            InitializeComponent();
        }

        SimpleTcpServer server;

        [Obsolete]
        private void Form1_Load(object sender, EventArgs e)
        {
            server = new SimpleTcpServer();
            //sınıftan yeni bir nesne oluşturur ve 'server' değişkenine atar.
            //Bu nesne, sunucu tarafında TCP/IP tabanlı iletişim işlemlerini yönetmek için kullanılır.

            server.Delimiter = 0x13; //giriş (enter)
            //Sunucu ve istemci arasındaki iletişimde kullanılacak veri parçalama (delimiter) işaretidir. 0x13 değeri, "Enter" tuşuna karşılık gelen ASCII kodunu temsil eder.
            //Bu, gelen verilerin "Enter" tuşuna basıldığında ayrılacağını ifade eder.

            server.StringEncoder = Encoding.UTF8;
            /* Sunucu tarafında alınan ve gönderilen metin verilerinin UTF-8 karakter kodlaması kullanılarak işlenmesini sağlar. 
             * UTF-8, geniş bir karakter kümesini destekleyen popüler bir metin kodlama biçimidir.*/

            server.DataReceived += Server_DataReceived;

            string bilgisayarAdi = Dns.GetHostName();
            string ipAdresi = Dns.GetHostByName(bilgisayarAdi).AddressList[0].ToString();
            label3.Text = ipAdresi;
        }

        private void Server_DataReceived(object sender, SimpleTCP.Message e)
        {
            string bilgisayarAdi = Dns.GetHostName();

            string receivedMessage = e.MessageString;
            // Gelen mesajı değerlendirerek cevap üretebilirsiniz
            string replyMessage = bilgisayarAdi + ": " + receivedMessage + Environment.NewLine;
            e.ReplyLine(replyMessage);
            // Gelen mesaja cevap verme işlemi
            textBox3.Invoke((MethodInvoker)delegate ()
            {
                textBox3.Text += replyMessage;
            });
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //Bu veritabanına bağlanmak için gerekli olan bağlantı cümlemiz.Bu satıra dikkat edelim.
            //Sql Servera bağlanırken kullandığımız bilgileri ve veritabanı ismini yazıyoruz.
            SqlConnection baglanti = new SqlConnection(@"Data Source=LAPTOP-6SP56EO4\SQLEXPRESS;Initial Catalog=IPmesajlasma;Integrated Security=True");
            ////bağlantı cümlemizi kullanarak bir SqlConnection bağlantısı oluşturuyoruz.

            // Bağlantı işlemini gerçekleştiren kodlarserver.Start(IPAddress.Parse(label3.Text), Convert.ToInt32(textBox2.Text));
            // Sunucuyu başlatmak için Start yöntemini kullan.


            button1.Enabled = false;

            // IP adresi ve bilgisayar adı ekleme işlemi
            if (baglanti.State == ConnectionState.Closed)
                baglanti.Open();

            string kayit_buton = "SELECT PcAdi FROM IPmesajlasma";
            SqlCommand komut_buton = new SqlCommand(kayit_buton, baglanti);
            SqlDataAdapter da = new SqlDataAdapter(komut_buton);
            DataTable dt = new DataTable();
            da.Fill(dt);
            dataGridView1.DataSource = dt;

            baglanti.Close();
            string ipAddressStr = label3.Text;
            if (System.Net.IPAddress.TryParse(ipAddressStr, out System.Net.IPAddress ipAddress))
            {
                textBox3.Text += "Server başladı..." + Environment.NewLine;
                server.Start(ipAddress, Convert.ToInt32(textBox2.Text));
                button1.Enabled = false;
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            // SQL bağlantı cümlesi
            string connectionString = @"Data Source=LAPTOP-6SP56EO4\SQLEXPRESS;Initial Catalog=IPmesajlasma;Integrated Security=True";

            // Veritabanı bağlantısını oluştur
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                // Bağlantıyı aç
                connection.Open();

                // Verileri silme sorgusu
                string deleteCommand = "DELETE FROM IPmesajlasma";

                // Sorguyu çalıştır
                using (SqlCommand command = new SqlCommand(deleteCommand, connection))
                {
                    command.ExecuteNonQuery();
                }
            }
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            SqlConnection baglanti = new SqlConnection(@"Data Source=LAPTOP-6SP56EO4\SQLEXPRESS;Initial Catalog=IPmesajlasma;Integrated Security=True");
            baglanti.Open();

            string kayit_buton = "SELECT PcAdi FROM IPmesajlasma";
            SqlCommand komut_buton = new SqlCommand(kayit_buton, baglanti);
            SqlDataAdapter da = new SqlDataAdapter(komut_buton);
            DataTable dt = new DataTable();
            da.Fill(dt);
            dataGridView1.DataSource = dt;
            baglanti.Close();
        }
    }
}
