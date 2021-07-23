using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TextReader
{
    public partial class ClipPanel : Form
    {
        Point startPos;
        Point currentPos;
        bool drawing;
        List<Rectangle> rectangles = new List<Rectangle>();
        private int ScaleFactor = 1;
        public ClipPanel()
        {
            InitializeComponent();
        }

        private Rectangle getRectangle()
        {
            return new Rectangle(
                Math.Min(startPos.X, currentPos.X),
                Math.Min(startPos.Y, currentPos.Y),
                Math.Abs(startPos.X - currentPos.X),
                Math.Abs(startPos.Y - currentPos.Y));
        }
        public void SetPicture(Bitmap image)
        {
            pictureBox1.Image = image;
            pictureBox1.Width = image.Width;
            pictureBox1.Height = image.Height;
            this.Width = image.Width;
            this.Height = image.Height;
        }

        private void PictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            currentPos = startPos = e.Location;
            drawing = true;
        }

        private void PictureBox1_MouseUp(object sender, MouseEventArgs e)
        {
            if (drawing)
            {
                drawing = false;
                var rc = getRectangle();
                if (rc.Width > 0 && rc.Height > 0) rectangles.Add(rc);

                Bitmap source = (Bitmap)pictureBox1.Image;
                pictureBox1.Image = source.Clone(new System.Drawing.Rectangle(startPos.X, startPos.Y, (currentPos.X - startPos.X), (currentPos.Y - startPos.Y)), source.PixelFormat);
                pictureBox1.Invalidate();

                MainTool form1 = new MainTool();
                form1.SetCroppedImage((Bitmap)pictureBox1.Image);
                this.Hide();
                form1.Show();
            }
        }

        private void PictureBox1_Paint(object sender, PaintEventArgs e)
        {
            if (drawing) e.Graphics.DrawRectangle(Pens.Red, getRectangle());
        }

        private void PictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            if (drawing)
            {
                currentPos = e.Location;
                if (drawing) pictureBox1.Invalidate();
            }
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }
    }
}
