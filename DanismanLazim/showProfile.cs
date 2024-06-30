using System;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection.Emit;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace DanismanLazim
{
    public partial class showProfile : Form
    {
        public string connectionString = "Server=tcp:viananyav2.database.windows.net,1433;Initial Catalog=viananyav2;Persist Security Info=False;User ID=viananyav2;Password=951753123789a.;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30";
        private int userId;
        public showProfile(int userId)
        {
            InitializeComponent();
            this.userId = userId;
            Console.WriteLine(userId);
        }

        private void showProfile_Load(object sender, EventArgs e)
        {
            if (IsUserAuthEqualTwo())
            {
                button2.Visible = true;
            }
            else
            {
                button2.Visible = false;
            }
            string query = "SELECT userName, userSurname, userMail, userProfilePicture FROM users WHERE id = @userId";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand(query, connection))
                {

                    command.Parameters.AddWithValue("@userId", userId);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {

                            label1.Text = reader["userName"] + " " + reader["userSurname"].ToString();
                            label3.Text = reader["userMail"].ToString();


                            if (reader["userProfilePicture"] != DBNull.Value)
                            {
                                byte[] imageBytes = (byte[])reader["userProfilePicture"];
                                pictureBox1.Image = ByteArrayToImage(imageBytes);
                            }
                        }
                        else
                        {
                            MessageBox.Show("Kullanıcı bulunamadı.");
                        }
                    }
                }
            }
        }

        private Image ByteArrayToImage(byte[] byteArray)
        {
            MemoryStream ms = new MemoryStream(byteArray);
            Image image = Image.FromStream(ms);
            return image;
        }
        private void button1_Click_1(object sender, EventArgs e)
        {
            formLogin loginForm = Application.OpenForms.OfType<formLogin>().FirstOrDefault();
            int currentUserID = loginForm.GetCurrentUserID();
            int otherUserID = userId;
            string sendName = label1.Text;
            var frm = new Messages(currentUserID, otherUserID, sendName);
            frm.Show();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            formLogin loginForm = Application.OpenForms.OfType<formLogin>().FirstOrDefault();
            int currentUserID = loginForm.GetCurrentUserID();
            int otherUserID = userId;
            var frm = new DanismanlikAl(currentUserID, otherUserID);
            frm.Show();
        }
        private bool IsUserAuthEqualTwo()
        {
            string query = "SELECT userAuth FROM users WHERE id = @userId";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@userId", userId);

                    object result = command.ExecuteScalar();

                    if (result != null)
                    {
                        return Convert.ToInt32(result) == 2;
                    }
                }
            }
            return false;
        }
    }
}
