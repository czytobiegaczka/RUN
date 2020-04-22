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
    
    public partial class Typ : Form
    {
        public string jakiTyp = "";
        public Typ()
        {
            InitializeComponent();
        }

        private void btnWybierzTyp_Click(object sender, EventArgs e)
        {
            if (jakiTyp=="")
            {
                MyMessageBox.ShowMessage("Proszę określ rodzaj zawodów!", "Message", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            else
            {
                this.Dispose();
            }
                
        }

        private void rdoMaraton_CheckedChanged(object sender, EventArgs e)
        {
            jakiTyp = "M";
        }

        private void rdoPolmaraton_CheckedChanged(object sender, EventArgs e)
        {
            jakiTyp = "P";
        }

        private void rdoDziesiec_CheckedChanged(object sender, EventArgs e)
        {
            jakiTyp = "D";
        }

        private void rdoInne_CheckedChanged(object sender, EventArgs e)
        {
            jakiTyp = "I";
        }
    }
}
