using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace StokTakip
{
    public partial class frmMusteriListele : Form
    {
        public frmMusteriListele()
        {
            InitializeComponent();
        }
        SqlConnection baglanti = new SqlConnection("Data Source=DESKTOP-RHOGDFE;Initial Catalog=Stok_Takip;Integrated Security=True");
        DataSet dataset = new DataSet();//kayıtları geçici tutmak için
        private void Kayitgoster()
        {
            baglanti.Open();
            SqlDataAdapter adtr = new SqlDataAdapter("select * from musteri",baglanti);
            adtr.Fill(dataset, "musteri");//kayitları tablaya ekle
            dataGridView1.DataSource = dataset.Tables["musteri"];//datagridde gösterdik
            baglanti.Close();
        }

        private void frmMusteriListele_Load(object sender, EventArgs e)
        {
            Kayitgoster();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            txtTc.Text = dataGridView1.CurrentRow.Cells["tc"].Value.ToString();
            txtAdSoyad.Text = dataGridView1.CurrentRow.Cells["adsoyad"].Value.ToString();
            txtTelefon.Text = dataGridView1.CurrentRow.Cells["telefon"].Value.ToString();
            txtAdres.Text = dataGridView1.CurrentRow.Cells["adres"].Value.ToString();
            txtEmail.Text = dataGridView1.CurrentRow.Cells["email"].Value.ToString();

        }

        private void btnGuncelle_Click(object sender, EventArgs e)
        {
            baglanti.Open();
            SqlCommand komut = new SqlCommand("update musteri set adsoyad=@adsoyad,telefon=@telefon,adres=@adres,email=@email where tc=@tc",baglanti);
            komut.Parameters.AddWithValue("@tc", txtTc.Text);
            komut.Parameters.AddWithValue("@adsoyad", txtAdSoyad.Text);
            komut.Parameters.AddWithValue("telefon", txtTelefon.Text);
            komut.Parameters.AddWithValue("@adres", txtAdres.Text);
            komut.Parameters.AddWithValue("@email", txtEmail.Text);
            komut.ExecuteNonQuery();
            baglanti.Close();
            dataset.Tables["musteri"].Clear();
            Kayitgoster();
            MessageBox.Show("Müşteri Kaydı Güncellendi");
            foreach (Control item in this.Controls)
            {
                if (item is TextBox)
                {
                    item.Text = "";
                }
            }
        }

        private void btnSil_Click(object sender, EventArgs e)
        {
            baglanti.Open();
            SqlCommand komut = new SqlCommand("delete from musteri where tc='"+dataGridView1.CurrentRow.Cells["tc"].Value.ToString()+"'  ", baglanti);
            komut.ExecuteNonQuery();
            baglanti.Close();
            dataset.Tables["musteri"].Clear();
            Kayitgoster();
            MessageBox.Show("Müşteri Kaydı Silindi");
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {
            DataTable tablo = new DataTable();
            baglanti.Open();
            SqlDataAdapter adtr = new SqlDataAdapter("select * from musteri where tc like '%"+textBox3.Text+"%' ",baglanti);
            adtr.Fill(tablo);
            dataGridView1.DataSource = tablo;
            baglanti.Close();
        }

        private void txtTc_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
