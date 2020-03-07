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
    public partial class frmSatisListele : Form
    {
        public frmSatisListele()
        {
            InitializeComponent();
        }
        public void satislistele()
        {
            baglanti.Open();
            SqlDataAdapter adtr = new SqlDataAdapter("select * from satis", baglanti);
            adtr.Fill(dataset, "satis");
            dataGridView1.DataSource = dataset.Tables["satis"];
            
            baglanti.Close();
        }
        SqlConnection baglanti = new SqlConnection("Data Source=DESKTOP-RHOGDFE;Initial Catalog=Stok_Takip;Integrated Security=True");
        DataSet dataset = new DataSet();
        private void frmSatisListele_Load(object sender, EventArgs e)
        {
            satislistele();
        }
    }
}
