using MySql.Data.MySqlClient;
using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using System.Xml;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.IO;
using System.Collections.Generic;

namespace RUN
{
    public partial class Main : Form
    {
        public static string connectionString = @"DATASOURCE=freedb.tech;PORT=3306;DATABASE=freedbtech_trening;UID=freedbtech_trening;PASSWORD=treningRTL;OldGuids=True;convert zero datetime=True";
        string[] miesiacNr = { "00", "01", "02", "03", "04", "05", "06", "07", "08", "09", "10", "11", "12" };
        string[] miesiacNazwa = { "Styczeń", "Luty", "Marzec", "Kwiecień", "Maj", "Czerwiec", "Lipiec", "Sierpień", "Wrzesień", "Październik", "Listopad", "Grudzień" };
        string[] tydzienNazwa = { "Niedziela","Poniedziałek", "Wtorek", "Środa", "Czwartek", "Piątek", "Sobota"};
        public MySqlConnection MyCon;
        public Boolean czyGodzina=false;
        private DataTable data;
        private static readonly HttpClient client = new HttpClient();
        public string zawartoscXML;
        private prognoza pogoda;
        private meteo Meteo;
        private int meteo_id = 0;
        

        public Main()
        {
            InitializeComponent();
        }

        private void Main_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
        }

        private void Main_Load(object sender, EventArgs e)
        {
            mati_connect();
            DataGridView(DateTime.Now);
        }

        public void mati_connect()
        {
            this.data = new DataTable();
            using (this.MyCon = new MySqlConnection(connectionString))
            {
                try
                {
                    MyCon.Open(); //otwarcie nowego połączenia
                    GetTable();
                    MyCon.Close(); //zamknięcie połaczenia

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
                        mati_connect();
                    }
                    else
                    {
                        Application.Exit();
                    }

                }
            }
        }

        public void GetTable()
        // pobranie tabeli z bazy danych do tabeli (data) w pamięci 
        {
            string query = "select * from ak_miesiac_zawody";
            MySqlDataAdapter MyDa = new MySqlDataAdapter(query, MyCon);

            MyDa.Fill(this.data);
        }

        void DataGridView(DateTime dzien)
        // obsługa wyświetlania przeglądarki bazy danych 
        {
            string tylkoRok = "'%." + dzien.Year.ToString() + "'";
            string tylkoMiesiac = "'%." + miesiacNr[dzien.Month] + "." + dzien.Year.ToString() + "'";
            double sumKm = 0;
            double liczKm = 1;
            double sredKm = 0;
            double minKm = 0;
            double maxKm = 0;
            double sumKg = 0;
            double liczKg = 1;
            double sredKg = 0;
            double minKg = 0;
            double maxKg = 0;

            this.Cursor = Cursors.WaitCursor;
            this.Cursor = this.DefaultCursor;

            // kalendarz

            DataTable MyTab = this.data;

            // zebranie danych o dystansie od początku kariery
            sumKm = Convert.ToDouble(MyTab.Compute("SUM(dystans)", string.Empty));
            liczKm = Convert.ToInt32(MyTab.Compute("COUNT(dystans)", string.Empty));
            minKm = Convert.ToDouble(MyTab.Compute("MIN(dystans)", string.Empty));
            maxKm = Convert.ToDouble(MyTab.Compute("MAX(dystans)", string.Empty));
            lblSumaAll.Text = "Dystans pokonany od 12.03.2011 : " + String.Format("{0:0.00}", sumKm) + " km";
            lblAllKm.Text = String.Format("{0:0.00}", sumKm) + " km";
            lblAllAvgKm.Text = String.Format("{0:0.00}", sumKm / liczKm) + " km";
            lblAllMinKm.Text = String.Format("{0:0.00}", minKm) + " km";
            lblAllMaxKm.Text = String.Format("{0:0.00}", maxKm) + " km";

            // zebranie danych o wadze od początku kariery
            sumKg = Convert.ToDouble(MyTab.Compute("SUM(waga)", string.Empty));
            liczKg = Convert.ToInt32(MyTab.Compute("COUNT(waga)", string.Empty));
            minKg = Convert.ToDouble(MyTab.Compute("MIN(waga)", string.Empty));
            maxKg = Convert.ToDouble(MyTab.Compute("MAX(waga)", string.Empty));
            lblAllAvgKg.Text = String.Format("{0:0.00}", sumKg / liczKg) + " kg";
            lblAllMinKg.Text = String.Format("{0:0.00}", minKg) + " kg";
            lblAllMaxKg.Text = String.Format("{0:0.00}", maxKg) + " kg";

            // zebranie danych o rekordach w zawodach

            DataRow[] wybor_M = MyTab.Select("zawody_typ = 'M'", "czas ASC"); // dla maratonu

            lblNazwaRekMaraton.Text = " " + wybor_M[0]["zawody"].ToString() + " ";
            lblDataRekMaraton.Text = wybor_M[0]["data"].ToString();
            lblCzasRekMaraton.Text = wybor_M[0]["czas"].ToString();
            lblNumerRekMaraton.Text = wybor_M[0]["numer"].ToString();

            DataRow[] wybor_P = MyTab.Select("zawody_typ = 'P'", "czas ASC"); // dla polmaratonu

            lblNazwaRekPolowka.Text = " " + wybor_P[0]["zawody"].ToString() + " ";
            lblDataRekPolowka.Text = wybor_P[0]["data"].ToString();
            lblCzasRekPolowka.Text = wybor_P[0]["czas"].ToString();
            lblNumerRekPolowka.Text = wybor_P[0]["numer"].ToString();


            DataRow[] wybor_D = MyTab.Select("zawody_typ = 'D'", "czas ASC"); // dla dychy

            lblNazwaRekDycha.Text = " " + wybor_D[0]["zawody"].ToString() + " ";
            lblDataRekDycha.Text = wybor_D[0]["data"].ToString();
            lblCzasRekDycha.Text = wybor_D[0]["czas"].ToString();
            lblNumerRekDycha.Text = wybor_D[0]["numer"].ToString();




            MyTab.DefaultView.RowFilter = "data like " + tylkoMiesiac; //filtr wierszy w tabeli do wyświetlenia

            // jeśli brak danych dla wybranego miesiąca
            if (MyTab.DefaultView.Count == 0)
            {

            }

            this.dataGrid.DataSource = MyTab; //wypełnienie GridView danymi


            // zebranie danych o dystansie dla wybranego roku
            liczKm = 0;
            sumKm = 0;
            sredKm = 0;
            minKm = 0;
            maxKm = 0;

            grpYearKm.Text = dzien.Year.ToString();
            liczKm = Convert.ToInt32(MyTab.Compute("COUNT(dystans)", "data like " + tylkoRok));

            if (liczKm != 0) // dla roku który ma dystanse
            {
                sumKm = Convert.ToDouble(MyTab.Compute("SUM(dystans)", "data like " + tylkoRok));
                sredKm = sumKm / liczKm;
                minKm = Convert.ToDouble(MyTab.Compute("MIN(dystans)", "data like " + tylkoRok));
                maxKm = Convert.ToDouble(MyTab.Compute("MAX(dystans)", "data like " + tylkoRok));
            }

            lblYearAllKm.Text = String.Format("{0:0.00}", sumKm) + " km";
            lblYearAvgKm.Text = String.Format("{0:0.00}", sumKm / liczKm) + " km";
            lblYearMinKm.Text = String.Format("{0:0.00}", minKm) + " km";
            lblYearMaxKm.Text = String.Format("{0:0.00}", maxKm) + " km";

            // zebranie danych o wadze dla wybranego roku
            liczKg = 0;
            sumKg = 0;
            sredKg = 0;
            minKg = 0;
            maxKg = 0;

            grpYearKg.Text = dzien.Year.ToString();
            liczKg = Convert.ToInt32(MyTab.Compute("COUNT(waga)", "data like " + tylkoRok));

            if (liczKg != 0) // dla roku który ma wagi
            {
                sumKg = Convert.ToDouble(MyTab.Compute("SUM(waga)", "data like " + tylkoRok));
                sredKg = sumKg / liczKg;
                minKg = Convert.ToDouble(MyTab.Compute("MIN(waga)", "data like " + tylkoRok));
                maxKg = Convert.ToDouble(MyTab.Compute("MAX(waga)", "data like " + tylkoRok));
            }

            lblYearAvgKg.Text = String.Format("{0:0.00}", sumKg / liczKg) + " kg";
            lblYearMinKg.Text = String.Format("{0:0.00}", minKg) + " kg";
            lblYearMaxKg.Text = String.Format("{0:0.00}", maxKg) + " kg";

            // zebranie danych o dystansie dla wybranego miesiąca
            liczKm = 0;
            sumKm = 0;
            sredKm = 0;
            minKm = 0;
            maxKm = 0;

            grpMounthKm.Text = miesiacNazwa[dzien.Month - 1] + " " + dzien.Year.ToString();
            liczKm = Convert.ToInt32(MyTab.Compute("COUNT(dystans)", "data like " + tylkoMiesiac));

            if (liczKm != 0) // dla miesiąca który ma dystanse
            {
                sumKm = Convert.ToDouble(MyTab.Compute("SUM(dystans)", "data like " + tylkoMiesiac));
                sredKm = sumKm / liczKm;
                minKm = Convert.ToDouble(MyTab.Compute("MIN(dystans)", "data like " + tylkoMiesiac));
                maxKm = Convert.ToDouble(MyTab.Compute("MAX(dystans)", "data like " + tylkoMiesiac));
            }

            lblMounthAllKm.Text = String.Format("{0:0.00}", sumKm) + " km";
            lblMounthAvgKm.Text = String.Format("{0:0.00}", sredKm) + " km";
            lblMounthMinKm.Text = String.Format("{0:0.00}", minKm) + " km";
            lblMounthMaxKm.Text = String.Format("{0:0.00}", maxKm) + " km";

            // zebranie danych o wadze dla wybranego miesiąca
            liczKg = 0;
            sumKg = 0;
            sredKg = 0;
            minKg = 0;
            maxKg = 0;

            grpMounthKg.Text = miesiacNazwa[dzien.Month - 1] + " " + dzien.Year.ToString();
            liczKg = Convert.ToInt32(MyTab.Compute("COUNT(waga)", "data like " + tylkoMiesiac));

            if (liczKg != 0) // dla miesiąca który ma wagi
            {
                sumKg = Convert.ToDouble(MyTab.Compute("SUM(waga)", "data like " + tylkoMiesiac));
                sredKg = sumKg / liczKg;
                minKg = Convert.ToDouble(MyTab.Compute("MIN(waga)", "data like " + tylkoMiesiac));
                maxKg = Convert.ToDouble(MyTab.Compute("MAX(waga)", "data like " + tylkoMiesiac));
            }

            lblMounthAvgKg.Text = String.Format("{0:0.00}", sredKg) + " kg";
            lblMounthMinKg.Text = String.Format("{0:0.00}", minKg) + " kg";
            lblMounthMaxKg.Text = String.Format("{0:0.00}", maxKg) + " kg";


            // wyświetlanie wykresu wag
            chrWaga.Series.Clear();
            chrWaga.Series.Add(new Series());
            chrWaga.Series[0].ChartType = SeriesChartType.Column;
            chrWaga.ChartAreas[0].AxisY.IsStartedFromZero = false;
            // rozmiar czcionki osi X i Y
            this.chrWaga.ChartAreas[0].AxisX.LabelStyle.Font = new System.Drawing.Font("Courier New", 6, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.chrWaga.ChartAreas[0].AxisY.LabelStyle.Font = new System.Drawing.Font("Courier New", 6, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            for (int i = 0; i < dataGrid.Rows.Count - 1; i++)
            {
                this.chrWaga.Series[0].Points.AddXY(dataGrid.Rows[i].Cells[1].Value.ToString(), dataGrid.Rows[i].Cells[3].Value);
            }


        }

        private void dataGrid_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        // dopisanie lub zmiana wartości wagi i dystansu
        {
            this.Cursor = Cursors.WaitCursor;
            if (dataGrid.CurrentCell != null)
            {
                using (this.MyCon = new MySqlConnection(connectionString))
                {
                    try
                    {
                        MyCon.Open(); //otwarcie nowego połączenia
                        DataGridViewRow dataGrRow = dataGrid.CurrentRow;
                        string komo = dataGrid.CurrentCellAddress.X.ToString();
                        int zmianaWag = Convert.ToInt16(Convert.ToDecimal(dataGrRow.Cells["txtWaga"].Value == DBNull.Value ? "0" : dataGrRow.Cells["txtWaga"].Value) * 100);
                        int zmianaDys = Convert.ToInt16(Convert.ToDecimal(dataGrRow.Cells["txtDystans"].Value == DBNull.Value ? "0" : dataGrRow.Cells["txtDystans"].Value) * 100);

                        string zapamietajData = Convert.ToString(dataGrRow.Cells["txtData"].Value); //określenie i zapamiętanie miesiąca do wyświetlenia
                        DateTime jakiMiesiac = DateTime.ParseExact(Convert.ToString(dataGrRow.Cells["txtData"].Value), "dd.MM.yyyy", null); //określenie i zapamiętanie miesiąca do wyświetlenia
                        zapamietajData = zapamietajData.Remove(0, 3);
                        zapamietajData = "'___" + zapamietajData + "'";


                        if (komo == "3") //zmiana z kolumnie waga
                        {
                            MySqlCommand mySqlCommand = new MySqlCommand("ak_waga_Add", MyCon); //deklaracja nowej komendy SQL                           
                            mySqlCommand.CommandType = CommandType.StoredProcedure;
                            mySqlCommand.Parameters.AddWithValue("mies", dataGrRow.Cells["txtMiesiac_Id"].Value);
                            mySqlCommand.Parameters.AddWithValue("wag", zmianaWag);
                            mySqlCommand.ExecuteNonQuery();
                        }
                        else if (komo == "4") //zmiana w kolumnie dystans
                        {
                            MySqlCommand mySqlCommand = new MySqlCommand("ak_trening_Add", MyCon); //deklaracja nowej komendy SQL
                            mySqlCommand.CommandType = CommandType.StoredProcedure;
                            mySqlCommand.Parameters.AddWithValue("mies", dataGrRow.Cells["txtMiesiac_Id"].Value);
                            mySqlCommand.Parameters.AddWithValue("dys", zmianaDys);
                            mySqlCommand.ExecuteNonQuery();
                        }
                        this.data = new DataTable();
                        GetTable();
                        MyCon.Close();
                        //mati_connect();
                        DataGridView(jakiMiesiac);
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
                                MyMessageBox.ShowMessage("Problem z połaczeniem, próbuj dalej!", "Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                break;
                        }
                    }
                }
            }
            this.Cursor = this.DefaultCursor;
        }

        private void dataGrid_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        //zmiana koloru wiersza z dystansem
        {
            /*
            foreach (DataGridViewRow Myrow in dataGrid.Rows)
            {
                if (Convert.ToDouble(Myrow.Cells["txtDystans"].Value == DBNull.Value ? "0" : Myrow.Cells["txtDystans"].Value) > 0)
                {
                    Myrow.DefaultCellStyle.BackColor = Color.White;
                }
                else
                {
                    Myrow.DefaultCellStyle.BackColor = Color.White;
                }
            }
            */
            foreach (DataGridViewRow Myrow in dataGrid.Rows)
            {
                //if (Convert.ToString(Myrow.Cells["txtZawody"].Value) != "") // kolorowanie zawodów
                //Myrow.DefaultCellStyle.BackColor = Color.Red;
                if (Convert.ToString(Myrow.Cells["txtDzien"].Value) == "sobota") //kolorowanie sobót i niedziel
                {

                    Myrow.DefaultCellStyle.BackColor = Color.LightYellow;
                }
                else if (Convert.ToString(Myrow.Cells["txtDzien"].Value) == "niedziela")
                {
                    Myrow.DefaultCellStyle.BackColor = Color.LightYellow;
                }
                else
                {
                    Myrow.DefaultCellStyle.BackColor = Color.White;
                }
            }
        }

        private void dataGridView1_CellMouseMove(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                if (!((DataGridViewRow)(dataGrid.Rows[e.RowIndex])).Selected)
                {
                    dataGrid.ClearSelection();
                    ((DataGridViewRow)dataGrid.Rows[e.RowIndex]).Selected = true;
                    if (dataGrid.SelectedRows.Count > 0)
                    {


                        DataGridViewRow row = (DataGridViewRow)dataGrid.Rows[e.RowIndex];
                        //textBox1.Text = e.RowIndex.ToString();// return row index of dataGridView On CellMouseMove Event and Display RowIndex in TextBox1.
                        if (row.Cells["txtZawody"].Value.ToString() != "")
                        {
                            int ZapZawodyID = Convert.ToInt16(row.Cells["txtZawody_ID"].Value);
                            string ZapNazwa = row.Cells["txtZawody"].Value.ToString();
                            string ZapData = row.Cells["txtData"].Value.ToString();
                            string ZapDystans = row.Cells["txtDystans"].Value.ToString();
                            string ZapNumer = row.Cells["txtNumer"].Value.ToString();
                            string ZapCzas = row.Cells["txtCzas"].Value.ToString();
                            string ZapOplata = row.Cells["txtOplata"].Value.ToString();

                            Zawody zawody = new Zawody(ZapZawodyID); // wyświetlenie okna zawodów

                            zawody.lblZawodyNazwa = new System.Windows.Forms.Label();
                            zawody.lblZawodyData = new System.Windows.Forms.Label();
                            zawody.lblZawodyDystans = new System.Windows.Forms.Label();
                            zawody.lblZawodyNumer = new System.Windows.Forms.Label();
                            zawody.lblZawodyCzas = new System.Windows.Forms.Label();

                            // 
                            // lblZawodyNazwa
                            // 
                            zawody.lblZawodyNazwa.Anchor = System.Windows.Forms.AnchorStyles.Left;
                            zawody.lblZawodyNazwa.AutoSize = true;
                            zawody.lblZawodyNazwa.Font = new System.Drawing.Font("Courier New", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
                            zawody.lblZawodyNazwa.ForeColor = System.Drawing.SystemColors.ControlLightLight;
                            zawody.lblZawodyNazwa.Location = new System.Drawing.Point(52, 44);
                            zawody.lblZawodyNazwa.Name = "lblZawodyNazwa";
                            zawody.lblZawodyNazwa.Size = new System.Drawing.Size(30, 31);
                            zawody.lblZawodyNazwa.TabIndex = 0;
                            zawody.lblZawodyNazwa.Text = " ";
                            // 
                            // lblZawodyData
                            // 
                            zawody.lblZawodyData.Anchor = System.Windows.Forms.AnchorStyles.None;
                            zawody.lblZawodyData.AutoSize = true;
                            zawody.lblZawodyData.Font = new System.Drawing.Font("Courier New", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
                            zawody.lblZawodyData.Location = new System.Drawing.Point(120, 25);
                            zawody.lblZawodyData.Name = "lblZawodyData";
                            zawody.lblZawodyData.Size = new System.Drawing.Size(0, 31);
                            zawody.lblZawodyData.TabIndex = 1;
                            // 
                            // lblZawodyDystans
                            // 
                            zawody.lblZawodyDystans.Anchor = System.Windows.Forms.AnchorStyles.None;
                            zawody.lblZawodyDystans.AutoSize = true;
                            zawody.lblZawodyDystans.Font = new System.Drawing.Font("Courier New", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
                            zawody.lblZawodyDystans.Location = new System.Drawing.Point(120, 25);
                            zawody.lblZawodyDystans.Name = "lblZawodyDystans";
                            zawody.lblZawodyDystans.Size = new System.Drawing.Size(0, 31);
                            zawody.lblZawodyDystans.TabIndex = 1;
                            // 
                            // lblZawodyNumer
                            // 
                            zawody.lblZawodyNumer.Anchor = System.Windows.Forms.AnchorStyles.None;
                            zawody.lblZawodyNumer.AutoSize = true;
                            zawody.lblZawodyNumer.Font = new System.Drawing.Font("Courier New", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
                            zawody.lblZawodyNumer.Location = new System.Drawing.Point(120, 25);
                            zawody.lblZawodyNumer.Name = "lblZawodyNumer";
                            zawody.lblZawodyNumer.Size = new System.Drawing.Size(0, 31);
                            zawody.lblZawodyNumer.TabIndex = 1;
                            // 
                            // lblZawodyCzas
                            // 
                            zawody.lblZawodyCzas.Anchor = System.Windows.Forms.AnchorStyles.None;
                            zawody.lblZawodyCzas.AutoSize = true;
                            zawody.lblZawodyCzas.Font = new System.Drawing.Font("Courier New", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
                            zawody.lblZawodyCzas.Location = new System.Drawing.Point(120, 25);
                            zawody.lblZawodyCzas.Name = "lblZawodyCzas";
                            zawody.lblZawodyCzas.Size = new System.Drawing.Size(0, 31);
                            zawody.lblZawodyCzas.TabIndex = 1;

                            zawody.tableLayoutPanel1.Controls.Add(zawody.lblZawodyNazwa, 1, 1);
                            zawody.tableLayoutPanel8.Controls.Add(zawody.lblZawodyDystans, 0, 0);
                            zawody.tableLayoutPanel9.Controls.Add(zawody.lblZawodyNumer, 0, 0);
                            zawody.tableLayoutPanel10.Controls.Add(zawody.lblZawodyCzas, 0, 0);
                            zawody.tableLayoutPanel7.Controls.Add(zawody.lblZawodyData, 0, 0);


                            zawody.Text = ZapNazwa;
                            zawody.lblZawodyNazwa.Text = ZapNazwa;
                            zawody.lblZawodyData.Text = ZapData;
                            zawody.lblZawodyDystans.Text = ZapDystans;
                            zawody.lblZawodyNumer.Text = ZapNumer;
                            zawody.lblZawodyCzas.Text = ZapCzas;
                            zawody.lblOplata.Text = ZapOplata;
                            /*
                            lblNazwaRekMaraton.Text = " " + wybor_M[0]["zawody"].ToString() + " ";
                            lblDataRekMaraton.Text = wybor_M[0]["data"].ToString();
                            lblCzasRekMaraton.Text = wybor_M[0]["czas"].ToString();
                            lblNumerRekMaraton.Text = wybor_M[0]["numer"].ToString();

                            zawody.picZawody.Image = wybor_zdjec[0]["fota"];
                            zawody.picZawody.SizeMode = PictureBoxSizeMode.Zoom;
                            Timer tm = new Timer();
                            tm.Interval = 2000;
                            tm.Tick += new EventHandler(changeimage);
                            tm.Start();
                            */
                            zawody.ShowDialog();
                        }

                    }
                }
            }
        }



        private void dataGrid_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        //blokada na wprowadzanie w wadze i dystansie innych znaków niż cyfry
        {
            if (dataGrid.CurrentCell.ColumnIndex == 3)
            {
                e.Control.KeyPress -= AllowNumberOnly;
                e.Control.KeyPress += AllowNumberOnly;
            }
            if (dataGrid.CurrentCell.ColumnIndex == 4)
            {
                e.Control.KeyPress -= AllowNumberOnly;
                e.Control.KeyPress += AllowNumberOnly;
            }
        }

        private void AllowNumberOnly(Object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && e.KeyChar != ',')
                e.Handled = true;
        }

        private void dateTimePicker_ValueChanged(object sender, EventArgs e)
        // zmiana wartości w polu wyboru daty
        {
            DateTime dzien = Convert.ToDateTime(dateTimePicker.Value);
            DataGridView(dzien);
        }

        private void NaszaObslugaZdarzenia(object ob, EventArgs info) //tworzenie własnego zdarzenia - ściąganie XML z pogodą z API
        {
            CallProcess();
        }

        public static async Task<string> ProcessRepositories()
        {
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.Add("User-Agent", ".NET Foundation Repository Reporter");

            var stringTask = client.GetStringAsync("http://api.openweathermap.org/data/2.5/forecast?q=Bydgoszcz&APPID=94a14e2fa2194f8d45bd96051905a4dd&mode=xml&lang=pl&units=metric&temperature=temperature.value&count=1");

            var msg = await stringTask;

            string result = msg;
            return await Task.FromResult(result);
        }

        public async void CallProcess()
        {

            String Value = await ProcessRepositories();
            zawartoscXML = Value;
            Calendar();
        }

        public void Calendar()
        {
            txtDzisData.Text = miesiacNazwa[DateTime.Now.Month - 1] + " " + DateTime.Now.Year.ToString();
            txtDzisNumer.Text = DateTime.Now.Day.ToString();
            int jakiDzien = (int)DateTime.Now.DayOfWeek;
            txtDzisDzien.Text = tydzienNazwa[jakiDzien];

            pogoda = new prognoza();

            Weather(pogoda);
            List<string> temp;
            temp = pogoda.WeatherNow(DateTime.Now.Hour);

            string icona = temp[2];
            string adres = "http://openweathermap.org/img/wn/" + icona + "@2x.png";
            picWeather.Load(adres);
            txtAPIOpis.Text = temp[3];
            txtAPIDeszcz.Text = "opady " + temp[4] + " mm";
            txtAPIWiatr.Text = "wiatr " + temp[5] + " m/s";
            txtAPITemp.Text = temp[6] + "°C";
            txtAPICisnienie.Text = "ciśnienie " + temp[7] + " hPa";
            txtAPIWilgotnosc.Text = "wilgotność " + temp[8] + " %";
        }

        private void Weather(prognoza wpiszPogoda)
        {
            
            //XmlReader xmlRreader = XmlReader.Create(@"c:\xxx\pogodapl.xml");
            XmlReader xmlRreader = XmlReader.Create(new StringReader(zawartoscXML));

            while (xmlRreader.Read())
            {
                if (xmlRreader.NodeType == XmlNodeType.Element)
                {
                    switch (xmlRreader.Name)
                    {
                        case "time":
                            //string dateTime = xmlRreader.GetAttribute("from");
                            //DateTime data_od = Convert.ToDateTime(dateTime);
                            wpiszPogoda.add_czas_od(xmlRreader.GetAttribute("from"));
                            wpiszPogoda.add_czas_do(xmlRreader.GetAttribute("to"));
                            break;
                        case "symbol":
                            wpiszPogoda.add_ikona(xmlRreader.GetAttribute("var"));
                            wpiszPogoda.add_opis(xmlRreader.GetAttribute("name"));
                            break;
                        case "precipitation":
                            if (xmlRreader.HasAttributes)
                            {
                                wpiszPogoda.add_opady(xmlRreader.GetAttribute("value"));
                            }
                            else
                            {
                                wpiszPogoda.add_opady("0.0");
                            }
                            break;
                        case "windSpeed":
                            wpiszPogoda.add_wiatr(xmlRreader.GetAttribute("mps"));
                            break;
                        case "temperature":
                            wpiszPogoda.add_temperatura(xmlRreader.GetAttribute("value"));
                            break;
                        case "pressure":
                            wpiszPogoda.add_cisnienie(xmlRreader.GetAttribute("value"));
                            break;
                        case "humidity":
                            wpiszPogoda.add_wilgotnosc(xmlRreader.GetAttribute("value"));
                            break;
                    }
                    
                }
                

            }
        }

        private void btnZawodyDodaj_Click(object sender, EventArgs e)
        {
            if (dataGrid.CurrentCell != null)
            {
                DataGridViewRow dataGrRow = dataGrid.CurrentRow;
                string komo = dataGrid.CurrentCellAddress.X.ToString();
                string zapamietajData = Convert.ToString(dataGrRow.Cells["txtData"].Value); //określenie i zapamiętanie miesiąca do wyświetlenia
                DateTime jakiMiesiac = DateTime.ParseExact(Convert.ToString(dataGrRow.Cells["txtData"].Value), "dd.MM.yyyy", null); //określenie i zapamiętanie miesiąca do wyświetlenia
                Console.WriteLine(zapamietajData);

                Zawody zawody = new Zawody(0); // wyświetlenie okna zawodów

                zawody.Text = "Dopisywanie zawodów";
                zawody.tableLayoutPanel11 = new System.Windows.Forms.TableLayoutPanel();
                zawody.tableLayoutPanel1.Controls.Add(zawody.tableLayoutPanel11, 1, 1);
                zawody.lblWpisNazwaZawodow = new System.Windows.Forms.Label();
                zawody.tableLayoutPanel11.Controls.Add(zawody.lblWpisNazwaZawodow, 0, 0);
                zawody.txtWpisNazwaZawodow = new System.Windows.Forms.TextBox();
                zawody.tableLayoutPanel11.Controls.Add(zawody.txtWpisNazwaZawodow, 1, 0);
                zawody.dateTimeZawody = new System.Windows.Forms.DateTimePicker();
                zawody.tableLayoutPanel7.Controls.Add(zawody.dateTimeZawody, 0, 0);
                zawody.txtZawodyDystans = new System.Windows.Forms.TextBox();
                zawody.tableLayoutPanel8.Controls.Add(zawody.txtZawodyDystans, 0, 0);
                zawody.txtZawodyNumer = new System.Windows.Forms.TextBox();
                zawody.tableLayoutPanel9.Controls.Add(zawody.txtZawodyNumer, 0, 0);
                zawody.dateTimeZawodyCzas = new System.Windows.Forms.DateTimePicker();
                zawody.tableLayoutPanel10.Controls.Add(zawody.dateTimeZawodyCzas, 0, 0);

                // 
                // tableLayoutPanel11
                // 
                zawody.tableLayoutPanel11.ColumnCount = 2;
                zawody.tableLayoutPanel11.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 30F));
                zawody.tableLayoutPanel11.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 70F));
                zawody.tableLayoutPanel11.Dock = System.Windows.Forms.DockStyle.Fill;
                zawody.tableLayoutPanel11.Location = new System.Drawing.Point(52, 9);
                zawody.tableLayoutPanel11.Name = "tableLayoutPanel11";
                zawody.tableLayoutPanel11.RowCount = 1;
                zawody.tableLayoutPanel11.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
                zawody.tableLayoutPanel11.Size = new System.Drawing.Size(887, 102);
                zawody.tableLayoutPanel11.TabIndex = 0;
                // 
                // lblWpisNazwaZawodow
                // 
                zawody.lblWpisNazwaZawodow.Anchor = System.Windows.Forms.AnchorStyles.Right;
                zawody.lblWpisNazwaZawodow.AutoSize = true;
                zawody.lblWpisNazwaZawodow.Font = new System.Drawing.Font("Courier New", 15F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
                zawody.lblWpisNazwaZawodow.Location = new System.Drawing.Point(85, 39);
                zawody.lblWpisNazwaZawodow.Name = "lblWpisNazwaZawodow";
                zawody.lblWpisNazwaZawodow.Size = new System.Drawing.Size(178, 23);
                zawody.lblWpisNazwaZawodow.TabIndex = 0;
                zawody.lblWpisNazwaZawodow.Text = "Nazwa zawodów:";
                // 
                // txtWpisNazwaZawodow
                // 
                zawody.txtWpisNazwaZawodow.Anchor = System.Windows.Forms.AnchorStyles.Left;
                zawody.txtWpisNazwaZawodow.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
                zawody.txtWpisNazwaZawodow.Font = new System.Drawing.Font("Courier New", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
                zawody.txtWpisNazwaZawodow.Location = new System.Drawing.Point(269, 32);
                zawody.txtWpisNazwaZawodow.Name = "txtWpisNazwaZawodow";
                zawody.txtWpisNazwaZawodow.Size = new System.Drawing.Size(615, 38);
                zawody.txtWpisNazwaZawodow.TabIndex = 1;
                zawody.txtWpisNazwaZawodow.TextChanged += new System.EventHandler(zawody.txtWpisNazwaZawodow_TextChanged);
                // 
                // dateTimeZawody
                // 
                zawody.dateTimeZawody.Anchor = System.Windows.Forms.AnchorStyles.None;
                zawody.dateTimeZawody.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
                zawody.dateTimeZawody.CalendarFont = new System.Drawing.Font("Courier New", 15F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
                zawody.dateTimeZawody.CustomFormat = "dd.MM.yyyy";
                zawody.dateTimeZawody.DropDownAlign = System.Windows.Forms.LeftRightAlignment.Right;
                zawody.dateTimeZawody.Font = new System.Drawing.Font("Courier New", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
                zawody.dateTimeZawody.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
                zawody.dateTimeZawody.Location = new System.Drawing.Point(20, 26);
                zawody.dateTimeZawody.MinDate = new System.DateTime(2011, 3, 1, 0, 0, 0, 0);
                zawody.dateTimeZawody.Name = "dateTimeZawody";
                zawody.dateTimeZawody.Size = new System.Drawing.Size(160, 30);
                zawody.dateTimeZawody.TabIndex = 0;
                zawody.dateTimeZawody.Text = jakiMiesiac.ToString();
                zawody.dateTimeZawody.Enabled = false;
                // 
                // txtZawodyDystans
                // 
                zawody.txtZawodyDystans.Anchor = System.Windows.Forms.AnchorStyles.None;
                zawody.txtZawodyDystans.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
                zawody.txtZawodyDystans.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
                zawody.txtZawodyDystans.Font = new System.Drawing.Font("Courier New", 20F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
                zawody.txtZawodyDystans.Location = new System.Drawing.Point(70, 29);
                zawody.txtZawodyDystans.Name = "txtZawodyDystans";
                zawody.txtZawodyDystans.Size = new System.Drawing.Size(100, 23);
                zawody.txtZawodyDystans.TabIndex = 0;
                zawody.txtZawodyDystans.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
                zawody.txtZawodyDystans.KeyPress += new KeyPressEventHandler(zawody.txtZawodyDystans_KeyPress);
                // 
                // txtZawodyNumer
                // 
                zawody.txtZawodyNumer.Anchor = System.Windows.Forms.AnchorStyles.None;
                zawody.txtZawodyNumer.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
                zawody.txtZawodyNumer.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
                zawody.txtZawodyNumer.Font = new System.Drawing.Font("Courier New", 20F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
                zawody.txtZawodyNumer.Location = new System.Drawing.Point(70, 29);
                zawody.txtZawodyNumer.Name = "txtZawodyNumer";
                zawody.txtZawodyNumer.Size = new System.Drawing.Size(100, 23);
                zawody.txtZawodyNumer.TabIndex = 0;
                zawody.txtZawodyNumer.KeyPress += new System.Windows.Forms.KeyPressEventHandler(zawody.txtZawodyNumer_KeyPress);
                // 
                // dateTimeZawodyCzas
                // 
                zawody.dateTimeZawodyCzas.Anchor = System.Windows.Forms.AnchorStyles.None;
                zawody.dateTimeZawodyCzas.Font = new System.Drawing.Font("Courier New", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
                zawody.dateTimeZawodyCzas.Format = System.Windows.Forms.DateTimePickerFormat.Time;
                zawody.dateTimeZawodyCzas.Location = new System.Drawing.Point(48, 26);
                zawody.dateTimeZawodyCzas.Name = "dateTimeZawodyCzas";
                zawody.dateTimeZawodyCzas.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
                zawody.dateTimeZawodyCzas.ShowUpDown = true;
                zawody.dateTimeZawodyCzas.Size = new System.Drawing.Size(145, 30);
                zawody.dateTimeZawodyCzas.TabIndex = 0;

                zawody.ShowDialog();
                zawody.Dispose();

                mati_connect();
                DataGridView(jakiMiesiac);

            }
        }

        private void dataGrid_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
            }
            else
            {
                ContextMenuStrip myMenu = new System.Windows.Forms.ContextMenuStrip();
                int position_xy_mouse_row = dataGrid.HitTest(e.X, e.Y).RowIndex;

                if (position_xy_mouse_row >= 0)
                {
                     myMenu.Items.Add("Importuj trening").Name = "addTrening";                   
                    myMenu.Items.Add("Dopisz zwody").Name = "addZawody";
                }
                myMenu.Show(dataGrid, new Point(e.X, e.Y));

                myMenu.ItemClicked += new ToolStripItemClickedEventHandler(myMenu_ItemClicked);

            }
        }

        void myMenu_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            ToolStripItem item = e.ClickedItem;
            if (item.Name=="addZawody")
            {
                add_Zawody();
            }
            else
            {
                OpenFileDialog openFileDialog = new OpenFileDialog();
                openFileDialog.Filter = "Choose xml file(*.xml; *.tcx;)|*.xml; *.tcx;";
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                   // picAddPicture.Image = Image.FromFile(openFileDialog.FileName);
                }
            }


        }

        private void add_Zawody()
        {
            if (dataGrid.CurrentCell != null)
            {
                DataGridViewRow dataGrRow = dataGrid.CurrentRow;
                string komo = dataGrid.CurrentCellAddress.X.ToString();
                string zapamietajData = Convert.ToString(dataGrRow.Cells["txtData"].Value); //określenie i zapamiętanie miesiąca do wyświetlenia
                DateTime jakiMiesiac = DateTime.ParseExact(Convert.ToString(dataGrRow.Cells["txtData"].Value), "dd.MM.yyyy", null); //określenie i zapamiętanie miesiąca do wyświetlenia
                Console.WriteLine(zapamietajData);

                Zawody zawody = new Zawody(0); // wyświetlenie okna zawodów

                zawody.Text = "Dopisywanie zawodów";
                zawody.tableLayoutPanel11 = new System.Windows.Forms.TableLayoutPanel();
                zawody.tableLayoutPanel1.Controls.Add(zawody.tableLayoutPanel11, 1, 1);
                zawody.lblWpisNazwaZawodow = new System.Windows.Forms.Label();
                zawody.tableLayoutPanel11.Controls.Add(zawody.lblWpisNazwaZawodow, 0, 0);
                zawody.txtWpisNazwaZawodow = new System.Windows.Forms.TextBox();
                zawody.tableLayoutPanel11.Controls.Add(zawody.txtWpisNazwaZawodow, 1, 0);
                zawody.dateTimeZawody = new System.Windows.Forms.DateTimePicker();
                zawody.tableLayoutPanel7.Controls.Add(zawody.dateTimeZawody, 0, 0);
                zawody.txtZawodyDystans = new System.Windows.Forms.TextBox();
                zawody.tableLayoutPanel8.Controls.Add(zawody.txtZawodyDystans, 0, 0);
                zawody.txtZawodyNumer = new System.Windows.Forms.TextBox();
                zawody.tableLayoutPanel9.Controls.Add(zawody.txtZawodyNumer, 0, 0);
                zawody.dateTimeZawodyCzas = new System.Windows.Forms.DateTimePicker();
                zawody.tableLayoutPanel10.Controls.Add(zawody.dateTimeZawodyCzas, 0, 0);

                // 
                // tableLayoutPanel11
                // 
                zawody.tableLayoutPanel11.ColumnCount = 2;
                zawody.tableLayoutPanel11.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 30F));
                zawody.tableLayoutPanel11.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 70F));
                zawody.tableLayoutPanel11.Dock = System.Windows.Forms.DockStyle.Fill;
                zawody.tableLayoutPanel11.Location = new System.Drawing.Point(52, 9);
                zawody.tableLayoutPanel11.Name = "tableLayoutPanel11";
                zawody.tableLayoutPanel11.RowCount = 1;
                zawody.tableLayoutPanel11.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
                zawody.tableLayoutPanel11.Size = new System.Drawing.Size(887, 102);
                zawody.tableLayoutPanel11.TabIndex = 0;
                // 
                // lblWpisNazwaZawodow
                // 
                zawody.lblWpisNazwaZawodow.Anchor = System.Windows.Forms.AnchorStyles.Right;
                zawody.lblWpisNazwaZawodow.AutoSize = true;
                zawody.lblWpisNazwaZawodow.Font = new System.Drawing.Font("Courier New", 15F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
                zawody.lblWpisNazwaZawodow.Location = new System.Drawing.Point(85, 39);
                zawody.lblWpisNazwaZawodow.Name = "lblWpisNazwaZawodow";
                zawody.lblWpisNazwaZawodow.Size = new System.Drawing.Size(178, 23);
                zawody.lblWpisNazwaZawodow.TabIndex = 0;
                zawody.lblWpisNazwaZawodow.Text = "Nazwa zawodów:";
                // 
                // txtWpisNazwaZawodow
                // 
                zawody.txtWpisNazwaZawodow.Anchor = System.Windows.Forms.AnchorStyles.Left;
                zawody.txtWpisNazwaZawodow.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
                zawody.txtWpisNazwaZawodow.Font = new System.Drawing.Font("Courier New", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
                zawody.txtWpisNazwaZawodow.Location = new System.Drawing.Point(269, 32);
                zawody.txtWpisNazwaZawodow.Name = "txtWpisNazwaZawodow";
                zawody.txtWpisNazwaZawodow.Size = new System.Drawing.Size(615, 38);
                zawody.txtWpisNazwaZawodow.TabIndex = 1;
                zawody.txtWpisNazwaZawodow.TextChanged += new System.EventHandler(zawody.txtWpisNazwaZawodow_TextChanged);
                // 
                // dateTimeZawody
                // 
                zawody.dateTimeZawody.Anchor = System.Windows.Forms.AnchorStyles.None;
                zawody.dateTimeZawody.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
                zawody.dateTimeZawody.CalendarFont = new System.Drawing.Font("Courier New", 15F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
                zawody.dateTimeZawody.CustomFormat = "dd.MM.yyyy";
                zawody.dateTimeZawody.DropDownAlign = System.Windows.Forms.LeftRightAlignment.Right;
                zawody.dateTimeZawody.Font = new System.Drawing.Font("Courier New", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
                zawody.dateTimeZawody.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
                zawody.dateTimeZawody.Location = new System.Drawing.Point(20, 26);
                zawody.dateTimeZawody.MinDate = new System.DateTime(2011, 3, 1, 0, 0, 0, 0);
                zawody.dateTimeZawody.Name = "dateTimeZawody";
                zawody.dateTimeZawody.Size = new System.Drawing.Size(160, 30);
                zawody.dateTimeZawody.TabIndex = 0;
                zawody.dateTimeZawody.Text = jakiMiesiac.ToString();
                zawody.dateTimeZawody.Enabled = false;
                // 
                // txtZawodyDystans
                // 
                zawody.txtZawodyDystans.Anchor = System.Windows.Forms.AnchorStyles.None;
                zawody.txtZawodyDystans.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
                zawody.txtZawodyDystans.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
                zawody.txtZawodyDystans.Font = new System.Drawing.Font("Courier New", 20F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
                zawody.txtZawodyDystans.Location = new System.Drawing.Point(70, 29);
                zawody.txtZawodyDystans.Name = "txtZawodyDystans";
                zawody.txtZawodyDystans.Size = new System.Drawing.Size(100, 23);
                zawody.txtZawodyDystans.TabIndex = 0;
                zawody.txtZawodyDystans.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
                zawody.txtZawodyDystans.KeyPress += new KeyPressEventHandler(zawody.txtZawodyDystans_KeyPress);
                // 
                // txtZawodyNumer
                // 
                zawody.txtZawodyNumer.Anchor = System.Windows.Forms.AnchorStyles.None;
                zawody.txtZawodyNumer.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
                zawody.txtZawodyNumer.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
                zawody.txtZawodyNumer.Font = new System.Drawing.Font("Courier New", 20F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
                zawody.txtZawodyNumer.Location = new System.Drawing.Point(70, 29);
                zawody.txtZawodyNumer.Name = "txtZawodyNumer";
                zawody.txtZawodyNumer.Size = new System.Drawing.Size(100, 23);
                zawody.txtZawodyNumer.TabIndex = 0;
                zawody.txtZawodyNumer.KeyPress += new System.Windows.Forms.KeyPressEventHandler(zawody.txtZawodyNumer_KeyPress);
                // 
                // dateTimeZawodyCzas
                // 
                zawody.dateTimeZawodyCzas.Anchor = System.Windows.Forms.AnchorStyles.None;
                zawody.dateTimeZawodyCzas.Font = new System.Drawing.Font("Courier New", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
                zawody.dateTimeZawodyCzas.Format = System.Windows.Forms.DateTimePickerFormat.Time;
                zawody.dateTimeZawodyCzas.Location = new System.Drawing.Point(48, 26);
                zawody.dateTimeZawodyCzas.Name = "dateTimeZawodyCzas";
                zawody.dateTimeZawodyCzas.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
                zawody.dateTimeZawodyCzas.ShowUpDown = true;
                zawody.dateTimeZawodyCzas.Size = new System.Drawing.Size(145, 30);
                zawody.dateTimeZawodyCzas.TabIndex = 0;

                zawody.ShowDialog();
                zawody.Dispose();

                mati_connect();
                DataGridView(jakiMiesiac);

            }
        }

        int x = 0, y = 0;

        private void pictureBox1_MouseClick(object sender, MouseEventArgs e)
        {
            if (meteo_id == 0)
            {
                //System.Drawing.Point scrLoc = this.PointToScreen(Location);
                System.Drawing.Point gdzieMeteo = this.tableLayoutPanel24.PointToScreen(Location);
                System.Drawing.Point gdzieForm = this.PointToScreen(Location);


                x = gdzieMeteo.X;
                y = gdzieMeteo.Y;
                this.pictureBox1.Image = global::RUN.Properties.Resources.top_arrow;
                meteo_id++;
                Meteo = new meteo(pogoda);
                Meteo.StartPosition = FormStartPosition.Manual;
                Meteo.Location = new System.Drawing.Point(x, y);
                Meteo.Show();
            }
            else
            {
                Meteo.Dispose();
                meteo_id = 0;
                this.pictureBox1.Image = global::RUN.Properties.Resources.bottom_arrow;
                x = 0;
                y = 0;
            }
        }

        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            this.Cursor = Cursors.Hand;
        }

        private void tableLayoutPanel4_Paint(object sender, PaintEventArgs e)
        {

        }

        DateTime endTime = new DateTime(2021, 10, 03, 0, 0, 0);

        private void tmrEvent_Tick(object sender, EventArgs e)
        {
            TimeSpan time = endTime.Subtract(DateTime.Now);

            lblWeeks.Text = (time.Days / 7).ToString();
            lblDays.Text = (time.Days%7).ToString();
            lblHours.Text = time.Hours.ToString();
            lblMinutes.Text = time.Minutes.ToString();

            if ((time.Days / 7) == 1)
            {
                lblW.Text = "week";
            }
            else
            {
                lblW.Text = "weeks";
            }

            if ((time.Days % 7) == 1)
            {
                lblD.Text = "day";
            }
            else
            {
                lblD.Text = "days";
            }

            if (time.Hours == 1)
            {
                lblH.Text = "hour";
            }
            else
            {
                lblH.Text = "hours";
            }

            if (time.Minutes == 1)
            {
                lblM.Text = "minute";
            }
            else
            {
                lblM.Text = "minutes";
            }
        }

        private void pictureBox1_MouseLeave(object sender, EventArgs e)
        {
            this.Cursor = this.DefaultCursor;
        }
    }
}
