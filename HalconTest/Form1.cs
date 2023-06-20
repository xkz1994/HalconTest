using System;
using System.Drawing;
using System.Windows.Forms;

namespace HalconTest
{
    public partial class Form1 : Form
    {
        private string _bitmapPath;
        public Form1()
        {
            InitializeComponent();
        }

        private void buttonOpenImage_Click(object sender, EventArgs e)
        {
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                _bitmapPath = openFileDialog.FileName;
                var _bitmap = new Bitmap(_bitmapPath);
                textBoxPath.Text = _bitmapPath;
                pictureBox.Image = _bitmap;
            }
        }

        private void buttonTest_Click(object sender, EventArgs e)
        {
            try
            {
                var mainImage = new Bitmap(_bitmapPath);
                HalconHelper.Bitmap2HObjectBpp8(mainImage, out var image);
                HalconHelper.findCircle(image, out var radius, out var offsetX, out var offsetY);
                richTextBox.Clear();
                richTextBox.AppendText($"{nameof(radius)} = {radius}\r\n");
                richTextBox.AppendText($"{nameof(offsetX)} = {offsetX}\r\n");
                richTextBox.AppendText($"{nameof(offsetY)} = {offsetY}\r\n");
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }
        }
    }
}
