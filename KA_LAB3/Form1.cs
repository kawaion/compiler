using KA_LAB3.Expression;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.Remoting;
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
        NodeExpression root;
        private void button1_Click(object sender, EventArgs e)
        {
            //string text = "1+2^2*3";
            string text = textBox1.Text;
            Lexer lexer = new Lexer(text);
            var tokens = lexer.Tokenisation();
            BuilderTokensIntoExpression buider = new BuilderTokensIntoExpression(tokens);
            root = buider.Build();
            button2.ForeColor = Color.Black;
            button2.Enabled = true;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Evaluator evaluator = new Evaluator();
            double res = evaluator.Evaluate(root);
            richTextBox1.Text = res.ToString();
        }
    }
}
