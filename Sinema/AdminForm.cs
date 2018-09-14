using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace Sinema
{
    public partial class AdminForm : Form
    {
        string secilen;
        int secilensalon;
        SqlConnection conn = new SqlConnection("Data Source = SKYWALKER\\SQLEXPRESS; Initial Catalog = Sinema; Integrated Security = True");
        string kayit = "";
        SqlCommand komut = null;
        string SeansSalonNO = "";
        public AdminForm()
        {
            InitializeComponent();
        }
        

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            seansBox.Items.Clear();
            if (salonBox1.SelectedIndex == 0)
                secilensalon = 1;
            if (salonBox1.SelectedIndex == 1)
                secilensalon = 2;
            conn.Open();
            
            SqlCommand command = new SqlCommand("Select SalonNoSeansNo from Seanslar where SalonNoSeansNo like '"+secilensalon+"%' and SalonNo IS NULL", conn);
            
            SqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                SeansSalonNO = reader["SalonNoSeansNo"].ToString();
                seansBox.Items.Add(reader["SalonNoSeansNo"].ToString().Substring(2, 5));
                
            }
            reader.Close();
            conn.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            conn.Open();
            kayit = "insert into FilmBilgileri(FilmAdi,Türü,Dili,Firma,Ozet) values (@filmadi,@turu,@dili,@firma,@ozet)";
            komut = new SqlCommand(kayit, conn);
            komut.Parameters.AddWithValue("@filmadi", FilmAdı.Text);
            komut.Parameters.AddWithValue("@turu", FilmTürü.Text);
            komut.Parameters.AddWithValue("@dili", FilmDili.Text);
            komut.Parameters.AddWithValue("@firma", YayıncıFirma.Text);
            komut.Parameters.AddWithValue("@ozet", FilmÖzeti.Text);
            komut.ExecuteNonQuery();
            conn.Close();
        }

        private void AdminForm_Load(object sender, EventArgs e)
        {
            SqlCommand command = new SqlCommand("Select FilmAdi from Filmler", conn);
            conn.Open();
            SqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                filmBox.Items.Add(reader["FilmAdi"].ToString());
                secilen = reader["FilmAdi"].ToString();
            }
            reader.Close();
            conn.Close();
        }

        private void filmBox_SelectedIndexChanged(object sender, EventArgs e)
        {
           
                salonBox1.Items.Add("1");
                salonBox1.Items.Add("2");
        }

        private void button2_Click(object sender, EventArgs e)
        {
            conn.Open();
            label11.Text = salonBox1.SelectedText;
            kayit = "IF NOT EXISTS(SELECT 1 FROM Seanslar WHERE SalonNoSeansNo!= '" + SeansSalonNO + "') insert into Seanslar(SalonNo) values (" + secilensalon+")";
            komut = new SqlCommand(kayit, conn);
            komut.ExecuteNonQuery();
            conn.Close();
        }
    }
}
