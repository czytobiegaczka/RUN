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
using System.Globalization;
using System.Windows.Forms.DataVisualization.Charting;




namespace RUN
{
    public partial class Main : Form
    {
        string connectionString = @"DATASOURCE=db4free.net;PORT=3306;DATABASE=trening;UID=trening;PASSWORD=treningRTL;OldGuids=True;convert zero datetime=True";
        string[] miesiacNr = { "00","01", "02", "03", "04", "05", "06", "07", "08", "09", "10", "11", "12" };
        string[] miesiacNazwa = { "Styczeń", "Luty", "Marzec", "Kwiecień", "Maj", "Czerwiec", "Lipiec", "Sierpień", "Wrzesień", "Październik", "Listopad", "Grudzień" };
        private MySqlConnection MyCon;
        private DataTable data;

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
                catch
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
                    if(MyMessageBox.ShowMessage("Błąd połączenia. Czy łączyć jeszcze raz?", "Message", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
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
            string tylkoRok= "'%." + dzien.Year.ToString() + "'";
            string tylkoMiesiac = "'%." + miesiacNr[dzien.Month] + "." + dzien.Year.ToString() + "'";
            double sumKm=0;
            double liczKm=1;
            double sredKm = 0;
            double minKm=0;
            double maxKm=0;
            double sumKg=0;
            double liczKg=1;
            double sredKg = 0;
            double minKg=0;
            double maxKg=0;

            this.Cursor = Cursors.WaitCursor;
             this.Cursor = this.DefaultCursor;




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
                        DateTime jakiMiesiac= DateTime.ParseExact(Convert.ToString(dataGrRow.Cells["txtData"].Value),"dd.MM.yyyy",null); //określenie i zapamiętanie miesiąca do wyświetlenia
                        zapamietajData = zapamietajData.Remove(0, 3);
                        zapamietajData = "'___" + zapamietajData+"'";


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
                        if (row.Cells["txtZawody"].Value.ToString()!="")
                        {
                            string ZapNazwa = row.Cells["txtZawody"].Value.ToString();
                            string ZapData = row.Cells["txtData"].Value.ToString();
                            string ZapDystans = row.Cells["txtDystans"].Value.ToString();
                            string ZapNumer = row.Cells["txtNumer"].Value.ToString();
                            string ZapCzas = row.Cells["txtCzas"].Value.ToString();
                            
                            Zawody zawody = new Zawody(); // wyświetlenie okna zawodów

                            zawody.lblZawodyData = new System.Windows.Forms.Label();
                            zawody.lblZawodyDystans = new System.Windows.Forms.Label();
                            zawody.lblZawodyNumer = new System.Windows.Forms.Label();
                            zawody.lblZawodyCzas = new System.Windows.Forms.Label();

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

        private void btnZawodyDodaj_Click(object sender, EventArgs e)
        {
            Zawody zawody = new Zawody(); // wyświetlenie okna zawodów

            zawody.Text = "Dopisywanie zawodów";
            zawody.dateTimeZawody = new System.Windows.Forms.DateTimePicker();
            zawody.tableLayoutPanel7.Controls.Add(zawody.dateTimeZawody, 0, 0);
            zawody.txtZawodyDystans = new System.Windows.Forms.TextBox();
            zawody.tableLayoutPanel8.Controls.Add(zawody.txtZawodyDystans, 0, 0);
            zawody.txtZawodyNumer = new System.Windows.Forms.TextBox();
            zawody.tableLayoutPanel9.Controls.Add(zawody.txtZawodyNumer, 0, 0);
            zawody.dateTimeZawodyCzas = new System.Windows.Forms.DateTimePicker();
            zawody.tableLayoutPanel10.Controls.Add(zawody.dateTimeZawodyCzas, 0, 0);


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
        }
    }
}
