﻿using System;
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
    public partial class frmSatis : Form
    {
        public frmSatis()
        {
            InitializeComponent();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            frmMusteriEkle ekle = new frmMusteriEkle();
            ekle.ShowDialog();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            frmMusteriListele listele = new frmMusteriListele();
            listele.ShowDialog();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            frmUrunEkle ekle = new frmUrunEkle();
            ekle.ShowDialog();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            frmKategori kategori = new frmKategori();
            kategori.ShowDialog();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            frmMarka marka = new frmMarka();
            marka.ShowDialog();
        }

        private void button8_Click(object sender, EventArgs e)
        {
            frmUrunListele listele = new frmUrunListele();
            listele.ShowDialog();
        }
        SqlConnection baglanti = new SqlConnection("Data Source=DESKTOP-RHOGDFE;Initial Catalog=Stok_Takip;Integrated Security=True");
        DataSet dataset = new DataSet();
        public void sepetlistele()
        {
            baglanti.Open();
            SqlDataAdapter adtr = new SqlDataAdapter("select * from sepet", baglanti);
            adtr.Fill(dataset, "sepet");
            dataGridView1.DataSource = dataset.Tables["sepet"];
            dataGridView1.Columns[0].Visible = false;
            dataGridView1.Columns[1].Visible = false;
            dataGridView1.Columns[2].Visible = false;
            baglanti.Close();
        }
        private void frmSatis_Load(object sender, EventArgs e)
        {

            sepetlistele();
        }
        private void hesapla()
        {
            try
            {
                baglanti.Open();
                SqlCommand komut = new SqlCommand("select sum(toplamfiyati) from sepet  ", baglanti);
                lblGenelToplam.Text = komut.ExecuteScalar() + "TL";
                baglanti.Close();
            }
            catch (Exception)
            {

                ;
            }
        }
        private void txtTc_TextChanged(object sender, EventArgs e)
        {
            if (txtTc.Text == "")
            {
                txtAdSoyad.Text = "";
                txtTelefon.Text = "";
            }
            baglanti.Open();
            SqlCommand komut = new SqlCommand("select * from musteri where tc like '" + txtTc.Text + "' ", baglanti);
            SqlDataReader read = komut.ExecuteReader();
            while (read.Read())
            {
                txtAdSoyad.Text = read["adsoyad"].ToString();
                txtTelefon.Text = read["telefon"].ToString();
            }
            baglanti.Close();
        }

        private void txtBarkodNo_TextChanged(object sender, EventArgs e)
        {
            temizle();
            baglanti.Open();
            SqlCommand komut = new SqlCommand("select * from urun where barkodno like '" + txtBarkodNo.Text + "'    ", baglanti);
            SqlDataReader read = komut.ExecuteReader();
            while (read.Read())
            {
                txtUrunAdi.Text = read["urunadi"].ToString();
                txtSatisFiyati.Text = read["satisfiyati"].ToString();
            }
            baglanti.Close();
        }

        private void temizle()
        {
            if (txtBarkodNo.Text == "")
            {
                foreach (Control item in groupBox2.Controls)
                {
                    if (item is TextBox)
                    {
                        if (item != txtMiktari)
                        {
                            item.Text = "";
                        }
                    }

                }
            }
        }
        bool durum;
        private void barkodkontrol()
        {
            durum = true;
            baglanti.Open();
            SqlCommand komut = new SqlCommand("select * from sepet", baglanti);
            SqlDataReader read = komut.ExecuteReader();
            while (read.Read())
            {
                if (txtBarkodNo.Text == read["barkodno"].ToString())
                {
                    durum = false;
                }
            }
            baglanti.Close();
            //barkod kullanarak ürün bilgilerini listeledik
        }
        private void btnEkle_Click(object sender, EventArgs e)
        {
            if (txtBarkodNo.Text == "")
            {
                MessageBox.Show("BarkodNo Boş", "Uyarı");
            }
            else
            {
                barkodkontrol();
                if (durum == true)
                {
                    baglanti.Open();
                    SqlCommand komut = new SqlCommand("insert into sepet(tc,adsoyad,telefon,barkodno,urunadi,miktari,satisfiyati,toplamfiyati,tarih) values(@tc,@adsoyad,@telefon,@barkodno,@urunadi,@miktari,@satisfiyati,@toplamfiyati,@tarih)", baglanti);
                    komut.Parameters.AddWithValue("@tc", txtTc.Text);
                    komut.Parameters.AddWithValue("@adsoyad", txtAdSoyad.Text);
                    komut.Parameters.AddWithValue("@telefon", txtTelefon.Text);
                    komut.Parameters.AddWithValue("@barkodno", txtBarkodNo.Text);
                    komut.Parameters.AddWithValue("@urunadi", txtUrunAdi.Text);
                    komut.Parameters.AddWithValue("@miktari", int.Parse(txtMiktari.Text));
                    komut.Parameters.AddWithValue("@satisfiyati", double.Parse(txtSatisFiyati.Text));
                    komut.Parameters.AddWithValue("@toplamfiyati", double.Parse(txtToplamFiyati.Text));
                    komut.Parameters.AddWithValue("@tarih", DateTime.Now.ToString());
                    komut.ExecuteNonQuery();
                    baglanti.Close();
                }
                else
                {
                    baglanti.Open();
                    SqlCommand komut2 = new SqlCommand("update sepet set miktari=miktari+'" + int.Parse(txtMiktari.Text) + "' where barkodno='" + txtBarkodNo.Text + "'  ", baglanti);
                    komut2.ExecuteNonQuery();
                    SqlCommand komut3 = new SqlCommand("update sepet set toplamfiyati=miktari*satisfiyati where barkodno='" + txtBarkodNo.Text + "'  ", baglanti);
                    komut3.ExecuteNonQuery();
                    baglanti.Close();
                }

                txtMiktari.Text = "1";
                dataset.Tables["sepet"].Clear();
                sepetlistele();
                hesapla();
                foreach (Control item in groupBox2.Controls)
                {
                    if (item is TextBox)
                    {
                        if (item != txtMiktari)
                        {
                            item.Text = "";
                        }
                    }

                }
            }


        }

        private void txtMiktari_TextChanged(object sender, EventArgs e)
        {
            try
            {
                txtToplamFiyati.Text = (double.Parse(txtMiktari.Text) * double.Parse(txtSatisFiyati.Text)).ToString();
            }
            catch (Exception)
            {

                ;
            }
        }

        private void txtSatisFiyati_TextChanged(object sender, EventArgs e)
        {
            try
            {
                txtToplamFiyati.Text = (double.Parse(txtMiktari.Text) * double.Parse(txtSatisFiyati.Text)).ToString();
            }
            catch (Exception)
            {

                ;
            }
        }

        private void btnSil_Click(object sender, EventArgs e)
        {
            baglanti.Open();
            SqlCommand komut = new SqlCommand("delete from sepet where barkodno='" + dataGridView1.CurrentRow.Cells["barkodno"].Value.ToString() + "'  ", baglanti);
            komut.ExecuteNonQuery();
            baglanti.Close();
            MessageBox.Show("Seçili Ürün Sepetten Çıkarıldı");
            dataset.Tables["sepet"].Clear();
            sepetlistele();
            hesapla();

        }

        private void btnSatisIptal_Click(object sender, EventArgs e)
        {
            baglanti.Open();
            SqlCommand komut = new SqlCommand("delete from sepet   ", baglanti);
            komut.ExecuteNonQuery();
            baglanti.Close();
            MessageBox.Show("Ürünler Sepetten Çıkarıldı");
            dataset.Tables["sepet"].Clear();
            sepetlistele();
            hesapla();
        }

        private void button9_Click(object sender, EventArgs e)
        {
            frmSatisListele satislistele = new frmSatisListele();
            satislistele.ShowDialog();

        }

        private void btnSatisYap_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < dataGridView1.Rows.Count - 1; i++)
            {
                baglanti.Open();
                SqlCommand komut = new SqlCommand("insert into satis(tc,adsoyad,telefon,barkodno,urunadi,miktari,satisfiyati,toplamfiyati,tarih) values(@tc,@adsoyad,@telefon,@barkodno,@urunadi,@miktari,@satisfiyati,@toplamfiyati,@tarih)", baglanti);
                komut.Parameters.AddWithValue("@tc", txtTc.Text);
                komut.Parameters.AddWithValue("@adsoyad", txtAdSoyad.Text);
                komut.Parameters.AddWithValue("@telefon", txtTelefon.Text);
                komut.Parameters.AddWithValue("@barkodno", dataGridView1.Rows[i].Cells["barkodno"].Value.ToString());
                komut.Parameters.AddWithValue("@urunadi", dataGridView1.Rows[i].Cells["urunadi"].Value.ToString());
                komut.Parameters.AddWithValue("@miktari", int.Parse(dataGridView1.Rows[i].Cells["miktari"].Value.ToString()));
                komut.Parameters.AddWithValue("@satisfiyati", double.Parse(dataGridView1.Rows[i].Cells["satisfiyati"].Value.ToString()));
                komut.Parameters.AddWithValue("@toplamfiyati", double.Parse(dataGridView1.Rows[i].Cells["toplamfiyati"].Value.ToString()));
                komut.Parameters.AddWithValue("@tarih", DateTime.Now.ToString());
                komut.ExecuteNonQuery();
                SqlCommand komut2 = new SqlCommand("update urun set miktari=miktari-'" + int.Parse(dataGridView1.Rows[i].Cells["miktari"].Value.ToString()) + "' where barkodno='" + dataGridView1.Rows[i].Cells["barkodno"].Value.ToString() + "' ", baglanti);
                komut2.ExecuteNonQuery();
                baglanti.Close();
            }
            baglanti.Open();
            SqlCommand komut3 = new SqlCommand("delete from sepet   ", baglanti);
            komut3.ExecuteNonQuery();
            baglanti.Close();
            dataset.Tables["sepet"].Clear();
            sepetlistele();
            hesapla();
        }
       


    }


}
    

