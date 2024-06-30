using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DanismanLazim
{
    public partial class formLogin : Form
    {
        public int CurrentUserID { get; set; }
        public formLogin()
        {
            InitializeComponent();
        }
        SqlConnection con = new SqlConnection("Server=tcp:viananyav2.database.windows.net,1433;Initial Catalog=viananyav2;Persist Security Info=False;User ID=viananyav2;Password=951753123789a.;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;");
        public String email, password, name = "a", midname = "a", surname = "a";

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void formLogin_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            var frm = new adminPanel();
            frm.Show();
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            var frm = new Form1();
            frm.Show();
            this.Hide();
        }

        public DataTable dtable = new DataTable();
        private void button2_Click(object sender, EventArgs e)
        {

            email = textBox1.Text;
            password = textBox2.Text;

            try
            {
                con.Open();
                String query = "SELECT id, userMail, userPW, userName, userSurname FROM users WHERE userMail = '" + email + "' AND userPW = '" + password + "'";
                SqlDataAdapter sda = new SqlDataAdapter(query, con);
                sda.Fill(dtable);
                if (dtable.Rows.Count > 0)
                {
                    MessageBox.Show("Succeeded!");
                    this.Hide();
                    DataRow row = dtable.Rows[0];
                    name = row["userName"].ToString();
                    surname = row["userSurname"].ToString();
                    CurrentUserID = Convert.ToInt32(row["id"].ToString());
                    var frmProfile = new formProfile(email, password);
                    frmProfile.email = email;
                    frmProfile.password = password;
                    var frmMain = new Main(email, password);
                    frmMain.email = email;
                    frmMain.password = password;
                    frmMain.Show();
                }
            }
            catch
            {
                MessageBox.Show("Login failed.");
            }
            finally
            {
                con.Close();
            }
        }
        public int GetCurrentUserID()
        {
            return CurrentUserID;
        }
    }
}
