using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DanismanLazim
{
    public partial class formProfile : Form
    {
        public formProfile(string email, string password)
        {
            InitializeComponent();
            this.email = email;
            this.password = password;
        }
        SqlConnection con = new SqlConnection("Server=tcp:viananyav2.database.windows.net,1433;Initial Catalog=viananyav2;Persist Security Info=False;User ID=viananyav2;Password=951753123789a.;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;");
        public string email { get; set; }
        public string password { get; set; }
        public DataTable dt { get; set; } = new DataTable();
        public DataRow row;

        private void button1_Click(object sender, EventArgs e)
        {
            var frm = new Form1();
            frm.Show();
            this.Hide();
        }
        public Image byteArrayToImage(byte[] byteArrayIn)
        {
            MemoryStream ms = new MemoryStream(byteArrayIn);
            Image returnImage = Image.FromStream(ms);
            return returnImage;
        }
        private void formProfile_Load(object sender, EventArgs e)
        {
            textBox1.Enabled = false;
            textBox2.Enabled = false;
            textBox3.Enabled = false;
            textBox4.Enabled = false;
            textBox5.Enabled = false;
            textBox6.Enabled = false;
            button4.Hide();
            button5.Hide();
            DataTable dt = new DataTable();
            con.Open();
            try
            {
                String query = "SELECT * FROM users WHERE userMail = '" + email + "' AND userPW = '" + password + "'";
                SqlCommand cmd = new SqlCommand(query, con);
                SqlDataAdapter sda = new SqlDataAdapter(cmd);
                sda.Fill(dt);

                cmd.ExecuteNonQuery();

                if (dt.Rows.Count > 0)
                {
                    MessageBox.Show("Data successfully readed");
                }
            }

            catch
            {
                MessageBox.Show("Data cannot be found.");
            }
            row = dt.Rows[0];
            try
            {
                var bytes = (byte[])row["userProfilePicture"];
                using (MemoryStream ms = new MemoryStream(bytes))
                {
                    pictureBox1.Image = Image.FromStream(ms);
                }
            }
            catch
            {
                return;
            }
            finally
            {
                textBox1.Text = row["userName"].ToString();
                textBox2.Text = row["userSurname"].ToString();
                textBox3.Text = row["userID"].ToString();
                textBox4.Text = row["userMail"].ToString();
                textBox5.Text = row["userPW"].ToString();
                textBox6.Text = row["userNum"].ToString();
            }
            con.Close();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            textBox1.Enabled = true;
            textBox2.Enabled = true;
            textBox3.Enabled = true;
            textBox4.Enabled = true;
            textBox5.Enabled = true;
            textBox6.Enabled = true;
            button3.Hide();
            button4.Show();
            button5.Show();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            con.Open();
            try
            {
                String query = "UPDATE users SET userName = @userName, userSurname = @userSurname, userID = @userID, userMail = @userMail, userPW = @userPW, userNum = @userNum WHERE userMail = '" + email + "' AND userPW = '" + password + "'";
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@userName", textBox1.Text);
                cmd.Parameters.AddWithValue("@userSurname", textBox2.Text);
                cmd.Parameters.AddWithValue("@userID", textBox3.Text);
                cmd.Parameters.AddWithValue("@userMail", textBox4.Text);
                cmd.Parameters.AddWithValue("@userPW", textBox5.Text);
                cmd.Parameters.AddWithValue("@userNum", textBox6.Text);
                cmd.ExecuteNonQuery();
                textBox1.Enabled = false;
                textBox2.Enabled = false;
                textBox3.Enabled = false;
                textBox4.Enabled = false;
                textBox5.Enabled = false;
                textBox6.Enabled = false;
                button4.Hide();
                button5.Hide();
                button3.Show();
                MessageBox.Show("Data has been updated");
            }
            catch
            {
                textBox1.Text = row["userName"].ToString();
                textBox2.Text = row["userSurname"].ToString();
                textBox3.Text = row["userID"].ToString();
                textBox4.Text = row["userMail"].ToString();
                textBox5.Text = row["userPW"].ToString();
                textBox6.Text = row["userNum"].ToString();
                textBox1.Enabled = true;
                textBox2.Enabled = true;
                textBox3.Enabled = true;
                textBox4.Enabled = true;
                textBox5.Enabled = true;
                textBox6.Enabled = true;
                MessageBox.Show("Data hasn't been updated");
            }
            con.Close();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            textBox1.Text = row["userName"].ToString();
            textBox2.Text = row["userSurname"].ToString();
            textBox3.Text = row["userID"].ToString();
            textBox4.Text = row["userMail"].ToString();
            textBox5.Text = row["userPW"].ToString();
            textBox6.Text = row["userNum"].ToString();
            textBox1.Enabled = false;
            textBox2.Enabled = false;
            textBox3.Enabled = false;
            textBox4.Enabled = false;
            textBox5.Enabled = false;
            textBox6.Enabled = false;
            button4.Hide();
            button5.Hide();
            button3.Show();
        }
        public byte[] imageToByteArray(System.Drawing.Image imageIn)
        {
            MemoryStream ms = new MemoryStream();
            imageIn.Save(ms, System.Drawing.Imaging.ImageFormat.Gif);
            return ms.ToArray();
        }
        private void button6_Click(object sender, EventArgs e)
        {
            con.Open();
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.Multiselect = false;
                ofd.Filter = "Image Files|*.jpg;*.jpeg;*.png";
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    pictureBox1.Image = Image.FromFile(ofd.FileName);
                }
            }
            try
            {
                String query = "UPDATE users SET userProfilePicture = @userProfilePicture WHERE userMail = '" + email + "' AND userPW = '" + password + "'";
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@userProfilePicture", imageToByteArray(pictureBox1.Image));
                cmd.ExecuteNonQuery();
                MessageBox.Show("Profil Fotoğrafı Güncellendi");
            }
            catch
            {
                MessageBox.Show("Profil Fotoğrafı Güncellenemedi");
            }
            con.Close();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            this.Close();
            var frm = new DanismanBasvur();
            frm.Show();
        }
    }
}
