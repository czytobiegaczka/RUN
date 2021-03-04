using System;
using System.Data;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using System.IO;
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
        public bool czyTimer = false;
        public string zawodyTyp = "";
        private Oplata oplata;
        private int oplata_id = 0;


        public Zawody(int zawID)
        {
            InitializeComponent();

            zawodyID = zawID;

          
            if (zawID != 0)
            {
                menuAddPicture.Enabled = true;
                menuSave.Visible = false;
                WaitForPicture(zawID);
                PictureTimer();
            }
            else
            {
                menuAddPicture.Enabled = false;
                menuSave.Visible = true;
                menuSave.Enabled = false;
                
            }

        }

        public void WaitForPicture(int zawodID)
        // obsługa 2 wątków podczas wgrywania zdjęć z bazy danych
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
            if (ileWierszy > 0)
            {
                //pobieranie zdjćeia z tabeli z danymi
                byte[] img = (byte[])table.Rows[licz][0];
                MemoryStream ms = new MemoryStream(img);
                picZawody.Image = Image.FromStream(ms);
                picZawody.Name=table.Rows[licz][1].ToString();
                picZawody.SizeMode = PictureBoxSizeMode.Zoom;

                // timer, który wyświetla zdjęcia co określony czas 
                tm = new Timer();
                tm.Tick += new EventHandler(changeimage);
                tm.Interval = 5000;
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
            string connectionString = @"DATASOURCE=freedb.tech;PORT=3306;DATABASE=freedbtech_trening;UID=freedbtech_trening;PASSWORD=treningRTL;OldGuids=True;convert zero datetime=True";
            

            using (MyCon = new MySqlConnection(connectionString))
            {
                try
                {
                    MyCon.Open(); //otwarcie nowego połączenia

                    string queryString = "select picture.fota,picture.nazwa from picture where zawody_id=" + zawID.ToString();
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

        private void picZawody_MouseMove(object sender, MouseEventArgs e)
        {

        }

        private void menuSave_Click(object sender, EventArgs e)
        {
            // wybór rodzajów zawodów
            Typ typ = new Typ();
            typ.ShowDialog();
            zawodyTyp = typ.jakiTyp;

            int zmianaDys = Convert.ToInt16(Convert.ToDecimal(txtZawodyDystans.Text == "" ? "0" : txtZawodyDystans.Text) * 100);
            if (zmianaDys != 0)
            {
                Wait wait = new Wait();

                var t1 = new Task(() =>
                {
                    //dzialanie na ktore czekamy zapisujace do now swoj progress
                    saveZawody();

                    wait.Invoke(new Action(() => { wait.Dispose(); })); //zamknięcie okna Wait
                });

                t1.Start();
                wait.ShowDialog(this); //w trakcie czekania na zapis zawodów uruchamia się Wait z ProgressBar
                t1.Wait();
                MyMessageBox.ShowMessage("Wgrywanie danych zakończone powodzeniem!", "Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
                menuAddPicture.Enabled = true;
            }
            else
            {
                this.Cursor = this.DefaultCursor;
                MyMessageBox.ShowMessage("UWAGA! Brak dystansu!", "Message", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
        }

        public void saveZawody()
        {
            int zmianaDys = Convert.ToInt16(Convert.ToDecimal(txtZawodyDystans.Text == "" ? "0" : txtZawodyDystans.Text) * 100);

            MySqlConnection MyCon;
            string connectionString = @"DATASOURCE=freedb.tech;PORT=3306;DATABASE=freedbtech_trening;UID=freedbtech_trening;PASSWORD=treningRTL;OldGuids=True;convert zero datetime=True";


            using (MyCon = new MySqlConnection(connectionString))
            {
                try
                {

                    MemoryStream ms = new MemoryStream();
                    Properties.Resources.m.Save(ms, Properties.Resources.m.RawFormat);
                    byte[] img = ms.ToArray();

                    MyCon.Open(); //otwarcie nowego połączenia

                    MySqlCommand mySqlCommand = new MySqlCommand("ak_zawody_Add", MyCon); //deklaracja nowej komendy SQL                           
                    mySqlCommand.CommandType = CommandType.StoredProcedure;
                    mySqlCommand.Parameters.AddWithValue("data", dateTimeZawody.Text);
                    mySqlCommand.Parameters.AddWithValue("naz", txtWpisNazwaZawodow.Text);
                    mySqlCommand.Parameters.AddWithValue("dys", zmianaDys);
                    mySqlCommand.Parameters.AddWithValue("num", txtZawodyNumer.Text);
                    mySqlCommand.Parameters.AddWithValue("cza", dateTimeZawodyCzas.Value);
                    mySqlCommand.Parameters.AddWithValue("typ", zawodyTyp);
                    mySqlCommand.Parameters.AddWithValue("pic", img);
                    mySqlCommand.ExecuteNonQuery();
                    mySqlCommand.Dispose();


                    // pobieranie zawodyId
                    string query = "SELECT zawody_Id FROM zawody LEFT JOIN miesiac on zawody.miesiac_Id=miesiac.miesiac_Id WHERE miesiac.data='" + dateTimeZawody.Text + "'";

                    MySqlCommand mySqlCommandNext = new MySqlCommand(query, MyCon);

                    if (mySqlCommandNext.ExecuteScalar() != DBNull.Value)
                    {
                        zawodyID = int.Parse(mySqlCommandNext.ExecuteScalar() + "");
                    }
                    Console.WriteLine(zawodyID.ToString());
                    MyCon.Close();

                }
                catch (Exception ex)
                {
                    //When handling errors, you can your application's response based 
                    //on the error number.
                    //The two most common error numbers when connecting are as follows:
                    //0: Cannot connect to server.
                    //1045: Invalid user name and/or password.
                    //ViewPicture(zawID);
                    saveZawody();
                }
            }
        }


        public void txtWpisNazwaZawodow_TextChanged(object sender, EventArgs e)
        {
            if (txtWpisNazwaZawodow.Text!="")
            {
                menuSave.Enabled = true;
            }
            else
            {
                menuSave.Enabled = false;
            }
        }

        private void poprawDaneToolStripMenuItem_Click(object sender, EventArgs e)
        {
        }

        int x = 0, y = 0;
        string dajOplata;

        private void picOplata_MouseLeave(object sender, EventArgs e)
        {
            oplata.Dispose();
            oplata_id = 0;
            this.Cursor = this.DefaultCursor;
        }



        private void pictureBox2_MouseMove(object sender, MouseEventArgs e)
        {
            this.Cursor = Cursors.Hand;
            if (oplata_id == 0)
            {
                oplata_id++;
                x = Cursor.Position.X;
                y = Cursor.Position.Y;
                dajOplata = lblOplata.Text;
                oplata = new Oplata(dajOplata);
                oplata.StartPosition = FormStartPosition.Manual;
                oplata.Location = new System.Drawing.Point(x - oplata.Width, y);
                oplata.Show();
            }
        }
    }
}
