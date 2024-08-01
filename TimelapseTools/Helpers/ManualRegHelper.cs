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
        private static readonly int _lineWidth = 5;
        private static readonly int _crosshairsSize = 50;
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
            if (point.Y >=0 || point.X >= 0 || point.Y>image.Height || point.X>image.Width)
            {
                return imagepart;
            }

            var x = (point.X - width / 2) > 0 ? point.X - width / 2 : 0;
            var y = (point.Y - height / 2) > 0 ? point.Y - height / 2 : 0;
            var roi = new Rectangle(point.X - width / 2, point.Y - height / 2, width, height);
            imagepart.ROI = roi;

            return imagepart;
        }

        public static Image<Bgr, Byte> DrawCrosshairs(Image<Bgr, Byte> sample, Point point)
        {
            var xMin = (point.X - _crosshairsSize / 2);
            var xMax = (point.X + _crosshairsSize / 2);
            var yMin = (point.Y - _crosshairsSize / 2);
            var yMax = (point.Y + _crosshairsSize / 2);
            try
            {
                for (int x = xMin; x < xMax; x++)
                {
                    if (x < 0 || x >= sample.Width)
                    {
                        continue;
                    }

                    for (int y = yMin; y < yMax; y++)
                    {
                        if (y < 0 || y >= sample.Height)
                        {
                            continue;
                        }

                        //Draw horizontal line
                        if (xMin < x && x < xMax
                            && y > point.Y - _lineWidth / 2 && y < point.Y + _lineWidth / 2)
                        {
                            sample[y, x] = new Bgr(0, 0, 255);
                        }

                        //Draw vertical line 
                        if (yMin < y && y < yMax
                            && x > point.X - _lineWidth / 2 && x < point.X + _lineWidth / 2)
                        {
                            sample[y, x] = new Bgr(0, 0, 255);
                        }
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }

            return sample;
        }

        public static Image<Bgr, Byte> DrawCrosshairs(Image<Bgr, Byte> sample, int pointX, int pointY)
        {
            var xMin = (pointX - _crosshairsSize / 2);
            var xMax = (pointX + _crosshairsSize / 2);
            var yMin = (pointY - _crosshairsSize / 2);
            var yMax = (pointY + _crosshairsSize / 2);
            try
            {
                for (int x = xMin; x < xMax; x++)
                {
                    if (x < 0 || x >= sample.Width)
                    {
                        continue;
                    }

                    for (int y = yMin; y < yMax; y++)
                    {
                        if (y < 0 || y >= sample.Height)
                        {
                            continue;
                        }

                        //Draw horizontal line
                        if (xMin < x && x < xMax
                            && y > pointY - 1 && y < pointY + 1)
                        {
                            sample[y, x] = new Bgr(0, 0, 255);
                        }

                        //Draw vertical line 
                        if (yMin < y && y < yMax
                            && x > pointX - 1 && x < pointX + 1)
                        {
                            sample[y, x] = new Bgr(0, 0, 255);
                        }
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }

            return sample;
        }

        public static Image<Bgr, Byte> DrawSampleArea(Image<Bgr, Byte> sample, int width, int height, Point point)
        {
            var xMin = (point.X - width / 2);
            var xMax = (point.X + width / 2);
            var yMin = (point.Y - height / 2);
            var yMax = (point.Y + height / 2);
            try
            {
                for (int x = xMin; x < xMax; x++)
                {
                    if (x < 0 || x >= sample.Width)
                    {
                        continue;
                    }

                    for (int y = yMin; y < yMax; y++)
                    {
                        if (y < 0 || y >= sample.Height)
                        {
                            continue;
                        }

                        //Draw left border
                        if (xMin < x && x < xMin + _lineWidth)
                        {
                            sample[y, x] = new Bgr(0, 0, 255);
                        }

                        //Draw right border
                        if (xMax - _lineWidth < x && x < xMax)
                        {
                            sample[y, x] = new Bgr(0, 0, 255);
                        }

                        //Draw top border
                        if (yMin < y && y < yMin + _lineWidth)
                        {
                            sample[y, x] = new Bgr(0, 0, 255);
                        }

                        //Draw bottom border
                        if (yMax - _lineWidth < y && y < yMax)
                        {
                            sample[y, x] = new Bgr(0, 0, 255);
                        }

                    }
                }
            }
            catch (Exception)
            {

                throw;
            }

            return sample;
        }

        public static Point GetActualPoint(PictureBox pictureBox, Point point)
        {
            Point unscaled_p = new Point();

            // image and container dimensions
            int w_i = pictureBox.Image.Width;
            int h_i = pictureBox.Image.Height;
            int w_c = pictureBox.Width;
            int h_c = pictureBox.Height;

            float imageRatio = w_i / (float)h_i; // image W:H ratio
            float containerRatio = w_c / (float)h_c; // container W:H ratio

            if (imageRatio >= containerRatio)
            {
                // horizontal image
                float scaleFactor = w_c / (float)w_i;
                float scaledHeight = h_i * scaleFactor;
                // calculate gap between top of container and top of image
                float filler = Math.Abs(h_c - scaledHeight) / 2;
                unscaled_p.X = (int)(point.X / scaleFactor);
                unscaled_p.Y = (int)((point.Y - filler) / scaleFactor);
            }
            else
            {
                // vertical image
                float scaleFactor = h_c / (float)h_i;
                float scaledWidth = w_i * scaleFactor;
                float filler = Math.Abs(w_c - scaledWidth) / 2;
                unscaled_p.X = (int)((point.X - filler) / scaleFactor);
                unscaled_p.Y = (int)(point.Y / scaleFactor);
            }

            return unscaled_p;
        }

        public static Image<Bgr, Byte> MergeImage(Image<Bgr, Byte> image, Image<Bgr, Byte> image1)
        {
            if (image == null || image1 == null)
            {
                return new Image<Bgr, byte>(Size.Empty);
            }
            var xMax = image.Width > image1.Width ? image.Width : image1.Width;
            var yMax = image.Height > image1.Height ? image.Height : image1.Height;

            var sample = new Image<Bgr, byte>(xMax, yMax);
            try
            {
                for (int x = 0; x < xMax; x++)
                {
                    for (int y = 0; y < yMax; y++)
                    {
                        var point1 = new Bgr();
                        var point2 = new Bgr();
                        if (image.Width > x && image.Height > y)
                        {
                            point1 = image[y, x];
                        }
                        if (image1.Width > x && image1.Height > y)
                        {
                            point2 = image1[y, x];
                        }
                        sample[y, x] = MergeBgr(point1, point2);
                    }
                }
            }
            catch (Exception)
            {

                throw;
            }

            return sample;
        }

        private static Bgr MergeBgr(Bgr bgr, Bgr bgr1)
        {
            try
            {
                var blue = bgr.Blue / 2 + bgr1.Blue / 2;
                var red = bgr.Red / 2 + bgr1.Red / 2;
                var green = bgr.Green / 2 + bgr1.Green / 2;
                return new Bgr(blue, green, red);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
