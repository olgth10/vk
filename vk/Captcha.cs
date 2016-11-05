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

namespace vk
{
    public partial class Captcha : Form
    {
        public string captchaSolve;
        public Captcha()
        {
            InitializeComponent();
            FileStream fs = File.OpenRead("123.jpg");
            Image img = Image.FromStream(fs);
            pictureBox1.Size = img.Size;
            pictureBox1.Image = img;
            fs.Close();                       
        }

        private void button1_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
            captchaSolve = textBox1.Text;
        }
    }
}
