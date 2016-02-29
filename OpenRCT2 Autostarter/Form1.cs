using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OpenRCT2_Autostarter
{
    public partial class Form1 : Form
    {
        ExecuteConsole ec;
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {


            try
            {
                ec = new ExecuteConsole();
                Properties.Settings.Default.installPath = textBox1.Text;
                Properties.Settings.Default.savesPath = textBox2.Text;
                ec.OnServerStateChanges += Ec_OnServerStateChanges;
                ec.setInstallPath(textBox1.Text);
                ec.setSavesPath(textBox2.Text);
                button1.Enabled = false;
                button2.Enabled = true;
                ec.startServer();
            }
            catch (Exception Penis)
            {
                ServerRunningEventArgs e2 = new ServerRunningEventArgs();
                e2.running = 2;
                Ec_OnServerStateChanges(this, e2);
                MessageBox.Show("No Saves Folder was given. Please enter Saves Folder. \n" + Penis);
            }
        }

        private void Ec_OnServerStateChanges(object sender, ServerRunningEventArgs e)
        {
            if(e.running==1)
            {
                setLabel3Text("Status: Online");
            }
            else if(e.running == 0)
            {
                setLabel3Text("Status: Offline");
            }
            else if (e.running == 2)
            {
                setLabel3Text("Status: Crashed");
                button1.Enabled = true;
                button2.Enabled = false;
            }
            
        }

        private void setLabel3Text(String text)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new Action<string>(this.setLabel3Text), text);
            }
            else
            {
                label3.Text = text;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            ec.killProcesses();
            button1.Enabled = true;
            button2.Enabled = false;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            button2.Enabled = false;
            textBox1.Text = Properties.Settings.Default.installPath;
            textBox2.Text = Properties.Settings.Default.savesPath;
            if (textBox1.Text == "")
            {
                textBox1.Text = System.IO.Directory.GetCurrentDirectory();
            }
        }


    }
}
