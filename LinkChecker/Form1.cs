using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LinkChecker
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Go_Click(object sender, EventArgs e)
        {
            Go.Enabled = false;
            if (openFileDialog1.ShowDialog() != DialogResult.OK) return;
            ExcelHandler excel = new ExcelHandler(openFileDialog1.FileName, URL.Text);
            excel.Parse();
            Go.Enabled = true;
        }
    }
}
