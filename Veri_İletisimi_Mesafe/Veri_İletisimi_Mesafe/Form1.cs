using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO.Ports;
using System.Windows.Forms.DataVisualization.Charting;
using System.Data.SqlClient;

namespace Veri_İletisimi_Mesafe
{
    public partial class Form1 : Form
    {
        string gelen = "0";
        DateTime yeni = DateTime.Now;
        int zaman = 0;
        int satir = 1;
        int satirNo = 1;
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            dataGridView2.Visible = false;
            label2.Visible = false;
            serialPort1.Close();
         DateTime yeni = DateTime.Now; // saat ve tarih bilgisini almak için tanımlıyoruz
        }
 private void btn_basla_Click(object sender, EventArgs e)
        {
            serialPort1.PortName = "COM8";
            serialPort1.Open();
             timer1.Enabled = true;
        }

        private void btn_dur_Click(object sender, EventArgs e)
        {
            timer1.Enabled = false;
            serialPort1.Close();
        }

        private void btn_sil_Click(object sender, EventArgs e)
        {
            dataGridView1.Rows.Clear();

        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            gelen = serialPort1.ReadLine();
            zaman = (zaman + 1);
            satir = dataGridView1.Rows.Add();

            dataGridView1.Rows[satir].Cells[0].Value = satirNo;
            dataGridView1.Rows[satir].Cells[1].Value = gelen; //ölçüm
            dataGridView1.Rows[satir].Cells[2].Value = yeni.ToLongTimeString(); //saat bilgisi
            dataGridView1.Rows[satir].Cells[3].Value = yeni.ToShortDateString(); //tarih bilgisi
            satir++;
            satirNo++;
            label1.Text = gelen;
        }

        private void btn_kayıt_Click(object sender, EventArgs e)
        {
            SqlConnection con = new SqlConnection(@"Data Source=LAPTOP-1SLQI3BT\OZLEM;Initial Catalog=veri_iletsi;Integrated Security=True");
            using (SqlCommand cmd = new SqlCommand("INSERT INTO mesafe VALUES(@sıra, @mesafe, @saat, @tarih)", con))
            {

                cmd.Parameters.Add(new SqlParameter("@sıra", SqlDbType.Int));
                cmd.Parameters.Add(new SqlParameter("@mesafe", SqlDbType.Int));
                cmd.Parameters.Add(new SqlParameter("@saat", SqlDbType.Time));
                cmd.Parameters.Add(new SqlParameter("@tarih", SqlDbType.Date));
                foreach (DataGridViewRow row in dataGridView1.Rows) //satır denetimi
                {
                    if (!row.IsNewRow)
                    {
                        cmd.Parameters["@sıra"].Value = row.Cells[0].Value;
                        cmd.Parameters["@mesafe"].Value = row.Cells[1].Value;
                        cmd.Parameters["@saat"].Value = row.Cells[2].Value;
                        cmd.Parameters["@tarih"].Value = row.Cells[3].Value;
                        con.Open();

                        cmd.ExecuteNonQuery();
                        con.Close();
                    }
                }
                MessageBox.Show("Kayıt Başarılı");
            }
        }

        private void btn_gös_Click(object sender, EventArgs e)
        {
            dataGridView2.Visible = true;
            label2.Visible = true;
            SqlConnection con = new SqlConnection(@"Data Source=LAPTOP-1SLQI3BT\OZLEM;Initial Catalog=veri_iletsi;Integrated Security=True");
            SqlDataAdapter da = new SqlDataAdapter("select*from mesafe", con);
            DataSet ds = new DataSet();
            con.Open();
            da.Fill(ds, "mesafe");
            dataGridView2.DataSource = ds.Tables["mesafe"];
            con.Close();
        }

       
    }
}
