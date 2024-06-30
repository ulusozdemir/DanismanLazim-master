using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace DanismanLazim
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

        }
        SqlConnection con = new SqlConnection("Server=tcp:viananyav2.database.windows.net,1433;Initial Catalog=viananyav2;Persist Security Info=False;User ID=viananyav2;Password=951753123789a.;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;");
        private void Form1_Load(object sender, EventArgs e)
        {
            
        }

        private void button2_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.Multiselect = false;
                ofd.Filter = "Image Files|*.jpg;*.jpeg;*.png";
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    pictureBox1.Image = Image.FromFile(ofd.FileName);
                }
            }
        }

        private void btnRegister_Click(object sender, EventArgs e)
        {
            
            con.Open();
            if (textBox1.Text == "" || textBox3.Text == "" || textBox4.Text == "" || textBox5.Text == "" || textBox6.Text == "" || textBox7.Text == "" || textBox8.Text == "")
            {
                MessageBox.Show("Lütfen boş alan bırakmayınız. Aksi takdirde veri bütünlüğünde sıkıntı çıkabilir.");
            }
            else
            {
                if (textBox6.Text != textBox7.Text)
                {
                    MessageBox.Show("Şifreler Uyuşmuyor. Lütfen kontrol edip tekrar deneyiniz.");
                }
                else
                {
                    try
                    {
                        SqlCommand cmd = new SqlCommand("INSERT INTO users (userID, userPW, userNum, userMail, userName, userSurname, userAuth, userProfilePicture) " +
                            "VALUES (@userID, @userPW, @userNum, @userMail, @userName, @userSurname, @userAuth, @userProfilePicture)", con);
                        cmd.Parameters.AddWithValue("@userID", textBox4.Text);
                        cmd.Parameters.AddWithValue("@userPW", textBox6.Text);
                        cmd.Parameters.AddWithValue("@userNum", textBox8.Text);
                        cmd.Parameters.AddWithValue("@userMail", textBox5.Text);
                        cmd.Parameters.AddWithValue("@userName", textBox1.Text);
                        cmd.Parameters.AddWithValue("@userSurname", textBox3.Text);
                        cmd.Parameters.AddWithValue("@userAuth", 1);
                        cmd.Parameters.AddWithValue("@userProfilePicture", imageToByteArray(pictureBox1.Image));
                        cmd.ExecuteNonQuery();
                        con.Close();
                        MessageBox.Show("Kullanıcı ekleme işlemi başarılı!");
                    }
                    catch
                    {
                        MessageBox.Show("Kullanıcı ekleme işlemi başarısız!");
                        textBox1.Text = "";
                        textBox3.Text = "";
                        textBox4.Text = "";
                        textBox5.Text = "";
                        textBox6.Text = "";
                        textBox7.Text = "";
                        textBox8.Text = "";
                    }
                }

            }
            con.Close();
        }

        public byte[] imageToByteArray(System.Drawing.Image imageIn)
        {
            MemoryStream ms = new MemoryStream();
            imageIn.Save(ms, System.Drawing.Imaging.ImageFormat.Gif);
            return ms.ToArray();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Hide();
            var frm = new formLogin();
            frm.Show();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            var frm = new adminPanel();
            frm.Show();
        }

        private void button3_Click_1(object sender, EventArgs e)
        {
            var frm = new adminPanel();
            frm.Show();
            this.Hide();
        }
    }
}
