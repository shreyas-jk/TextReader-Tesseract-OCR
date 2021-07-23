using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Tesseract;

namespace TextReader
{
    public partial class MainTool : Form
    {
        public MainTool()
        {
            InitializeComponent();
            flowLayoutPanel1.Controls.Add(pictureBox1);
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            Point currentPosition = this.Location;

            this.StartPosition = FormStartPosition.Manual;
            this.Location = new Point(Screen.PrimaryScreen.Bounds.Width + 10, Screen.PrimaryScreen.Bounds.Height + 10);

            Bitmap bitmapImage = new Bitmap(Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height);
            Graphics gr = Graphics.FromImage(bitmapImage);
            gr.CopyFromScreen(0, 0, 0, 0, bitmapImage.Size);

            ClipPanel form2 = new ClipPanel();

            form2.SetPicture(bitmapImage);
            this.Hide();
            form2.Show();
            this.Location = currentPosition;
        }

        public Bitmap GrayScaleImage(Bitmap image)
        {
            Bitmap grayScale = new Bitmap(image.Width, image.Height);

            for (Int32 y = 0; y < grayScale.Height; y++)
                for (Int32 x = 0; x < grayScale.Width; x++)
                {
                    Color c = image.GetPixel(x, y);

                    Int32 gs = (Int32)(c.R * 0.3 + c.G * 0.59 + c.B * 0.11);

                    grayScale.SetPixel(x, y, Color.FromArgb(gs, gs, gs));
                }
            return grayScale;
        }
        private Bitmap InvertImage(Bitmap bitmap)
        {
            Bitmap bmap = (Bitmap)bitmap.Clone();
            Color col;
            for (int i = 0; i < bmap.Width; i++)
            {
                for (int j = 0; j < bmap.Height; j++)
                {
                    col = bmap.GetPixel(i, j);
                    bmap.SetPixel(i, j,
          Color.FromArgb(255 - col.R, 255 - col.G, 255 - col.B));
                }
            }

            return (Bitmap)bmap.Clone();
        }

        internal void SetCroppedImage(Bitmap image)
        {
            this.pictureBox1.Image = image;
            this.pictureBox1.Invalidate();
            this.pictureBox1.Width = image.Width;
            this.pictureBox1.Height = image.Height;
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            var ocr = new TesseractEngine("./tessdata", "eng", EngineMode.Default);
            dynamic page = null;
            if (radioGrayscale.Checked == true)
            {
                var grayImage = GrayScaleImage((Bitmap)pictureBox1.Image);
                this.pictureBox1.Image = grayImage;
                this.pictureBox1.Invalidate();
                this.pictureBox1.Width = grayImage.Width;
                this.pictureBox1.Height = grayImage.Height;
                page = ocr.Process(grayImage);
            }
            else if (radioInvert.Checked == true)
            {
                var invertedImage = InvertImage((Bitmap)pictureBox1.Image);
                this.pictureBox1.Image = invertedImage;
                this.pictureBox1.Invalidate();
                this.pictureBox1.Width = invertedImage.Width;
                this.pictureBox1.Height = invertedImage.Height;
                page = ocr.Process(invertedImage);
            }
            else
            {
                page = ocr.Process((Bitmap)pictureBox1.Image);
            }

            richTextBox1.Clear();
            richTextBox1.Text = page.GetText();
        }

        private void Label1_Click(object sender, EventArgs e)
        {

        }

        private void Button3_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }
    }
}
