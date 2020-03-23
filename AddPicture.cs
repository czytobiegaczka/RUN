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


namespace RUN
{
    public partial class AddPicture : Form
    {
        public AddPicture()
        {
            InitializeComponent();
        }

        private void btnOpenFile_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Choose imge(*.jpg; *.png; *.gif;)|*.jpg; *.png; *.gif;";
            if (openFileDialog.ShowDialog()==DialogResult.OK)
            {
                picAddPicture.Image = Image.FromFile(openFileDialog.FileName);
            }
        }

        private void btnSaveImageToMySQL_Click(object sender, EventArgs e)
        {
            MySqlConnection MyCon;
            string connectionString = @"DATASOURCE=db4free.net;PORT=3306;DATABASE=trening;UID=trening;PASSWORD=treningRTL;OldGuids=True;convert zero datetime=True";
            
            MemoryStream memoryStream = new MemoryStream();
            picAddPicture.Image.Save(memoryStream, picAddPicture.Image.RawFormat);
            byte[] img = memoryStream.ToArray();

            using (MyCon = new MySqlConnection(connectionString))
            {
                try
                {
                    MyCon.Open(); //otwarcie nowego połączenia


                    MySqlCommand mySqlCommand = new MySqlCommand("ak_picture_Add", MyCon); //deklaracja nowej komendy SQL                           
                    mySqlCommand.CommandType = CommandType.StoredProcedure;
                    mySqlCommand.Parameters.AddWithValue("naz", txtNazwaPicture.Text);
                    mySqlCommand.Parameters.AddWithValue("zaw", 5);
                    //mySqlCommand.Parameters.AddWithValue("pic", img);

                    if (mySqlCommand.ExecuteNonQuery()==1)
                    {
                        MyMessageBox.ShowMessage("Wgrywanie zdjęcia zakończone powodzeniem!", "Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
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
    }
}
