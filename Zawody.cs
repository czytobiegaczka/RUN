using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using System.IO;
using System.Threading;
using Timer = System.Windows.Forms.Timer;

namespace RUN
{
    public partial class Zawody : Form
    {
        public byte[] img;
        public int licz=0;
        public DataTable table;
        public int ileWierszy=0;

        public Zawody()
        {
            InitializeComponent();
            //testujemy czy działa formularz Wait

            using (Wait frm = new Wait(Testujemy))
            {
                frm.ShowDialog(this);
            }

            ViewPicture();
         }
        void Testujemy()
        {
            for (int i = 0; i <= 500; i++)
                Thread.Sleep(10);
        }

        public void changeimage(object sender, EventArgs e)
        {
                 byte[] img = (byte[])table.Rows[licz][0];

                MemoryStream ms = new MemoryStream(img);

                picZawody.Image = Image.FromStream(ms);
                picZawody.SizeMode = PictureBoxSizeMode.Zoom;          
            
            if (licz<ileWierszy-1)
            {
                licz++;
            }
            else
            {
                licz = 0;
            }
            

        }

        private void ViewPicture()
        {
            MySqlConnection MyCon;
            string connectionString = @"DATASOURCE=db4free.net;PORT=3306;DATABASE=trening;UID=trening;PASSWORD=treningRTL;OldGuids=True;convert zero datetime=True";

            using (MyCon = new MySqlConnection(connectionString))
            {
                try
                {
                    MyCon.Open(); //otwarcie nowego połączenia


                    MySqlCommand mySqlCommand = new MySqlCommand("select picture.fota from picture where zawody_id=5", MyCon); //deklaracja nowej komendy SQL                           

                    MySqlDataAdapter mySqlDataAdapter = new MySqlDataAdapter(mySqlCommand);
                    table = new DataTable();

                    mySqlDataAdapter.Fill(table);
                    ileWierszy = table.Rows.Count;

                    Timer tm = new Timer();
                    tm.Interval = 2000;
                    tm.Tick += new EventHandler(changeimage);
                    tm.Start();

                    mySqlDataAdapter.Dispose();
                    MyCon.Close();

                }
                catch (MySqlException ex)
                {
                    //When handling errors, you can your application's response based 
                    //on the error number.
                    //The two most common error numbers when connecting are as follows:
                    //0: Cannot connect to server.
                    //1045: Invalid user name and/or password.
                    switch (ex.Number)
                    {
                        case 0:
                            MyMessageBox.ShowMessage("Problem z połaczeniem, próbuj dalej!", "Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            break;

                        case 1045:
                            MessageBox.Show("Invalid username/password, please try again");
                            break;
                        default:
                            MyMessageBox.ShowMessage(ex.ToString(), "Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            break;
                    }
                }
            }
        }

        public void txtZawodyDystans_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && e.KeyChar != ',')
                e.Handled = true;
        }

        public void txtZawodyNumer_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && e.KeyChar != ',')
                e.Handled = true;
        }

        private void Zawody_FormClosing(object sender, FormClosingEventArgs e)
        {
        }

        private void button1_Click(object sender, EventArgs e)
        {
            AddPicture addPicture = new AddPicture();
            addPicture.ShowDialog();
        }
    }
}
