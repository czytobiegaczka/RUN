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
        public int zawodyID;
        public Timer tm;
        bool czyTimer = false;

        public Zawody(int zawID)
        {
            InitializeComponent();

            zawodyID = zawID;

            WaitForPicture(zawID);
            PictureTimer();
        }

        public void WaitForPicture(int zawodID)
        {
            Wait wait = new Wait();

            var t1 = new Task(() =>
            {
                //dzialanie na ktore czekamy zapisujace do now swoj progress
                ViewPicture(zawodID);

                wait.Invoke(new Action(() => { wait.Dispose(); })); //zamknięcie okna Wait
            });

            t1.Start();
            wait.ShowDialog(this); //w trakcie czekania na wgranie zdjęć uruchamia się Wait z ProgressBar
            t1.Wait();
        }

        public void PictureTimer()
        {
            Console.WriteLine(ileWierszy.ToString());
            if (ileWierszy > 0)
            {
                byte[] img = (byte[])table.Rows[licz][0];
                MemoryStream ms = new MemoryStream(img);
                picZawody.Image = Image.FromStream(ms);
                picZawody.SizeMode = PictureBoxSizeMode.Zoom;

                // timer, który wyświetla zdjęcia co określony czas 
                tm = new Timer();
                tm.Tick += new EventHandler(changeimage);
                tm.Interval = 1500;
                tm.Start();
                czyTimer = true;
            }
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

        public void ViewPicture(int zawID)
        {
            MySqlConnection MyCon;
            string connectionString = @"DATASOURCE=db4free.net;PORT=3306;DATABASE=trening;UID=trening;PASSWORD=treningRTL;OldGuids=True;convert zero datetime=True";
            

            using (MyCon = new MySqlConnection(connectionString))
            {
                try
                {
                    MyCon.Open(); //otwarcie nowego połączenia

                    string queryString = "select picture.fota from picture where zawody_id=" + zawID.ToString();
                    Console.WriteLine(queryString);
                    MySqlCommand mySqlCommand = new MySqlCommand(queryString, MyCon); //deklaracja nowej komendy SQL                           

                    MySqlDataAdapter mySqlDataAdapter = new MySqlDataAdapter(mySqlCommand);
                    table = new DataTable();

                    mySqlDataAdapter.Fill(table);
                    ileWierszy = table.Rows.Count;
                    
                    mySqlDataAdapter.Dispose();
                    MyCon.Close();

                }
                catch (Exception ex)
                {
                    //When handling errors, you can your application's response based 
                    //on the error number.
                    //The two most common error numbers when connecting are as follows:
                    //0: Cannot connect to server.
                    //1045: Invalid user name and/or password.
                    ViewPicture(zawID);
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

         public void menuAddPicture_Click_1(object sender, EventArgs e)
        {
            AddPicture addPicture = new AddPicture(zawodyID);
            addPicture.ShowDialog();

            //odświeżanie okna zawodów
            licz = 0;
            if(czyTimer)
            {
                tm.Stop();
            }
            WaitForPicture(zawodyID);
            PictureTimer();
        }
    }
}
