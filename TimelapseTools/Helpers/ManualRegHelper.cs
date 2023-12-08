using Emgu.CV;
using Emgu.CV.Structure;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Rebar;

namespace MergePics
{
    internal class ManualRegHelper
    {
        private static readonly Random rand = new Random();

        public static void LoadPictureBox(PictureBox pictureBox, Image<Bgr, Byte> image, PictureBoxSizeMode pictureBoxSizeMode, Label label, int index, int totalCount)
        {
            LoadPictureBox(pictureBox, image, pictureBoxSizeMode);
            label.Text = $"{(index + 1).ToString()} / {totalCount.ToString()}";
        }

        public static void LoadPictureBox(PictureBox pictureBox, Image<Bgr, Byte> image, PictureBoxSizeMode pictureBoxSizeMode)
        {
            pictureBox.Image = Emgu.CV.BitmapExtension.ToBitmap(image.Mat);
            pictureBox.SizeMode = pictureBoxSizeMode;
        }

        public static Image<Bgr, Byte> GetSampleAreaImg(Image<Bgr, Byte> image, int width, int height, Point point)
        {
            var imagepart = image.Copy();
            var roi = new Rectangle(point.X, point.Y, width, height);
            imagepart.ROI = roi;

            return imagepart;
        }

        public static Image<Bgr, Byte> DrawSampleArea(Image<Bgr, Byte> sample, int width, int height, Point point)
        {
            var seed = rand.Next(100, 400);
            try
            {
                for (int x = point.X - width / 2; x < point.X + width / 2; x++)
                {
                    if (x < 0 || x >= sample.Width)
                    {
                        continue;
                    }

                    for (int y = point.Y - height / 2; y < point.Y + height / 2; y++)
                    {
                        if (y < 0 || y >= sample.Height)
                        {
                            continue;
                        }

                        var currentBGR = sample[y, x];
                        var blue = 255 - currentBGR.Blue + (seed < 200 ? (seed - 100d) / 100 * 255 : 0);
                        var red = 255 - currentBGR.Red + ((200 <= seed && seed < 300) ? (seed - 200d) / 100 * 255 : 0);
                        var green = 255 - currentBGR.Green + (300 <= seed ? (seed - 300d) / 100 * 255 : 0);
                        var halfBGR = new Bgr(blue / 2, green / 2, red / 2);
                        sample[y, x] = halfBGR;

                    }
                }
            }
            catch (Exception)
            {

                throw;
            }

            return sample;
        }
    }
}
