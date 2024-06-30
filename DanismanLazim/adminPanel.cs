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
    public partial class adminPanel : Form
    {
        public adminPanel()
        {
            InitializeComponent();
        }
        SqlConnection con = new SqlConnection("Server=tcp:viananyav2.database.windows.net,1433;Initial Catalog=viananyav2;Persist Security Info=False;User ID=viananyav2;Password=951753123789a.;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;");
        SqlDataReader reader;
        private void adminPanel_Load(object sender, EventArgs e)
        {
            ShowApplications();
            comboBox1.Items.Add("");
            loadUsers();
            loadCategories();
            loadReqs();
            con.Open();

            string query = "SELECT * FROM categories";
            SqlCommand cmd = new SqlCommand(query, con);
            reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                comboBox1.Items.Add(reader[0] + " - " + reader[1].ToString());
            }
            con.Close();

            this.dataGridView3.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            this.dataGridView3.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            this.dataGridView3.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            this.dataGridView4.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            this.dataGridView4.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            this.dataGridView4.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            this.dataGridView4.Columns[3].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
        }

        private void ShowApplications()
        {
            try
            {
                con.Open();

                string query = "SELECT UserId, CategoryName, DocumentContent FROM DanısmanBasvuru;";
                SqlDataAdapter adapter = new SqlDataAdapter(query, con);

                DataTable dataTable = new DataTable();
                adapter.Fill(dataTable);

                dataGridView3.DataSource = dataTable;

                AddApproveRejectButtons();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Başvurular alınırken bir hata oluştu:\n{ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                con.Close();
            }
        }

        private void AddApproveRejectButtons()
        {
            DataGridViewButtonColumn approveButtonColumn = new DataGridViewButtonColumn();
            approveButtonColumn.Name = "ApproveButton";
            approveButtonColumn.HeaderText = "Onayla";
            approveButtonColumn.Text = "Onayla";
            approveButtonColumn.UseColumnTextForButtonValue = true;
            dataGridView3.Columns.Add(approveButtonColumn);

            DataGridViewButtonColumn rejectButtonColumn = new DataGridViewButtonColumn();
            rejectButtonColumn.Name = "RejectButton";
            rejectButtonColumn.HeaderText = "Reddet";
            rejectButtonColumn.Text = "Reddet";
            rejectButtonColumn.UseColumnTextForButtonValue = true;
            dataGridView3.Columns.Add(rejectButtonColumn);

            DataGridViewButtonColumn viewButtonColumn = new DataGridViewButtonColumn();
            viewButtonColumn.Name = "ViewButton";
            viewButtonColumn.HeaderText = "Görüntüle";
            viewButtonColumn.Text = "Görüntüle";
            viewButtonColumn.UseColumnTextForButtonValue = true;
            dataGridView3.Columns.Add(viewButtonColumn);

            dataGridView3.Columns["DocumentContent"].Visible = false;
        }

        public void loadUsers()
        {
            string query = "SELECT * FROM users";
            SqlCommand cmd = new SqlCommand(query, con);
            SqlDataAdapter sda = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            sda.Fill(dt);
            dataGridView1.DataSource = dt;
        }
        public void loadCategories()
        {
            string query2 = "SELECT * FROM categories";
            SqlCommand cmd2 = new SqlCommand(query2, con);
            SqlDataAdapter sda2 = new SqlDataAdapter(cmd2);
            DataTable dt2 = new DataTable();
            sda2.Fill(dt2);
            dataGridView2.DataSource = dt2;
        }

        public void loadReqs()
        {
            string query3 = "SELECT * FROM DanismanlikTalep";
            SqlCommand cmd3 = new SqlCommand(query3, con);
            SqlDataAdapter sda3 = new SqlDataAdapter(cmd3);
            DataTable dt3 = new DataTable();
            sda3.Fill(dt3);
            dataGridView4.DataSource = dt3;
        }

        public void deleteData(int id)
        {
            try
            {
                con.Open();
                string query = "DELETE FROM users WHERE id = @id";
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@id", id);
                cmd.ExecuteNonQuery();
                MessageBox.Show("Deleted!");
            }
            catch
            {
                MessageBox.Show("Failed.");
            }
            con.Close();
        }
        public void deleteReqs(int id)
        {
            con.Open();
            string query = "DELETE FROM DanismanlikTalep WHERE ID = @id";
            SqlCommand cmd = new SqlCommand(query, con);
            cmd.Parameters.AddWithValue("@id", id);
            cmd.ExecuteNonQuery();
            MessageBox.Show("Deleted!");
        }
        public void deleteCategory(int id)
        {
            try
            {
                con.Open();
                string query = "DELETE FROM categories WHERE id = @id";
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@id", id);
                cmd.ExecuteNonQuery();
                MessageBox.Show("Deleted!");
            }
            catch
            {
                MessageBox.Show("Failed.");
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
            con.Open();

            try
            {
                SqlCommand cmd = new SqlCommand("INSERT INTO users (userID, userPW, userNum, userMail, userName, userSurname, userAuth, userProfilePicture, consCatID) " +
                    "VALUES (@userID, @userPW, @userNum, @userMail, @userName, @userSurname, @userAuth, @userProfilePicture, @consCatID)", con);
                cmd.Parameters.AddWithValue("@userID", textBox1.Text);
                cmd.Parameters.AddWithValue("@userPW", textBox2.Text);
                cmd.Parameters.AddWithValue("@userNum", textBox3.Text);
                cmd.Parameters.AddWithValue("@userMail", textBox4.Text);
                cmd.Parameters.AddWithValue("@userName", textBox5.Text);
                cmd.Parameters.AddWithValue("@userSurname", textBox6.Text);
                cmd.Parameters.AddWithValue("@userAuth", textBox7.Text);
                cmd.Parameters.AddWithValue("@userProfilePicture", imageToByteArray(pictureBox1.Image));
                string cellValue = this.comboBox1.GetItemText(this.comboBox1.SelectedItem);
                int id = int.Parse(cellValue.Substring(0, 2));
                cmd.Parameters.AddWithValue("@consCatID", id);
                cmd.ExecuteNonQuery();
                MessageBox.Show("Kullanıcı ekleme işlemi başarılı!");
            }
            catch
            {
                MessageBox.Show("Kullanıcı ekleme işlemi başarısız!");
                textBox1.Text = "";
                textBox2.Text = "";
                textBox3.Text = "";
                textBox4.Text = "";
                textBox5.Text = "";
                textBox6.Text = "";
                textBox7.Text = "";
                comboBox1.Text = "";
            }
            con.Close();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            con.Open();
            string query = "SELECT * FROM users WHERE userName LIKE @userName0 OR userName LIKE @userName1 OR userName LIKE @userName2";
            SqlCommand cmd = new SqlCommand(query, con);
            cmd.Parameters.AddWithValue("@userName0", "%" + textBox8.Text + "%");
            cmd.Parameters.AddWithValue("@userName1", textBox8.Text + "%");
            cmd.Parameters.AddWithValue("@userName2", "%" + textBox8.Text);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            dataGridView1.DataSource = dt;
            con.Close();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult = MessageBox.Show("Are you sure?", "Deleting", MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.Yes)
            {
                foreach (DataGridViewRow row in dataGridView1.SelectedRows)
                {
                    int id = Convert.ToInt32(row.Cells[0].Value);
                    deleteData(id);
                    loadUsers();
                }
            }
            else if (dialogResult == DialogResult.No)
            {
                return;
            }

        }

        private void button7_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.Multiselect = false;
                ofd.Filter = "Image Files|*.jpg;*.jpeg;*.png";
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    pictureBox2.Image = Image.FromFile(ofd.FileName);
                }
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            SqlCommand cmd = new SqlCommand("INSERT INTO categories (categoryName, categoryPicture) " +
                            "VALUES (@categoryName, @categoryPicture)", con);
            con.Open();
            try
            {
                cmd.Parameters.AddWithValue("@categoryName", textBox9.Text);
                cmd.Parameters.AddWithValue("@categoryPicture", imageToByteArray(pictureBox2.Image));
                cmd.ExecuteNonQuery();
                MessageBox.Show("Succeeded");
                loadCategories();
            }
            catch
            {
                MessageBox.Show("Failed");
            }
            con.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult = MessageBox.Show("Are you sure?", "Deleting", MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.Yes)
            {
                foreach (DataGridViewRow row in dataGridView2.SelectedRows)
                {
                    int id = Convert.ToInt32(row.Cells[0].Value);
                    deleteCategory(id);
                    loadCategories();
                }
            }
            else if (dialogResult == DialogResult.No)
            {
                return;
            }
        }

        private void dataGridView3_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                int userID = Convert.ToInt32(dataGridView3.Rows[e.RowIndex].Cells["UserId"].Value);

                if (e.ColumnIndex == dataGridView3.Columns["ApproveButton"].Index)
                {
                    ApproveApplication(userID);
                }
                else if (e.ColumnIndex == dataGridView3.Columns["RejectButton"].Index)
                {
                    RejectApplication(userID);
                }
                else if (e.ColumnIndex == dataGridView3.Columns["ViewButton"].Index)
                {
                    ViewApplication(userID);
                }
            }
        }
        private string connectionString = "Server=tcp:viananyav2.database.windows.net,1433;Initial Catalog=viananyav2;Persist Security Info=False;User ID=viananyav2;Password=951753123789a.;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";
        private void ApproveApplication(int userID)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    string updateQuery = "UPDATE users SET UserAuth = 2, consCatID = @CategoryID WHERE id = @UserID;";
                    SqlCommand updateCommand = new SqlCommand(updateQuery, connection);
                    updateCommand.Parameters.AddWithValue("@UserID", userID);

                    string getCategoryQuery = "SELECT CategoryName FROM DanısmanBasvuru WHERE UserID = @UserID;";
                    SqlCommand getCategoryCommand = new SqlCommand(getCategoryQuery, connection);
                    getCategoryCommand.Parameters.AddWithValue("@UserID", userID);

                    string categoryName = getCategoryCommand.ExecuteScalar()?.ToString();

                    if (!string.IsNullOrEmpty(categoryName))
                    {
                        int categoryID = GetCategoryID(categoryName);

                        updateCommand.Parameters.AddWithValue("@CategoryID", categoryID);
                        int userRowsAffected = updateCommand.ExecuteNonQuery();

                        if (userRowsAffected > 0)
                        {
                            string deleteQuery = "DELETE FROM DanısmanBasvuru WHERE UserID = @UserID;";
                            SqlCommand deleteCommand = new SqlCommand(deleteQuery, connection);
                            deleteCommand.Parameters.AddWithValue("@UserID", userID);

                            int applicationRowsAffected = deleteCommand.ExecuteNonQuery();

                            if (applicationRowsAffected > 0)
                            {
                                MessageBox.Show($"Başvuru {userID} onaylandı ve görüntüleme tablosundan silindi.", "Başarılı", MessageBoxButtons.OK, MessageBoxIcon.Information);

                                foreach (DataGridViewRow row in dataGridView3.Rows)
                                {
                                    if (Convert.ToInt32(row.Cells["UserID"].Value) == userID)
                                    {
                                        dataGridView3.Rows.Remove(row);
                                        break;
                                    }
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"İşlem sırasında bir hata oluştu:\n{ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }


        private void RejectApplication(int userID)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    string updateQuery = "UPDATE users SET UserAuth = 1 WHERE id = @UserID;";
                    SqlCommand updateCommand = new SqlCommand(updateQuery, connection);
                    updateCommand.Parameters.AddWithValue("@UserID", userID);

                    int userRowsAffected = updateCommand.ExecuteNonQuery();

                    if (userRowsAffected > 0)
                    {
                        string deleteQuery = "DELETE FROM DanısmanBasvuru WHERE UserID = @UserID;";
                        SqlCommand deleteCommand = new SqlCommand(deleteQuery, connection);
                        deleteCommand.Parameters.AddWithValue("@UserID", userID);

                        int applicationRowsAffected = deleteCommand.ExecuteNonQuery();

                        if (applicationRowsAffected > 0)
                        {
                            MessageBox.Show($"Başvuru {userID} onaylanmadı.", "Başarılı", MessageBoxButtons.OK, MessageBoxIcon.Information);

                            foreach (DataGridViewRow row in dataGridView3.Rows)
                            {
                                if (Convert.ToInt32(row.Cells["UserID"].Value) == userID)
                                {
                                    dataGridView3.Rows.Remove(row);
                                    break;
                                }
                            }
                        }
                    }
                    ShowApplications();

                }
                catch (Exception ex)
                {
                    MessageBox.Show($"İşlem sırasında bir hata oluştu:\n{ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void LoadDataGrid()
        {

        }

        private void ViewApplication(int userID)
        {
            byte[] pdfData = (byte[])dataGridView3.Rows[dataGridView3.CurrentRow.Index].Cells["DocumentContent"].Value;

            string tempFilePath = Path.Combine(Path.GetTempPath(), $"{userID}_temp.pdf");
            File.WriteAllBytes(tempFilePath, pdfData);

            try
            {
                System.Diagnostics.Process.Start(tempFilePath);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"PDF görüntüleme hatası:\n{ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private int GetCategoryID(string categoryName)
        {
            int categoryID = -1;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    string query = "SELECT id FROM categories WHERE categoryName = @CategoryName;";
                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@CategoryName", categoryName);

                    object result = command.ExecuteScalar();

                    if (result != null && result != DBNull.Value)
                    {
                        categoryID = Convert.ToInt32(result);
                    }
                }
                catch (Exception ex)
                {

                    MessageBox.Show($"Kategori ID alınırken bir hata oluştu:\n{ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

            return categoryID;
        }

        private void button8_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult = MessageBox.Show("Are you sure?", "Deleting", MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.Yes)
            {
                foreach (DataGridViewRow row in dataGridView4.SelectedRows)
                {
                    int id = Convert.ToInt32(row.Cells[0].Value);
                    deleteReqs(id);
                    loadReqs();
                }
            }
            else if (dialogResult == DialogResult.No)
            {
                return;
            }
        }
    }
}