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
    public partial class Login : Form
    {
        public Login()
        {
            InitializeComponent();
        }

        private void txtUsername_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)13)
                txtPassword.Focus();
        }

        private void txtPassword_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)13)
                btnLogin.PerformClick();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            if (string.IsNullOrEmpty(txtUsername.Text))
            {
                MyMessageBox.ShowMessage("Please enter Your username.", "Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Cursor = this.DefaultCursor;
                txtUsername.Focus();
                return;
            }
            try
            {
                AppDataTableAdapters.usersTableAdapter user = new AppDataTableAdapters.usersTableAdapter();
                AppData.usersDataTable dt = user.Login(txtUsername.Text, txtPassword.Text);
                if (dt.Rows.Count > 0)
                {
                    Main Mform = new Main();
                    Mform.Show();
                    this.Hide();
                    this.Cursor = this.DefaultCursor;
                    
                    /*MyMessageBox.ShowMessage("Zalogowany!!!", "Message", MessageBoxButtons.OK, MessageBoxIcon.Information);*/
                }
                else
                {
                    MyMessageBox.ShowMessage("Niepoprawny login lub hasło!", "Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.Cursor = this.DefaultCursor;
                }
            }
            catch (Exception ex)
            {
                MyMessageBox.ShowMessage(ex.Message, "Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Cursor = this.DefaultCursor;
            }
        }
    }
}
