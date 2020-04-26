using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RUN
{
    public partial class Oplata : Form
    {
        public Oplata(string oplata)
        {
            InitializeComponent();
            lblOplata.Text = oplata;
        }

        private void Oplata_FormClosed(object sender, FormClosedEventArgs e)
        {
            this.Dispose();
        }
    }
}
