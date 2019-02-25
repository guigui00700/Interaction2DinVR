using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Nescafe
{
    public partial class TCPSetting : Form
    {
        Ui _master;
        public TCPSetting(Ui master,int myId, int SId, string Ip, int Port)
        {
            InitializeComponent();
            masterId.Text = myId.ToString();
            SlaveId.Text = SId.ToString();
            IpAddress.Text = Ip;
            PortNumber.Text = Port.ToString();
            _master = master;

        }
        private void textBox5_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox6_TextChanged(object sender, EventArgs e)
        {

        }

        private void SetButton_Click(object sender, EventArgs e)
        {
            _master.setTcpParam(int.Parse(masterId.Text), int.Parse(SlaveId.Text),IpAddress.Text,int.Parse(PortNumber.Text));
            Close();
        }
    }
}
