using System;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace DanismanLazim
{
    public partial class DanismanlikAl : Form
    {
        private const string connectionString = "Server = tcp:viananyav2.database.windows.net,1433;Initial Catalog = viananyav2; Persist Security Info=False;User ID = viananyav2; Password=951753123789a.;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout = 30";
        private int currentUserID;
        private int otheruserID;
        public DanismanlikAl(int currentUserID, int otheruserID)
        {
            this.currentUserID = currentUserID;
            this.otheruserID = otheruserID;
            InitializeComponent();
        }

        private void DanismanlikAl_Load(object sender, EventArgs e)
        {
            dateTimePicker1.Format = DateTimePickerFormat.Custom;
            dateTimePicker1.CustomFormat = "dd/MM/yyyy HH:mm:ss";
            dateTimePicker1.ShowUpDown = false;
            dateTimePicker1.Value = DateTime.Today;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            DateTime selectedDateTime = dateTimePicker1.Value;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string insertQuery = "INSERT INTO DanismanlikTalep (UserID, OtherUserID, SelectedDate) VALUES (@UserID, @OtherUserID, @SelectedDate)";
                using (SqlCommand cmd = new SqlCommand(insertQuery, connection))
                {
                    cmd.Parameters.AddWithValue("@UserID", currentUserID);
                    cmd.Parameters.AddWithValue("@OtherUserID", otheruserID);
                    cmd.Parameters.AddWithValue("@SelectedDate", selectedDateTime);
                    cmd.ExecuteNonQuery();
                }

                connection.Close();
            }
            MessageBox.Show("Talebiniz Başarıyla Alındı! Lütfen Randevu Gününe kadar X banka hesabına havale yaptığınızın dekontunu danismanlazimiletisim@gmail.com mail adresine gönderiniz!", "Bildirim", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}