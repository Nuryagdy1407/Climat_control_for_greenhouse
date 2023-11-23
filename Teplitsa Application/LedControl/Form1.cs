using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO.Ports;
using System.Threading;

namespace LedControl
{
    public partial class Form1 : Form
    {
        private SerialPort RFID;
        private string DispString; 
        public Form1()
        {
            InitializeComponent();
        }

        private void yap_btn_Click(object sender, EventArgs e)
        {
            //if (Port.IsOpen) { Port.Close(); }
            Application.Exit();
            //this.Close();
        }

        private void kicelt_btn_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        System.IO.Ports.SerialPort Port;
        bool IsClosed = false;
      
        private void Form1_Load(object sender, EventArgs e)
        {
            timer1.Start();

            temp_gauge.Value = 0;
            hum_gauge.Value = 0;
            soil_gauge.Value = 0;

            string[] ports = SerialPort.GetPortNames();
            foreach(string port in ports)
            {  ports_cmb.Items.Add(port);  }

            if (ports_cmb.Items.Count > 0) { ports_cmb.SelectedIndex = 0; }
             
        }

        private void ListenSerial()
        {
            while (!IsClosed)
            {
                try
                {
                    string AString = Port.ReadLine();
                    txtSomething.Invoke(new MethodInvoker(
                        delegate
                        {
                            //string[] field = AString.Split('/');
                            txtSomething.Text = AString;
                        }
                        ));
                }
                catch { }
            }
        }
        int r = 0;
        private void gunaGoogleSwitch1_CheckedChanged(object sender, EventArgs e)
        {
            if (ports_cmb.Items.Count > 0)
            {
                if (r==0)
                {
                    Port = new System.IO.Ports.SerialPort();
                    Port.PortName = ports_cmb.SelectedItem.ToString();
                    Port.Open();
                    Port.BaudRate = 9600;
                    Port.ReadTimeout = 500;

                    try
                    {
                        Port.Open();
                    }
                    catch { }
                    r = 1;

                    Thread Hilo = new Thread(ListenSerial);
                    Hilo.Start();

                    timer2.Start();
                }
                else
                {
                    timer2.Stop();
                    Port.Close();
                    r = 0;
                    temp_gauge.Value = 0; hum_gauge.Value = 0; soil_gauge.Value = 0;
                }
               
            }
            else
            {
                MessageBox.Show("Enjam baglanmadyk!");
                //gunaGoogleSwitch1.Checked = false;
            }
            
        }

        int temp=0, hum=0, shum=0;
        private void timer2_Tick(object sender, EventArgs e)
        {
            if (txtSomething.Text != "")
            {
                Console.WriteLine(txtSomething.Text);
                string[] field = txtSomething.Text.Split('/');
                string a = field[0]; double g = Double.Parse(a, System.Globalization.CultureInfo.InvariantCulture); temp = Convert.ToInt32(g/1);
                string b = field[1]; double h = Convert.ToDouble(b, System.Globalization.CultureInfo.InvariantCulture); hum = Convert.ToInt32(h / 1);
                string c = field[2]; double j = Convert.ToDouble(c, System.Globalization.CultureInfo.InvariantCulture); shum = Convert.ToInt32(j/1); 
            }

            temp_gauge.Value = temp; temp_txt.Text = (temp+" C").ToString(); if (temp < temp1_nud.Value || temp>temp2_nud.Value) { temp_gauge.ArrowColor = Color.Red; temp_gauge.ProgressColor = Color.Red; } else { temp_gauge.ArrowColor = Color.Green; temp_gauge.ProgressColor = Color.Green; }
            hum_gauge.Value = hum; hum_txt.Text = (hum + " %").ToString(); if (hum < hum1_nud.Value || hum > hum2_nud.Value) { hum_gauge.ArrowColor = Color.Red; hum_gauge.ProgressColor = Color.Red; } else { hum_gauge.ArrowColor = Color.Blue; hum_gauge.ProgressColor = Color.Blue; }
            soil_gauge.Value =shum; toprak_txt.Text = (shum + " %").ToString(); if (shum < soil1_nud.Value || shum > soil2_nud.Value) { soil_gauge.ArrowColor = Color.Red; soil_gauge.ProgressColor = Color.Red; } else { soil_gauge.ArrowColor = Color.Purple; soil_gauge.ProgressColor = Color.Purple; }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            time_lbl.Text=(DateTime.Now.Hour+":"+DateTime.Now.Minute+":"+DateTime.Now.Second).ToString();
        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {
            timer1.Start();

            temp_gauge.Value = 0;
            hum_gauge.Value = 0;
            soil_gauge.Value = 0;

            ports_cmb.Items.Clear();
            string[] ports = SerialPort.GetPortNames();
            foreach (string port in ports)
            { ports_cmb.Items.Add(port); }

            if (ports_cmb.Items.Count > 0) { ports_cmb.SelectedIndex = 0; }
        }
      }
}
