using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


using System.Net;
using System.Net.Sockets;
using System.IO;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        private bool serveurStart;
        private serverTCP ServerTCP;
        public Form1()
        {
            InitializeComponent();
            serveurStart = false;
            ServerTCP = new serverTCP();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (!serveurStart)
            {
                int port = int.Parse(portNumber.Text);
                //startServeur
                ServerTCP.StartServer(ipAddress.Text,port,this);
                LogBox.Text += "Serveur Start on port :" + port + Environment.NewLine;
                serveurStart = true;
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (serveurStart)
            {
                //Disconnect
                ServerTCP.CloseServer();
                LogBox.Text += "Server Close"+ Environment.NewLine;
                serveurStart = false;
            }
        }

        public void WriteInLogBox(string message)
        {
            LogBox.Text += message + Environment.NewLine;
        }
    }
}
