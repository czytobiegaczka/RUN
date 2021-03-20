using MySql.Data.MySqlClient;
using System;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Security.Cryptography;
using System.IO;


namespace RUN
{
    public partial class Login : Form
    {
        public static string connectionString = @"DATASOURCE=freedb.tech;PORT=3306;DATABASE=freedbtech_trening;UID=freedbtech_trening;PASSWORD=treningRTL;OldGuids=True;convert zero datetime=True";
        public MySqlConnection MyLoginCon;
        private DataTable users;
        private string haslo;
        private int potwierdz = 0;


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
            string source = txtPassword.Text;
            using (SHA256 sha256Hash = SHA256.Create())
            {
                //From String to byte array
                byte[] sourceBytes = Encoding.UTF8.GetBytes(source);
                byte[] hashBytes = sha256Hash.ComputeHash(sourceBytes);
                string hash = BitConverter.ToString(hashBytes).Replace("-", String.Empty);

                haslo = hash;
            }

                if (string.IsNullOrEmpty(txtUsername.Text) ^ string.IsNullOrEmpty(txtPassword.Text))
            {
                MyMessageBox.ShowMessage("Please enter Your username and password.", "Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Cursor = this.DefaultCursor;
                txtUsername.Focus();
                return;
            }

            try
            {
                baza_connect();

                if (potwierdz > 0) 
                {
                    Main Mform = new Main();
                    Mform.Show();
                    this.Hide();
                    this.Cursor = this.DefaultCursor;
                    
                    
                }
                else
                {
                    MyMessageBox.ShowMessage("Niepoprawny login lub hasło!", "Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.Cursor = this.DefaultCursor;
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                MyMessageBox.ShowMessage(ex.Message, "Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Cursor = this.DefaultCursor;
            }
        }


        public void baza_connect()
        {
            this.users = new DataTable();
            using (this.MyLoginCon = new MySqlConnection(connectionString))
            {
                try
                {
                    MyLoginCon.Open(); //otwarcie nowego połączenia
                    potwierdz=LicznikRekordow(txtUsername.Text,haslo);
                    MyLoginCon.Close(); //zamknięcie połaczenia

                }
                //catch (MySqlException ex)
                catch (Exception ex)
                {
                    //When handling errors, you can your application's response based 
                    //on the error number.
                    //The two most common error numbers when connecting are as follows:
                    //0: Cannot connect to server.
                    //1045: Invalid user name and/or password.
                    /*
                    switch (ex.Number)
                    {
                        case 0:
                            MyMessageBox.ShowMessage("Problem z połaczeniem, próbuj dalej!", "Message", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            break;
                        case 1045:
                            MyMessageBox.ShowMessage("UWAGA! Nieprawidłowe hasło lub login, próbuj dalej!", "Message", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            break;
                        default:
                            MyMessageBox.ShowMessage("Wystąpił problem z połaczeniem, próbuj dalej!", "Message", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            break;
                            
                    }
                    */
                    //MessageBox.Show(ex.Message.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    if (MyMessageBox.ShowMessage("Błąd połączenia. Czy łączyć jeszcze raz?", "Message", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                    {
                        baza_connect();
                    }
                    else
                    {
                        Application.Exit();
                    }

                }
            }
        }

        private int LicznikRekordow(string userName, string userPassword)
        {
            string like = "'" + userName + "'"+ " and password="+"'"+userPassword+"'";
            string query = "SELECT COUNT(*) FROM users WHERE username=" + like;
            int suma = 0;

                //Create Command
                MySqlCommand cmd = new MySqlCommand(query, MyLoginCon);
                suma = int.Parse(cmd.ExecuteScalar().ToString());

            return suma;
        }
    }
}
