using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DanismanLazim
{
    public class Data
    {
        SqlConnection con = new SqlConnection("Server=tcp:viananyav2.database.windows.net,1433;Initial Catalog=viananyav2;Persist Security Info=False;User ID=viananyav2;Password=951753123789a.;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;");

        public string name { get; set; }
        public string categoryName { get; set; }
        public string userAuth { get; set; }

        public Image nameImage { get; set; }
        public Image categoryImage { get; set; }
        public string userID { get; set; }

        public static List<Data> list = new List<Data>();

        public byte[] imageToByteArray(System.Drawing.Image imageIn)
        {
            MemoryStream ms = new MemoryStream();
            imageIn.Save(ms, System.Drawing.Imaging.ImageFormat.Gif);
            return ms.ToArray();
        }

        public Image convertBytesToImage(byte[] byteArrayIn)
        {
            using (var ms = new MemoryStream(byteArrayIn))
            {
                return Image.FromStream(ms);
            }
        }
        public void search(string key)
        {
            con.Open();
            string query = "SELECT userName, userSurname, userAuth, userProfilePicture, id, NULL AS categoryName, NULL AS categoryPicture FROM users WHERE users.userName LIKE @searchText UNION Select NULL AS userName, NULL AS userSurname, NULL AS userAuth, NULL AS userProfilePicture, NULL AS id, categoryName, categoryPicture FROM categories WHERE categories.categoryName LIKE @searchText";
            SqlCommand cmd = new SqlCommand(query, con);
            cmd.Parameters.AddWithValue("@searchText", key + "%");
            SqlDataReader reader = cmd.ExecuteReader();
            list.Clear();

            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    Data data = new Data
                    {
                        name = reader["userName"].ToString() + " " + reader["userSurname"].ToString(),
                        categoryName = reader["categoryName"].ToString(),
                        userAuth = reader["userAuth"].ToString(),
                        userID = reader["id"].ToString()
                    };

                    if (reader["userProfilePicture"] != DBNull.Value)
                    {
                        data.nameImage = convertBytesToImage((byte[])reader["userProfilePicture"]);
                        Console.WriteLine("userProfilePicture Type: " + reader["userProfilePicture"].GetType().FullName);

                    }

                    if (reader["categoryPicture"] != DBNull.Value)
                    {
                        data.categoryImage = convertBytesToImage((byte[])reader["categoryPicture"]);
                        Console.WriteLine("categoryPicture Type: " + reader["categoryPicture"].GetType().FullName);
                    }

                    if (data.userAuth == "1")
                    {
                        data.userAuth = "Kullanıcı";
                    }
                    else if (data.userAuth == "2")
                    {
                        data.userAuth = "Danışman";
                    }
                    else if (data.userAuth == "")
                    {
                        data.userAuth = "Kategori";
                    }

                    list.Add(data);

                }
            }
            reader.Dispose();
            cmd.Dispose();
            con.Close();
        }

        public void getSelected(string id)
        {
            con.Open();
            string q = "SELECT userName, userSurname, userAuth, userProfilePicture, id, NULL AS categoryName, NULL AS categoryPicture, NULL AS id FROM users WHERE users.id LIKE @id UNION Select NULL AS userName, NULL AS userSurname, NULL AS userAuth, NULL AS userProfilePicture, NULL AS id, id, categoryName, categoryPicture FROM categories WHERE categories.id LIKE @id";
            SqlCommand cmd = new SqlCommand(q, con);
            cmd.Parameters.AddWithValue("@id", id);
            SqlDataReader r = cmd.ExecuteReader();
            if(r.Read())
            {
                this.name = r["userName"].ToString();
                if (r["userAuth"].ToString() == "1")
                {
                    this.userAuth = "Kullanıcı";
                }
                else if (r["userAuth"].ToString() == "2")
                {
                    this.userAuth = "Danışman";
                }
                else if (r["userAuth"].ToString() == "")
                {
                    this.userAuth = "Kategori";
                }

                if (r["userProfilePicture"] != DBNull.Value)
                {
                    this.nameImage = convertBytesToImage((byte[])r["userProfilePicture"]);
                    Console.WriteLine("userProfilePicture Type: " + r["userProfilePicture"].GetType().FullName);

                }

                if (r["categoryPicture"] != DBNull.Value)
                {
                    this.categoryImage = convertBytesToImage((byte[])r["categoryPicture"]);
                    Console.WriteLine("categoryPicture Type: " + r["categoryPicture"].GetType().FullName);
                }
            }
            r.Dispose();
            cmd.Dispose();
            con.Close();
        }
    }
}