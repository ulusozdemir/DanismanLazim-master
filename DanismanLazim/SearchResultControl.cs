using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DanismanLazim
{
    public partial class SearchResultControl : UserControl
    {
        public SearchResultControl()
        {
            InitializeComponent();
        }
        string temp_id;
        public static string id;

        private void SearchResultControl_Load(object sender, EventArgs e)
        {

        }

        public void details(Data d)
        {
            label1.Text = d.name;
            label2.Text = d.userAuth;
            label3.Text = d.categoryName;
            temp_id = d.userID;
            if(d.categoryImage == null)
            {
                roundedPictureBox1.Image = d.nameImage;
            }
            else if(d.nameImage == null)
            {
                roundedPictureBox1.Image = d.categoryImage;
            }
            else
            {
                roundedPictureBox1.Image = roundedPictureBox1.Image;
            }
        }

        public void searchResult(string key)
        {
            Data get = new Data();
            get.search(key);
            label1.Text = get.name;
            label2.Text = get.userAuth;
            label3.Text = get.categoryName;
            temp_id = get.userID;
            if (get.categoryImage == null)
            {
                roundedPictureBox1.Image = get.nameImage;
            }
            else if (get.nameImage == null)
            {
                roundedPictureBox1.Image = get.categoryImage;
            }
            else
            {
                roundedPictureBox1.Image = roundedPictureBox1.Image;
            }
        }

        private void SearchResultControl_MouseHover(object sender, EventArgs e)
        {
            this.BackColor = Color.WhiteSmoke;
        }

        private void SearchResultControl_MouseLeave(object sender, EventArgs e)
        {
            this.BackColor = Color.White;
        }
        public static bool clicked = false;
        private void SearchResultControl_Click(object sender, EventArgs e)
        {
            clicked = true;
            id = temp_id;
        }
    }
}
