using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace DanismanLazim
{
    public partial class DanismanBasvur : Form
    {
        private string connectionString = "Server = tcp:viananyav2.database.windows.net,1433;Initial Catalog = viananyav2; Persist Security Info=False;User ID = viananyav2; Password=951753123789a.;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout = 30";
        public DanismanBasvur()
        {
            InitializeComponent();
            FillCategoriesComboBox();
            listView1.View = View.Details;
            listView1.Columns.Add("PDF Dosyaları", 200);
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }
        private void FillCategoriesComboBox()
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    string query = "SELECT categoryName FROM categories;";
                    SqlCommand command = new SqlCommand(query, connection);
                    SqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        string categoryName = reader["CategoryName"].ToString();
                        comboBox1.Items.Add(categoryName);
                    }

                    reader.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Kategoriler alınırken bir hata oluştu:\n{ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }


        private void button3_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();

            openFileDialog.Filter = "PDF Dosyaları (*.pdf)|*.pdf|Tüm Dosyalar (*.*)|*.*";
            openFileDialog.Title = "PDF Dosyalarını Seç";
            openFileDialog.Multiselect = true;

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                foreach (string selectedFilePath in openFileDialog.FileNames)
                {
                    if (IsPDF(selectedFilePath))
                    {
                        ListViewItem item = new ListViewItem(Path.GetFileName(selectedFilePath));
                        item.Tag = selectedFilePath;
                        listView1.Items.Add(item);
                    }
                }
            }

        }
        private void listViewPDFs_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ListViewHitTestInfo hitTest = listView1.HitTest(e.Location);
                ListViewItem clickedItem = hitTest.Item;

                if (clickedItem != null)
                {
                    string selectedPDFPath = clickedItem.Tag.ToString();
                    OpenPDF(selectedPDFPath);
                }
            }
        }
        private void OpenPDF(string pdfPath)
        {
            try
            {
                ProcessStartInfo psi = new ProcessStartInfo
                {
                    FileName = pdfPath,
                    UseShellExecute = true
                };

                MessageBox.Show($"Başlatılan dosya: {psi.FileName}", "Debug", MessageBoxButtons.OK, MessageBoxIcon.Information);

                Process.Start(psi);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"PDF açılırken bir hata oluştu:\n{ex.Message}\n\nHata Detayları:\n{ex.StackTrace}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private bool IsPDF(string filePath)
        {
            string extension = Path.GetExtension(filePath);
            return extension.Equals(".pdf", StringComparison.OrdinalIgnoreCase);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            {
                string newCategory = textBox1.Text.Trim();

                if (!string.IsNullOrEmpty(newCategory))
                {
                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        try
                        {
                            connection.Open();
                            string checkIfExistsQuery = "SELECT COUNT(*) FROM Categories WHERE CategoryName = @CategoryName;";
                            SqlCommand checkIfExistsCommand = new SqlCommand(checkIfExistsQuery, connection);
                            checkIfExistsCommand.Parameters.AddWithValue("@CategoryName", newCategory);
                            int categoryCount = (int)checkIfExistsCommand.ExecuteScalar();

                            if (categoryCount == 0)
                            {
                                string addCategoryQuery = "INSERT INTO Categories (CategoryName , categoryPicture) VALUES (@CategoryName,@categoryPicture);";

                                SqlCommand addCategoryCommand = new SqlCommand(addCategoryQuery, connection);
                                addCategoryCommand.Parameters.AddWithValue("@CategoryName", newCategory);
                                addCategoryCommand.Parameters.AddWithValue("@categoryPicture", imageToByteArray(pictureBox1.Image));
                                addCategoryCommand.ExecuteNonQuery();

                                comboBox1.Items.Add(newCategory);
                                MessageBox.Show("Kategori başarıyla eklendi.", "Başarılı", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            }
                            else
                            {
                                MessageBox.Show("Bu kategori zaten mevcut.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            }
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show($"Kategori eklenirken bir hata oluştu:\n{ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Lütfen geçerli bir kategori adı girin.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
        }
        public byte[] imageToByteArray(System.Drawing.Image imageIn)
        {
            MemoryStream ms = new MemoryStream();
            imageIn.Save(ms, System.Drawing.Imaging.ImageFormat.Gif);
            return ms.ToArray();
        }
        private void button2_Click(object sender, EventArgs e)
        {
            formLogin loginForm = Application.OpenForms.OfType<formLogin>().FirstOrDefault();
            int currentUserID = loginForm.GetCurrentUserID();

            string selectedCategory = comboBox1.SelectedItem?.ToString();

            if (string.IsNullOrEmpty(selectedCategory))
            {
                MessageBox.Show("Lütfen bir kategori seçin.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            using (SqlConnection connection = new SqlConnection(connectionString))
            {

                connection.Open();

                foreach (ListViewItem item in listView1.Items)
                {
                    string filePath = item.Tag.ToString();
                    byte[] fileData = File.ReadAllBytes(filePath);

                    string insertQuery = "INSERT INTO DanısmanBasvuru (UserId, CategoryName, DocumentContent) VALUES (@UserId, @CategoryName, @DocumentContent);";

                    using (SqlCommand command = new SqlCommand(insertQuery, connection))
                    {
                        command.Parameters.AddWithValue("@UserId", currentUserID);
                        command.Parameters.AddWithValue("@CategoryName", selectedCategory);
                        command.Parameters.AddWithValue("@DocumentContent", fileData);

                        int rowsAffected = command.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Başvuru başarıyla eklendi.", "Başarılı", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        else
                        {
                            MessageBox.Show("Başvuru eklenirken bir hata oluştu.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            }
        }

        private void DanismanBasvur_Load(object sender, EventArgs e)
        {

        }
    }
}
