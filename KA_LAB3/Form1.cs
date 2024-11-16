using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace KA_LAB3
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string text = "12+22+32";
            Lexer lexer = new Lexer(text);
            var tokens = lexer.Tokenisation();
            foreach (var token in tokens)
            {
                richTextBox1.Text += $"{token.Kind} {token.Value} \n";
            }
        }
    }
}
