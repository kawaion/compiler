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
            Lexer lexer = new Lexer(text,GetVarsFromDataGrid());
            var tokens = lexer.Tokenisation();
            BuilderExpressionTree buider = new BuilderExpressionTree(tokens);
            root = buider.Build();
            button2.ForeColor = Color.Black;
            button2.Enabled = true;
            CreateDataGridForVarsValue();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Evaluator evaluator = new Evaluator(GetDictionaryVarsValue());
            double res = evaluator.Evaluate(root);
            richTextBox1.Text = res.ToString();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (int.TryParse(textBox2.Text, out int size))
            {
                dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                dataGridView1.RowCount = 1;
                dataGridView1.ColumnCount = size;

            }
        }
        private void CreateDataGridForVarsValue()
        {
            if (dataGridView1 != null)
            {
                dataGridView2.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                dataGridView2.RowCount = 1;
                dataGridView2.ColumnCount = dataGridView1.ColumnCount;

            }
        }
        private Dictionary<string,double> GetDictionaryVarsValue()
        {
            if(dataGridView1 != null)
            {
                Dictionary<string, double> dict = new Dictionary<string, double>();
                for (int i = 0; i < dataGridView1.ColumnCount; i++)
                {
                    dict.Add((string)dataGridView1.Rows[0].Cells[i].Value, Convert.ToDouble(dataGridView2.Rows[0].Cells[i].Value));
                }
                return dict;
            }
            return null;
        }

        private List<string> GetVarsFromDataGrid()
        {
            if (dataGridView1 != null)
            {
                List<string> vars = new List<string>();
                for (int i = 0; i < dataGridView1.ColumnCount; i++)
                {
                    vars.Add((string)dataGridView1.Rows[0].Cells[i].Value);
                }
                return vars;
            }
            return null;
        }
    }
}
