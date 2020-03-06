using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            
        }

        public Form1(string message, string buttonText1, string buttonText2)
        {
            label1.Text = message;
            button1.Text = buttonText1;
            button2.Text = buttonText2;
        }

    }
}
