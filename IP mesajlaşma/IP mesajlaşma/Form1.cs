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

namespace IP_mesajlaşma
{
    public partial class Form1 : Form
    {
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
            server.Delimiter = 0x13; //giriş (enter)
            server.StringEncoder = Encoding.UTF8;
            server.DataReceived += Server_DataReceived;

            string bilgisayarAdi = Dns.GetHostName();
            string ipAdresi = Dns.GetHostByName(bilgisayarAdi).AddressList[0].ToString();
            label3.Text = ipAdresi;
            button2.Enabled = false;
        }

        private void Server_DataReceived(object sender, SimpleTCP.Message e)
        {
            
            textBox3.Invoke((MethodInvoker)delegate ()
            {
                textBox3.Text += e.MessageString;
                // Gelen mesaja cevap verme işlemi:
                e.ReplyLine("Mesaj alındı: " + e.MessageString);
            });
        }

        private void button1_Click(object sender, EventArgs e)
        {
            
            string ipAddressStr = label3.Text;
            if (System.Net.IPAddress.TryParse(ipAddressStr, out System.Net.IPAddress ipAddress))
            {
                textBox3.Text += "Server başladı...";
                server.Start(ipAddress, Convert.ToInt32(textBox2.Text));
                button1.Enabled = false;
                button2.Enabled = true;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if(server.IsStarted)
            {
                server.Stop();
                button1.Enabled = true;
            }
            this.Close();
        }
    }
}
