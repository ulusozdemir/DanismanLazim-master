using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Drawing.Printing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;

namespace DanismanLazim
{
    public partial class Main : Form
    {
        SqlConnection con = new SqlConnection("Server=tcp:viananyav2.database.windows.net,1433;Initial Catalog=viananyav2;Persist Security Info=False;User ID=viananyav2;Password=951753123789a.;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;");
        bool sidebarExpand;
        public int selectedUserID;
        Button ctgBtn;
        Button cnsBtn;
        public int currentUserID;
        public Main(string email, string password)
        {
            this.currentUserID = 0;
            this.email = email;
            this.password = password;
            InitializeComponent();
            timer1.Interval = 5;
        }
        public string email { get; set; }
        public string password { get; set; }

        private void flowLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            var frm1 = new formProfile(email, password);
            frm1.Show();
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void Main_Load(object sender, EventArgs e)
        {
            con.Open();
            string query2 = "SELECT * FROM users WHERE userAuth = 2";
            SqlCommand command2 = new SqlCommand(query2, con);
            SqlDataReader reader2 = command2.ExecuteReader();
            while (reader2.Read())
            {
                cnsBtn = new Button()
                {
                    TextImageRelation = TextImageRelation.ImageBeforeText,
                    Width = panel.Width / 10,
                    Height = panel.Height - 10,
                    Name = reader2["id"].ToString(),
                    BackgroundImage = convertBytesToImage((byte[])reader2["userProfilePicture"]),
                    Anchor = AnchorStyles.None,
                    BackgroundImageLayout = ImageLayout.Stretch
                };
                cnsBtn.Click += new EventHandler(userBtnClick);
                flowLayoutPanel5.Controls.Add(cnsBtn);
            }
            con.Close();
            reader2.Dispose();
            command2.Dispose();
            con.Open();

            string query = "SELECT TOP 9 * FROM categories ORDER BY id";
            SqlCommand cmd = new SqlCommand(query, con);
            SqlDataReader reader = cmd.ExecuteReader();
            panel.GrowStyle = TableLayoutPanelGrowStyle.AddColumns;
            panel.CellBorderStyle = TableLayoutPanelCellBorderStyle.Inset;
            panel.RowStyles.Clear();
            panel.ColumnStyles.Clear();
            panel.Controls.Clear();
            while (reader.Read())
            {
                ctgBtn = new Button()
                {
                    TextImageRelation = TextImageRelation.ImageBeforeText,
                    Width = panel.Width / 10,
                    Height = panel.Height - 10,
                    Name = reader["categoryName"].ToString(),
                    BackgroundImage = convertBytesToImage((byte[])reader["categoryPicture"]),
                    Anchor = AnchorStyles.None,
                    BackgroundImageLayout = ImageLayout.Stretch
                };
                ctgBtn.Click += new EventHandler(btnClick);
                panel.Controls.Add(ctgBtn);
            }
            reader.Dispose();
            cmd.Dispose();

            con.Close();

            con.Open();
            string getUserQuery = "SELECT id FROM users WHERE userMail = @userMail AND userPW = @userPW";
            SqlCommand getUser = new SqlCommand(getUserQuery, con);
            getUser.Parameters.AddWithValue("@userMail", this.email);
            getUser.Parameters.AddWithValue("@userPW", this.password);
            SqlDataReader userID = getUser.ExecuteReader();
            
            while(userID.Read())
            {
                currentUserID = int.Parse(userID["id"].ToString());
            }
            getUser.Dispose();
            userID.Dispose();
            con.Close();
        }
        protected void btnClick(object sender, EventArgs e)
        {
            flowLayoutPanel5.Controls.Clear();
            con.Open();
            string catText = (sender as Button).Name;
            string query = "SELECT * FROM users INNER JOIN categories ON users.consCatID=categories.id WHERE userAuth = 2 AND categoryName = @categoryName";
            SqlCommand cmd = new SqlCommand(query, con);
            cmd.Parameters.AddWithValue("@categoryName", catText);
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                cnsBtn = new Button()
                {
                    TextImageRelation = TextImageRelation.ImageBeforeText,
                    Width = panel.Width / 10,
                    Height = panel.Height - 10,
                    Name = reader["id"].ToString(),
                    BackgroundImage = convertBytesToImage((byte[])reader["userProfilePicture"]),
                    Anchor = AnchorStyles.None,
                    BackgroundImageLayout = ImageLayout.Stretch
                };
                selectedUserID = int.Parse(reader["id"].ToString());
                cnsBtn.Click += new EventHandler(userBtnClick);
                flowLayoutPanel5.Controls.Add(cnsBtn);
            }
            reader.Dispose();
            cmd.Dispose();
            con.Close();
        }

        protected void userBtnClick(object sender, EventArgs e)
        {
            selectedUserID = int.Parse((sender as Button).Name);
            var frm = new showProfile(selectedUserID);
            frm.Show();
        }
        public Image convertBytesToImage(byte[] byteArrayIn)
        {
            using (var ms = new MemoryStream(byteArrayIn))
            {
                return Image.FromStream(ms);
            }
        }
        private void loadDetails()
        {
            foreach (Data data in Data.list)
            {
                SearchResultControl res = new SearchResultControl();
                res.details(data);
                flowLayoutPanel3.Controls.Add(res);
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            var frm1 = new formLogin();
            frm1.Show();
            this.Close();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            timer1.Start();
        }
        private void timer1_Tick(object sender, EventArgs e)
        {
            if (sidebarExpand)
            {
                flowLayoutPanel1.Width -= 10;
                if (flowLayoutPanel1.Width == flowLayoutPanel1.MinimumSize.Width)
                {
                    sidebarExpand = false;
                    timer1.Stop();
                    button1.Enabled = false;
                    button2.Enabled = false;
                    button4.Enabled = false;
                }
            }
            else
            {
                flowLayoutPanel1.Width += 10;
                if (flowLayoutPanel1.Width == flowLayoutPanel1.MaximumSize.Width)
                {
                    sidebarExpand = true;
                    timer1.Stop();
                    button1.Enabled = true;
                    button2.Enabled = true;
                    button4.Enabled = true;
                }
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            if (textBox1.TextLength >= 3)
            {
                flowLayoutPanel3.Controls.Clear();
                SearchResultControl res = new SearchResultControl();
                res.searchResult(textBox1.Text);
                loadDetails();
                label2.Visible = false;
                panel.Visible = false;
                panel.Enabled = false;
                flowLayoutPanel5.Visible = false;
                flowLayoutPanel5.Enabled = false;
                flowLayoutPanel3.Height = flowLayoutPanel3.Controls.Count * 62;
            }
            else
            {
                label2.Visible = true;
                flowLayoutPanel3.Height = 0;
                panel.Visible = true;
                panel.Enabled = true;
                flowLayoutPanel5.Visible = true;
                flowLayoutPanel5.Enabled = true;
            }
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            if (SearchResultControl.clicked == true)
            {
                Data get = new Data();
                get.getSelected(SearchResultControl.id);
                flowLayoutPanel3.Height = 0;
                SearchResultControl.clicked = false;
                int id = Convert.ToInt32(SearchResultControl.id);
                var profileForm = new showProfile(id);
                profileForm.Show();
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            var frm = new formdms(currentUserID);
            frm.Show();
        }
    }
}