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
using Microsoft.VisualBasic;

namespace MainApp
{
    public partial class Form2 : Form
    {

        List<string> lines;



        public Form2()
        {
            InitializeComponent();
            string[] cos = File.ReadAllLines(Environment.CurrentDirectory + "\\commands.ands");
            lines = new List<string>(cos);
            foreach (string line in lines)
            {
                comboBox1.Items.Add(line);
            }

        }


        private void button1_Click(object sender, EventArgs e)
        {//zapisz i ex
            File.WriteAllText(Environment.CurrentDirectory + "\\commands.ands", string.Empty);
            File.WriteAllLines(Environment.CurrentDirectory + "\\commands.ands", lines);
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {//ex
            this.Close();
        }

        private void button3_Click(object sender, EventArgs e)
        {//usun
            int index = this.comboBox1.SelectedIndex;
            if (index < 0) MessageBox.Show("Nie wybrałeś żadnego polecenia!", "Usuwamy");
            else
            {
                DialogResult dialogResult = MessageBox.Show("Czy na pewno chcesz usunąć to polecenie?", "Usuwamy", MessageBoxButtons.YesNo);
                if (dialogResult == DialogResult.Yes)
                {
                    this.comboBox1.Items.RemoveAt(index);
                    lines.RemoveAt(index);

                }
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {//dodaj
            string input = Microsoft.VisualBasic.Interaction.InputBox("Wpisz polecenie jakie chcesz dodać", "Dodaj", "Default", -1, -1);
            lines.Add(input);
            comboBox1.Items.Add(input);
        }
    }
}
