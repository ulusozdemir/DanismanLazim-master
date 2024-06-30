using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DanismanLazim
{
    public partial class formdms : Form
    {
        SqlConnection con = new SqlConnection("Server=tcp:viananyav2.database.windows.net,1433;Initial Catalog=viananyav2;Persist Security Info=False;User ID=viananyav2;Password=951753123789a.;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;");
        public int id { get; set; }
        Button dmBtn;
        Label dmLbl;
        Panel dmPanel;
        public formdms(int id)
        {
            this.id = id;
            InitializeComponent();
        }

        private void formdms_Load(object sender, EventArgs e)
        {
            con.Open();
            panel.GrowStyle = TableLayoutPanelGrowStyle.AddRows;
            panel.CellBorderStyle = TableLayoutPanelCellBorderStyle.Inset;
            panel.RowStyles.Clear();
            panel.ColumnStyles.Clear();
            panel.Controls.Clear();

            string query = "SELECT * FROM users INNER JOIN messages ON users.id = messages.senderID WHERE receiverID = @receiverID";
            SqlCommand cmd = new SqlCommand(query, con);
            cmd.Parameters.AddWithValue("@receiverID", this.id);
            SqlDataReader reader = cmd.ExecuteReader();
            while(reader.Read())
            {
                dmPanel = new Panel();
                dmBtn = new Button()
                {
                    Width = 100,
                    Height = 100,
                    Name = reader["id"].ToString(),
                    BackColor = Color.WhiteSmoke,
                    Text = reader["userName"] + " " + reader["userSurname"].ToString(),
                    Anchor = AnchorStyles.Left
                };

                dmBtn.Font = new Font("Segoe UI Semibold", 12);
                dmPanel.Controls.Add(dmBtn);
                dmBtn.Click += new EventHandler(btnClick);
                panel.Controls.Add(dmPanel);
            }
            reader.Dispose();
            cmd.Dispose();
            con.Close();
        }

        protected void btnClick(object sender, EventArgs e)
        {
            int otherUserID = int.Parse((sender as Button).Name);
            var frm = new Messages(this.id, otherUserID, (sender as Button).Text);
            frm.Show();
        }
    }
}
