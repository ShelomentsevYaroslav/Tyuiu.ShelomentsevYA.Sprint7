using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace Tyuiu.ShelomentsevYA.Sprint7.V11
{
    public partial class FormAbout_SYA : Form
    {
        public FormAbout_SYA()
        {
            InitializeComponent();
            LoadPhoto();
        }

        private void LoadPhoto()
        {
            try
            {
                string path = Path.Combine(
                    AppDomain.CurrentDomain.BaseDirectory,
                    "Images",
                    "author.jpg");

                if (File.Exists(path))
                {
                    pictureBoxPhoto_SYA.Image?.Dispose();
                    pictureBoxPhoto_SYA.Image = Image.FromFile(path);
                }
                else
                {
                    pictureBoxPhoto_SYA.BackColor = Color.LightGray;
                }
            }
            catch
            {
                pictureBoxPhoto_SYA.BackColor = Color.LightGray;
            }
        }

    }
}
