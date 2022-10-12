using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Calulator
{
    public partial class Form1 : Form
    {
        Double results = 0;
        String operation;
        bool enter_value = false;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button_Decimal(object sender, EventArgs e)
        {
            Button num = (Button)sender;
            txtDisplay.Text += num.Text;
        }

        private void button_Negative(object sender, EventArgs e)
        {
            txtDisplay.Text += "-";
        }

        private void button_Equal(object sender, EventArgs e)
        {

        }

        private void button_Plus(object sender, EventArgs e)
        {
            txtDisplay.Text += "+";
        }

        private void button_Minus(object sender, EventArgs e)
        {
            txtDisplay.Text += "-";

        }

        private void button_Mult(object sender, EventArgs e)
        {
            txtDisplay.Text += "*";

        }

        private void button_Div(object sender, EventArgs e)
        {
            txtDisplay.Text += "/";

        }

        private void button_Back(object sender, EventArgs e)
        {
            if (txtDisplay.Text.Length > 0)
                txtDisplay.Text = txtDisplay.Text.Remove(txtDisplay.Text.Length - 1, 1);
        }

        private void button_Clear(object sender, EventArgs e)
        {
            txtDisplay.Text = "";
        }

        private void button_power(object sender, EventArgs e)
        {
            txtDisplay.Text += "^";
        }

        private void button_Nat_Log(object sender, EventArgs e)
        {
            txtDisplay.Text += "Ln(";
        }

        private void button_Log(object sender, EventArgs e)
        {
            txtDisplay.Text += "Log(";

        }

        private void button_Cos(object sender, EventArgs e)
        {
            txtDisplay.Text += "cos(";

        }

        private void button_Sin(object sender, EventArgs e)
        {
            txtDisplay.Text += "sin(";

        }

        private void button_Tan(object sender, EventArgs e)
        {
            txtDisplay.Text += "tan(";

        }

        private void button_Cot(object sender, EventArgs e)
        {
            txtDisplay.Text += "cot(";

        }

        private void button_Left_Curl(object sender, EventArgs e)
        {
            txtDisplay.Text += "{";

        }

        private void button_Right_Curl(object sender, EventArgs e)
        {
            txtDisplay.Text += "}";

        }

        private void button_Left_Para(object sender, EventArgs e)
        {
            txtDisplay.Text += "(";

        }

        private void button_Right_Para(object sender, EventArgs e)
        {
            txtDisplay.Text += ")";

        }

        private void button_Num(object sender, EventArgs e)
        {
            Button num = (Button)sender;
            txtDisplay.Text += num.Text;
        }
    }
}
