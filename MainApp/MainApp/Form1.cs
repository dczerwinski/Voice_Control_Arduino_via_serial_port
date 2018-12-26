using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Speech.Recognition;
using System.Windows.Forms;
using System.Speech.Synthesis;
using System.Threading;
using System.IO;
using System.IO.Ports;

namespace MainApp
{
    public partial class Form1 : Form
    {
        SpeechRecognitionEngine sre = new SpeechRecognitionEngine();
        SpeechSynthesizer SpeechSynthesizer = new SpeechSynthesizer();
        SerialPort serial;


        public Form1()
        {
            InitializeComponent();
            com_box();
            BtS();
            if (File.Exists(Environment.CurrentDirectory + "\\commands.ands") == false) File.Create(Environment.CurrentDirectory + "\\commands.ands");
            string[] lines = File.ReadAllLines(Environment.CurrentDirectory + "\\commands.ands");
            foreach (string line in lines)
            {
                comboBox3.Items.Add(line);
            }
            textBox1.Enabled = false;
        }

        private void get_speech(object sender, SpeechRecognizedEventArgs e)
        {
            string a = e.Result.Text.ToString();
            textBox1.AppendText(a);
            textBox1.AppendText(Environment.NewLine);
            serial.Write(a);
        }

        void com_box()
        {
            string[] porty = SerialPort.GetPortNames();
            foreach (string port in porty)
            {
                comboBox1.Items.Add(port);
            }

        }

        void BtS()
        {
            string[] bity = { "75", "110", "300", "600", "1200", "2400", "4800", "9600", "14400", "19200", "38400", "57600", "115200", "128000", "256000" };
            foreach (string bit in bity)
            {
                comboBox2.Items.Add(bit);
            }
            comboBox2.SelectedIndex = 7;
        }


        private void LoadGrammar()
        {
            Choices choices = new Choices();
            string[] lines = File.ReadAllLines(Environment.CurrentDirectory + "\\commands.ands");
            choices.Add(lines);
            GrammarBuilder grammarBuilder = new GrammarBuilder(choices);
            grammarBuilder.Culture = new System.Globalization.CultureInfo("en-GB");
            Grammar wordlist = new Grammar(grammarBuilder);

            sre.LoadGrammar(wordlist);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string port = this.comboBox1.GetItemText(this.comboBox1.SelectedItem);
            string bts = this.comboBox1.GetItemText(this.comboBox2.SelectedItem);
            if (port == "") MessageBox.Show("Musisz wybrać port!", "Error");
            else if (bts == "") MessageBox.Show("Musisz wybrać BpS!", "Error");
            else
            {
                int b = Int32.Parse(bts);
                comboBox1.Enabled = false;
                comboBox2.Enabled = false;
                button1.Enabled = false;
                button2.Enabled = false;

                serial = new SerialPort(port);
                serial.BaudRate = b;
                serial.Open();

                sre.SpeechRecognized += new EventHandler<SpeechRecognizedEventArgs>(get_speech);
                LoadGrammar();
                sre.SetInputToDefaultAudioDevice();
                sre.RecognizeAsync(RecognizeMode.Multiple);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Form2 form2 = new Form2();
            form2.Show();
            form2.FormClosed += new FormClosedEventHandler(form2_formclosed);

        }

        private void form2_formclosed(object sender, FormClosedEventArgs e)
        {
            comboBox3.Items.Clear();
            string[] lines = File.ReadAllLines(Environment.CurrentDirectory + "\\commands.ands");
            foreach (string line in lines)
            {
                comboBox3.Items.Add(line);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            comboBox1.Enabled = true;
            comboBox2.Enabled = true;
            button1.Enabled = true;
            button2.Enabled = true;
            if (serial != null) serial.Close();
        
        
            sre.RecognizeAsyncStop();
        }

   
    }
}
