using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;

namespace DanismanLazim
{
    public partial class Messages : Form
    {

        private int currentUser;
        private int otherUser;
        private string sendname;
        public Messages(int currentUserID, int otherUserID, string sendname)
        {
            InitializeComponent();
            this.currentUser = currentUserID;
            this.otherUser = otherUserID;
            this.sendname = sendname;
            this.Load += new EventHandler(Messages_Load);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string messageText = textBox1.Text;
            int senderID = currentUser;
            int receiverID = otherUser;
            SendMessage(senderID, receiverID, messageText);
        }
        public class MessageData
        {
            public int SenderID { get; set; }
            public string Message { get; set; }
            public DateTime SendDateTime { get; set; }
        }
        private void Messages_Load(object sender, EventArgs e)
        {
            int currentUserID = currentUser;
            int otherUserID = otherUser;
            label1.Text = sendname;
            List<MessageData> messageHistory = GetMessageHistory(currentUserID, otherUserID);

            foreach (MessageData messageData in messageHistory)
            {
                AddMessageToFlowLayout($"{messageData.Message}", messageData.SenderID, currentUserID);
            }
        }

        public string connectionString = "Server=tcp:viananyav2.database.windows.net,1433;Initial Catalog=viananyav2;Persist Security Info=False;User ID=viananyav2;Password=951753123789a.;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30";
        public void SendMessage(int sender, int receiver, string message)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string insertQuery = "INSERT INTO messages (senderID, receiverID, messageText, sendDateTime) VALUES (@senderID, @receiverID, @messageText, @sendDateTime)";

                using (SqlCommand command = new SqlCommand(insertQuery, connection))
                {
                    command.Parameters.AddWithValue("@senderID", sender);
                    command.Parameters.AddWithValue("@receiverID", receiver);
                    command.Parameters.AddWithValue("@messageText", message);
                    command.Parameters.AddWithValue("@sendDateTime", DateTime.Now);

                    command.ExecuteNonQuery();
                }
            }
            AddMessageToFlowLayout($"{message}({DateTime.Now.ToString("HH:mm:ss")})", sender, sender);
        }
        private void AddMessageToFlowLayout(string formattedMessage, int senderID, int currentUser)
        {
            Label label = new Label();
            Label label2 = new Label();
            label.Width = flowLayoutPanel1.Width;
            label.Margin = new Padding(0, 0, 0, 5);
            label2.Width = 0;
            label2.Margin = new Padding(0, 0, 0, 15);
            label.Font = new Font("Segoe UI", 12, FontStyle.Regular);
            label2.Font = new Font("Segoe UI", 12, FontStyle.Regular);
            if (senderID == currentUser)
            {
                label.Text = formattedMessage;
                label.AutoSize = false;
                label.TextAlign = ContentAlignment.TopRight;
                flowLayoutPanel1.Controls.Add(label);
                flowLayoutPanel1.SetFlowBreak(label, true);
                flowLayoutPanel1.ScrollControlIntoView(label);
            }
            if (senderID != currentUser)
            {
                label2.Text = formattedMessage;
                label2.AutoSize = true;
                label2.TextAlign = ContentAlignment.TopLeft;
                flowLayoutPanel1.Controls.Add(label2);
                flowLayoutPanel1.SetFlowBreak(label2, true);
                flowLayoutPanel1.ScrollControlIntoView(label2);
            }
        }
        public List<MessageData> GetMessageHistory(int currentUserID, int otherUserID)
        {
            List<MessageData> messageHistory = new List<MessageData>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string selectQuery = "SELECT senderID, messageText, sendDateTime FROM messages WHERE (senderID = @currentUserID AND receiverID = @otherUserID) OR (senderID = @otherUserID AND receiverID = @currentUserID) ORDER BY sendDateTime";

                using (SqlCommand command = new SqlCommand(selectQuery, connection))
                {
                    command.Parameters.AddWithValue("@currentUserID", currentUserID);
                    command.Parameters.AddWithValue("@otherUserID", otherUserID);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            int senderID = (int)reader["senderID"];
                            string messageText = reader["messageText"].ToString();
                            DateTime sendDateTime = (DateTime)reader["sendDateTime"];

                            messageHistory.Add(new MessageData
                            {
                                SenderID = senderID,
                                Message = $"{messageText} ({sendDateTime.ToString("HH:mm:ss")})"
                            });
                        }
                    }
                }
            }
            return messageHistory;
        }
        private void flowLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void Messages_Load_1(object sender, EventArgs e)
        {

        }
    }
}